using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Models
{
    public class Prescriptions
    {
        public int Id { get; set; }
        public string Medicine { get; set; }
        public string Dosage { get; set; }
        public string Quantity { get; set; }
        public User AddedBy { get; set; }
        public int AddedById { get; set; }
        public User Patient { get; set; }
        public int? PatientId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
