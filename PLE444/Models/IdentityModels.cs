using System.Data.Entity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;

namespace PLE444.Models
{
    public enum GenderType { Kadın, Erkek, Belirtilmedi };
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {      
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public string ProfilePicture { get; set; }

        public string PhoneNo { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        public string UserPhoto()
        {
            return ProfilePicture.IsNullOrWhiteSpace() ? "/Content/img/pp.jpg" : ProfilePicture;
        }

        //public int RoleId { get; set; }
        // public virtual Role Role { get; set; }

    }
    public class PleDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Friendship> Friendship { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<UserCommunity> UserCommunities { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<GradeType> GradeTypes { get; set; }
        public DbSet<UserGrade> UserGrades { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<TimelineEntry> TimelineEntries { get; set; }

        public PleDbContext()
            : base("PleDbContext", throwIfV1Schema: false)
        {
        }

        public static PleDbContext Create()
        {
            return new PleDbContext();
        }
    }
}