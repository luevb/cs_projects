namespace WebPizzeria.Services;

public class CartItem
{
    public int PizzaId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class CartService
{
    private List<CartItem> _items = new();
    public event Action? OnChange;

    public IReadOnlyList<CartItem> Items => _items;
    public int TotalCount => _items.Sum(i => i.Quantity);
    public decimal TotalPrice => _items.Sum(i => i.Price * i.Quantity);

    public void AddItem(int pizzaId, string name, decimal price, int quantity = 1)
    {
        var existing = _items.FirstOrDefault(i => i.PizzaId == pizzaId);
        if (existing != null)
            existing.Quantity += quantity;
        else
            _items.Add(new CartItem { PizzaId = pizzaId, Name = name, Price = price, Quantity = quantity });
        OnChange?.Invoke();
    }

    public void RemoveItem(int pizzaId)
    {
        _items.RemoveAll(i => i.PizzaId == pizzaId);
        OnChange?.Invoke();
    }

    public void Clear()
    {
        _items.Clear();
        OnChange?.Invoke();
    }
}