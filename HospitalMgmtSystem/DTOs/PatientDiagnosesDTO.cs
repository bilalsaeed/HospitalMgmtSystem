using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.DTOs
{
    public class PatientDiagnosesDTO
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public string DoctorName { get; set; }
    }

    public class PrescriptionsDTO
    {
        public int Id { get; set; }
        public string Medicine { get; set; }
        public string Dosage { get; set; }
        public string Quantity { get; set; }
        public string AddedBy { get; set; }      
        public string AddedAt { get; set; } 
    }
}
