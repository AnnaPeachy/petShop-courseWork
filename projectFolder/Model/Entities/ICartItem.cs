using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model
{
    // Интерфейс для единицы корзины — может быть товаром или услугой
    public interface ICartItem
    {
        string Name { get; }
        decimal GetTotalPrice();
    }

}
