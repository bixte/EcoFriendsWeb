using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcoFriendsWeb.DataModels
{
    public class AppDataBaseContext : IdentityDbContext<User>
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<EventPost> EventPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public AppDataBaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogPost>()
                .HasMany(p => p.MainImagesStore)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlogPost>()
                .HasMany(p => p.PostParticals)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlogPost>()
                .HasMany(p => p.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EventPost>()
                .HasMany(e => e.ImagesStore)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

    }
}
