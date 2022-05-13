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
        HttpClient cliente;
        public CategoryService()
        {
            
        }

        public async Task<List<CategoriaSlideModelo>> GetCategoriaSlide()
        {
            cliente = new HttpClient();
            //var request = new HttpRequestMessage();

            //cliente.BaseAddress = new Uri("");
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);
            //string body = @"{""withDisabled"":""true""}";

            var ElementosCategorias = new List<CategoriaSlideModelo>();

            try
            {
                var response = await cliente.GetAsync("https://monolith-stage.herokuapp.com/api/v1/categories/category");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CategoriaSlideResponseModelo>(contenido);
                    ElementosCategorias.AddRange(resultado.data);

                    return ElementosCategorias;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de categorias", "OK");
                    return ElementosCategorias;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                Console.WriteLine(ex.ToString());
                return ElementosCategorias;
            }
        }

        public async Task<DataCategorias> GetPaginacionCategoria(Uri direccion, SearchCourseFilters filtro)
        {
            //Definiendo Paginacion Vacia
            var PaginacionCategorias = new DataCategorias();

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            cliente = new HttpClient(httpClientHandler);

            //https://monolith-stage.herokuapp.com/api/v1/categories/search


            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var filtrojson = JsonConvert.SerializeObject(filtro);
            var contentJson = new StringContent(filtrojson, Encoding.UTF8, "application/json");
            

            
            try
            {
                //HttpResponseMessage response = await clie.GetAsync("?" + direccion +"withDisabled" + "=" + Convert.ToString(MostrarOcultos).ToLower());
                var response = await cliente.PostAsync(direccion, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CategoriaResponseModelo>(await response.Content.ReadAsStringAsync());
                    //var resultado = JsonConvert.DeserializeObject<PaginationResponse>(contenido);

                    //ElementosCursos.AddRange(resultado.data.coleccion);
                    PaginacionCategorias = resultado.data;

                    return PaginacionCategorias;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de las categorias", "OK");
                    return PaginacionCategorias;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                Console.WriteLine(ex.ToString());
                return PaginacionCategorias;
            }

        }


        //public async Task<DataCategorias> GetPaginacionCategoria(Uri direccion)
        //{
        //    bool MostrarOcultos = false;
        //    var httpClientHandler = new HttpClientHandler();

        //    httpClientHandler.ServerCertificateCustomValidationCallback =
        //    (message, cert, chain, errors) => { return true; };

        //    clie = new HttpClient(httpClientHandler);

        //    //https://monolith-stage.herokuapp.com/api/v1/courses/all?page=1&withDisabled=false


        //    clie.DefaultRequestHeaders.Add("Accept", "application/json");
        //    clie.Timeout = TimeSpan.FromMinutes(2);

        //    //var filtrojson = JsonConvert.SerializeObject(filtro);
        //    //var contentJson = new StringContent(filtrojson, Encoding.UTF8, "application/json");
        //    var PaginacionCategorias = new DataCategorias();

            
        //    try
        //    {
        //        //HttpResponseMessage response = await clie.GetAsync("?" + direccion +"withDisabled" + "=" + Convert.ToString(MostrarOcultos).ToLower());
        //        HttpResponseMessage response = await clie.GetAsync(direccion);

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            //string contenido = await response.Content.ReadAsStringAsync();
        //            var resultado = JsonConvert.DeserializeObject<CategoriaResponseModelo>(await response.Content.ReadAsStringAsync());
        //            //var resultado = JsonConvert.DeserializeObject<PaginationResponse>(contenido);

        //            //ElementosCursos.AddRange(resultado.data.coleccion);
        //            PaginacionCategorias = resultado.data;

        //            return PaginacionCategorias;
        //        }
        //        else
        //        {
        //            await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de cursos", "OK");
        //            return PaginacionCategorias;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
        //        Console.WriteLine(ex.ToString());
        //        return PaginacionCategorias;
        //    }

        //}


    }
}
