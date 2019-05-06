using System;
using Cet.PrinciplesOfDistanceEducation.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Cet.PrinciplesOfDistanceEducation.Data
{
    public class CetDbContext: IdentityDbContext<CetUser>
    {
        public CetDbContext(DbContextOptions<CetDbContext> options): base(options) {}
    
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoState> VideoStates { get; set; }
        public DbSet<VideoWatchLog> VideoWatchLogs { get; set; }
        public DbSet<Credits> Credits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<VideoState>()
                    .HasData(new VideoState { Id = 1, StateName = "Active"});
            builder.Entity<VideoState>()
                    .HasData(new VideoState {Id = 2, StateName = "Inactive"});
            builder.Entity<VideoState>()
                    .HasData(new VideoState {Id = 3, StateName = "Deleted"});
        }
    }
}
