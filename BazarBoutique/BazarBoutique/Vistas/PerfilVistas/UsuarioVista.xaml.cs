using BazarBoutique.Services;
using BazarBoutique.VistaModelos.PerfilViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.PerfilVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsuarioVista : ContentPage
    {
        UsuarioViewModel vistamodel;
        public UsuarioVista()
        {
            InitializeComponent();
            BindingContext = vistamodel = new UsuarioViewModel(Navigation, this);


            ImagenPerfil.Source = "user.png";

            if (SesionServicios.apiResponse.success == true)
            {
                PrimerNombrelbl.Text = SesionServicios.apiUser.name;
                Correolbl.Text = SesionServicios.apiUser.email;
                ImagenPerfil.Source = SesionServicios.apiUser.detail.PhotoUser;

                if (String.IsNullOrEmpty(SesionServicios.apiUser.detail.phone))
                {
                    Telefonolbl.Text = "-";
                }
                if (string.IsNullOrEmpty(Paislbl.Text))
                {
                    Paislbl.Text = "-";
                }
                //Telefonolbl.Text = SesionServicios.apiUser.detail.phone;
                 //= SesionServicios.apiUser.detail.address;

            }
            if (!string.IsNullOrEmpty(SesionServicios.UsuarioGoogle.Name))
            {
                PrimerNombrelbl.Text = SesionServicios.UsuarioGoogle.Name;
                Correolbl.Text = SesionServicios.UsuarioGoogle.Email;
                ImagenPerfil.Source = SesionServicios.UsuarioGoogle.Picture;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vistamodel.OnAppearing();
        }
    }
}