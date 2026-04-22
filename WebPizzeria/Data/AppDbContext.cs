using Microsoft.EntityFrameworkCore;
using WebPizzeria.Models;

namespace WebPizzeria.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PizzaIngredient> PizzaIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PizzaIngredient>()
            .HasKey(pi => new { pi.PizzaId, pi.IngredientId });

        modelBuilder.Entity<PizzaIngredient>()
            .HasOne(pi => pi.Pizza)
            .WithMany(p => p.PizzaIngredients)
            .HasForeignKey(pi => pi.PizzaId);

        modelBuilder.Entity<PizzaIngredient>()
            .HasOne(pi => pi.Ingredient)
            .WithMany(i => i.PizzaIngredients)
            .HasForeignKey(pi => pi.IngredientId);
    }
}