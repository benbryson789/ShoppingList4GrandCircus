using System;
using System.Collections.Generic;

namespace ShoppingList4.Models
{
    public partial class CartItems
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }

        public virtual Carts Cart { get; set; }
        public virtual Products Product { get; set; }
    }
}
