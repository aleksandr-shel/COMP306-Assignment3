using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Models
{
    [DynamoDBTable("Movies")]
    public class Movie
    {
        [DynamoDBHashKey]
        public string MovieTitle { get; set; }

        [DynamoDBRangeKey]
        public int UserId { get; set; }

        //public string Link { get; set; }

        public string Description{ get; set; }

        public string Director { get; set; }

        public Movie()
        {
        }

        public Movie(string movieTitle, int userId, string description, string director)
        {
            MovieTitle = movieTitle;
            UserId = userId;
            Description = description;
            Director = director;
        }
    }
}
