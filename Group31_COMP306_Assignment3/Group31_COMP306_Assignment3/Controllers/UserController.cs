using Group31_COMP306_Assignment3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class UserController : BaseController
    {
        private readonly COMP306LAB3Context _context;
        public UserController(COMP306LAB3Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["username"] = loggedUser?.Username;
            return RedirectToAction(nameof(Index), "Home");
        }
        public IActionResult Registration()
        {
            ViewData["username"] = loggedUser?.Username;
            return View();
        }

        public IActionResult Login()
        {
            ViewData["username"] = loggedUser?.Username;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User userLoggedIn = _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (userLoggedIn != null)
            {
                signedIn = true;
                loggedUser = userLoggedIn;
            }

            ViewData["username"] = loggedUser?.Username;
            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Logout()
        {
            signedIn = false;
            loggedUser = null;
            ViewData["username"] = loggedUser?.Username;
            return RedirectToAction(nameof(Index), "Home");
        }


        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                ViewData["username"] = loggedUser?.Username;
                return RedirectToAction(nameof(Index), "Home");
            }
            ViewData["username"] = loggedUser?.Username;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
