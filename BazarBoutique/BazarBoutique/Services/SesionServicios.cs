using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Services
{
    public static class SesionServicios
    {
        public static ApiResponseModelo apiResponse = new ApiResponseModelo();

        public static GoogleUser UsuarioGoogle = new GoogleUser();

        public static UsuarioModelo apiUser = new UsuarioModelo();

        //public IGoogleManager AutenticacionGoogle = DependencyService.Get<IGoogleManager>();
    }

   
}
