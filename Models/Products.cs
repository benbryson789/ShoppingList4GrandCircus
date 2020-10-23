using System;
using System.Collections.Generic;

namespace ShoppingList4.Models
{
    public partial class Products
    {
        public Products()
        {
            CartItems = new HashSet<CartItems>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
