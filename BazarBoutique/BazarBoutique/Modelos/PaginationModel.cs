using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Modelos
{
    //public class PaginateData:IData<CursosModelo>
    //{
    //    public List<CursosModelo> coleccion { get; set; }
    //    public PaginationModel paginate { get; set; }
    //}

    //public class PaginationResponse : ApiResponse
    //{
    //    public PaginateData data { get; set; }
    //}

    //public class PaginateData
    //{
    //    public PaginationModel paginate { get; set; }
    //    public List<CoreModel> lis { get; set; }
    //}


    //
    public interface IData<T>
    {
        List<T> coleccion { get; set;}
    }

    public class PaginaRedireccion 
    {
        public int NumeroPagina { get; set; }
        public Uri LinkPagina { get; set; }
        public bool PaginaSeleccionada { get; set; }
    }

    public class PaginateData
    {
        public PaginationModel pagination { get; set; }
        public FiltrosModelo filters { get; set; }
    }

    public class SearchCourseFilters
    {
        public FiltrosModelo filters { get; set; }
    }

    public class PaginationModel
    {
        public Uri first_page { get; set; }
        public Uri next_page { get; set; }
        public Uri current_page { get; set; }
        public int total { get; set; }
    }

    public class FiltrosModelo
    {
        public string title { get; set; }
        public List<int> categories { get; set; }
        public List<int> authors { get; set; }
    }

}
