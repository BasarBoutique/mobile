using BazarBoutique.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazarBoutique.Services.CategoriaServices
{
    public interface ICategoryService
    {
        Task<List<CategoriaModelo>> GetCategoriaSlide();
    }
}