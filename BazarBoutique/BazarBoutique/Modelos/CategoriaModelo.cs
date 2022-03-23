using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{

    public class CategoriaResponseModelo
    {
        public List<CategoriaModelo>  data { get; set; }
        public string message { get; set;}
        public bool success { get; set; }
    }

    public class CategoriaModelo
    {
        public int id { get; set; }
        public string category{ get; set; }
        public string photo { get; set; }
        public string PhotoCategory
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


        //public int category_id { get; set; }
        //public string category_tile{ get; set; }
        //public string category_ico { get; set; }
        //public bool is_enabled { get; set; }
        //public DateTime created_at { get; set; }
        //public DateTime updated_at { get; set; }


        public string ColorRandom1
        {
            get {
                var random = new Random();
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                return color; }
        }
    }
}
