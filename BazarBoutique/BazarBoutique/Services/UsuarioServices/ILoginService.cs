using BazarBoutique.Modelos;
using System.Threading.Tasks;

namespace BazarBoutique.Services.UsuarioServices
{
    public interface ILoginService
    {
        Task<bool> IniciarSesion(LoginModelo user);
    }
}