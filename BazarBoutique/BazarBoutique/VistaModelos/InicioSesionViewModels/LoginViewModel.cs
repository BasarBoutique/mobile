using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Vistas.BarraNavegacion;
using BazarBoutique.Vistas.CatalogoCursosVistas;
using BazarBoutique.Vistas.InicioSesíonVistas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.InicioSesionViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Propiedades
        private readonly INavigation navigation;
        private readonly ContentPage page;
        public string Email { get; set; }
        public string Password { get; set; }
        public Command IniciarSesionCommand { get; }
        public Command RegistrarsePageCommand { get; }

        #endregion

        public LoginViewModel(INavigation navigation, ContentPage page)
        {
            IniciarSesionCommand = new Command(SePresionoLogin);
            RegistrarsePageCommand = new Command(SePresionoRegister);
            
            this.navigation = navigation;
            this.page = page;
        }

        private async void SePresionoRegister(object obj)
        {
            await navigation.PushAsync(new RegistrarseVista());
        }

        private async void SePresionoLogin(object obj)
        {
            

            IsBusy = true;

            var login = new Modelos.LoginModelo
            {
                email = Email,
                password = Password
            };

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };


            Uri RequestUri = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/login");
            var client = new HttpClient(httpClientHandler);
            var json = JsonConvert.SerializeObject(login);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    var ResponsePerfil = JsonConvert.DeserializeObject<ApiResponseModelo>(await response.Content.ReadAsStringAsync());

                    SesionServicios.apiResponse = ResponsePerfil;
                    Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("ENNETO", "Usuario y/o clave incorrectos", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ENNETO", "No se puede autenticar al usuario", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
