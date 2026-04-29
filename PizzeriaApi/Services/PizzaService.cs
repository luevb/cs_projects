using Microsoft.EntityFrameworkCore;
using PizzeriaApi.Data;
using PizzeriaApi.Models;

namespace PizzeriaApi.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly PizzaDbContext _context;

        public PizzaService(PizzaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pizza>> GetPizzasAsync()
        {
            var pizzas = await _context.Pizzas
                .Include(p => p.Category)
                .Include(p => p.PizzaSizes)
                .ThenInclude(ps => ps.Size)
                .ToListAsync();

            return pizzas;
        }
    }
}
