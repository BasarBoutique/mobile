using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazarBoutique.Services.CategoriaServices
{
    public interface ICategoryService
    {
        Task<List<CategoriaSlideModelo>> GetCategoriaSlide();
        Task<DataCategorias> GetPaginacionCategoria(Uri direccion, SearchCourseFilters filtro);
    }
}