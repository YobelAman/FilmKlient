using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FilmKlient.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public TimeSpan Speltid { get; set; }
        public string Genre { get; set; }
        public int Aldergrans { get; set; }
        public string Beskrivning { get; set; }
        public DateTime Utgivningsdatum { get; set; }
        public string Sprak { get; set; }
        public string Filmbild { get; set; }
        public string Huvudregissor { get; set; }
        public HttpPostedFileBase File { get; set; }




    }
}