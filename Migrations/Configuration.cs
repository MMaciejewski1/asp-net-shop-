namespace testowo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using testowo.DAL;

    public sealed class Configuration : DbMigrationsConfiguration<testowo.DAL.CourseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "testowo.DAL.CourseContext";
        }

        protected override void Seed(testowo.DAL.CourseContext context)
        {
            CourseInitializer.SeedCourseData(context);
            CourseInitializer.SeedUser(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
