using HospitalMgmtSystem.Attributes;
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
        [ColumnNameAttribute("First Name")]
        public string FirstName { get; set; }
        [ColumnNameAttribute("Last Name")]
        public string LastName { get; set; }
        public string Password { get; set; }
        public UserTypeEnum UserType { get; set; }

        [ColumnNameAttribute("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [ColumnNameAttribute("City")]
        public string City { get; set; }

        [ColumnNameAttribute("Country")]

        public string Country { get; set; }

        [ColumnNameAttribute("Postal Code")]

        public string PostalCode { get; set; }

        [ColumnNameAttribute("Gender")]

        public string Gender { get; set; }

        [ColumnNameAttribute("Nationality")]

        public string Nationality { get; set; }

        [ColumnNameAttribute("Ethnicity")]

        public string Ethnicity { get; set; }

        [ColumnNameAttribute("Address")]

        public string Address { get; set; }

        [ColumnNameAttribute("Occupation")]

        public string Occupation { get; set; }

        [ColumnNameAttribute("Contact Number")]

        public string ContactNumber { get; set; }

        [ColumnNameAttribute("Alternate Phone Number")]

        public string AlternatePhoneNumber { get; set; }

        [ColumnNameAttribute("Email Address")]

        public string EmailAddress { get; set; }

        [ColumnNameAttribute("Previous GP Name")]

        public string PreviousGPName { get; set; }

        [ColumnNameAttribute("GP Address")]

        public string GPAddress { get; set; }

        [ColumnNameAttribute("GP Postal Code")]

        public string GPPostalCode { get; set; }

        [ColumnNameAttribute("Doctor Name")]

        public string DoctorName { get; set; }

        [ColumnNameAttribute("Kin Name")]

        public string KinName { get; set; }

        [ColumnNameAttribute("Kin Phone Number")]

        public string KinPhoneNumber { get; set; }

        [ColumnNameAttribute("Connect Via Post")]

        public bool ConnectPost { get; set; }

        [ColumnNameAttribute("Connect Via Calls or SMS")]

        public bool ConnectSMS { get; set; }

        [ColumnNameAttribute("Connect Via Email")]

        public bool ConnectEmail { get; set; }

        [ColumnNameAttribute("Accepted Terms and Conditions")]

        public bool TermsNCondition { get; set; }

        [ColumnNameAttribute("Agree To Collect Past Medical History from previous GP")]

        public bool CollectInformation { get; set; }

        [ColumnNameAttribute("Relevant Information")]

        public string RelevantInformation { get; set; }
    }
}
