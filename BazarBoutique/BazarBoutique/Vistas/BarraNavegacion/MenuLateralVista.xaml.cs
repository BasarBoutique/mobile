using BazarBoutique.Modelos;
using BazarBoutique.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.BarraNavegacion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuLateralVista : FlyoutPage
    {
        private readonly IGoogleManager AdministradorGoogle;

        public MenuLateralVista()
        {
            InitializeComponent();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
            AdministradorGoogle = DependencyService.Get<IGoogleManager>();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuLateralVistaFlyoutMenuItem;
            if (item == null)
                return;
            if (item.Title == "Cerrar Sesion")
            {

                if (!string.IsNullOrEmpty(SesionServicios.UsuarioGoogle.IdToken))
                {
                    GoogleUser Usuario = new GoogleUser();
                    SesionServicios.UsuarioGoogle = Usuario;
                    AdministradorGoogle.Logout();
                }
                if (SesionServicios.apiResponse.success == true)
                {
                    ApiResponseModelo Usuario = new ApiResponseModelo();
                    UsuarioModelo DatosPersonales = new UsuarioModelo();

                    SesionServicios.apiResponse = Usuario;
                    SesionServicios.apiUser = DatosPersonales;
                }

                Navigation.InsertPageBefore(new MenuLateralVista(), this);
                FlyoutPage.ListView.SelectedItem = null;
                await Navigation.PopAsync().ConfigureAwait(false);
            }
            else
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                //Este codigo mostrara los titulos de las pagina automaticamente
                //page.Title = item.Title;
                Detail = new NavigationPage(page);
                IsPresented = false;

                FlyoutPage.ListView.SelectedItem = null;
            }

        }
    }
}