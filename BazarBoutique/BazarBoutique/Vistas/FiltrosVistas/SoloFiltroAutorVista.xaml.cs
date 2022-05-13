using BazarBoutique.VistaModelos.FiltrosViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.FiltrosVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoloFiltroAutorVista : ContentPage
    {
        SoloFiltroAutoresViewModel vistamodel;
        public SoloFiltroAutorVista()
        {
            InitializeComponent();
            BindingContext = vistamodel = new SoloFiltroAutoresViewModel(Navigation, this);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}