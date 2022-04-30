using BazarBoutique.Services;
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
        public UsuarioVista()
        {
            InitializeComponent();

            ImagenPerfil.Source = "user.png";

            if (SesionServicios.apiResponse.success == true)
            {
                PrimerNombrelbl.Text = SesionServicios.apiUser.name;
                Correolbl.Text = SesionServicios.apiUser.email;
                ImagenPerfil.Source = SesionServicios.apiUser.detail.photo;
                

            }
            if (!string.IsNullOrEmpty(SesionServicios.UsuarioGoogle.Name))
            {
                PrimerNombrelbl.Text = SesionServicios.UsuarioGoogle.Name;
                Correolbl.Text = SesionServicios.UsuarioGoogle.Email;
                ImagenPerfil.Source = SesionServicios.UsuarioGoogle.Picture;
            }
        }
    }
}