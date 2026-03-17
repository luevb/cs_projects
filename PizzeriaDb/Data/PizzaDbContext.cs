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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=pizzeria.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PizzaSize>().HasKey(ps => new { ps.SizeId, ps.PizzaId });

            modelBuilder.Entity<Size>()
                .HasIndex(s => s.Name)
                .IsUnique();
            modelBuilder.Entity<Pizza>().HasIndex(p => new { p.Name, p.CategoryId });

            modelBuilder.Entity<PizzaSize>()
                .HasOne(ps => ps.Pizza)
                .WithMany(p => p.PizzaSizes)
                .HasForeignKey(ps => ps.PizzaId);

            modelBuilder.Entity<PizzaSize>()
                .HasOne(ps => ps.Size)
                .WithMany(s => s.PizzaSizes)
                .HasForeignKey(ps => ps.SizeId);

            modelBuilder.Entity<Pizza>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Pizzas)
                .HasForeignKey(p => p.CategoryId);
        }

       
    }
}
