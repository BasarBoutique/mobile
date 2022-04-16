using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.DetallesViewModels
{
    public class DetallesCursoViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;
        public DetallesCursoViewModel(INavigation navigation, ContentPage page)
        {
            this.navigation = navigation;
            this.page = page;
        }
    }
}
