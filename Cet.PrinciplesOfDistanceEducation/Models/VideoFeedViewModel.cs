using System;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class VideoFeedViewModel
    {
        public VideoViewModel[] Videos { get; set; }
        public string Title { get; set; }
        public bool IsFlex { get; set; }

        public VideoFeedViewModel()
        {
            IsFlex = false;
        }
    }
}