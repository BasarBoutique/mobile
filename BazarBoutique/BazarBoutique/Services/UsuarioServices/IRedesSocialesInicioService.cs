using BazarBoutique.Modelos;
using System.Threading.Tasks;

namespace BazarBoutique.Services.UsuarioServices
{
    public interface IRedesSocialesInicioService
    {
        Task<bool> AutenticacionGoogle(GoogleUser user);
    }
}