using System.ComponentModel.DataAnnotations;

namespace PizzeriaDb.Enities
{
    public class PizzaSize
    {
        [Required]
        public int PizzaId { get; set; }
        [Required]
        public int SizeId { get; set; }
        [Required]
        public int Price { get; set; }

        public Pizza Pizza { get; set; }
        public Size Size { get; set; }
        public PizzaSize(int PizzaId, int SizeId, int Price)
        {
            this.PizzaId = PizzaId;
            this.SizeId = SizeId;
            this.Price = Price;
        }

    }
}
