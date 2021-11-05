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
                
                await dBOperations.CreateRating(movieTitle, userId, rate);
            }
            ViewData["username"] = loggedUser?.Username;
            return RedirectToAction("Page", "Movie", new { key = movieTitle });
        }

        [HttpGet]
        public async Task<Models.Rating> GetRating(int userId, string movieTitle)
        {
            if (userId == 0)
            {
                return null;
            }
            ViewData["username"] = loggedUser?.Username;
            return await dBOperations.GetRating(movieTitle, userId);
        }
    }
}
