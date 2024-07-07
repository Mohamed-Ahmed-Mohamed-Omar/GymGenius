using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using GymGenius.Data.Entities;
using GymGenius.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IEncryptionProvider _encryptionProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            _encryptionProvider = new GenerateEncryptionProvider("66774a9639904269a8eb2315d04e522a5542F7069A7244148C7C713AEBF9CD77");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
        }
        
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
                new IdentityRole() { Name = "Coach", ConcurrencyStamp = "3", NormalizedName = "Coach" }
                );
        }

        public DbSet<Advertisement> advertisements { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Gender> genders { get; set; }
        public DbSet<Training_From> training_froms { get; set; }
        public DbSet<Current_Goal> current_goals { get; set; }
        public DbSet<Level_Train> level_Trains { get; set; }
        public DbSet<Shape_Training> shape_Trainings { get; set; }
        public DbSet<Subscription> subscriptions { get; set; }
        public DbSet<Plan> plans { get; set; }
        public DbSet<Track_Progress> track_Progresses { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Rate> rates { get; set; }
        public DbSet<SubGoal> subGoals { get; set; }
        public DbSet<DayTrainNumber> dayTrainNumbers { get; set; }
    }
}
