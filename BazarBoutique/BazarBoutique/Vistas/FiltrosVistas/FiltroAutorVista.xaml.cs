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
    public partial class FiltroAutorVista : ContentPage
    {
        FiltroAutoresViewModel vistamodel;
        public FiltroAutorVista()
        {
            InitializeComponent();
            BindingContext = vistamodel = new FiltroAutoresViewModel(Navigation, this);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }


    }
}