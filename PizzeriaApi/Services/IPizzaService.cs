using PizzeriaApi.Models;

namespace PizzeriaApi.Services
{
    public interface IPizzaService
    {
        Task<List<Pizza>> GetPizzasWithMinPriceAsync();
    }
}
