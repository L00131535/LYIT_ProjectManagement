using crowsoftmvc.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace crowsoftmvc.Models
{
    public class UserAccount
    {
        [Key]
        public int idUserAccount { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNo { get; set; }
        public string AddressLine { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string EirCode { get; set; }
        public string CompanyName { get; set; }
        public string TypeUser { get; set; }
        public Nullable<DateTime> DateCreated { get; set; }
    }



}
