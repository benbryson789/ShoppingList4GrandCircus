using System;
using System.Collections.Generic;

namespace ShoppingList4.Models
{
    public partial class Carts
    {
        public Carts()
        {
            CartItems = new HashSet<CartItems>();
        }

        public int CartId { get; set; }

        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
