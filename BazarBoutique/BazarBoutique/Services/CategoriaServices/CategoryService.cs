using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CategoryService))]
namespace BazarBoutique.Services.CategoriaServices
{
    public class CategoryService : ICategoryService
    {
        HttpClient clie;
        public CategoryService()
        {
            
        }

        public async Task<List<CategoriaModelo>> GetCategoriaSlide()
        {
            clie = new HttpClient();
            //var request = new HttpRequestMessage();

            //cliente.BaseAddress = new Uri("");
            clie.DefaultRequestHeaders.Add("Accept", "application/json");
            clie.Timeout = TimeSpan.FromMinutes(2);
            //string body = @"{""withDisabled"":""true""}";

            var ElementosCategorias = new List<CategoriaModelo>();

            try
            {
                var response = await clie.GetAsync("https://monolith-stage.herokuapp.com/api/v1/categories/category");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CategoriaResponseModelo>(contenido);
                    ElementosCategorias.AddRange(resultado.data);

                    return ElementosCategorias;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible autenticar", "OK");
                    return ElementosCategorias;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                return ElementosCategorias;
            }
        }


    }
}
