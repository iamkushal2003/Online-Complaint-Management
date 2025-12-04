using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ComplaintSystem.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // store hashed password

        [Required]
        public string Role { get; set; } = "User"; // "User" or "Admin"

        public List<Complaint> Complaints { get; set; }
    }
}

