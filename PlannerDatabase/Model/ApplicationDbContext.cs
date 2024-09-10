using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Model
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Item> Items { get; set; }
        //public ApplicationDbContext()
        //{
        //    Database.EnsureDeleted(); // гарантируем, что бд удалена
        //    Database.EnsureCreated(); // гарантируем, что бд будет создана
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {            
            string connectionString = @"
			    Data Source=HOME\SQLEXPRESS;
			    Initial Catalog=Planner_db;
                Integrated Security = false;
                TrustServerCertificate=true;
			    Integrated Security=SSPI;
			    Timeout=5;
		    ";
            if (!options.IsConfigured)
            {
                options.UseSqlServer(connectionString, bilder => bilder.EnableRetryOnFailure());
            }
        }
        
    }
}
