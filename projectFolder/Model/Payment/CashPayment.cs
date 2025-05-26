using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model.Payment
{
    // Оплата наличными
    public class CashPayment : IPaymentStrategy
    {
        public string Name => "Наличные";

        public bool CanPay(decimal amount, Customer customer)
        {
            if (customer.WalletBalance >= amount)
            {
                customer.WalletBalance -= amount;
                return true;
            }
            return false; // Недостаточно средств
        }
    }
}
