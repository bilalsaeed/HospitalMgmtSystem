using HospitalMgmtSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem
{
    public class AppointmentsView
    {
        public string DoctorName { get; set; }
        public ObservableCollection<AppointmentsDto> Appointments { get; set; }

    }
}
