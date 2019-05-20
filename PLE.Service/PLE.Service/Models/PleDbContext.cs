using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PLE.Service.Models
{
	public class PleDbContext : IdentityDbContext<ApplicationUser>
	{
		public PleDbContext() : base("PleDbContext", throwIfV1Schema: false) {
			Configuration.LazyLoadingEnabled = false;
		}

		public static PleDbContext Create() {
			return new PleDbContext();
		}

		public DbSet<Answer> Answers { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Chapter> Chapters { get; set; }
		public DbSet<Community> Communities { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Discussion> Discussions { get; set; }
		public DbSet<Document> Documents { get; set; }
		public DbSet<Friendship> Friendship { get; set; }
		public DbSet<GradeType> GradeTypes { get; set; }
		public DbSet<LetterGrade> LetterGrades { get; set; }
		public DbSet<Material> Materials { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<PrivateMessage> PrivateMessages { get; set; }
		public DbSet<Discussion.Reading> Readings { get; set; }
		public DbSet<Space> Spaces { get; set; }
		public DbSet<TimelineEntry> TimelineEntries { get; set; }
		public DbSet<UserCommunity> UserCommunities { get; set; }
		public DbSet<UserCourse> UserCourses { get; set; }
		public DbSet<UserGrade> UserGrades { get; set; }
		public DbSet<UserAnswer> UserAnswers { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Quiz> Quizzes { get; set; }
	}
}