using BazarBoutique.Modelos;
using BazarBoutique.Services.UsuarioServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(RegisterService))]
namespace BazarBoutique.Services.UsuarioServices
{
    public class RegisterService : IRegisterService
    {
        HttpClient cliente;
        public RegisterService()
        {
        }

        public async Task<bool> RegistrarseUsuario(RegisterModelo user)
        {

            Uri RequestUri = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/signup");
            cliente = new HttpClient();

            var UsuarioSerealizado = JsonConvert.SerializeObject(user);
            HttpContent httpcontent = new StringContent(UsuarioSerealizado, Encoding.UTF8, "application/json");

            try
            {
                var response = await cliente.PostAsync(RequestUri, httpcontent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Fue Registrado Exitosamente", "OK");
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Modifique los campos con otros datos", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Bazar Boutique", "Ocuerrieron problemas al registrar su usuario", "OK");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

    }
}
