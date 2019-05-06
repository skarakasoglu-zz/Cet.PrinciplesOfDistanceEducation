using System;
using System.ComponentModel.DataAnnotations;

namespace Cet.PrinciplesOfDistanceEducation.Data.Models
{
    public class VideoWatchLog
    {
        public int Id { get; set; }
        [Required]
        public DateTime WatchDate { get; set; }
        [Required]
        public virtual Video Video { get; set; }
        public virtual CetUser User { get; set; }
        [Required]
        public string ViewerIp { get; set; }
    }
}