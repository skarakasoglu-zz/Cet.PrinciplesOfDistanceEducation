using System;
using System.ComponentModel.DataAnnotations;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(4, ErrorMessage = "Username cannot be less than 4 characters.")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Password cannot be less than 4 characters.")]
        [MaxLength(32, ErrorMessage = "Password cannot be more than 32 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,32}$",
         ErrorMessage = "Password must meet requirements")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}