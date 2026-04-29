using Microsoft.EntityFrameworkCore;
using PizzeriaApi.Data;
using PizzeriaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PizzaDbContext>(options =>
    options.UseSqlite(connectionString));

// Сервисы
builder.Services.AddScoped<IPizzaService, PizzaService>();
// Для отступов
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

var app = builder.Build();

// Тестовые данные
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();
    DbInitializer.Initialize(dbContext);
}

// Endpoints

// GET /api/pizzas - основной endpoint
app.MapGet("/api/pizzas", async (IPizzaService pizzaService) =>
{
    var pizzas = await pizzaService.GetPizzasAsync();

    // Форматируем результат
    var result = pizzas.Select(p => new
    {
        p.Id,
        p.Name,
        p.Description,
        Category = p.Category?.Name,
        MinPrice = p.PizzaSizes.Any() ? p.PizzaSizes.Min(ps => ps.Price) : 0,
        Sizes = p.PizzaSizes.Select(ps => new
        {
            Size = ps.Size?.Name,
            Diameter = ps.Size?.Diametr,
            ps.Price
        })
    });

    return Results.Ok(result);
});

// GET /api/config - endpoint для конфигурации
app.MapGet("/api/config", (IConfiguration config) =>
{
    return Results.Ok(new
    {
        AppName = config["AppSettings:AppName"],
        Version = config["AppSettings:Version"],
        MaxItems = config.GetValue<int>("AppSettings:MaxItems")
    });
});

// GET /api/pizzas/{id} - конкретная пицца
app.MapGet("/api/pizzas/{id}", async (int id, PizzaDbContext db) =>
{
    var pizza = await db.Pizzas
        .Include(p => p.Category)
        .Include(p => p.PizzaSizes)
            .ThenInclude(ps => ps.Size)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (pizza == null)
        return Results.NotFound(new { message = $"Пицца с id {id} не найдена" });

    return Results.Ok(new
    {
        pizza.Id,
        pizza.Name,
        pizza.Description,
        Category = pizza.Category?.Name,
        Sizes = pizza.PizzaSizes.Select(ps => new
        {
            Size = ps.Size?.Name,
            Diameter = ps.Size?.Diametr,
            ps.Price
        })
    });
});

// Корневой endpoint для проверки
app.MapGet("/", () => "Pizzeria API is running! Use /api/pizzas to get data \n" +
"Use /api/config to get config \n" +
"Use /api/pizzas/{id} to get the pizza information");

app.Run();