using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Model.Payment
{
    // Оплата картой
    public class CardPayment : IPaymentStrategy
    {
        public string Name => "Банковская карта";

        public bool CanPay(decimal amount, Customer customer)
        {
            if (customer.CardBalance >= amount)
            {
                customer.CardBalance -= amount;
                return true;
            }
            return false;
        }
    }
}
