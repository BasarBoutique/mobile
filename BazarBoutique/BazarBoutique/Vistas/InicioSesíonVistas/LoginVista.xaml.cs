using BazarBoutique.VistaModelos.InicioSesionViewModels;
using BazarBoutique.Vistas.CatalogoCursosVistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.InicioSesíonVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginVista : ContentPage
    {
        public LoginVista()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation, this);
        }

        //private void BtnRegistrarse_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new RegistrarseVista());
        //}

        //private void LoguinButton_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new CatalogoCursosVista());
        //}
    }
}