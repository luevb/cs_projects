using System.ComponentModel.DataAnnotations;

namespace PizzeriaDb.Enities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<Pizza> Pizzas { get; set; }
        public Category(string Name) { this.Name = Name; }
    }
}
