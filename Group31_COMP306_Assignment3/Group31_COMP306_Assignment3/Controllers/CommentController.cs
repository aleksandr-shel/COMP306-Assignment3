using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class CommentController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> Add(string username, String movieTitle, String comment)
        {
            await dBOperations.CreateComment(movieTitle, username, comment);
            return RedirectToAction("Page", "Movie", new {key= movieTitle});
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string username, String movieTitle, String comment, string time)
        {
            if (loggedUser?.Username == username)
                await dBOperations.CreateComment(movieTitle, username, comment, time);

            return RedirectToAction("Page", "Movie", new { key = movieTitle });
        }
    }
}
