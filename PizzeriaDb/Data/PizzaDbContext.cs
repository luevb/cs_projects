using Microsoft.EntityFrameworkCore;
using PizzeriaDb.Enities;

namespace PizzeriaDb.Data
{
    public class PizzaDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<PizzaSize> PizzaSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PizzaSize>()
                .HasKey(ps => new { ps.SizeId, ps.PizzaId });
            modelBuilder.Entity<Size>()
                .HasIndex(s => s.Name)
                .IsUnique();
            modelBuilder.Entity<Pizza>()
                .HasIndex(p => new { p.Name, p.CategoryId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=pizzeria.db");
        }
    }
}
