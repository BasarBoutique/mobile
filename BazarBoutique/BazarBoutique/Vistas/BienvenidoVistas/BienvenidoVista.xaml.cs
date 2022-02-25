using BazarBoutique.Vistas.InicioSesíonVistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.BienvenidoVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BienvenidoVista : ContentPage
    {
        public BienvenidoVista()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginVista());
        }
    }
}