using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cet.PrinciplesOfDistanceEducation.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Cet.PrinciplesOfDistanceEducation.Data
{
    public interface ICetUser
    {
        IEnumerable<CetUser> GetAll();
        CetUser GetById(string id);
        CetUser GetByUserName(string userName);
        CetUser GetCurrentUser(ClaimsPrincipal user);

        void Add(CetUser user);
        void Update(CetUser user);
        void Remove(CetUser user);

        bool Signin(string userName, string password, bool isPersistent);
        bool Signup(string userName, string password, string email, string firstName, string lastName);
        void Signout();

        IEnumerable<string> GetUserRoles(string userName);

        Task<IQueryable<IdentityRole>> CreateRole(string role);
        Task<IList<string>> AssignRoleToUser(string userName, string role);
    }
}