using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Controles.DataTemplateSelectors
{
    public class PaginaSeleccionadaDataTemplate : DataTemplateSelector
    {
        public DataTemplate PaginaNoSeleccionada { get; set; }
        public DataTemplate PaginaSeleccionada { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((PaginaRedireccion)item).PaginaSeleccionada)
            {
                return PaginaSeleccionada;
            }
            else
            {
                return PaginaNoSeleccionada;
            }
        }

    }
}
