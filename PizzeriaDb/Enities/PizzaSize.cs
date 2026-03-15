using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
    }
}
