using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class SearchCoursesModeloResponse : ApiResponse
    {
        public SearchCoursesModelo data { get; set; }
    }

    public class SearchCoursesModelo
    {
        public List<CursosModelo> courses { get; set; }
        public FiltersModelo filters { get; set; }
        public PaginationModel pagination { get; set; }
    }

    
}
