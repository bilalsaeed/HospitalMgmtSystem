using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.DTOs
{
    public class AppointmentsDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public int Duration { get; set; }
        public int Bill { get; set; }
    }
}
