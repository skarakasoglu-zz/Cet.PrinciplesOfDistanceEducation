using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cet.PrinciplesOfDistanceEducation.Data
{
    public class CetDbContextFactory: IDesignTimeDbContextFactory<CetDbContext>
    {
        // Server=193.140.200.25;Database=cet441demo_db;User Id=cet441demo_user;Password=c3VwZXJ1c2Vy;
        public CetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CetDbContext>();
            optionsBuilder.UseSqlServer(@"Server=193.140.200.25;Database=cet441demo_db;User Id=cet441demo_user;Password=c3VwZXJ1c2Vy;");
            return new CetDbContext(optionsBuilder.Options);
        }
    }
}