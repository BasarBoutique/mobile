using BazarBoutique.Modelos;
using BazarBoutique.Services.UsuarioServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(RedesSocialesInicioService))]
namespace BazarBoutique.Services.UsuarioServices
{
    public class RedesSocialesInicioService : IRedesSocialesInicioService
    {
        HttpClient clie;
        public RedesSocialesInicioService()
        {
        }

        public async Task<bool> AutenticacionGoogle(GoogleUser user)
        {

            return false;
        }
    }
}
