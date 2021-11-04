using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Models
{
    [DynamoDBTable("Ratings")]
    public class Rating
    {
        [DynamoDBHashKey]
        public String MovieTitle { get; set; }

        [DynamoDBRangeKey]
        public int UserId { get; set; }

        public int Value { get; set; }

        public Rating()
        {

        }

        public Rating(string movieTitle, int userId, int value)
        {
            MovieTitle = movieTitle;
            UserId = userId;
            Value = value;
        }
    }
}
