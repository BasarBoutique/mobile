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
    public partial class SoloFiltroCategoriaVista : ContentPage
    {
        SoloFiltroCategoriaViewModel vistamodel;
        public SoloFiltroCategoriaVista()
        {
            InitializeComponent();
            BindingContext = vistamodel = new SoloFiltroCategoriaViewModel(Navigation, this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vistamodel.OnDissapering();
        }
    }
}