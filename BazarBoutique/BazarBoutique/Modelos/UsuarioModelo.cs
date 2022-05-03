using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class UsuarioResponseModelo : ApiResponse
    {
        public UsuarioModelo data { get; set; }
    }

    public class UsuarioSlideResponseModelo : ApiResponse
    {
        public List<UsuarioModelo> data { get; set; }

    }

    //Creo que se puede simplificar si solo hacemos que DataUsuarios herede de Api Response
    public class UsuarioPaginacionResponseModelo : ApiResponse
    {
        public DataUsuario data { get; set; }
    }

    public class DataUsuario
    {
        public List<UsuarioModelo> users { get; set; }
        public FiltrosModelo filters { get; set; }
        public PaginationModel paginate { get; set; }
    }


    public class UsuarioModelo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DetallesUsuario detail { get; set; }
        public List<RolesUsuarios> roles { get; set; }
    }

    public class DetallesUsuario
    {
        public string fullname { get; set; }
        public string photo { get; set; }
        public string PhotoUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(photo) || photo == "TITLE.ico")
                {
                    return "not_image.jpg";
                }
                else
                {
                    return photo;
                }
            }
        }
        public string address { get; set; }
        public string phone{ get; set; }
        public string uuid { get; set; }
    }

    public class RolesUsuarios
    {
        public int permission_level { get; set; }
        public string permission_name { get; set; }

    }


    //public class LoginResponse
    //{
    //    public string BearerToken { get; set; }

    //    public bool IsAuthenticated { get; set; }

    //    public UsuarioResponse User { get; set; }
    //}

    //public class UsuarioResponse
    //{
    //    public Guid Id { get; set; }

    //    public string Username { get; set; }

    //    public string Password { get; set; }

    //    public string FirstName { get; set; }

    //    public string LastName { get; set; }

    //    public string Avatar { get; set; }
    //}

}
