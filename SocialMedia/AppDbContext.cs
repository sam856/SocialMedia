using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;

namespace SocialMedia
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
           public DbSet<Comments > Comments { get; set; }
            public DbSet <Posts> Posts { get; set; }
            public DbSet <Story> Stories { get; set; }    
            public DbSet <Media> Media { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var confing = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var ConnectionString = confing.GetSection("constr").Value;
            optionsBuilder.UseSqlServer(ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
