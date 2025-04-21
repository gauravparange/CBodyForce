
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class EditMemberDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string? ParentPhoneNo { get; set; }

        [Required(ErrorMessage = "Please enter your Date Of Joining")]
        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime DOJ { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter your Date Of Birth")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; } = DateTime.Now.AddYears(-10);    
        [Required(ErrorMessage = "Please enter your current address")]
        [Display(Name = "Address")]
        [MinLength(5, ErrorMessage = "Address cannot be less than 10 characters")]

        public string? Address { get; set; }

        [Display(Name = "Height")]
        public string? Height { get; set; }

        [Display(Name = "Weight")]
        public string? Weight { get; set; }

        [Display(Name = "Member Code")]
        public string? UserName { get; set; }
    }
}
