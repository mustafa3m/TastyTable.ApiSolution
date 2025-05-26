using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuApi.Domain.Entities
{
    public class Menu
    {
        public int Id {  get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int PurchaseQuantity { get; set; }


        //public string Description { get; set; }

        //public string? Category { get; set; }
        //public string SpecialTag { get; set; }
        //public double Price { get; set; }
        //public string Image { get; set; }
    }
}
