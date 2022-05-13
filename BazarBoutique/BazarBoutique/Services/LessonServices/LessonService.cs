using BazarBoutique.Modelos;
using BazarBoutique.Services.LessonServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LessonService))]
namespace BazarBoutique.Services.LessonServices
{
    public class LessonService : ILessonService
    {
        HttpClient cliente;
        public LessonService()
        {
        }

        public async Task<List<LessonsModelo>> GetLessonsForUser(int IdLexion)
        {
            cliente = new HttpClient();

            cliente.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/lesson/retrieveByCourse");
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionServicios.apiResponse.data.access_token);
            cliente.Timeout = TimeSpan.FromMinutes(2);
            //string body = @"{""withDisabled"":""true""}";

            ///////LessonsResponseModelo

            var LexionesDelCurso = new List<LessonsModelo>();

            try
            {
                var response = await cliente.GetAsync("?course=" + IdLexion);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<LessonsResponseModelo>(contenido);
                    LexionesDelCurso.AddRange(resultado.data);

                    return LexionesDelCurso;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido posible traer las lexiones", "OK");
                    return LexionesDelCurso;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al traer datos de las lexiones", "OK");
                Console.WriteLine(ex.ToString());
                return LexionesDelCurso;
            }
        }



        public async Task<bool> ComprobarSiEstaSuscrito(int IdLexion)
        {
            cliente = new HttpClient();

            cliente.BaseAddress = new Uri("https://monolith-stage.herokuapp.com/api/v1/lesson/retrieveByCourse");
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionServicios.apiResponse.data.access_token);
            cliente.Timeout = TimeSpan.FromMinutes(2);
            //string body = @"{""withDisabled"":""true""}";

            ///////LessonsResponseModelo

            bool ElUsuarioTieneElCurso = false;

            try
            {
                var response = await cliente.GetAsync("?course=" + IdLexion);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    
                    if (!string.IsNullOrEmpty(contenido))
                    {
                        var resultado = JsonConvert.DeserializeObject<LessonsResponseModelo>(contenido);
                        ElUsuarioTieneElCurso = true;
                    }
                    return ElUsuarioTieneElCurso;
                }
                else
                {
                    //await Application.Current.MainPage.DisplayAlert("BazarBoutique", "No ha sido validar si ha adquirido el curso", "OK");
                    return ElUsuarioTieneElCurso;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("BazarBoutique", "Error al comprobar la existencia del curso", "OK");
                Console.WriteLine(ex.ToString());
                return ElUsuarioTieneElCurso;
            }
        }












    }
}
