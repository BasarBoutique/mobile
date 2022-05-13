using BazarBoutique.Modelos;
using BazarBoutique.VistaModelos.DetallesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.DetallesVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LexionesVista : ContentPage
    {
        LexionesViewModel vistamodel;
        public LexionesVista(int idCursoLexion)
        {
            InitializeComponent();
            BindingContext = vistamodel = new LexionesViewModel(Navigation, this, idCursoLexion);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}