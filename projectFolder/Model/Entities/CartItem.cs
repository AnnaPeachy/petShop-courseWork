using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    public class CartItem
    {
        public ShopItem Item { get; set; }
        public double Quantity { get; set; } = 1;

        public CartItem(ShopItem item, double quantity = 1)
        {
            Item = item;
            Quantity = quantity;
        }

        // Вычисляет общую стоимость товара с учётом количества
        public decimal GetTotalPrice()
        {
            return Item.GetTotalPrice() * (decimal)Quantity;
        }

        // Возвращает строку с информацией для отображения в корзине
        public string DisplayInfo()
        {
            return $"{Item.Name} x{Quantity} = {GetTotalPrice():C}";
        }
    }
}

