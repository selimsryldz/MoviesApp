using MoviesProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesProject.Models
{
    public class DetayModel
    {
        public Movies movie { get; set; }
        public List<Comments> comments { get; set; }
        public String eklenen_Yorum { get; set; }
        public int movieId { get; set; }
    }
}