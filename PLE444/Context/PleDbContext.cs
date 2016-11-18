using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PLE444.Context
{
	public class PleDbContext : DbContext
	{
        
        public DbSet <Friendship> Friendship { get; set; }
        public DbSet<Community> Communities { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Document> Documents { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventReponse> EventReponses { get; set; }
		public DbSet<Interest> Interests { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserCommunity> UserCommunities { get; set; }
		public DbSet<UserCourse> UserCourses { get; set; }
		public DbSet<UserInterest> UserInterests { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Post> Posts { get; set; }
	}
}