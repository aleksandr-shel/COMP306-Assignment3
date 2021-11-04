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
        public async Task<IActionResult> AddRating(int userId, string movieTitle, int rate)
        {
            if (userId != 0)
            {
                Temporary temporary = new Temporary();
                await temporary.CreateRating(movieTitle, userId, rate);
            }
            return RedirectToAction("Page", "Movie", new { key = movieTitle });
        }

        [HttpGet]
        public async Task<int> GetRating(int userId, string movieTitle)
        {
            return 2;
        }
    }
}
