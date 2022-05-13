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
    public partial class PagandoConTarjetaVista : ContentPage
    {
        PagandoConTarjetaViewModel vistamodel;
        public PagandoConTarjetaVista(decimal TotalPrecio)
        {
            InitializeComponent();
            BindingContext = vistamodel = new PagandoConTarjetaViewModel(Navigation, this, TotalPrecio);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}