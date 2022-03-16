using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.InicioSesionViewModels
{
    public class RegisterViewModel: BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;
        public bool IsRegistrable { get; set; }
        public string Nombre{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Command btnRegistrarseCommand { get; set; }

        public RegisterViewModel(INavigation navigation, ContentPage page)
        {
            IsRegistrable = false;

        }
    }
}
