namespace PLE444.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<Models.PleDbContext>
	{
		public Configuration() {
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(Models.PleDbContext context) {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
		}
	}
}
