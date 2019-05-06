using System;

namespace Cet.PrinciplesOfDistanceEducation.Data.Models
{
    public class Credits 
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public CetUser CreateUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}