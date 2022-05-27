using BazarBoutique.Services;
using BazarBoutique.Vistas.CatalogoCursosVistas;
using BazarBoutique.Vistas.CursosPorUsuarioVistas;
using BazarBoutique.Vistas.InicioSesíonVistas;
using BazarBoutique.Vistas.PerfilVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.BarraNavegacion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuLateralVistaFlyout : ContentPage
    {
        public ListView ListView;

        public MenuLateralVistaFlyout()
        {
            InitializeComponent();

            BindingContext = new MenuLateralVistaFlyoutViewModel();
            ListView = MenuItemsListView;
            if (!string.IsNullOrEmpty(SesionServicios.UsuarioGoogle.IdToken))
            {
                NombreUsuario.Text = Services.SesionServicios.UsuarioGoogle.Name;
            }
            if (SesionServicios.apiResponse.success == true)
            {
                NombreUsuario.Text = SesionServicios.apiUser.name;
                Correolbl.Text = SesionServicios.apiUser.email;
                FotoPerfil.Source = SesionServicios.apiUser.detail.PhotoUser;
            }

        }

        private class MenuLateralVistaFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuLateralVistaFlyoutMenuItem> MenuItems { get; set; }

            public MenuLateralVistaFlyoutViewModel()
            {
                if (!string.IsNullOrEmpty(SesionServicios.UsuarioGoogle.IdToken) || SesionServicios.apiResponse.success == true) 
                {
                    MenuItems = new ObservableCollection<MenuLateralVistaFlyoutMenuItem>(new[]
                    {
                        new MenuLateralVistaFlyoutMenuItem { Id = 0, Title = "Listado de Cursos",TargetType= typeof(CatalogoCursosVista)},
                        new MenuLateralVistaFlyoutMenuItem { Id = 1, Title = "Perfil",TargetType= typeof(UsuarioVista)},
                        new MenuLateralVistaFlyoutMenuItem { Id = 2, Title = "Mis Cursos", TargetType= typeof(CursosPorUsuarioVista)},
                        new MenuLateralVistaFlyoutMenuItem { Id = 3, Title = "Cerrar Sesion",TargetType= typeof(FlyoutPage)}
                    });
                }
                else
                {
                    MenuItems = new ObservableCollection<MenuLateralVistaFlyoutMenuItem>(new[]
{
                        new MenuLateralVistaFlyoutMenuItem { Id = 0, Title = "Listado de Cursos",TargetType= typeof(CatalogoCursosVista)},
                        new MenuLateralVistaFlyoutMenuItem { Id = 1, Title = "Inicio Sesión",TargetType= typeof(LoginVista)},
                        new MenuLateralVistaFlyoutMenuItem { Id = 2, Title = "Registrarse",TargetType= typeof(RegistrarseVista)},
                    });
                }


            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}