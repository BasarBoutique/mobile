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

[assembly: Dependency(typeof(UsuarioServices))]
namespace BazarBoutique.Services.UsuarioServices
{
    public class UsuarioServices : IUsuarioServices
    {
        HttpClient cliente;
        public UsuarioServices()
        {
        }

        public async Task<DataUsuario> GetPaginacionUsuario(Uri direccion, SearchCourseFilters filtro)
        {
            //Definiendo Paginacion Vacia
            var PaginacionUsuario = new DataUsuario();

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            cliente = new HttpClient(httpClientHandler);

            // clie.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search");

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var filtrojson = JsonConvert.SerializeObject(filtro);
            var contentJson = new StringContent(filtrojson, Encoding.UTF8, "application/json");

            try
            {
                //HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());
                var response = await cliente.PostAsync(direccion, contentJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<UsuarioPaginacionResponseModelo>(await response.Content.ReadAsStringAsync());
                    //var resultado = JsonConvert.DeserializeObject<PaginationResponse>(contenido);

                    PaginacionUsuario = resultado.data;

                    return PaginacionUsuario;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de cursos", "OK");
                    return PaginacionUsuario;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                Console.WriteLine(ex.ToString());
                return PaginacionUsuario;
            }

        }


        public async Task<List<UsuarioModelo>> GetUsuarioSlide()
        {
            cliente = new HttpClient();
            //var request = new HttpRequestMessage();

            //cliente.BaseAddress = new Uri("");
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);
            //string body = @"{""withDisabled"":""true""}";

            var ElementosUsuarios = new List<UsuarioModelo>();

            try
            {
                var response = await cliente.GetAsync("https://monolith-stage.herokuapp.com/api/v1/auth/author");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<UsuarioSlideResponseModelo>(contenido);
                    ElementosUsuarios.AddRange(resultado.data);

                    return ElementosUsuarios;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de autores", "OK");
                    return ElementosUsuarios;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos de autores", "OK");
                Console.WriteLine(ex.ToString());
                return ElementosUsuarios;
            }
        }
    }
}
