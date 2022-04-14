using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.DTOs
{
    public class MedicalRecordsDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public string LastAccessedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}
