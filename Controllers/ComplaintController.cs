using Microsoft.AspNetCore.Mvc;
using ComplaintSystem.Data;
using ComplaintSystem.Models;
using System.Linq;

namespace ComplaintSystem.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly AppDbContext _db;
        public ComplaintController(AppDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Create()
        {
            if(HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login","Account");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Complaint model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null) return RedirectToAction("Login","Account");

            if(!ModelState.IsValid) return View(model);

            model.UserId = userId.Value;
            model.Status = "Open";
            model.CreatedAt = DateTime.UtcNow;

            _db.Complaints.Add(model);
            _db.SaveChanges();
            return RedirectToAction("MyComplaints");
        }

        public IActionResult MyComplaints()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null) return RedirectToAction("Login","Account");

            var data = _db.Complaints
                .Where(c => c.UserId == userId.Value)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return View(data);
        }

        public IActionResult Details(int id)
        {
            var complaint = _db.Complaints.FirstOrDefault(c => c.ComplaintId == id);
            if(complaint == null) return NotFound();
            return View(complaint);
        }
    }
}

