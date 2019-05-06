using System;
using System.Linq;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cet.PrinciplesOfDistanceEducation.Controllers
{
    public class VideoController : Controller
    {
        private IVideo _videoService;
        private IHttpContextAccessor _accessor;

        public VideoController(IVideo videoService, IHttpContextAccessor accessor)
        {
            _videoService = videoService;
            _accessor = accessor;
        }

        [Route("/feed")]
        public IActionResult FeedIndex()
        {
            Bundle<IndexViewModel> model = new Bundle<IndexViewModel>();
            model.PageModel = new IndexViewModel();
            model.PageModel.Videos = _videoService.GetAll()
                            .Where(v => v.State.StateName == "Active")
                            .OrderByDescending(v => v.CreatedDate)
                            .ToList().Select(v => new VideoViewModel
                            {
                                Id = v.Id,
                                Title = v.Title,
                                VideoUrl = v.VideoUrl,
                                ThumbnailUrl = v.ThumbnailUrl,
                                GroupName = v.GroupName,
                                Description = v.Description,
                                ViewCount = v.ViewCount,
                                Duration = v.Duration.TotalHours >= 1 ? v.Duration.ToString("hh\\:mm\\:ss")
                                            : v.Duration.Minutes >= 1 ? v.Duration.ToString($"mm\\:ss")
                                            : v.Duration.ToString("ss"),
                                CreateUser = new UserViewModel
                                {
                                    UserName = v.CreateUser.UserName,
                                    FirstName = v.CreateUser.FirstName,
                                    LastName = v.CreateUser.LastName,
                                    AvatarUrl = v.CreateUser.AvatarUrl
                                },
                                CreatedDate = v.CreatedDate,
                                UpdateUser = new UserViewModel
                                {
                                    UserName = v.UpdateUser.UserName,
                                    FirstName = v.UpdateUser.FirstName,
                                    LastName = v.UpdateUser.LastName,
                                    AvatarUrl = v.UpdateUser.AvatarUrl
                                },
                                UpdatedDate = v.UpdatedDate
                            }).ToArray();
            ViewData["Title"] = "All Demos";
            return View("Feed", model);
        }

        [Route("/feed/{year}")]
        public IActionResult Feed(int year)
        {
            Bundle<IndexViewModel> model = new Bundle<IndexViewModel>();
            model.PageModel = new IndexViewModel();
            model.PageModel.Videos = _videoService.GetByYear(year)
                            .Where(v => v.State.StateName == "Active")
                            .OrderByDescending(v => v.CreatedDate)
                            .ToList().Select(v => new VideoViewModel
                            {
                                Id = v.Id,
                                Title = v.Title,
                                VideoUrl = v.VideoUrl,
                                ThumbnailUrl = v.ThumbnailUrl,
                                GroupName = v.GroupName,
                                Description = v.Description,
                                ViewCount = v.ViewCount,
                                Duration = v.Duration.TotalHours >= 1 ? v.Duration.ToString("hh\\:mm\\:ss")
                                            : v.Duration.Minutes >= 1 ? v.Duration.ToString($"mm\\:ss")
                                            : v.Duration.ToString("ss"),
                                CreateUser = new UserViewModel
                                {
                                    UserName = v.CreateUser.UserName,
                                    FirstName = v.CreateUser.FirstName,
                                    LastName = v.CreateUser.LastName,
                                    AvatarUrl = v.CreateUser.AvatarUrl
                                },
                                CreatedDate = v.CreatedDate,
                                UpdateUser = new UserViewModel
                                {
                                    UserName = v.UpdateUser.UserName,
                                    FirstName = v.UpdateUser.FirstName,
                                    LastName = v.UpdateUser.LastName,
                                    AvatarUrl = v.UpdateUser.AvatarUrl
                                },
                                UpdatedDate = v.UpdatedDate
                            }).ToArray();
            if (model.PageModel.Videos != null && model.PageModel.Videos.Length > 0)
                ViewData["Title"] = $"Videos from {year}";
            else
                ViewData["Title"] = $"No videos found from {year}";
            return View(model);
        }

        [HttpGet("/watch")]
        public IActionResult Watch(string video)
        {
            var currentVideo = _videoService.GetById(video);

            Bundle<WatchVideoModel> model = new Bundle<WatchVideoModel>();
            if (currentVideo != null)
            {
                model.PageModel = new WatchVideoModel
                {

                    Video = new VideoViewModel
                    {
                        Id = currentVideo.Id,
                        Title = currentVideo.Title,
                        VideoUrl = currentVideo.VideoUrl,
                        ThumbnailUrl = currentVideo.ThumbnailUrl,
                        GroupName = currentVideo.GroupName,
                        Description = currentVideo.Description,
                        ViewCount = currentVideo.ViewCount,
                        Duration = currentVideo.Duration.TotalHours >= 1 ? currentVideo.Duration.ToString("hh\\:mm\\:ss")
                    : currentVideo.Duration.Minutes >= 1 ? currentVideo.Duration.ToString($"mm\\:ss")
                    : currentVideo.Duration.ToString("ss"),
                        CreateUser = new UserViewModel
                        {
                            UserName = currentVideo.CreateUser.UserName,
                            FirstName = currentVideo.CreateUser.FirstName,
                            LastName = currentVideo.CreateUser.LastName,
                            AvatarUrl = currentVideo.CreateUser.AvatarUrl
                        },
                        CreatedDate = currentVideo.CreatedDate,
                        UpdateUser = new UserViewModel
                        {
                            UserName = currentVideo.UpdateUser.UserName,
                            FirstName = currentVideo.UpdateUser.FirstName,
                            LastName = currentVideo.UpdateUser.LastName,
                            AvatarUrl = currentVideo.UpdateUser.AvatarUrl
                        },
                        UpdatedDate = currentVideo.UpdatedDate
                    }
                };

                model.PageModel.VideosFromTheSameYear = _videoService.GetByYear(currentVideo.Year)
                    .Where(v => v.State.StateName == "Active" && v.Id != video)
                    .OrderByDescending(v => v.CreatedDate)
                    .ToList().Select(v => new VideoViewModel
                    {
                        Id = v.Id,
                        Title = v.Title,
                        VideoUrl = v.VideoUrl,
                        ThumbnailUrl = v.ThumbnailUrl,
                        GroupName = v.GroupName,
                        Description = v.Description,
                        ViewCount = v.ViewCount,
                        Duration = v.Duration.TotalHours >= 1 ? v.Duration.ToString("hh\\:mm\\:ss")
                    : v.Duration.Minutes >= 1 ? v.Duration.ToString($"mm\\:ss")
                    : v.Duration.ToString("ss"),
                        CreateUser = new UserViewModel
                        {
                            UserName = v.CreateUser.UserName,
                            FirstName = v.CreateUser.FirstName,
                            LastName = v.CreateUser.LastName,
                            AvatarUrl = v.CreateUser.AvatarUrl
                        },
                        CreatedDate = v.CreatedDate,
                        UpdateUser = new UserViewModel
                        {
                            UserName = v.UpdateUser.UserName,
                            FirstName = v.UpdateUser.FirstName,
                            LastName = v.UpdateUser.LastName,
                            AvatarUrl = v.UpdateUser.AvatarUrl
                        },
                        UpdatedDate = v.UpdatedDate
                    }).ToArray();
                var ip = _accessor.HttpContext.Connection.RemoteIpAddress;

                if (User.Identity.Name == null) _videoService.ViewVideo(video, ip);
                else _videoService.ViewVideo(video, ip, User.Identity.Name);

                ViewData["Title"] = model.PageModel.Video.Title;
            }
            else 
            {
                model.PageModel = new WatchVideoModel();
                ViewData["Title"] = "Video Bulunamadı";
            }
            return View(model);
        }

        [Authorize(Roles = "Superuser,Administrator")]
        [HttpDelete("/video/remove")]
        public IActionResult Remove(string video)
        {
            _videoService.RemoveVideo(video, User.Identity.Name);
            return Json("Success"); 
        }

        [Authorize(Roles = "Superuser,Administrator")]
        [HttpGet("/video/edit")]
        public IActionResult Edit(string video)
        {
            var currentVideo = _videoService.GetById(video);

            Bundle<EditViewModel> model = new Bundle<EditViewModel>();
            model.PageModel = new EditViewModel
            {
                CurrentVideo = new VideoViewModel{
                    Id = video,
                    Title = currentVideo.Title,
                    GroupName = currentVideo.GroupName,
                    Description = currentVideo.Description,
                    CreatedDate = currentVideo.CreatedDate,
                    ThumbnailUrl = currentVideo.ThumbnailUrl,
                    Year = currentVideo.Year,
                    StateName  =currentVideo.State.StateName
                }
            };
            ViewData["Title"]= "Edit " + model.PageModel.CurrentVideo.Title;
            return View(model);
        }

        [Authorize(Roles = "Superuser,Administrator")]
        [HttpPost("/video/editsave")]
        public IActionResult EditSave(Bundle<EditViewModel> model)
        {
            VideoViewModel video = model.PageModel.CurrentVideo;
            bool warning = false;

            if (string.IsNullOrWhiteSpace(video.Title) || string.IsNullOrWhiteSpace(video.GroupName) || string.IsNullOrWhiteSpace(video.Description) || video.Year <= 2000)
                warning = true;
            else
                _videoService.EditVideo(video.Id, video.Title, video.Year, video.GroupName, video.Description, User.Identity.Name, video.StateName);
            return Json(new { warning = warning});
        }
    }
}