using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Vistas.CarritoVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.CarritoViewModels
{
    public class CarritoViewModel : BaseViewModel
    {
        readonly INavigation navigation;
        readonly ContentPage page;

        public Command GestoRefrescamientoCommand { get; set; }
        public Command<CarritoModelo> EliminandoCurso { get; set; }
        public Command RedireccionAMedioPago { get; set; }

        private string preciocarrito;
        public string PrecioTotal
        {
            get => preciocarrito;
            set => SetProperty(ref preciocarrito, value);
        }

        private ObservableCollection<CarritoModelo> _carritoElementos;
        public ObservableCollection<CarritoModelo> CarritoElementos
        {
            get => _carritoElementos;
            set
            {
                _carritoElementos = value;
                OnPropertyChanged();
            }
        }
        private decimal PrecioContado;

        public CarritoViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            CarritoElementos = new ObservableCollection<CarritoModelo>();
            GestoRefrescamientoCommand = new Command(RecargarCarro);
            EliminandoCurso = new Command<CarritoModelo>(EliminandoElementoCarrito);

            RedireccionAMedioPago = new Command(RedirigiendoApartadoPago);

        }

        public void OnAppearing()
        {
            IsBusy = true;

            MostrandoDatosDelCarro();
            CalculandoTotal();

            IsBusy = false;
        }

        private void RedirigiendoApartadoPago()
        {
            if (CarroServices.Carritos.Count > 0)
                CalculandoTotal();
                navigation.PushAsync(new PagandoConTarjetaVista(PrecioContado));
        }

        private async void EliminandoElementoCarrito(CarritoModelo obj)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Aviso", "Estas seguro de querer eliminar el curso de "+obj.TituloCurso + " de tu carrito ?", "Si", "No");
            if (answer)
            {
                CarroServices.Carritos.Remove(obj);
                CarritoElementos.Remove(obj);

                CalculandoTotal();
            }
            else
            {
                return;
            }
        }

        private void RecargarCarro()
        {
            IsLoading = true;
            MostrandoDatosDelCarro();
            CalculandoTotal();
            IsLoading = false;
        }

        private void MostrandoDatosDelCarro()
        {
            CarritoElementos.Clear();

            var CarroFresco = CarroServices.Carritos;
            foreach (var curso in CarroFresco)
            {
                CarritoElementos.Add(curso);
            }
        }

        public void CalculandoTotal()
        {
            PrecioContado = 0.00m;

            foreach (var item in CarroServices.Carritos)
            {
                PrecioContado += Convert.ToDecimal(item.PrecioCurso);
            }
            PrecioTotal = String.Format("{0:N}", PrecioContado);
        }


    }
}
