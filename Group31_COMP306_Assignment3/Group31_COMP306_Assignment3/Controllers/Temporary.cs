using Group31_COMP306_Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class Temporary : DynamoDBOperations
    {
        public async Task CreateRating(string movieTitle, int userId, int value)
        {
            await CreateRatingsTable();
            Rating rating = new Rating(movieTitle, userId, value);
            await context.SaveAsync<Rating>(rating);
        }
    }
}
