using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Modelos
{

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
        public Order order { get; set; }
        public int paginate { get; set; }
    }

    public class Order
    {
        public string sort_by { get; set; }
        public string order_by { get; set; } 
    }

    public class PaginationModel
    {
        public Uri first_page { get; set; }
        public Uri next_page { get; set; }
        public int total { get; set; }
        public Uri current_page { get; set; }
    }

    //Este modelo sirve para mostrar y agregar la lista de id
    public class FiltroPorNombreId
    {
        public int CodigoFiltro { get; set; }
        public string TipoFiltro { get; set; }
        public string NombreFiltro { get; set; }
    }


    public class FiltrosModelo
    {
        public string title { get; set; }
        //public string name { get; set; }
        public List<int> categories { get; set; }
        public List<int> authors { get; set; }
        ////
        public bool withDisabled { get; set; }
        public List<int> roles { get; set; }
    }

}
