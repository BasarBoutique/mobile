using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class UsuarioResponseModelo : ApiResponse
    {
        public UsuarioModelo data { get; set; }

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
        public Uri photo { get; set; }
        public string address { get; set; }
        public string phone{ get; set; }
        public string uuid { get; set; }
    }

    public class RolesUsuarios
    {

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
