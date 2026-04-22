using System.ComponentModel.DataAnnotations;

namespace PizzeriaApi.Models
{
    public class PizzaSize
    {
        [Required]
        public int PizzaId { get; set; }

        [Required]
        public int SizeId { get; set; }

        [Required]
        public int Price { get; set; }

        public Pizza? Pizza { get; set; }
        public Size? Size { get; set; }

        public PizzaSize() { }

        public PizzaSize(int pizzaId, int sizeId, int price)
        {
            PizzaId = pizzaId;
            SizeId = sizeId;
            Price = price;
        }
    }
}
