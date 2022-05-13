using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class CursoResponseModelo : ApiResponse
    {
        public DataCursos data { get; set; }
    }
    public class CursosPorUsuarioResponseModelo : ApiResponse
    {
        public List<DataCursosPorUsuario> data { get; set; }
    }

    public class DataCursosPorUsuario
    {
        public int id { get; set; }
        public DateTime purcharsed_at { get; set; }
        public CursosPorUsuarioDetalle course { get; set; }
        public Authors author { get; set; }
    }

    public class RegistrandoCursoUsuario
    {
        public int user_id { get; set; }
        public int course_id { get; set; }
    }


    public class CursoDetallesResponseModelo : ApiResponse
    {
        public CursosModelo data { get; set; }
    }

    public class DataCursos : PaginateData
    {
        public List<CursosModelo> courses { get; set; }
    }

    public class CursosPorUsuarioDetalle 
    {
        public int id { get; set; }
        public string title { get; set; }
        public Uri image { get; set; }
    }

    public class CursosModelo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string photo { get; set; }
        public string ImagenCurso { 
            get
            {
                if (string.IsNullOrWhiteSpace(photo) || photo == "TITLE.ico")
                {
                    return "not_image.jpg";
                }
                else
                {
                    return photo;
                }
            }
        }
        public DetailCursoModelo detail { get; set; }
        public CategoriaModelo category { get; set; }
        public int nro_lessons { get; set; }
        public int nro_students { get; set; }
        public bool enabled { get; set; }
        public DateTime updatedAt { get; set; }
    }


    public class DetailCursoModelo
    {
        public About about { get; set; }
        public Authors author { get; set; }
    }
    public class About
    {
        public string description { get; set; }
        public decimal price { get; set; }
    }

    public class Authors
    {
        public int id { get; set; }
        public string name{ get; set; }
        public string photo_url { get; set; }
        public string Foto_Author { 
            get
            {
                if (string.IsNullOrWhiteSpace(photo_url) || photo_url == "TITLE.ico")
                {
                    return "EmpyUser.png";
                }
                else
                {
                    return photo_url;
                }
            } 
        }
    }
}
