﻿

using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazarBoutique.Services.CursoServices
{
    public interface ICursoService
    {
        Task<List<CursosModelo>> GetCurso(bool withDisabled);
        Task<DataCursos> GetPaginationCurso(Uri direccion, SearchCourseFilters filtro);
        Task<CursosModelo> GetCursoDetail(CursosModelo Curso);
        Task<List<DataCursosPorUsuario>> GetCursosUsuario(string Token);
        Task RegistrandoCursos(int idcurso, int idusuario);
    }
}