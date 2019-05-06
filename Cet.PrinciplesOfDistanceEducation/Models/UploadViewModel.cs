using System;
using System.ComponentModel.DataAnnotations;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class UploadViewModel
    {
        public string Id { get; set; }
        [Required]
        public string VideoTitle { get; set; }
        public VideoThumbnail[] ThumbnailOptions { get; set; }
        public string Duration { get; set; }
        public int Year { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string TempVideoPath { get; set; }

        public UploadViewModel()
        {

        }
    }

    public class VideoThumbnail
    {
        public int Id { get; set; }
        public string ThumbnailUrl { get; set; }

        public VideoThumbnail()
        {

        }
    }
}