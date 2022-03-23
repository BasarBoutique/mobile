using BazarBoutique.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BazarBoutique.Services.CategoriaServices
{
    public class CategoryService
    {
        public static async Task<IList<CategoriaModelo>> GetCategorias()
        {
            var cliente = new HttpClient();
            //var request = new HttpRequestMessage();

            //cliente.BaseAddress = new Uri("");
            //cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);
            //string body = @"{""withDisabled"":""true""}";

            try
            {
                var response = await cliente.GetAsync("https://monolith-stage.herokuapp.com/api/v1/categories/category");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CategoriaResponseModelo>(contenido);

                    return resultado.data;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible autenticar", "OK");
                    return null;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique","Error al traer datos","OK");
                //await Application.Current.MainPage.DisplayAlert("Error:",ex,"ok");
                return null;
            }

            
        }

    }
}
