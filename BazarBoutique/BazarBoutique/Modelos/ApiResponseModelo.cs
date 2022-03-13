using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class ApiResponseModelo
    {
        public DataModelo data { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
    public class DataModelo
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public DateTime expire_at { get; set; }
    }
}
