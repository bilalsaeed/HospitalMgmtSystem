using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Models
{
    public class PatientDiagnosis
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public User Doctor { get; set; }
        public int? DoctorId { get; set; }
        public User Patient { get; set; }
        public int? PatientId { get; set; }
    }
}
