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
        base.OnModelCreating(modelBuilder);

        // Composite primary key for many-to-many
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

        // Seed some pizzas
        modelBuilder.Entity<Pizza>().HasData(
            new Pizza { Id = 1, Name = "Маргарита", Description = "Томатный соус, моцарелла, базилик", Price = 450, Size = "Medium", IsVegetarian = true, ImageUrl = "/images/margherita.jpg" },
            new Pizza { Id = 2, Name = "Пепперони", Description = "Пепперони, моцарелла, томатный соус", Price = 550, Size = "Medium", IsVegetarian = false, ImageUrl = "/images/pepperoni.jpg" },
            new Pizza { Id = 3, Name = "Гавайская", Description = "Курица, ананас, моцарелла", Price = 600, Size = "Medium", IsVegetarian = false, ImageUrl = "/images/hawaiian.jpg" }
        );
    }
}