using BazarBoutique.Vistas.CarritoVistas;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.PerfilViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;

        public Command RedireccionCarritoCommand { get; }
        public UsuarioViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            RedireccionCarritoCommand = new Command(RedireccionACarritoPagina);
        }
        public void RedireccionACarritoPagina()
        {
            navigation.PushAsync(new CarritoVista());
        }

        public void OnAppearing()
        {
            VerificandoUsuario();
        }

    }
}
