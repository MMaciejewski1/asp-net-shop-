using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using testowo.Models;
using testowo.Migrations;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace testowo.DAL
{
    // public class CourseInitializer:DropCreateDatabaseAlways<CourseContext>
    public class CourseInitializer : MigrateDatabaseToLatestVersion<CourseContext, Configuration>
    {

      //  protected override void Seed(CourseContext context)
        //{

          //  SeedCourseData(context);
            //base.Seed(context);
        //}

       public static void SeedCourseData(CourseContext context)
        {
            var cat = new List<Category>
            {
                new Category(){ CategoryId=1,CategoryName="asp",CategoryDesc="asp",NameFileIcon="asp.png"},
                new Category(){ CategoryId=2,CategoryName="java",CategoryDesc="java",NameFileIcon="java.png"},
                new Category(){ CategoryId=3,CategoryName="php",CategoryDesc="php",NameFileIcon="php.png"},
                new Category(){ CategoryId=4,CategoryName="html",CategoryDesc="html",NameFileIcon="html.png"},
                new Category(){ CategoryId=5,CategoryName="css",CategoryDesc="css",NameFileIcon="css.png"},
                new Category(){ CategoryId=6,CategoryName="xml",CategoryDesc="xml",NameFileIcon="xml.png"},
                new Category(){ CategoryId=7,CategoryName="c#",CategoryDesc="c#",NameFileIcon="c#.png"},
                new Category(){ CategoryId=8,CategoryName="swift",CategoryDesc="swift",NameFileIcon="swift.png"},
                new Category(){ CategoryId=9,CategoryName="objective-c",CategoryDesc="objective-c",NameFileIcon="objective-c.png"},
                new Category(){ CategoryId=10,CategoryName="javascript",CategoryDesc="javascript",NameFileIcon="javascript.png"},

            };
            cat.ForEach(k => context.Category.AddOrUpdate(k));
            var course = new List<Course>
            {
                new Course(){CourseId=4,TitleCourse="asp",CategoryId=1,PriceCourse=666,AutorCourse="Janusz",DescCourse="dobre i tanie elo",Bestseller=true,NamePicture="asp.png",DateAdd=DateTime.Parse("2030-02-02"),ShortDesc="pol godzinki z dotnetcorkiem",Hidden=false },
                new Course(){CourseId=1,TitleCourse="html",CategoryId=1,PriceCourse=666,AutorCourse="Staszek",DescCourse="dobre i tanie elo",Bestseller=true,NamePicture="html.png",DateAdd=DateTime.Parse("2029-02-02"),ShortDesc=" ",Hidden=false },
                new Course(){CourseId=2,TitleCourse="css",CategoryId=1,PriceCourse=666,AutorCourse="Grazyna",DescCourse="dobre i tanie elo",Bestseller=true,NamePicture="css.png",DateAdd=DateTime.Parse("2028-02-02"),ShortDesc="dobre obrazowe",Hidden=false },
                new Course(){CourseId=3,TitleCourse="java",CategoryId=1,PriceCourse=666,AutorCourse="Janusz",DescCourse="dobre i tanie elo",Bestseller=true,NamePicture="asp.png",DateAdd=DateTime.Parse("2027-02-02"),ShortDesc=" ",Hidden=false },
            };
            course.ForEach(k => context.Course.AddOrUpdate(k));
            context.SaveChanges();
        }

        public static void SeedUser(CourseContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            const string name = "admin@praktycznekursy.pl";
            const string password = "P@ssw0rd";
            const string roleName = "Admin";

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, UserData = new UserData() };
                var result = userManager.Create(user, password);
            }

            // utworzenie roli Admin jeśli nie istnieje 
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            // dodanie uzytkownika do roli Admin jesli juz nie jest w roli
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}