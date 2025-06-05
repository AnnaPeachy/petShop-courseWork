using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    // Представляет услугу, которую можно добавить в корзину
    public class Service : ShopItem
    {
        public decimal Price { get; set; }
        public string Description { get; set; }

        // Услуга имеет фиксированную цену, возвращаем её
        public override decimal GetTotalPrice()
        {
            return Price;
        }
    }
}

