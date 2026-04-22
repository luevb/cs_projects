using System.ComponentModel.DataAnnotations;

namespace PizzeriaApi.Models
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Название пиццы")]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<PizzaSize> PizzaSizes { get; set; }
    }
}
