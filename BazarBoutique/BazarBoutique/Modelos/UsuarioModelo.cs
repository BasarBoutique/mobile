using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class UsuarioResponseModelo
    {
        public UsuarioModelo data { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
    public class UsuarioModelo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }

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
