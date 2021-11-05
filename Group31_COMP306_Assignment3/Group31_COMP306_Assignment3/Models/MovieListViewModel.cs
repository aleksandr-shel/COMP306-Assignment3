using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Models
{
    public class MovieListViewModel
    {
        public List<S3Object> ListOfMovies { get; set; }
        public Dictionary<String, double> RatingsDict { get; set; }
    }
}
