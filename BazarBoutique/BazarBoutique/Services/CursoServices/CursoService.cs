using BazarBoutique.Modelos;
using BazarBoutique.Services.CursoServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CursoService))]
namespace BazarBoutique.Services.CursoServices
{
    public class CursoService: ICursoService
    {
        HttpClient cliente;
        public CursoService()
        {
            //cliente = new HttpClient();
        }

        public async Task<DataCursos> GetPaginationCurso(Uri direccion, SearchCourseFilters filtro)
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            cliente = new HttpClient(httpClientHandler);

            //clie.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search");

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var filtrojson = JsonConvert.SerializeObject(filtro);
            var contentJson = new StringContent(filtrojson, Encoding.UTF8, "application/json");
            var PaginacionCurso = new DataCursos();

            try
            {
                //HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());
                var response = await cliente.PostAsync(direccion, contentJson);
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

        public async Task<CursosModelo> GetCursoDetail(CursosModelo Curso)
        {
            cliente = new HttpClient();

            cliente.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/detail/");
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var CursoDetallesCompletos = new CursosModelo();

            try
            {
                HttpResponseMessage response = await cliente.GetAsync(Curso.id.ToString());
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var CursoResponse = JsonConvert.DeserializeObject<CursoDetallesResponseModelo>(contenido);

                    CursoDetallesCompletos = CursoResponse.data;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos del curso " + Curso.title, "OK");
                }

                return CursoDetallesCompletos;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer los datos del curso " + Curso.title, "OK");
                return CursoDetallesCompletos;
            }

        }

        public async Task<List<DataCursosPorUsuario>> GetCursosUsuario(string Token)
        {
            cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            Uri link = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/user");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var ElementosCursos = new List<DataCursosPorUsuario>();
            try
            {
                HttpResponseMessage response = await cliente.GetAsync(link);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CursosPorUsuarioResponseModelo>(contenido);
                    //var resultado = JsonConvert.DeserializeObject<PaginationResponse>(contenido);

                    ElementosCursos.AddRange(resultado.data);

                    return ElementosCursos;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de sus cursos", "OK");
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




        public async Task<List<CursosModelo>> GetCurso(bool withDisabled)
        {
            string parametro = "withDisabled";
            cliente = new HttpClient();

            cliente.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/all");
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var ElementosCursos = new List<CursosModelo>();

            try
            {
                HttpResponseMessage response = await cliente.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());

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

        public async Task RegistrandoCursos(int idcurso, int idusuario)
        {
            RegistrandoCursoUsuario Registro = new RegistrandoCursoUsuario(){ 
                course_id = idcurso, 
                user_id = idusuario
            };

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            cliente = new HttpClient(httpClientHandler);

            var direccion = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/attachUser");

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            cliente.Timeout = TimeSpan.FromMinutes(2);

            var filtrojson = JsonConvert.SerializeObject(Registro);
            var contentJson = new StringContent(filtrojson, Encoding.UTF8, "application/json");
            var PaginacionCurso = new DataCursos();

            try
            {
                //HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());
                var response = await cliente.PostAsync(direccion, contentJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<CursoResponseModelo>(await response.Content.ReadAsStringAsync());


                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer los datos de cursos", "OK");
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos", "OK");
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
