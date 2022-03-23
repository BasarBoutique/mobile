using BazarBoutique.VistaModelos.CatalogoCursosViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.CatalogoCursosVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogoCursosVista : ContentPage
    {
        public CatalogoCursosViewModel vistamodelo;
        public CatalogoCursosVista()
        {
            InitializeComponent();
            BindingContext =  vistamodelo = new CatalogoCursosViewModel();

            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodelo.OnAppearing();
        }




    }
}