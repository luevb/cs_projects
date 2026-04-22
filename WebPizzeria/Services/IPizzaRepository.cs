using WebPizzeria.Models;

namespace WebPizzeria.Services;

public interface IPizzaRepository
{
    Task<IEnumerable<Pizza>> GetAllAsync();
    Task<Pizza?> GetByIdAsync(int id);
    Task AddAsync(Pizza pizza);
    Task UpdateAsync(Pizza pizza);
    Task DeleteAsync(int id);
}