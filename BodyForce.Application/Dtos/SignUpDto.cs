using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BodyForce
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [Remote(action: "IsEmailAvailable", controller: "Account", ErrorMessage = "Email is already in use.")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please enter your phone number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }
        public string? ParentPhoneNo { get; set; }
        [Required(ErrorMessage = "Please enter your Date Of Birth")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Display(Name ="Role")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Please enter your current address")]
        [Display(Name = "Address")]
        [MinLength(5,ErrorMessage = "Address cannot be less than 10 characters")]

        public string? Address { get; set; }

        [Display(Name = "Height")]
        public string? Height { get; set; }

        [Display(Name = "Weight")]
        public string? Weight { get; set; }

        [Display(Name = "Member Code")]
        public string? Username { get; set; }
    }
}
