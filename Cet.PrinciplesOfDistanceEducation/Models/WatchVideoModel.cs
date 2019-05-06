using System;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class WatchVideoModel
    {
        public VideoViewModel Video { get; set; }
        public VideoViewModel[] VideosFromTheSameYear { get; set; }
    }
}