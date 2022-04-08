using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Models
{
    public class PatientProfile
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
