using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model.Payment
{
    // Оплата бонусами
    public class BonusPayment : IPaymentStrategy
    {
        public string Name => "Бонусы";

        public bool CanPay(decimal amount, Customer customer)
        {
            if (customer.BonusBalance >= amount)
            {
                customer.BonusBalance -= (int)amount; // 1 бонус = 1 рубль
                return true;
            }
            return false;
        }
    }
}
