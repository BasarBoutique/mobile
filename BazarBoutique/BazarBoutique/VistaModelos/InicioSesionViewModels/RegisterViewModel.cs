using BazarBoutique.Modelos;
using BazarBoutique.Vistas.BarraNavegacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.InicioSesionViewModels
{
    public class RegisterViewModel: BaseViewModel
    {
        #region Campos

        private readonly INavigation navigation;
        private readonly ContentPage page;

        private bool _isregistrable;

        private string _password;
        private string _nombre;
        private string _email;
        
        #endregion

        #region Propiedades
        
        //Campos para vista
        public bool IsRegistrable
        {
            get => _isregistrable; 
            set { 
                    SetProperty(ref _isregistrable, value); 
            }
        }

        //Campos de validacion
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
            }
        }
        
        public string Paswword
        {
            get => _password; 
            set {
                   SetProperty(ref _password, value) ; 
            }
        }

        private string _passwordAuth;
        public string PasswordAuth
        {
            get => _passwordAuth; 
            set => SetProperty(ref _passwordAuth, value); 
        }

        public string Nombre
        {
            get => _nombre;
            set {
                SetProperty(ref _nombre, value);
            }
        }

        //Commands Funciones
        public Command btnRegistrarseCommand { get; set; }

        #endregion


        public RegisterViewModel(INavigation navigation, ContentPage page)
        {
            //IsRegistrable = true;
            this.navigation = navigation;
            this.page = page;

            btnRegistrarseCommand = new Command(RegistrarUsuario, ValidandoCampos);
            this.PropertyChanged +=
                        (_, __) => btnRegistrarseCommand.ChangeCanExecute();
        }

        private bool ValidandoCampos()
        {
            bool ValidandoContraseña = false;
            bool ValidandoEmail = false;
            bool ValidandoNombre = false;
            if (Paswword == PasswordAuth && !string.IsNullOrWhiteSpace(PasswordAuth) && !string.IsNullOrWhiteSpace(Paswword) && Paswword.Length > 5)
            {
                 ValidandoContraseña = true;
            }

            if (!string.IsNullOrWhiteSpace(Email) && Email.Contains("@") && (Email.Contains(".com") || Email.Contains(".pe") ) )
            {
                ValidandoEmail = true;
            }
            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                ValidandoNombre = true;
            }
            return (ValidandoEmail && ValidandoContraseña && ValidandoNombre);
        }

        private async void RegistrarUsuario()
        {
            IsBusy = true;

            var usuario = new RegisterModelo
            {
                name = Nombre,
                email = Email,
                password = Paswword
            };


            Uri RequestUri = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/signup");
            var cliente = new HttpClient();

            var UsuarioSerealizado = JsonConvert.SerializeObject(usuario);
            HttpContent httpcontent = new StringContent(UsuarioSerealizado, Encoding.UTF8, "application/json");

            try
            {
                var response = await cliente.PostAsync(RequestUri, httpcontent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Fue Registrado Exitosamente, vaya a iniciar sesión", "OK");
                    Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Tiene campos incorrectos, vuelva a intentarlo", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Ocuerrieron problemas al registrar su usuario", "OK");
            }


            IsBusy = false;
        }
    }
}
