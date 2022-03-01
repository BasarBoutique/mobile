using BazarBoutique.Vistas.BienvenidoVistas;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique
{

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new BienvenidoVista());
            //{
            //    BarBackgroundColor = Color.White,
            //};
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
