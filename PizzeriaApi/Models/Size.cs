using System.ComponentModel.DataAnnotations;

namespace PizzeriaApi.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Diametr { get; set; }

        public ICollection<PizzaSize> PizzaSizes { get; set; }

        public Size() { }

        public Size(string name, int diametr)
        {
            Name = name;
            Diametr = diametr;
        }
    }
}
