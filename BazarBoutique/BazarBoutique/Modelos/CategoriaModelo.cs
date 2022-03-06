using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class CategoriaModelo
    {
        public int category_id { get; set; }
        public string category_tile{ get; set; }
        public string category_ico { get; set; }
        public bool is_enabled { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
