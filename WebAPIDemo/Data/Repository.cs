using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Models;

namespace WebAPIDemo.Data
{
    public class Repository : DbContext
    {
        public Repository(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Shirt> Shirts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //data seed

            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { ShirtId = 1, Brand = "Polo", Color = "Blue", Size = 'M', Gender = "Men", Price = 200 },
                new Shirt { ShirtId = 2, Brand = "Polo", Color = "Red", Size = 'L', Gender = "Men", Price = 300 },
                new Shirt { ShirtId = 3, Brand = "Polo", Color = "Green", Size = 'S', Gender = "Women", Price = 100 },
                new Shirt { ShirtId = 4, Brand = "Polo", Color = "Black", Size = 'M', Gender = "Men", Price = 500 });
        }
    }
}
