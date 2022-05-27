using BazarBoutique.Modelos;
using BazarBoutique.Modelos.ModeloSQL;
using BazarBoutique.Services.UsuarioServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginService))]
namespace BazarBoutique.Services.UsuarioServices
{
    public class LoginService : ILoginService
    {
        HttpClient client;
        public LoginService()
        {
        }

        public async Task<bool> IniciarSesion(LoginModelo user)
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            Uri RequestUri = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/login");
            client = new HttpClient(httpClientHandler);
            var json = JsonConvert.SerializeObject(user);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var ResponsePerfilAuth = JsonConvert.DeserializeObject<ApiResponseModelo>(await response.Content.ReadAsStringAsync());

                    SesionServicios.apiResponse = ResponsePerfilAuth;

                    DataModelo InformacionToken = ResponsePerfilAuth.data;
                    //////
                    HttpClient client2 = new HttpClient(httpClientHandler);
                    client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(InformacionToken.token_type, InformacionToken.access_token);

                    Uri ConfirmarUsuario = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/user");

                    var responsePerfil = await client2.GetAsync(ConfirmarUsuario);

                    if (responsePerfil.StatusCode == HttpStatusCode.OK)
                    {
                        //eliminar esto cuando corrijan la api
                        var responsePerfilInformation = JsonConvert.DeserializeObject<UsuarioResponseModelo>(await responsePerfil.Content.ReadAsStringAsync());

                        SesionServicios.apiUser = responsePerfilInformation.data;


                        //RegistrandoSQLite

                        await App.SQLiteDB.GuardandoUsuarioAsync(new UsuarioSql
                        {
                            id = responsePerfilInformation.data.id,
                            Nombre = responsePerfilInformation.data.name,
                            Correo = responsePerfilInformation.data.email,
                            fullname = responsePerfilInformation.data.detail.fullname,
                            photo = responsePerfilInformation.data.detail.photo,
                            address = responsePerfilInformation.data.detail.address,
                            phone = responsePerfilInformation.data.detail.phone,
                            uuid = responsePerfilInformation.data.detail.uuid,


                            roles = responsePerfilInformation.data.roles,

                            succes = ResponsePerfilAuth.success,
                            message = ResponsePerfilAuth.message,
                            acces_token = ResponsePerfilAuth.data.access_token,
                            token_type = ResponsePerfilAuth.data.token_type,
                            FechaExpiracion = ResponsePerfilAuth.data.expire_at
                            //ResponseUsuario = responsePerfilInformation.data

                        });

                        return true;
                        //Application.Current.MainPage = new NavigationPage(new MenuLateralVista());
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Error Al Autenticar Usuario", "OK");
                        return false;
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "El Usuario no existe", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Error al autenticar usuario", "OK");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }
    }
}
