using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using petShop_courseWork.Model;

namespace petShop_courseWork.Model.Payment
{
    public interface IPaymentStrategy
    {
        string Name { get; } // Название метода оплаты (для отображения)
        bool CanPay(decimal amount, Customer customer); // true, если оплата прошла успешно
    }
}
