using HospitalMgmtSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.DBContext
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext() : base("SchoolContext") { }
        public DbSet<User> User { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<PatientDiagnosis> PatientDiagnosis { get; set; }
        public DbSet<PatientMedicalRecords> PatientMedicalRecords { get; set; }
        public DbSet<Prescriptions> Prescriptions { get; set; }

        public DbSet<TestTable> TestTable { get; set; }
    }
}
