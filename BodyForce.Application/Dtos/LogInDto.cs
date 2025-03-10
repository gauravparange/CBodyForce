using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class LogInDto
    {
        [Required(ErrorMessage =("Please enter your member code"))]
        [Display(Name ="Member Code")]
        public string UserName { get; set; }
        [Required(ErrorMessage =  "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

