using WebPizzeria.Models;

namespace WebPizzeria.Services;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateStatusAsync(int id, string status);
}