using System;
using System.Collections.Generic;
using petShop_courseWork.Model;
using petShop_courseWork.Model.Payment;
using petShop_courseWork.View;

namespace petShop_courseWork.ConsoleApp
{
    /// <summary>
    /// Консольное представление магазина - реализация интерфейса IShopView
    /// </summary>
    public class ConsoleShopView : IShopView
    {
        private Customer _customer; // Текущий покупатель

        public ConsoleShopView(Customer customer)
        {
            _customer = customer; // Инициализация покупателя
        }

        // Отображение информации о покупателе
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

        // Отображение главного меню
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

        // Получение выбора из главного меню
        public int GetMainMenuChoice()
        {
            Console.Write("Введите нужное действие (цифру): ");
            return ReadInt(); // Использование вспомогательного метода
        }

        // Подтверждение выхода с сохранением
        public bool ConfirmExit()
        {
            Console.Write("\nСохранить данные перед выходом? (д/н): ");
            string input = Console.ReadLine()?.ToLower();
            return input == "д" || input == "l"; // только "да" — true
        }

        // Отображение списка товаров
        public void DisplayProducts(List<Product> products)
        {
            Console.WriteLine("\n--- Список товаров ---");
            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                Console.WriteLine($"{i}. ({p.Category}) {p.Name} - {p.PricePerUnit} руб. {(p.RequiresWeighing ? "(по весу)" : "")}");
            }
        }

        // Отображение списка услуг
        public void DisplayServices(List<Service> services)
        {
            Console.WriteLine("\n--- Список услуг ---");
            for (int i = 0; i < services.Count; i++)
            {
                var s = services[i];
                Console.WriteLine($"{i}. {s.Name} - {s.Price} руб. ({s.Description})");
            }
        }

        // Выбор товара из списка
        public int GetProductSelection(List<Product> products)
        {
            Console.Write("Введите номер товара (или -1 для отмены): ");
            return ReadInt();
        }

        // Выбор услуги из списка
        public int GetServiceSelection(List<Service> services)
        {
            Console.Write("Введите номер услуги (или -1 для отмены): ");
            return ReadInt();
        }

        // Отображение содержимого корзины
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
        }

        // Получение выбора в меню корзины
        public int GetCartMenuChoice()
        {
            Console.Write("Введите ваш выбор: ");
            return ReadInt();
        }

        // Подтверждение покупки
        public bool ConfirmPurchase()
        {
            Console.Write("Подтвердить покупку? (д/н): ");
            string input = Console.ReadLine()?.ToLower();
            return input == "д" || input == "l";
        }

        // Выбор способа оплаты
        public int GetPaymentChoice()
        {
            Console.WriteLine("\nВыберите способ оплаты:");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Карта");
            Console.WriteLine("3. Бонусы");
            Console.Write("Ваш выбор: ");
            return ReadInt();
        }

        // Получение суммы частичной оплаты
        public decimal GetPartialPaymentAmount()
        {
            Console.Write("Введите сумму оплаты: ");
            return ReadDecimal();
        }

        // Получение суммы оплаты с учетом доступных средств
        public decimal GetPaymentAmount(decimal remaining, IPaymentStrategy strategy, Customer customer)
        {
            decimal maxAvailable = 0;

            // Определение максимально доступной суммы для выбранного способа оплаты
            if (strategy is CashPayment)
                maxAvailable = customer.WalletBalance;
            else if (strategy is CardPayment)
                maxAvailable = customer.CardBalance;
            else if (strategy is BonusPayment)
                maxAvailable = customer.BonusBalance;

            decimal maxAmount = Math.Min(remaining, maxAvailable);

            Console.Write($"Введите сумму для оплаты {strategy.Name} (макс. {maxAmount} руб.): ");
            decimal amount = ReadDecimal();

            return Math.Min(amount, maxAmount); // Гарантируем, что не превысим доступные средства
        }

        // Отображение доступных способов оплаты
        public void DisplayPaymentOptions(Customer customer)
        {
            Console.WriteLine("\nДоступные средства:");
            Console.WriteLine($"1. Наличные: {customer.WalletBalance} руб.");
            Console.WriteLine($"2. Карта: {customer.CardBalance} руб.");
            Console.WriteLine($"3. Бонусы: {customer.BonusBalance} руб.");
        }

        // Выбор способа пополнения баланса
        public int GetTopUpMethod()
        {
            Console.WriteLine("\nВыберите способ пополнения:");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Банковская карта");
            Console.Write("Ваш выбор: ");
            return ReadInt();
        }

        // Выбор товара для удаления из корзины
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

        // Получение суммы частичной оплаты с ограничением
        public decimal GetPartialPaymentAmount(decimal maxAmount)
        {
            Console.Write($"Введите сумму оплаты (макс. {maxAmount} руб.): ");
            decimal amount = ReadDecimal();
            return Math.Min(amount, maxAmount);
        }

        // Вывод сообщения
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        // Вспомогательные методы для ввода данных
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

        // Получение веса товара
        public decimal GetProductWeight()
        {
            Console.Write("Введите вес товара (кг): ");
            return ReadDecimal();
        }

        // Инициализация данных нового покупателя
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