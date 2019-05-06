using System;
using System.Collections.Generic;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Service;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public List<string> UserRoles { get; set; }

        public bool IsAuthorized
        {
            get 
            {
                if (UserRoles == null) return false;
                else return UserRoles.Contains("Superuser") || UserRoles.Contains("Administrator");
            }
        }
    }
}