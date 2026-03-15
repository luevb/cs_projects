using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaDb.Enities
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

        public Category Category { get; set; }

        [NotMapped]
        public string Description { get; set; }

        public ICollection<PizzaSize> PizzaSizes { get; set; }

    }
}
