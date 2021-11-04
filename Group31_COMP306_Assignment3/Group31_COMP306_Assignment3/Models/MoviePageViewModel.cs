using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Models
{
    public class MoviePageViewModel
    {
        public string MovieTitle { get; set; }
        public int UserId { get; set; }

        public List<Comment> Comments { get; set; }
        public MoviePageViewModel(string movieTitle, int userId)
        {
            MovieTitle = movieTitle;
            UserId = userId;
        }

        public MoviePageViewModel(string movieTitle, int userId, List<Comment> comments) : this(movieTitle, userId)
        {
            Comments = comments;
        }
    }
}
