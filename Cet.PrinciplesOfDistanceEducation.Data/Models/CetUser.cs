using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Cet.PrinciplesOfDistanceEducation.Data.Models
{
    public class CetUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AvatarUrl { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public CetUser()
        {
            AvatarUrl = "/cet441demo/images/avatars/no-avatar.png";
            IsDeleted = false;
        }
    }
}