using System;
using System.Collections.Generic;
using petShop_courseWork.Model;
using petShop_courseWork.Model.Payment;
using petShop_courseWork.View;

namespace petShop_courseWork.ConsoleApp
{
    public class ConsoleShopView : IShopView
    {
        private Customer _customer;

        public ConsoleShopView(Customer customer)
        {
            _customer = customer;
        }
        public void DisplayCustomerInfo(Customer customer)
        {
            Console.WriteLine("\n===== Информация о покупателе =====");
            Console.WriteLine($"Имя: {customer.Name}");
            Console.WriteLine($"Наличные: {customer.WalletBalance} руб.");
            Console.WriteLine($"Карта: {customer.CardBalance} руб.");
            Console.WriteLine($"Бонусы: {customer.BonusBalance} бонусов. (1 бонус = 1 рубль)");
            Console.WriteLine($"Товаров в корзине: {customer.ShoppingCart.Count}");
            Console.WriteLine("===================================");
        }

        public void ShowMainMenu()
        {
            Console.WriteLine("=== Главное меню ===");
            Console.WriteLine("1. Просмотреть товары");
            Console.WriteLine("2. Просмотреть услуги");
            Console.WriteLine("3. Просмотреть корзину");
            Console.WriteLine("4. Оплатить");
            Console.WriteLine("5. Пополнить баланс");
            Console.WriteLine("0. Выйти");
        }

        public int GetMainMenuChoice()
        {
            Console.Write("Введите нужное действие (цифру): ");
            return ReadInt();
        }

        public bool ConfirmExit()
        {
            Console.Write("\nСохранить данные перед выходом? (д/н): ");
            string input = Console.ReadLine()?.ToLower();
            return input == "д" || input == "y"; // только "да" — true
        }


        public void DisplayProducts(List<Product> products)
        {
            Console.WriteLine("\n--- Список товаров ---");
            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                Console.WriteLine($"{i}. ({p.Category}) {p.Name} - {p.PricePerUnit} руб. {(p.RequiresWeighing ? "(по весу)" : "")}");
            }
        }

        public void DisplayServices(List<Service> services)
        {
            Console.WriteLine("\n--- Список услуг ---");
            for (int i = 0; i < services.Count; i++)
            {
                var s = services[i];
                Console.WriteLine($"{i}. {s.Name} - {s.Price} руб. ({s.Description})");
            }
        }

        public int GetProductSelection(List<Product> products)
        {
            Console.Write("Введите номер товара (или -1 для отмены): ");
            return ReadInt();
        }

        public int GetServiceSelection(List<Service> services)
        {
            Console.Write("Введите номер услуги (или -1 для отмены): ");
            return ReadInt();
        }

        public void ShowCart(List<CartItem> cart)
        {
            Console.WriteLine("\n=== Корзина ===");
            if (cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            foreach (var item in cart)
            {
                Console.WriteLine(item.DisplayInfo());
            }

            Console.WriteLine($"Итог: {GetCartTotal(cart):F2} руб.\n");
        }

        public bool ConfirmPurchase()
        {
            Console.Write("Подтвердить покупку? (д/н): ");
            string input = Console.ReadLine()?.ToLower();
            return input == "д" || input == "y";
        }

        public int GetPaymentChoice()
        {
            Console.WriteLine("\nВыберите способ оплаты:");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Карта");
            Console.WriteLine("3. Бонусы");
            Console.Write("Ваш выбор: ");
            return ReadInt();
        }

        public decimal GetPartialPaymentAmount()
        {
            Console.Write("Введите сумму оплаты: ");
            return ReadDecimal();
        }

        public decimal GetPaymentAmount(decimal remaining, IPaymentStrategy strategy, Customer customer)
        {
            decimal maxAvailable = 0;

            if (strategy is CashPayment)
                maxAvailable = customer.WalletBalance;
            else if (strategy is CardPayment)
                maxAvailable = customer.CardBalance;
            else if (strategy is BonusPayment)
                maxAvailable = customer.BonusBalance;

            decimal maxAmount = Math.Min(remaining, maxAvailable);

            Console.Write($"Введите сумму для оплаты {strategy.Name} (макс. {maxAmount} руб.): ");
            decimal amount = ReadDecimal();

            return Math.Min(amount, maxAmount);
        }

        public void DisplayPaymentOptions(Customer customer)
        {
            Console.WriteLine("\nДоступные средства:");
            Console.WriteLine($"1. Наличные: {customer.WalletBalance} руб.");
            Console.WriteLine($"2. Карта: {customer.CardBalance} руб.");
            Console.WriteLine($"3. Бонусы: {customer.BonusBalance} руб.");
        }

        public int GetTopUpMethod()
        {
            Console.WriteLine("\nВыберите способ пополнения:");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Банковская карта");
            Console.Write("Ваш выбор: ");
            return ReadInt();
        }

        public int GetItemToRemove(List<CartItem> cart)
        {
            Console.WriteLine("\nВведите номер товара для удаления:");
            for (int i = 0; i < cart.Count; i++)
            {
                Console.WriteLine($"{i}. {cart[i].Item.Name} - {cart[i].GetTotalPrice()} руб.");
            }
            Console.Write("Ваш выбор (или -1 для отмены): ");
            return ReadInt();
        }

        public decimal GetPartialPaymentAmount(decimal maxAmount)
        {
            Console.Write($"Введите сумму оплаты (макс. {maxAmount} руб.): ");
            decimal amount = ReadDecimal();
            return Math.Min(amount, maxAmount);
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        // Вспомогательные методы
        private int ReadInt()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
                Console.Write("Неверный ввод. Повторите: ");
            return result;
        }

        private decimal ReadDecimal()
        {
            decimal result;
            while (!decimal.TryParse(Console.ReadLine(), out result))
                Console.Write("Неверный ввод. Повторите: ");
            return result;
        }

        private decimal GetCartTotal(List<CartItem> cart)
        {
            decimal total = 0;
            foreach (var item in cart)
                total += item.GetTotalPrice();
            return total;
        }
        public decimal GetProductWeight()
        {
            Console.Write("Введите вес товара (кг): ");
            return ReadDecimal();
        }
        // Запрос баланса перед стартом
        public void RequestInitialCustomer()
        {
            Console.Write("Введите имя клиента: ");
            _customer.Name = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите баланс на кошельке: ");
            decimal walletBalance;
            while (!decimal.TryParse(Console.ReadLine(), out walletBalance))
            {
                Console.Write("Неверный ввод. Введите баланс на кошельке: ");
            }
            _customer.WalletBalance = walletBalance;

            Console.Write("Введите баланс на карте: ");
            decimal cardBalance;
            while (!decimal.TryParse(Console.ReadLine(), out cardBalance))
            {
                Console.Write("Неверный ввод. Введите баланс на карте: ");
            }
            _customer.CardBalance = cardBalance;

            Console.Write("Введите количество бонусов: ");
            decimal bonusBalance;
            while (!decimal.TryParse(Console.ReadLine(), out bonusBalance))
            {
                Console.Write("Неверный ввод. Введите количество бонусов: ");
            }
            _customer.BonusBalance = bonusBalance;
        }
    }
}
