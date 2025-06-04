using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    public abstract class ShopItem : ICartItem
    {
        public string Name { get; set; }
        public abstract decimal GetTotalPrice();
    }
}

