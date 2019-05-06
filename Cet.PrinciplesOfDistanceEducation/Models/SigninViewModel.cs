using System;
using System.ComponentModel.DataAnnotations;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class SigninViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}