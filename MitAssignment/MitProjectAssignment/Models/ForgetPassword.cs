using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MitProjectAssignment.Models
{
    public class ForgetPassword
    {
        
            [Display(Name = "User Email ID")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "User Email Id Required")]
            public string Email { get; set; }
        
    
}
}