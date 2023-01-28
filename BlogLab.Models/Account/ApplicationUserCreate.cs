using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Account
{
    public class ApplicationUserCreate: ApplicationUserLogin
    {
        [MinLength(10,ErrorMessage ="Must be atleast 10-30 charecters")]
        [MaxLength(30,ErrorMessage = "Must be atleast 10-30 charecters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [MaxLength(30, ErrorMessage = "Must be atleast 30 charecters")]
        [EmailAddress(ErrorMessage ="Email address is required")]
        public string Email { get; set; }
    }
}
