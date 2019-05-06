using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cet.PrinciplesOfDistanceEducation.Service
{
    public class VideoService : IVideo
    {
        private CetDbContext _context;

        public VideoService(CetDbContext context)
        {
            _context = context;
        }

        public void Add(Video video)
        {
            _context.Add(video);
            _context.SaveChanges();
        }

        public void Update(Video video)
        {
            _context.Update(video);
            _context.SaveChanges();
        }

        public void Remove(Video video)
        {
            _context.Remove(video);
            _context.SaveChanges();
        }

        public IEnumerable<Video> GetAll()
        {
            return _context.Videos
                        .Where(v => v.State.StateName == "Active" || v.State.StateName == "Inactive")
                        .Include(v => v.CreateUser)
                        .Include(v => v.UpdateUser)
                        .Include(v => v.State);
        }

        public IEnumerable<Video> GetByCreateUser(string userName)
        {
            return GetAll()
                    .Where(v => v.CreateUser.UserName == userName);
        }

        public IEnumerable<Video> GetByYear(int year)
        {
            return GetAll()
                        .Where(v => v.Year == year);
        }

        public IEnumerable<int> GetYearsContainingVideo()
        {
            return GetAll()
                        .Where(v => v.State.StateName == "Active")
                        .GroupBy(v => v.Year)
                        .OrderByDescending(group => group.Key)
                        .Select(group => group.Key);
        }

        public Video GetById(string id)
        {
            return GetAll()
                    .SingleOrDefault(v => v.Id == id);
        }

        public void CreateVideo(string id, string videoUrl, string thumbnailUrl, TimeSpan duration, 
                                    string videoTitle, int year, string groupName, string description, string createUserName)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == createUserName);
            var now = DateTime.Now;
            var state = _context.VideoStates.SingleOrDefault(s => s.StateName == "Inactive");

            var video = new Video
            {
                Id = id,
                VideoUrl = videoUrl,
                ThumbnailUrl = thumbnailUrl,
                Duration = duration,
                Title = videoTitle,
                Year = year,
                GroupName = groupName,
                Description = description,
                CreateUser = user,
                CreatedDate = now,
                UpdateUser = user,
                UpdatedDate = now,
                State = state
            };
            Add(video);
        }

        public void EditVideo(string id, string videoTitle, int year, string groupName, string description, string updateUserName, string stateName)
        {
            var video = GetById(id);
            var state = _context.VideoStates.SingleOrDefault(vs => vs.StateName == stateName);

            video.Title = videoTitle;
            video.Year = year;
            video.GroupName = groupName;
            video.Description = description;
            video = UpdateVideoUpdateUser(video, updateUserName);
            video.State = state;
            Update(video);
        }

        public void RemoveVideo(string id, string userName)
        {
            var video = GetById(id);
            var state = _context.VideoStates.SingleOrDefault(vs => vs.StateName == "Deleted");

            video.State = state;
            Update(video);
        }

        public void ChangeVideoState(string id, string stateName)
        {
            var video = GetById(id);
            var state = _context.VideoStates.SingleOrDefault(vs => vs.StateName == stateName);

            video.State = state;
            Update(video);
        }

        public void ViewVideo(string id, IPAddress viewerIp)
        {
            var lastVisit = GetLastViewFromCurrentSession(id, viewerIp);
            if ((DateTime.Now - lastVisit).Days < 1) return;

            var video = GetById(id);
            _context.Add(new VideoWatchLog {Video = video, ViewerIp = viewerIp.ToString(), WatchDate = DateTime.Now});
            video.ViewCount++;
            Update(video);
            _context.SaveChanges();
        }

        public void ViewVideo(string id, IPAddress viewerIp, string viewerUserName)
        {
            var lastVisit = GetLastViewFromCurrentSession(id, viewerIp);
            if ((DateTime.Now - lastVisit).Days < 1) return;

            var video = GetById(id);
            var user = _context.Users.SingleOrDefault(u => u.UserName == viewerUserName);

            _context.Add(new VideoWatchLog {
                Video = video,
                ViewerIp = viewerIp.ToString(),
                WatchDate = DateTime.Now,
                User = user
            });
            video.ViewCount++;
            Update(video);
            _context.SaveChanges();
        }

        private DateTime GetLastViewFromCurrentSession(string id, IPAddress viewerIp)
        {
            var watchLog = _context.VideoWatchLogs
                        .Where(vw => vw.ViewerIp == viewerIp.ToString() && vw.Video.Id == id)
                        .OrderByDescending(vw => vw.WatchDate)
                        .FirstOrDefault();

            if (watchLog == null) return DateTime.MinValue;
            else return watchLog.WatchDate;
        }
        
        private Video UpdateVideoUpdateUser(Video video, string updateUserName)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == updateUserName);
            var now = DateTime.Now;

            video.UpdateUser = user;
            video.UpdatedDate = now;

            return video;
        }

    }
}