using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Account
{
    public class ApplicationUserLogin
    {
        [Required(ErrorMessage ="Username is required")]
        [MinLength(5,ErrorMessage = "Must be atleast 5-20 charectes")]
        [MaxLength(20, ErrorMessage = "Must be atleast 5-20 charectes")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Must be atleast 8-50 charectes")]
        [MaxLength(50, ErrorMessage = "Must be atleast 8-50 charectes")]
        public string Password { get; set; }
    }
}
