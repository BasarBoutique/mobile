using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class GoogleUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }
        public string FamilyName { get; set; }
        public string IdToken { get; set; }
        public string Id { get; set; }
        public string ServerAuthCode { get; set; }
    }


    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);
        void Logout();
    }

}
