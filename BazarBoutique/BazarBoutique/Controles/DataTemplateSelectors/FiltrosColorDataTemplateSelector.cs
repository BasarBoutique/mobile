using System;
using BazarBoutique.Modelos;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Controles.DataTemplateSelectors
{
    public class FiltrosColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FiltrosDeCategorias { get; set; }
        public DataTemplate FiltrosDeAutores { get; set; }
        public DataTemplate FiltroNoAsignado { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (((FiltroPorNombreId)item).TipoFiltro)
            {
                case "categories":
                    return FiltrosDeCategorias;
                case "authors":
                    return FiltrosDeAutores;
                default:
                    return FiltroNoAsignado;
            }


            //if (((FiltroPorNombreId)item).TipoFiltro == "categories")
            //{

            //}else if ()
            //{

            //}
            //else
            //{

            //}
        }

    }
}
