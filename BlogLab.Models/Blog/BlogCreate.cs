using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Blog
{
    public class BlogCreate
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(10, ErrorMessage = "Must be atleast 10-50 charecters")]
        [MaxLength(50, ErrorMessage = "Must be atleast 10-50 charecters")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Content is required")]
        [MinLength(300, ErrorMessage = "Must be atleast 300-3000 charecters")]
        [MaxLength(3000, ErrorMessage = "Must be atleast 300-3000 charecters")]
        public string Content { get; set; }

        public int? PhotoId { get; set; }
    }
}
