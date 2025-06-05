using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    // Абстрактный базовый класс для товаров и услуг
    public abstract class ShopItem : ICartItem
    {
        public string Name { get; set; }

        // Метод, который должен быть реализован в подклассах
        public abstract decimal GetTotalPrice();
    }
}


