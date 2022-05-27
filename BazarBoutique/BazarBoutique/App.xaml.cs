using BazarBoutique.Data;
using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Vistas.BarraNavegacion;
using BazarBoutique.Vistas.BienvenidoVistas;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique
{

    public partial class App : Application
    {
        static InformacionSQLite BD;
        public App()
        {
            InitializeComponent();

            var users = SQLiteDB.RecivirUsuarioAsync().Result;

            if (users != null && users.Count > 0)
            {
                var UsuarioPrincipal = users[0];

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(UsuarioPrincipal.acces_token);
                if (jwtSecurityToken != null && jwtSecurityToken.ValidTo < DateTime.Now)
                {
                    var result = SQLiteDB.DeleteAllAsync().Result;
                    MainPage = new NavigationPage(new MenuLateralVista());
                }
                else
                {
                    //var result = SQLiteDB.DeleteAllAsync().Result;
                    //SesionServicios.apiResponse = UsuarioPrincipal.ResponseApi;
                    //SesionServicios.apiUser = UsuarioPrincipal.ResponseUsuario;
                    //SesionServicios.apiResponse = 
                    SesionServicios.apiResponse = new ApiResponseModelo
                    {
                        data = new DataModelo
                        {
                            token_type = UsuarioPrincipal.token_type,
                            access_token = UsuarioPrincipal.acces_token,
                            expire_at = UsuarioPrincipal.FechaExpiracion
                        },
                        message = UsuarioPrincipal.message,
                        success = UsuarioPrincipal.succes
                    };

                    SesionServicios.apiUser = new UsuarioModelo
                    {
                        id = UsuarioPrincipal.id,
                        name = UsuarioPrincipal.Nombre,
                        email = UsuarioPrincipal.Correo,
                        roles = UsuarioPrincipal.roles,
                        detail = new DetallesUsuario
                        {
                            address = UsuarioPrincipal.address,
                            fullname = UsuarioPrincipal.fullname,
                            phone = UsuarioPrincipal.phone,
                            photo = UsuarioPrincipal.photo,
                            uuid = UsuarioPrincipal.uuid
                        }
                    };
                    
                    //SesionServicios.apiResponse.data.token_type = UsuarioPrincipal.token_type;
                    //SesionServicios.apiResponse.data.access_token = UsuarioPrincipal.acces_token;
                    //SesionServicios.apiResponse.data.expire_at = UsuarioPrincipal.FechaExpiracion;

                    //SesionServicios.apiUser.id = UsuarioPrincipal.id;
                    //SesionServicios.apiUser.name = UsuarioPrincipal.Nombre;
                    //SesionServicios.apiUser.email = UsuarioPrincipal.Correo;

                    //SesionServicios.apiUser.detail.photo = UsuarioPrincipal.photo;
                    //SesionServicios.apiUser.detail.fullname = UsuarioPrincipal.fullname;
                    //SesionServicios.apiUser.detail.address = UsuarioPrincipal.address;
                    //SesionServicios.apiUser.detail.phone = UsuarioPrincipal.phone;
                    //SesionServicios.apiUser.detail.uuid = UsuarioPrincipal.uuid;



                    //SesionServicios.apiUser.roles = UsuarioPrincipal.roles;

                    //SesionServicios.apiUser 

                    MainPage = new NavigationPage(new MenuLateralVista());
                }
            }
            else
            {
                MainPage = new NavigationPage(new BienvenidoVista());
            }



            /////
            //var users = SQLiteDB.RecivirUsuarioAsync().Result;

            //MainPage = new NavigationPage(new BienvenidoVista());
            //{
            //    BarBackgroundColor = Color.White,
            //};
        }

        public static InformacionSQLite SQLiteDB
        {
            get
            {
                if (BD == null)
                {
                    BD = new InformacionSQLite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BazarBoutiqueLocal.db3"));
                }

                return BD;
            }
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
