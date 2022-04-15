using BazarBoutique.Modelos;
using System.Threading.Tasks;

namespace BazarBoutique.Services.UsuarioServices
{
    public interface IRegisterService
    {
        Task<bool> RegistrarseUsuario(RegisterModelo user);
    }
}