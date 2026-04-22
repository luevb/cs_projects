using Microsoft.EntityFrameworkCore;
using PizzeriaApi.Models;

namespace PizzeriaApi.Data
{
    public class PizzaDbContext : DbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<PizzaSize> PizzaSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PizzaSize>()
                .HasKey(ps => new { ps.PizzaId, ps.SizeId });

            modelBuilder.Entity<Size>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Pizza>()
                .HasIndex(p => new { p.Name, p.CategoryId });
        }
    }
}
