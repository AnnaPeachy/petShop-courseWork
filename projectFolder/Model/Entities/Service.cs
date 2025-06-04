using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    public class Service : ShopItem
    {
        public decimal Price { get; set; }
        public string Description { get; set; }

        public override decimal GetTotalPrice()
        {
            return Price;
        }
    }


}
