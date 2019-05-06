using System;
using System.ComponentModel.DataAnnotations;

namespace Cet.PrinciplesOfDistanceEducation.Data.Models
{
    public class Video
    {
        public string Id { get; set; }
        [Required]
        public string VideoUrl { get; set; }
        [Required]
        public string ThumbnailUrl { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        [Required]
        public virtual CetUser CreateUser { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public virtual CetUser UpdateUser { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ViewCount { get; set; }
        [Required]
        public virtual VideoState State { get; set; }

        public Video()
        {
            ThumbnailUrl = "";
            ViewCount = 0;
        }
    }
}