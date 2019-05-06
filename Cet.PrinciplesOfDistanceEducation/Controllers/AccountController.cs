using System;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cet.PrinciplesOfDistanceEducation.Controllers
{
    public class AccountController : Controller
    {
        private ICetUser _cetUserService;
        public AccountController(ICetUser cetUserService)
        {
            _cetUserService = cetUserService;
        }

        [HttpGet("/root/addrole")]
        [Authorize(Roles = "Superuser,Administrator")]
        public IActionResult AddRole(string role, string key)
        {
            if (key == "c3VwZXJ1c2Vy")
                return Json(_cetUserService.CreateRole(role).Result);
            else
                return Json("Permission denied.");
        }

        [HttpGet("/root/addroletouser")]
        [Authorize(Roles = "Superuser,Administrator")]
        public IActionResult AddRoleToUser(string userName, string role, string key)
        {
            if (key == "c3VwZXJ1c2Vy")
                return Json(_cetUserService.AssignRoleToUser(userName, role).Result);
            else
                return Json("Permission denied.");
        }

        [Route("/signup")]
        public IActionResult Signup()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            else return View();
        }

        [Route("/signin")]
        public IActionResult Signin()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            else return View();
        }

        [HttpPost("/signin")]
        public IActionResult Signin(SigninViewModel model)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");

            var succeed = _cetUserService
                                .Signin(model.UserName, model.Password, false);
            if (succeed)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Signin", new { error = "signin" });
        }

        [HttpPost("/signup")]
        public IActionResult Signup(SignupViewModel model)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");

            var succeed = _cetUserService
                        .Signup(model.UserName, model.Password, model.Email, model.FirstName, model.LastName);
            if (succeed)
                return RedirectToAction("Signin");
            else
                return RedirectToAction("Signup", new { error = "signup" });
        }

        [Route("/signout")]
        [Authorize]
        public IActionResult Signout()
        {
            _cetUserService.Signout();
            return RedirectToAction("Index", "Home");
        }
    }
}