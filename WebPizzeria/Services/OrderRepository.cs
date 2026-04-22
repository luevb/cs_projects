using Microsoft.EntityFrameworkCore;
using WebPizzeria.Data;
using WebPizzeria.Models;

namespace WebPizzeria.Services;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Pizza).ToListAsync();

    public async Task<Order?> GetByIdAsync(int id) =>
        await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, string status)
    {
        var order = await GetByIdAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}