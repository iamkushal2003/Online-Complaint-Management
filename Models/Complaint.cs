using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplaintSystem.Models
{
    public class Complaint
    {
        [Key]
        public int ComplaintId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Resolved, Closed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}

