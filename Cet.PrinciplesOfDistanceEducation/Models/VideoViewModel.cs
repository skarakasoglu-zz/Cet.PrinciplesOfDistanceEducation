using System;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class VideoViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int Year { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public string Duration { get; set; }
        public UserViewModel CreateUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserViewModel UpdateUser { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedDateString
        {
            get
            {
                var now = DateTime.Now;
                TimeSpan timeDiffSpan = now - this.CreatedDate;
                DateTime diffDate = DateTime.MinValue + timeDiffSpan;
                
                if (diffDate.Year - 1 > 0)
                {
                    var diffYear = diffDate.Year - 1;
                    return diffYear > 1 ? $"{diffYear} years ago" : $"{diffYear} year ago";
                }
                else if (diffDate.Month - 1 > 0)
                {
                    var diffMonth = diffDate.Month - 1;
                    return diffMonth > 1 ? $"{diffMonth} months ago" : $"{diffMonth} month ago";
                }
                else if (diffDate.Day - 1 >= 7)
                {
                    var diffWeek = (diffDate.Day - 1) / 7;
                    return diffWeek > 1 ? $"{diffWeek} weeks ago" : $"1 week ago";
                }
                else if (diffDate.Day - 1 > 0)
                {
                    var diffDay = diffDate.Day - 1;
                    return diffDay > 1 ? $"{diffDay} days ago" : $"{diffDay} day ago";
                }
                else if (diffDate.Hour > 0)
                {
                    var diffHour = diffDate.Hour;
                    return diffHour > 1 ? $"{diffHour} hours ago" : $"{diffHour} hour ago";
                }
                else if (diffDate.Minute > 0)
                {
                    return diffDate.Minute > 1 ? $"{diffDate.Minute} minutes ago" : $"{diffDate.Minute} minute ago";
                }
                else
                {
                    return diffDate.Second > 1 ? $"{diffDate.Second} seconds ago" : $"{diffDate.Second} second ago";
                }
            }
        }
        public string StateName { get; set; }
    }
}