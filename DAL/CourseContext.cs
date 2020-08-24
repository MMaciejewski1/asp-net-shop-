using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace testowo.DAL
{
    public class CourseContext : IdentityDbContext<ApplicationUser>
    {
        public CourseContext() : base("CourseContext")
        {
        }
        static CourseContext() {
            Database.SetInitializer<CourseContext>(new CourseInitializer());
        }
       
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        public static CourseContext Create()
        {
            return new CourseContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // using System.Data.Entity.ModelConfiguration.Conventions;
            // Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel w bazie danych
            // Zamiast Kategorie zostałaby stworzona tabela o nazwie Kategories
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}