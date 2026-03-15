using Microsoft.EntityFrameworkCore;
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
                db.Database.Migrate();

                // Проверяем, нужно ли заполнять БД начальными данными
                if (!db.Categories.Any() && !db.Sizes.Any())
                {
                    Console.WriteLine("Первоначальная настройка базы данных...");
                    InitializeDatabase(db);
                }
                else
                {
                    Console.WriteLine("База данных уже содержит данные.");
                    ShowDatabaseStats(db);
                }

                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static void InitializeDatabase(PizzaDbContext db)
        {
            Console.WriteLine("Добавление категорий...");
            var categories = AddCategories(db);

            Console.WriteLine("Добавление размеров...");
            var sizes = AddSizes(db);

            Console.WriteLine("Добавление пицц...");
            var pizzas = AddPizza(db, categories);

            Console.WriteLine("Добавление цен...");
            AddPizzaPrices(db, pizzas, sizes);

            Console.WriteLine("База данных успешно инициализирована!");
        }

        static void ShowDatabaseStats(PizzaDbContext db)
        {
            var stats = new
            {
                Categories = db.Categories.Count(),
                Sizes = db.Sizes.Count(),
                Pizzas = db.Pizzas.Count(),
            };

            Console.WriteLine($"Статистика базы данных:");
            Console.WriteLine($"- Категорий: {stats.Categories}");
            Console.WriteLine($"- Размеров: {stats.Sizes}");
            Console.WriteLine($"- Пицц: {stats.Pizzas}");
        }
        static List<Category> AddCategories(PizzaDbContext db)
        {
            var categories = new List<Category>
            {
                new Category { Name = "Классические" },
                new Category { Name = "Острые" },
                new Category { Name = "Вегетарианские" },
                new Category { Name = "Тропические" }
            };
            db.Categories.AddRange(categories);
            db.SaveChanges();
            return categories;
        }

        static List<Size> AddSizes(PizzaDbContext db)
        {
            var sizes = new List<Size>
            {
                new Size { Name = "Маленькая", Diametr = 25 },
                new Size { Name = "Средняя", Diametr = 30 },
                new Size { Name = "Большая", Diametr = 35 }
            };
            db.Sizes.AddRange(sizes);
            db.SaveChanges();
            return sizes;
        }

        static List<Pizza> AddPizza(PizzaDbContext db, List<Category> categories)
        {
            var pizzas = new List<Pizza>
            {
                new Pizza {
                    Name = "Маргарита",
                    Description = "Томатный соус, моцарелла, базилик",
                    CategoryId = categories[0].Id,
                },
                new Pizza {
                    Name = "Пепперони",
                    Description = "Томатный соус, пепперони, моцарелла",
                    CategoryId = categories[0].Id,
                },
                new Pizza {
                    Name = "Четыре сыра",
                    Description = "Сливочный соус, моцарелла, пармезан, горгонзола, фета",
                    CategoryId = categories[2].Id,
                },
                new Pizza {
                    Name = "Гавайская",
                    Description = "Томатный соус, курица, ананас, моцарелла",
                    CategoryId = categories[3].Id,
                },
                new Pizza {
                    Name = "Грибная",
                    Description = "Сметанный соус, шампиньоны, лук, моцарелла",
                    CategoryId = categories[2].Id,
                },
                new Pizza {
                    Name = "Вулкан",
                    Description = "Томатный соус, пепперони, халапеньо, перец чили, моцарелла",
                    CategoryId = categories[1].Id,
                },
                new Pizza {
                    Name = "Вулкан v2",
                    Description = "Томатный соус, пепперони, халапеньо, перец чили, моцарелла",
                    CategoryId = categories[1].Id,
                }
            };
            db.Pizzas.AddRange(pizzas);
            db.SaveChanges();
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
        }
    }
}