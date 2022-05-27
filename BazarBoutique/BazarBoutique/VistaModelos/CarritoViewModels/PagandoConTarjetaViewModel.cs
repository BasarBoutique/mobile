using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Stripe;
using BazarBoutique.Services;
using System.Threading.Tasks;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Vistas.BarraNavegacion;
using Application = Xamarin.Forms.Application;

namespace BazarBoutique.VistaModelos.CarritoViewModels
{
    public class PagandoConTarjetaViewModel : BaseViewModel
    {
        ICursoService ServiciosCursos = DependencyService.Get<ICursoService>();
        readonly INavigation navigation;
        readonly ContentPage page;
        private decimal PrecioTotal;
        private string _preciototalcom;


        private string _precioEnDolares;
        public string PrecioEnDolares
        {
            get => _precioEnDolares;
            set => SetProperty(ref _precioEnDolares,value);
        }
        
        public string PrecioConComision
        {
            get => _preciototalcom; 
            set => SetProperty(ref _preciototalcom, value); 
                
        }


        private string _numerotarjeta;
        public string NumeroTarjeta
        {
            get => _numerotarjeta;
            set => SetProperty(ref _numerotarjeta, value);
        }

        private DateTime _fechaVencimiento;

        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set => SetProperty(ref _fechaVencimiento, value);

        }

        private string _cvc;

        public string CVC
        {
            get => _cvc;
            set => SetProperty(ref _cvc, value);

        }

        public Command ProcesarCompraCommand { get; }
        private decimal PrecioEnSolesCuenta;
        private decimal PrecioEnDolaresCuenta;
        public PagandoConTarjetaViewModel(INavigation navigation, ContentPage page, Decimal PrecioTotal)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;
            PrecioEnSolesCuenta = PrecioTotal;

            ProcesarCompraCommand = new Command(ProcesandoCompra, ValidandoCampos);
            this.PropertyChanged +=
                        (_, __) => ProcesarCompraCommand.ChangeCanExecute();
        }

        public void OnAppearing()
        {
            PrecioEnDolaresCuenta = PrecioEnSolesCuenta * 0.26m;
            PrecioEnDolares = String.Format("{0:N}", PrecioEnDolaresCuenta);
            PrecioConComision = String.Format("{0:N}", (PrecioEnDolaresCuenta + 2.26m));
        }

        private bool ValidandoCampos()
        {
            string ComparandoValores= NumeroTarjeta;

            //ComparandoValores.Replace(" ", String.Empty);
            bool ValidandoNumeroTarjeta = false;
            bool ValidandoFechaV= false;
            bool ValidandoCVC = false;

            if (!string.IsNullOrEmpty(ComparandoValores))
            {
                ComparandoValores = NumeroTarjeta.Replace(" ", String.Empty);
                if(ComparandoValores.Length >= 15)
                {
                    ValidandoNumeroTarjeta = true;
                }  
            }

            if (!string.IsNullOrEmpty(CVC))
            {
                if (CVC.Length >= 3)
                {
                    ValidandoFechaV = true;
                }
            }

            if (FechaVencimiento > DateTime.Now)
            {
                ValidandoCVC = true;
            }


            return (ValidandoNumeroTarjeta && ValidandoFechaV && ValidandoCVC);
        }



        private async void ProcesandoCompra()
        {
            string DigitosTarjeta = NumeroTarjeta.Replace(" ", String.Empty);
            //DigitosTarjeta;
            try
            {

                StripeConfiguration.ApiKey = "sk_test_51Jt1e3HeKaFOYcokzGiWasVgvpzO0oYuNrHr3mZuHiTabUPx4eZ3gPefXQhzKU8k9MeimT1oQPvXyOF3mY6wOvhg00ea5NjxVS";

                //datos almacenados
                string mycliente;
                string getchargedID;
                string refundID;


                /////Crear un Token
                ///Codigo Mejorado
                var stripecard = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = DigitosTarjeta,
                        ExpYear = FechaVencimiento.Year,
                        ExpMonth = FechaVencimiento.Month,
                        Cvc = CVC
                    },
                };

                var TokenGenerado = new TokenService();
                Token newToken = TokenGenerado.Create(stripecard);

                //////This are the sample test data use MVVM bindings to send data to the ViewModel

                //Stripe.CreditCardOptions stripcard = new Stripe.CreditCardOptions();
                //stripcard.Number = "4000000000003055";
                //stripcard.ExpYear = 2020;
                //stripcard.ExpMonth = 08;
                //stripcard.Cvc = "199";

           
                //////Step 1 : Assign Card to Token Object and create Token


                //Stripe.TokenCreateOptions token = new TokenCreateOptions();
                //token.Card = stripcard;
                //Stripe.TokenService serviceToken = new Stripe.TokenService();
                //Stripe.Token newToken = serviceToken.Create(token);


                // Step 2 : Assign Token to the Source

                var options = new SourceCreateOptions
                {
                    Type = SourceType.Card,
                    Currency = "usd",
                   
                    Token = newToken.Id
                };

                var service = new SourceService();
                Source source = service.Create(options);




                //Step 3 : Now generate the customer who is doing the payment


                //Codigo Padado
                //CustomerCreateOptions myCustomer = new CustomerCreateOptions()
                //{
                //    Name = "Samir",
                //    Email = "SamirGc@gmail.com",
                //    Description = "un ",
                //};


                //var customerService = new CustomerService();
                //Customer stripeCustomer = customerService.Create(myCustomer);

                //mycustomer = stripeCustomer.Id; // Not needed


                //Codigo Mejorado

                CustomerCreateOptions cliente = new CustomerCreateOptions()
                {
                    Name = SesionServicios.apiUser.name,
                    Email = SesionServicios.apiUser.email,
                    Description = "comprando un articulo",
                };


                var customerService = new CustomerService();
                Customer stripeCustomer = customerService.Create(cliente);

                mycliente = stripeCustomer.Id; // Not needed


                //Step 4 : Now Create Charge Options for the customer. 
                var chargeoptions = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(PrecioEnDolaresCuenta * 100),
                    Currency = "USD",
                    ReceiptEmail = SesionServicios.apiUser.email,
                    Customer = stripeCustomer.Id,
                    Source = source.Id

                };

                //Step 5 : Perform the payment by  Charging the customer with the payment. 
                var service1 = new ChargeService();
                Charge charge = service1.Create(chargeoptions); // This will do the Payment

                getchargedID = charge.Id;

                await App.Current.MainPage.DisplayAlert("Pago","Pago realizado","Ok");
                await RegistrandoCursos();
                CarroServices.Carritos.Clear();
                Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");

            }

        }

        private async Task RegistrandoCursos()
        {
            foreach (var curso in CarroServices.Carritos)
            {
                await ServiciosCursos.RegistrandoCursos(curso.CursoId, SesionServicios.apiUser.id);
            }
            await Application.Current.MainPage.DisplayAlert("Muchas gracias por comprar", "El curso se registro correctamente", "OK");

        }



















    }
}
