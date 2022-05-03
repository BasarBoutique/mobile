using BazarBoutique.Modelos;
using BazarBoutique.Services.CursoServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CursoService))]
namespace BazarBoutique.Services.CursoServices
{
    public class CursoService: ICursoService
    {
        HttpClient clie;
        public CursoService()
        {

        }

        public async Task<DataCursos> GetPaginationCurso(Uri direccion, SearchCourseFilters filtro)
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            clie = new HttpClient(httpClientHandler);

            //clie.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search");

            clie.DefaultRequestHeaders.Add("Accept", "application/json");
            clie.Timeout = TimeSpan.FromMinutes(2);

            var filtrojson = JsonConvert.SerializeObject(filtro);
            var contentJson = new StringContent(filtrojson, Encoding.UTF8, "application/json");
            var PaginacionCurso = new DataCursos();

            try
            {
                //HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());
                var response = await clie.PostAsync(direccion, contentJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CursoResponseModelo>(await response.Content.ReadAsStringAsync());
                    //var resultado = JsonConvert.DeserializeObject<PaginationResponse>(contenido);

                    //ElementosCursos.AddRange(resultado.data.coleccion);
                    PaginacionCurso = resultado.data;

                    return PaginacionCurso;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de cursos", "OK");
                    return PaginacionCurso;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                Console.WriteLine(ex.ToString());
                return PaginacionCurso;
            }
        }

        public async Task<List<CursosModelo>> GetCurso(bool withDisabled)
        {
            string parametro = "withDisabled";
            clie = new HttpClient();

            clie.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/all");
            clie.DefaultRequestHeaders.Add("Accept", "application/json");
            clie.Timeout = TimeSpan.FromMinutes(2);

            var ElementosCursos = new List<CursosModelo>();

            try
            {
                HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CursoResponseModelo>(contenido);
                    //var resultado = JsonConvert.DeserializeObject<PaginationResponse>(contenido);

                    ElementosCursos.AddRange(resultado.data.courses);

                    return ElementosCursos;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de cursos", "OK");
                    return ElementosCursos;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                Console.WriteLine(ex.ToString());
                return ElementosCursos;
            }
        }
    }
}
