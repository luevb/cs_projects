using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PizzeriaDb.Data;
using PizzeriaDb.Enities;

namespace PizzeriaDb
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var db = new PizzaDbContext())
            {
                db.Database.EnsureCreated();
                if (db.Categories.Any())
                {
                    Console.WriteLine("В базе уже есть данные. Очищаем...");
                    db.PizzaSizes.RemoveRange(db.PizzaSizes);
                    db.Pizzas.RemoveRange(db.Pizzas);
                    db.Sizes.RemoveRange(db.Sizes);
                    db.Categories.RemoveRange(db.Categories);
                    db.SaveChanges();
                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence;");
                }
                db.SaveChanges();
                var categories = AddCategories(db);
                var sizes = AddSizes(db);
                var pizzas = AddPizza(db, categories);
                AddPizzaPrices(db, pizzas, sizes);
            }
        }

        static List<Category> AddCategories(PizzaDbContext db)
        {
            var categories = new List<Category>
            {
                new Category {Name = "Классические"},
                new Category {Name = "Острые"},
                new Category {Name = "Вегетарианские"},
                new Category {Name = "Тропические"}
            };
            db.Categories.AddRange(categories);
            db.SaveChanges();
            Console.WriteLine($"Добавили категории.");
            return categories;
        }

        static List<Size> AddSizes(PizzaDbContext db)
        {
            var sizes = new List<Size>
            {
                new Size {Name = "Маленькая", Diametr = 25},
                new Size {Name = "Средняя", Diametr= 30},
                new Size {Name = "Большая", Diametr = 35}
            };
            db.Sizes.AddRange(sizes);
            db.SaveChanges();
            Console.WriteLine("Добавили размеры.");
            return sizes;
        }

        static List<Pizza> AddPizza(PizzaDbContext db, List<Category> categories)
        {
            var pizzas = new List<Pizza>
            {
                new Pizza {
                Name = "Маргарита",
                Description = "Томатный соус, моцарелла, базилик",
                CategoryId = categories[0].Id
                },
                new Pizza
                {
                    Name = "Пепперони",
                    Description = "Томатный соус, пепперони, моцарелла",
                    CategoryId = categories[0].Id
                },
                new Pizza
                {
                    Name = "Четыре сыра",
                    Description = "Сливочный соус, моцарелла, пармезан, горгонзола, фета",
                    CategoryId = categories[2].Id
                },
                new Pizza
                {
                    Name = "Гавайская",
                    Description = "Томатный соус, курица, ананас, моцарелла",
                    CategoryId = categories[3].Id
                },
                new Pizza
                {
                    Name = "Грибная",
                    Description = "Сметанный соус, шампиньоны, лук, моцарелла",
                    CategoryId = categories[2].Id
                },
                new Pizza
                {
                    Name = "Вулкан",
                    Description = "Томатный соус, пепперони, халапеньо, перец чили, моцарелла",
                    CategoryId = categories[1].Id
                }
            };
            db.Pizzas.AddRange(pizzas);
            db.SaveChanges();
            Console.WriteLine("Добавили пиццы.");
            return pizzas;
        }

        static void AddPizzaPrices(PizzaDbContext db, List<Pizza> pizzas, List<Size> sizes)
        {
            var pizzaPrices = new List<PizzaSize>();
            foreach (var pizza in pizzas)
            {
                int defaultPrice = pizza.Name switch
                {
                    "Маргарита" => 349,
                    "Пепперони" => 449,
                    "Четыре сыра" => 419,
                    "Гавайская" => 449,
                    "Грибная" => 379,
                    "Вулкан" => 399,
                    _ => 499
                };

                foreach (var size in sizes)
                {
                    int price = size.Name switch
                    {
                        "Маленькая" => defaultPrice,
                        "Средняя" => defaultPrice + 100,
                        "Большая" => defaultPrice + 200,
                        _ => defaultPrice + 100
                    };

                    pizzaPrices.Add(new PizzaSize
                    {
                        PizzaId = pizza.Id,
                        SizeId = size.Id,
                        Price = price
                    });
                }
            }
            db.PizzaSizes.AddRange(pizzaPrices);
            db.SaveChanges();
            Console.WriteLine("Добавили цены.");
        }
    }
}
