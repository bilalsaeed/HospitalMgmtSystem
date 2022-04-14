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
}
