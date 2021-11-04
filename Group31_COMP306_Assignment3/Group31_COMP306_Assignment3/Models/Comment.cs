using Amazon.DynamoDBv2.DataModel;
using System;

namespace Group31_COMP306_Assignment3.Models
{
    [DynamoDBTable("Comments")]
    public class Comment
    {
        [DynamoDBHashKey]
        public string MovieTitle { get; set; }

        [DynamoDBRangeKey]
        public string Time { get; set; }

        public string Username{ get; set; }
        public string Content { get; set; }

        public Comment()
        {
        }

        public Comment(string movieTitle, string userName, string content)
        {
            MovieTitle = movieTitle;
            Time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            Username = userName;
            Content = content;
        }
    }
}
