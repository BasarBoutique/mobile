using BazarBoutique.Modelos;
using BazarBoutique.VistaModelos.DetallesViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.DetallesVistas
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetallesCursoVista : ContentPage
    {
        DetallesCursoViewModel vistamodel;
        public DetallesCursoVista(CursosModelo curso)
        {
            InitializeComponent();
            BindingContext = vistamodel = new DetallesCursoViewModel(Navigation, this, curso);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}