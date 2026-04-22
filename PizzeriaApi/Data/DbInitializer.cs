using PizzeriaApi.Models;

namespace PizzeriaApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PizzaDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Pizzas.Any())
            {
                Console.WriteLine("База данных уже содержит данные.");
                return;
            }

            Console.WriteLine("Заполняем базу данных тестовыми данными...");

            // Категории
            var categories = new List<Category>
        {
            new Category("Классические"),
            new Category("Острые"),
            new Category("Вегетарианские")
        };
            context.Categories.AddRange(categories);
            context.SaveChanges();
            Console.WriteLine($"Добавлено категорий: {categories.Count}");

            // Размеры
            var sizes = new List<Size>
        {
            new Size("Маленькая", 25),
            new Size("Стандартная", 30),
            new Size("Большая", 35)
        };
            context.Sizes.AddRange(sizes);
            context.SaveChanges();
            Console.WriteLine($"Добавлено размеров: {sizes.Count}");

            // Пиццы
            var pizzas = new List<Pizza>
        {
            new Pizza
            {
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
            }
        };
            context.Pizzas.AddRange(pizzas);
            context.SaveChanges();
            Console.WriteLine($"Добавлено пицц: {pizzas.Count}");

            // Цены
            // Базовая цена для каждой пиццы (маленький размер)
            var basePrice = new Dictionary<string, int>
            {
                ["Маргарита"] = 349,
                ["Пепперони"] = 449,
                ["Четыре сыра"] = 419
            };

            // Наценка за размер
            var sizePrice = new Dictionary<string, int>
            {
                ["Маленькая"] = 0,
                ["Стандартная"] = 100,
                ["Большая"] = 200
            };

            var pizzaSizes = new List<PizzaSize>();

            foreach (var pizza in pizzas)
            {
                int currentPrice = basePrice[pizza.Name];

                foreach (var size in sizes)
                {
                    int markup = sizePrice[size.Name];
                    pizzaSizes.Add(new PizzaSize(pizza.Id, size.Id, currentPrice + markup));
                }
            }

            context.PizzaSizes.AddRange(pizzaSizes);
            context.SaveChanges();
            Console.WriteLine($"Добавлено цен: {pizzaSizes.Count}");

            Console.WriteLine($"Инициализация завершена!");
            Console.WriteLine($"Итого: {categories.Count} категорий, {sizes.Count} размеров, {pizzas.Count} пицц, {pizzaSizes.Count} цен");
        }
    }
}
