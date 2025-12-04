using System.ComponentModel.DataAnnotations;

namespace ComplaintSystem.Models
{
    public class Technician
    {
        public int TechnicianId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}

