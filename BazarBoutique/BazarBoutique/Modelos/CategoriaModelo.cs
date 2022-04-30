using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class CategoriaSlideResponseModelo : ApiResponse
    {
        public List<CategoriaSlideModelo>  data { get; set; }

    }

    public class CategoriaSlideModelo
    {
        public int id { get; set; }
        public string category{ get; set; }
        public string photo { get; set; }
        public int user { get; set; }
        public DateTime date { get; set; }
        public bool enabled { get; set; }

        public bool IsNew
        {
            get 
            {
                var dww = (DateTime.Now - date).Days;
                if (dww < 32)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsMoreElement { get; set; }
        //Campos creados para especificamente para el sistema
        public string PhotoCategory
        {
            get
            {
                //if (string.IsNullOrWhiteSpace(photo) || photo == "TITLE.ico")
                //{
                //    return "not_image.jpg";
                //}
                //else
                //{
                return photo;
                //}
            }
        }

        public string ColorRandom1
        {
            get {
                var random = new Random();
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                return color; }
        }
    }
        public class CategoriaResponseModelo : ApiResponse
        {
            public DataCategorias data { get; set; }
        }

        public class DataCategorias
        {
            public List<CategoriaModelo> categories { get; set; }
            public PaginationModel pagination { get; set; }
        }

        public class CategoriaModelo
        {
            public int id { get; set; }
            public string title { get; set; }

            [JsonProperty(propertyName: "photo-url")]
            public Uri photo { get; set; }
            public bool enabled { get; set; }
        }

}
