using System;
using System.Collections.Generic;
using System.Net;
using Cet.PrinciplesOfDistanceEducation.Data.Models;

namespace Cet.PrinciplesOfDistanceEducation.Data
{
    public interface IVideo
    {
        IEnumerable<Video> GetAll();
        IEnumerable<Video> GetByYear(int year);
        IEnumerable<Video> GetByCreateUser(string userName);
        IEnumerable<int> GetYearsContainingVideo();

        Video GetById(string id);

        void Add(Video video);
        void Update(Video video);
        void Remove(Video video);

        void CreateVideo(string id, string videoUrl, string thumbnailUrl, TimeSpan duration,
                            string videoTitle, int year, string groupName, string description, string createUserName);
        void EditVideo(string id, string videoTitle, int year, string groupName, string description, string updateUserName, string stateName);
        void RemoveVideo(string id, string userName);
        void ChangeVideoState(string id, string stateName);
        void ViewVideo(string id, IPAddress viewerIp);
        void ViewVideo(string id, IPAddress viewerIp, string viewerUserName);
    }
}