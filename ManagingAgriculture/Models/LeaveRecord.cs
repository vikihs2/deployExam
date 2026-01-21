using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
    public class LeaveRecord
    {
        [Key]
        public int Id { get; set; }

        public DateTime LeaveDate { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public string Reason { get; set; }
        public bool IsApproved { get; set; } = true; // Auto-approved if added by Boss?
    }
}
