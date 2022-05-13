using BazarBoutique.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazarBoutique.Services.LessonServices
{
    public interface ILessonService
    {
        Task<List<LessonsModelo>> GetLessonsForUser(int IdLexion);
        Task<bool> ComprobarSiEstaSuscrito(int IdLexion);
    }
}