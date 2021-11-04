using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class RatingController : BaseController
    {
        [HttpPost]
        public async Task AddRating(int userId, string movieTitle, int rate)
        {
            Temporary temporary = new Temporary();
            await temporary.CreateRating(movieTitle, userId, rate);
            RedirectToAction("Page", "Movie", new { key = movieTitle });
        }
    }
}
