using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    public class CartItem
    {
        public ICartItem Item { get; set; }
        public double Quantity { get; set; } = 1;

        public CartItem(ICartItem item, double quantity = 1)
        {
            Item = item;
            Quantity = quantity;
        }


        public decimal GetTotalPrice()
        {
            return Item.GetTotalPrice() * (decimal)Quantity;
        }

        public string DisplayInfo()
        {
            return $"{Item.Name} x{Quantity} = {GetTotalPrice():C}";
        }
    }

}
