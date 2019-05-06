using System;
using System.IO;
using System.Linq;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cet.PrinciplesOfDistanceEducation.Controllers
{
    public class HomeController : Controller
    {
        private IVideo _videoService;
        private ICredits _creditsService;
        private IConfiguration _configuration;

        public HomeController(IVideo videoService, ICredits creditsService, IConfiguration configuration)
        {
            _videoService = videoService;
            _creditsService = creditsService;
            _configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            IndexViewModel pageModel = new IndexViewModel();
            pageModel.Videos = _videoService.GetAll()
                            .Where(v => v.State.StateName == "Active")
                            .OrderByDescending(v => v.CreatedDate)
                            .Take(12)
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
            Bundle<IndexViewModel> model = new Bundle<IndexViewModel>();
            model.PageModel = pageModel;
            ViewData["Title"] = "Home Page";
            return View(model);
        }

        [Route("/about")]
        public IActionResult About()
        {
            ViewData["Title"] = "About this site";
            IndexViewModel pageModel = new IndexViewModel();
            Bundle<IndexViewModel> model = new Bundle<IndexViewModel>();
            model.PageModel = pageModel;
            return View(model);
        }

        [Route("/credits")]
        public IActionResult Credits()
        {
            Bundle<CreditsViewModel> model = new Bundle<CreditsViewModel>();
            CreditsViewModel pageModel = new CreditsViewModel();

            pageModel.Content = _creditsService.GetLastCreditsContent();
            model.PageModel = pageModel;
            ViewData["Title"] = "Site Credits";

            return View(model);
        }

        [Authorize(Roles = "Superuser,Administrator")]
        [HttpPost("/credits/edit")]
        public IActionResult EditCredits(Bundle<CreditsViewModel> model)
        {
            _creditsService.CreateCredits(model.PageModel.Content, User.Identity.Name);
            return RedirectToAction("Credits", "Home");            
        }
    }
}