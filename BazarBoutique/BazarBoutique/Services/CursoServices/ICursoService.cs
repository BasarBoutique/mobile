

using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazarBoutique.Services.CursoServices
{
    public interface ICursoService
    {
        Task<List<CursosModelo>> GetCurso(bool withDisabled);
        Task<DataCursos> GetPaginationCurso(SearchCourseFilters filtro, Uri direccion);
    }
}