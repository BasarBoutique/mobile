using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Controles.DataTemplateSelectors
{
    public class CateogirasDataTemplate : DataTemplateSelector
    {
        //Instanciando Los diseños
        public DataTemplate ElementoVerMas { get; set; }
        public DataTemplate ElementoNormal { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((CategoriaSlideModelo)item).IsMoreElement)
            {
                return ElementoVerMas;
            }
            else
            {
                return ElementoNormal;
            }
        }
    }
}
