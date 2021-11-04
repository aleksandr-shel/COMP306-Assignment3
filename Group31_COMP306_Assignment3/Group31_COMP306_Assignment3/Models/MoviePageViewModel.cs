using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Models
{
    public class MoviePageViewModel
    {
        public string MovieTitle { get; set; }
        public string Username { get; set; }

        public List<Comment> Comments { get; set; }

        public int UserId { get; set; }

        public MoviePageViewModel(string movieTitle, string username)
        {
            MovieTitle = movieTitle;
            Username = username;
        }

        public MoviePageViewModel(string movieTitle, string username, List<Comment> comments) : this(movieTitle, username)
        {
            Comments = comments;
        }

        public MoviePageViewModel(string movieTitle, string username, List<Comment> comments, int userId)
        {
            MovieTitle = movieTitle;
            Username = username;
            Comments = comments;
            UserId = userId;
        }
    }
}
