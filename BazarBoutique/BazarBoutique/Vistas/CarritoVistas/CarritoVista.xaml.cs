using BazarBoutique.VistaModelos.CarritoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.CarritoVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarritoVista : ContentPage
    {
        CarritoViewModel vistamodel;
        public CarritoVista()
        {
            InitializeComponent();
            BindingContext = vistamodel = new CarritoViewModel(Navigation, this);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}