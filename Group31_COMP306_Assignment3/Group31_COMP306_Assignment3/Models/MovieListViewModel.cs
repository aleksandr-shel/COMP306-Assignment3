using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Models
{
    public class MovieListViewModel
    {
        public int UserId { get; set; }
        public List<S3Object> ListOfMoviesObject { get; set; }
        public Dictionary<String, double> RatingsDict { get; set; }

        public Dictionary<String, Movie> MoviesDict { get; set; }
    }
}
