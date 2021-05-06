using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MitProjectAssignment.Models
{
    public class UserProfileModel
    {
        public int ID { get; set; }

        public int USerID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public Nullable<System.DateTime> DOB { get; set; }

        public int? Gender { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }



        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required]
        public string Address_Line_1 { get; set; }

        [Required]
        public string Address_Line_2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip_Code { get; set; }

        [Required]
        public string Country { get; set; }

        public string University { get; set; }

        public string College { get; set; }

        public string profileUrl { get; set; }
        public IEnumerable<Countrie> CountryList { get; set; }

        public IEnumerable<ReferenceData> GenderList { get; set; }


    }
}
