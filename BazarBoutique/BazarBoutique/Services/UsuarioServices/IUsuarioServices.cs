using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazarBoutique.Services.UsuarioServices
{
    public interface IUsuarioServices
    {
        Task<DataUsuario> GetPaginacionUsuario(Uri direccion, SearchCourseFilters filtro);
        Task<List<UsuarioModelo>> GetUsuarioSlide();
    }
}