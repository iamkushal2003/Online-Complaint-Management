using Microsoft.AspNetCore.Mvc;
using ComplaintSystem.Data;
using ComplaintSystem.Models;
using ComplaintSystem.Utilities;
using System.Linq;

namespace ComplaintSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User model, string password)
        {
            if(!ModelState.IsValid) return View(model);

            if(_db.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email already registered");
                return View(model);
            }

            model.PasswordHash = PasswordHelper.HashPassword(password);
            model.Role = "User";
            _db.Users.Add(model);
            _db.SaveChanges();

            // set session
            HttpContext.Session.SetInt32("UserId", model.UserId);
            HttpContext.Session.SetString("UserRole", model.Role);
            HttpContext.Session.SetString("UserName", model.FullName);

            return RedirectToAction("Create","Complaint");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if(user == null || !PasswordHelper.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FullName);

            if(user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("MyComplaints", "Complaint");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

