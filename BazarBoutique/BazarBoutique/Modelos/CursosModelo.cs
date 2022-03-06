using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class CursosModelo
    {
        public int course_id { get; set; }
        public string course_title { get; set; }
        public string course_photo { get; set; }
        public bool is_enabled { get; set; }
        public DateTime created_at { get; set; }
    }
}
