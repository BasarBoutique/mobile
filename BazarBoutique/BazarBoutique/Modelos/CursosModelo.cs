using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class CursoResponseModelo : ApiResponse
    {
        public DataCursos data { get; set; }
    }

    public class DataCursos : PaginateData
    {
        public List<CursosModelo> courses { get; set; }
    }

    public class CursosModelo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string photo { get; set; }
        public DetailCursoModelo detail { get; set; }
        public bool enabled { get; set; }
    }


    public class DetailCursoModelo
    {
        public string about { get; set; }
    }
}
