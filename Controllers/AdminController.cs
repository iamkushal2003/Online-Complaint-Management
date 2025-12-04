using Microsoft.AspNetCore.Mvc;
using ComplaintSystem.Data;
using ComplaintSystem.Models;
using System.Linq;

namespace ComplaintSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) => _db = db;

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        public IActionResult Dashboard()
        {
            if(!IsAdmin()) return RedirectToAction("Login","Account");

            var stats = new {
                Open = _db.Complaints.Count(c => c.Status == "Open"),
                InProgress = _db.Complaints.Count(c => c.Status == "InProgress"),
                Resolved = _db.Complaints.Count(c => c.Status == "Resolved"),
                Closed = _db.Complaints.Count(c => c.Status == "Closed"),
            };

            ViewBag.Stats = stats;
            return View();
        }

        public IActionResult AllComplaints()
        {
            if(!IsAdmin()) return RedirectToAction("Login","Account");
            var list = _db.Complaints.OrderByDescending(c => c.CreatedAt).ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            if(!IsAdmin()) return RedirectToAction("Login","Account");

            var comp = _db.Complaints.FirstOrDefault(c => c.ComplaintId == id);
            if(comp == null) return NotFound();

            comp.Status = status;
            comp.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();
            return RedirectToAction("AllComplaints");
        }

        [HttpGet]
        public IActionResult Assign(int id)
        {
            if(!IsAdmin()) return RedirectToAction("Login","Account");
            var complaint = _db.Complaints.FirstOrDefault(c => c.ComplaintId == id);
            if(complaint == null) return NotFound();
            ViewBag.Techs = _db.Technicians.ToList();
            return View(complaint);
        }

        [HttpPost]
        public IActionResult Assign(int complaintId, int technicianId)
        {
            if(!IsAdmin()) return RedirectToAction("Login","Account");

            var assign = new ComplaintAssignment
            {
                ComplaintId = complaintId,
                TechnicianId = technicianId
            };
            _db.ComplaintAssignments.Add(assign);

            var comp = _db.Complaints.FirstOrDefault(c => c.ComplaintId == complaintId);
            if(comp != null)
            {
                comp.Status = "InProgress";
                comp.UpdatedAt = DateTime.UtcNow;
            }

            _db.SaveChanges();
            return RedirectToAction("AllComplaints");
        }
    }
}

