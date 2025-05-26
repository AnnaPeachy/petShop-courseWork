using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    public interface ICartItem
    {
        string Name { get; }
        decimal GetTotalPrice();
    }

}
