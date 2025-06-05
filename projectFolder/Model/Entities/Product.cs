using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    public class Product : ShopItem
    {
        public decimal PricePerUnit { get; set; }
        public bool RequiresWeighing { get; set; }
        public decimal? Weight { get; set; } // null, если товар ещё не взвешен
        public string Category { get; set; }

        // Возвращает цену товара — если товар требует взвешивания, то умножает на вес
        public override decimal GetTotalPrice()
        {
            if (RequiresWeighing)
            {
                if (Weight == null || Weight <= 0)
                    throw new InvalidOperationException("Товар требует взвешивания.");
                return PricePerUnit * (decimal)Weight;
            }
            return PricePerUnit;
        }
    }
}

