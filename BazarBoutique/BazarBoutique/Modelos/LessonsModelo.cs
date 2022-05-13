using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Modelos
{
    public class LessonsResponseModelo : ApiResponse
    {
        public List<LessonsModelo> data { get; set; }
    }

    public class LessonsModelo
    {
        public int id { get; set; }
        public LessonsDetail detail { get; set; }
        public bool enabled { get; set; }
    }
    public class LessonsDetail
    {
        public string title { get; set; }
        public Uri url { get; set; }
        public LessonsDescription description { get; set; }
    }

    public class LessonsDescription
    {
        public string about { get; set; }
        public string videoDuration { get; set; }
        public Uri imageUrl { get; set; }
    }
}
