﻿using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.UsuarioServices;
using BazarBoutique.Vistas.BarraNavegacion;
using BazarBoutique.Vistas.CatalogoCursosVistas;
using BazarBoutique.Vistas.InicioSesíonVistas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.InicioSesionViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Propiedades
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private readonly IGoogleManager AdministradorGoogle;
        GoogleUser Usuario = new GoogleUser();
        public string Email { get; set; }
        public string Password { get; set; }
        public Command IniciarSesionCommand { get; }
        public Command RegistrarsePageCommand { get; }
        public Command btnAutenticacionGoogle { get; }

        ILoginService ServiciosLogin = DependencyService.Get<ILoginService>();
        IRedesSocialesInicioService ServiciosLoginRedes = DependencyService.Get<IRedesSocialesInicioService>();
        
        #endregion

        public LoginViewModel(INavigation navigation, ContentPage page)
        {
            IniciarSesionCommand = new Command(SePresionoLogin);
            RegistrarsePageCommand = new Command(RedireccionApartadoRegister);
            btnAutenticacionGoogle = new Command(AutenticacionGoogle);
            this.navigation = navigation;
            this.page = page;
            AdministradorGoogle = DependencyService.Get<IGoogleManager>();
        }

        private void AutenticacionGoogle()
        {
            AdministradorGoogle.Login(OnLoginComplete);
        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            IsBusy = true;
            //Aqui va la comprobacion 
            if (googleUser != null)
            {

                Usuario = googleUser;
                bool IsLogued = await ServiciosLoginRedes.AutenticacionGoogle(googleUser.IdToken);

                if (IsLogued)
                {
                    Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
                }
                //SesionServicios.UsuarioGoogle = Usuario;
                //Usuario.IdToken 
                //Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
            }

            IsBusy = false;
        }

        private async void RedireccionApartadoRegister(object obj)
        {
            navigation.InsertPageBefore(new RegistrarseVista(), navigation.NavigationStack[0]);
            await navigation.PopAsync();
        }

        private async void SePresionoLogin(object obj)
        {
            
            IsBusy = true;

            
            var login = new Modelos.LoginModelo
            {
                email = Email,
                password = Password
            };

            bool IsLogued = await ServiciosLogin.IniciarSesion(login);

            if (IsLogued)
            {
                Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
            }

            IsBusy = false;
            //var httpClientHandler = new HttpClientHandler();

            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) => { return true; };


            //Uri RequestUri = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/login");
            //var client = new HttpClient(httpClientHandler);
            //var json = JsonConvert.SerializeObject(login);
            //var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            //try
            //{
            //    var response = await client.PostAsync(RequestUri, contentJson);
            //    if (response.StatusCode == HttpStatusCode.Accepted)
            //    {
            //        var ResponsePerfilAuth = JsonConvert.DeserializeObject<ApiResponseModelo>(await response.Content.ReadAsStringAsync());

            //        SesionServicios.apiResponse = ResponsePerfilAuth;

            //        DataModelo InformacionToken = ResponsePerfilAuth.data;
            //        //////
            //        var client2 = new HttpClient(httpClientHandler);
            //        client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(InformacionToken.token_type, InformacionToken.access_token);

            //        Uri ConfirmarUsuario = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/user");

            //        var responsePerfil = await client2.GetAsync(ConfirmarUsuario);

            //        if(responsePerfil.StatusCode == HttpStatusCode.OK)
            //        {
            //            var responsePerfilInformation = JsonConvert.DeserializeObject<UsuarioResponseModelo>(await responsePerfil.Content.ReadAsStringAsync());

            //            SesionServicios.apiUser = responsePerfilInformation.data;

            //            Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
            //        }
            //        else {
            //            await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Error Al Autenticar Usuario", "OK");
            //        }

            //    }
            //    else
            //    {
            //        await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "El Usuario no existe", "OK");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Error al autenticar usuario", "OK");
            //}
            //finally
            //{
            //    IsBusy = false;
            //}
        }
    }
}
