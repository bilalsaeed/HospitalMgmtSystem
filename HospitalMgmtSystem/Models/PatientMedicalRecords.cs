using HospitalMgmtSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Models
{
    public class PatientMedicalRecords
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public User Patient { get; set; }
        public int? PatientId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
        public User LastAccessedBy { get; set; }
        public int? LastAccessedById { get; set; }
        public User CreatedBy { get; set; }
        public int? CreatedById { get; set; }
        public MedicalRecordType? RecordType { get; set; }
    }
}
