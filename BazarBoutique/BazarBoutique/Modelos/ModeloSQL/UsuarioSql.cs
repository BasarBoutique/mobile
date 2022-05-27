using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos.ModeloSQL
{
    public class UsuarioSql
    {
        [PrimaryKey]
        public int id { get; set; }

        //public ApiResponseModelo ResponseApi { get; set; }
        //public UsuarioModelo ResponseUsuario { get; set; }

        public string Nombre { get; set; }
        public string Correo { get; set; }

        //public DetallesUsuario detail { get; set; }

        /// </detail>
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
        public string phone { get; set; }
        public string uuid { get; set; }



        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert)]
        public List<RolesUsuarios> roles { get; set; }


        public bool succes { get; set; }
        public string message { get; set; }

        public DateTime FechaExpiracion { get; set; }
        public string acces_token { get; set; }
        public string token_type { get; set; }

    }
}
