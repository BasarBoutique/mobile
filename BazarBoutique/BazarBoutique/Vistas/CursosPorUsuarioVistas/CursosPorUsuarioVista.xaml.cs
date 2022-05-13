using BazarBoutique.VistaModelos.CursosPorUsuarioViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.CursosPorUsuarioVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CursosPorUsuarioVista : ContentPage
    {
        CursosPorUsuarioViewModel vistamodel;
        public CursosPorUsuarioVista()
        {
            InitializeComponent();
            BindingContext = vistamodel = new CursosPorUsuarioViewModel(Navigation, this);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}