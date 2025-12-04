using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintSystem.Models
{
    public class ComplaintAssignment
    {
        public int AssignmentId { get; set; }

        [Required]
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }

        [Required]
        public int TechnicianId { get; set; }
        public Technician Technician { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}

