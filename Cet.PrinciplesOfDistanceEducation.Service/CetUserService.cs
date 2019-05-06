using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Cet.PrinciplesOfDistanceEducation.Service
{
    public class CetUserService : ICetUser
    {
        private CetDbContext _context;
        private UserManager<CetUser> _userManager;
        private SignInManager<CetUser> _signinManager;
        private RoleManager<IdentityRole> _roleManager;

        public CetUserService(CetDbContext context, UserManager<CetUser> userManager, 
                                SignInManager<CetUser> signinManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }

        public void Add(CetUser user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public void Update(CetUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
        
        public void Remove(CetUser user)
        {
            _context.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<CetUser> GetAll()
        {
            return _context.Users;
        }

        public CetUser GetById(string id)
        {
            return GetAll()
                    .SingleOrDefault(u => u.Id == id);
        }

        public CetUser GetByUserName(string userName)
        {
            return GetAll()
                    .SingleOrDefault(u => u.UserName == userName);
        }

        public CetUser GetCurrentUser(ClaimsPrincipal user)
        {
            if (_signinManager.IsSignedIn(user)) return _userManager.GetUserAsync(user).Result;
            else return null;
        }

        public bool Signin(string userName, string password, bool isPersistent)
        {
            var result = _signinManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure: true);
            return result.Result.Succeeded;
        }

        public void Signout()
        {
            var result = _signinManager.SignOutAsync();
        }

        public bool Signup(string userName, string password, string email, string firstName, string lastName)
        {
            var user = new CetUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };
            var result = _userManager.CreateAsync(user, password);

            if (result.Result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User");
                return true;
            }
            else { return false; }
        }

        public IEnumerable<string> GetUserRoles(string userName)
        {
            CetUser user = GetByUserName(userName);
            return _userManager.GetRolesAsync(user).Result;
        }

        public async Task<IList<string>> AssignRoleToUser(string userName, string role)
        {
            CetUser user = GetByUserName(userName);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                return await _userManager.GetRolesAsync(user);
            }
            return null;
        }

        public async Task<IQueryable<IdentityRole>> CreateRole(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            return _roleManager.Roles;
        }
    }
}
