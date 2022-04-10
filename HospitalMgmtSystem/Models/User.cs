using HospitalMgmtSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_UserName", 1, IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(100)]
        public string UserName { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string Password { get; set; }
        public UserTypeEnum UserType { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Ethnicity { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string ContactNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PreviousGPName { get; set; }
        public string GPAddress { get; set; }
        public string GPPostalCode { get; set; }
        public string DoctorName { get; set; }
        public string KinName { get; set; }
        public string KinPhoneNumber { get; set; }
    }
}
