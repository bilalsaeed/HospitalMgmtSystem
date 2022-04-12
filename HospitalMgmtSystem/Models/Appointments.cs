using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public User Doctor { get; set; }
        public int? DoctorId { get; set; }
        public User Patient { get; set; }
        public int? PatientId { get; set; }
        public DateTime AppointmentDate { get; set; } = DateTime.UtcNow;
        public DateTime AppointmentTime { get; set; } = DateTime.UtcNow;
        public int? Duration { get; set; }
        public int? Bill { get; set; }
    }
}
