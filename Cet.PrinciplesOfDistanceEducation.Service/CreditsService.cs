using System;
using System.Linq;
using Cet.PrinciplesOfDistanceEducation.Data;
using Cet.PrinciplesOfDistanceEducation.Data.Models;

namespace Cet.PrinciplesOfDistanceEducation.Service
{
    public class CreditsService : ICredits
    {
        private CetDbContext _context;

        public CreditsService(CetDbContext context)
        {
            _context = context;
        }

        public void Add(Credits credits)
        {
            _context.Add(credits);
            _context.SaveChanges();
        }

        public void Update(Credits credits)
        {
            _context.Update(credits);
            _context.SaveChanges();
        }

        public void Remove(Credits credits)
        {
            _context.Remove(credits);
            _context.SaveChanges();
        }

        public void CreateCredits(string content, string userName)
        {
            var now = DateTime.Now;

            Credits credits = new Credits
            {
                Content = content,
                CreatedDate = now,
                CreateUser = _context.Users.FirstOrDefault(u => u.UserName == userName)
            };
            Add(credits);
        }

        public string GetLastCreditsContent()
        {
            return _context.Credits
                            .OrderByDescending(c => c.CreatedDate)
                            .FirstOrDefault().Content;
        }
    }
}