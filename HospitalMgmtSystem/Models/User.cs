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
    }
}
