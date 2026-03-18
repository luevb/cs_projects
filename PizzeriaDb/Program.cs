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
                if (!db.Categories.Any())
                {
                    Console.WriteLine("Заполняем базу данных...");

                    var categories = AddCategories(db);
                    var sizes = AddSizes(db);
                    var pizzas = AddPizza(db, categories);
                    AddPizzaPrice(db, pizzas, sizes);

                    Console.WriteLine("Готово!");
                }
                while (true)
                {
                    Console.WriteLine("Добро пожаловать в пиццерию. Здесь вы можете ознакомиться с меню.");
                    Console.WriteLine("1. Показать меню");
                    Console.WriteLine("2. Показать статистику");
                    Console.WriteLine("3. Выход из приложения");
                    Console.Write("Выберите: ");
                    string choice = Console.ReadLine();
                    Console.WriteLine();
                    switch (choice)
                    {
                        case "1":
                            ShowMenu(db);
                            return;
                        case "2":
                            Console.WriteLine("");
                            Console.WriteLine($"Количество категорий - {db.Categories.Count()}");
                            Console.WriteLine($"Количество пицц - {db.Pizzas.Count()}");
                            Console.WriteLine($"Количество размеров - {db.Sizes.Count()}");
                            return;
                        case"3":
                            Console.WriteLine("До свидания. Выход...");
                            return;
                        default:
                            Console.WriteLine("Неправильно набран номер! Попробуйте еще раз.");
                            break;
                    }
                }
            }
        }

        static void ShowMenu( PizzaDbContext db)
        {
            var pizza = db.Pizzas.Include(p => p.Category).Include(p => p.PizzaSizes).ThenInclude(ps => ps.Size).ToList();
            Console.WriteLine("***Menu***");
            foreach(var p in pizza)
            {
                Console.WriteLine($"- Пицца {p.Name}, категория: {p.Category.Name}");
                Console.WriteLine($"- Описание: {p.Description}");
                var price = p.PizzaSizes.Min(ps => ps.Price);
                Console.WriteLine($"- Цена - {price}");
                Console.WriteLine();
            }
            Console.WriteLine("*Цены указаны на маленький размер пиццы*");
        }

        static List<Category> AddCategories(PizzaDbContext db)
        {
            var categories = new List<Category>()
            {
                new Category("Классические"),
                new Category("Острые"),
                new Category("Вегетарианские")
            };
            db.Categories.AddRange(categories);
            db.SaveChanges();
            return categories;
        }
        
        static List<Size> AddSizes(PizzaDbContext db)
        {
            var sizes = new List<Size>()
            {
                new Size("Маленькая", 25),
                new Size("Стандартная", 30),
                new Size("Большая", 35)
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
                    CategoryId = categories[0].Id,
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

        static void AddPizzaPrice(PizzaDbContext db,  List<Pizza> pizzas, List<Size> sizes)
        {
            var basePrice = new Dictionary<string, int>
            {
                ["Маргарита"] = 349,
                ["Пепперони"] = 449,
                ["Четыре сыра"] = 419,
                ["Гавайская"] = 449,
                ["Грибная"] = 379,
                ["Вулкан"] = 399,
                ["Вулкан v2"] = 429
            };

            var sizePrice = new Dictionary<string, int>
            {
                ["Маленькая"] = 0,
                ["Стандартная"] = 100,
                ["Большая"] = 200
            };
            var pizzaPrice = new List<PizzaSize>();
            foreach(var pizza in pizzas)
            {
                int currentPrice = basePrice.GetValueOrDefault(pizza.Name, 500);
                foreach(var size in sizes)
                {
                    int markup = sizePrice.GetValueOrDefault(size.Name, 100);
                    pizzaPrice.Add(new PizzaSize(pizza.Id, size.Id, markup + currentPrice));
                }
            }
            db.PizzaSizes.AddRange(pizzaPrice);
            db.SaveChanges();

        }
    }
}