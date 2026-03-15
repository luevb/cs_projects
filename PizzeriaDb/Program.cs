using PizzeriaDb.Data;
using PizzeriaDb.Enities;

namespace PizzeriaDb
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new PizzaDbContext();
            Console.WriteLine("Hello, World!");
        }
    }
}
