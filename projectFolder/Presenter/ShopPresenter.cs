using System;
using System.Collections.Generic;
using petShop_courseWork.Model;
using petShop_courseWork.Model.Payment;
using petShop_courseWork.View;
using petShop_courseWork.Services;
using System.Linq;
using System.IO;

namespace petShop_courseWork.Presenter
{
    public class ShopPresenter
    {
        private readonly IShopView _view;
        private readonly Customer _customer;
        private List<Product> _products;
        private List<Service> _services;

        public ShopPresenter(IShopView view, Customer customer)
        {
            _view = view;
            _customer = customer;

            // Загрузка данных (сначала пробуем из сессии)
            LoadSessionData();
        }

        private void LoadSessionData()
        {
            var session = ProductLoader.LoadSession();

            if (session != null)
            {
                // Подгружаем сохранённые данные
                _products = session.Products ?? ProductLoader.LoadProducts("Data/products.json");
                _services = session.Services ?? ProductLoader.LoadServices("Data/services.json");

                // Копируем сохранённые данные покупателя
                _customer.Name = session.Customer.Name;
                _customer.WalletBalance = session.Customer.WalletBalance;
                _customer.CardBalance = session.Customer.CardBalance;
                _customer.BonusBalance = session.Customer.BonusBalance;

                // Восстанавливаем корзину
                _customer.ShoppingCart.Clear();
                foreach (var item in session.Customer.ShoppingCart)
                {
                    _customer.ShoppingCart.Add(item);
                }

                _view.ShowMessage("\nЗагружена предыдущая сессия.");
                _view.DisplayCustomerInfo(_customer);
            }
            else
            {
                // Обычная загрузка если нет сессии
                _products = ProductLoader.LoadProducts("Data/products.json");
                _services = ProductLoader.LoadServices("Data/services.json");
                _view.RequestInitialCustomer(); // Запрашиваем данные только при новом сеансе
            }
        }

        // Добавляем метод для сохранения перед выходом
        public void SaveBeforeExit()
        {
            var session = new SessionData
            {
                Customer = _customer,
                Products = _products,
                Services = _services
            };
            ProductLoader.SaveSession(session);
        }
    

        public void Start()
        {
            //_view.RequestInitialCustomer(); // Запрос данных ОДИН РАЗ

            bool running = true;
            while (running)
            {
                _view.DisplayCustomerInfo(_customer);
                _view.ShowMainMenu();
                int choice = _view.GetMainMenuChoice();

                switch (choice)
                {
                    case 1:
                        ShowAndSelectProduct();
                        break;
                    case 2:
                        ShowAndSelectService();
                        break;
                    case 3:
                        _view.ShowCart(_customer.ShoppingCart);
                        break;
                    case 4:
                        ProcessPayment();
                        break;
                    case 0:
                        if (_view.ConfirmExit())
                        {
                            SaveBeforeExit();
                        }
                        else
                        {
                            // Удаляем файл сессии, если он существует
                            string sessionPath = "Data/session.json";
                            if (System.IO.File.Exists(sessionPath))
                            {
                                try
                                {
                                    System.IO.File.Delete(sessionPath);
                                    _view.ShowMessage("Сессия удалена.");
                                }
                                catch (Exception ex)
                                {
                                    _view.ShowMessage($"Не удалось удалить сессию: {ex.Message}");
                                }
                            }
                        }
                        running = false;
                        break;

                    default:
                        _view.ShowMessage("Неверный выбор.");
                        break;
                }
            }
        }

        private void ShowAndSelectProduct()
        {
            if (_products.Count == 0)
            {
                _view.ShowMessage("Список товаров пуст.");
                return;
            }

            _view.DisplayProducts(_products); // Показываем список
            int index = _view.GetProductSelection(_products);

            if (index >= 0 && index < _products.Count)
            {
                Product selected = _products[index];

                if (selected.RequiresWeighing)
                {
                    decimal weight = _view.GetProductWeight(); // Запрашиваем вес
                    selected.Weight = weight;
                }

                _customer.AddToCart(new CartItem(selected));
                _view.ShowMessage($"Товар «{selected.Name}» добавлен в корзину.");
            }
        }

        private void ShowAndSelectService()
        {
            _view.DisplayServices(_services);
            int index = _view.GetServiceSelection(_services);

            if (index >= 0 && index < _services.Count)
            {
                Service selected = _services[index];
                _customer.AddToCart(new CartItem(selected));
                _view.ShowMessage($"Услуга «{selected.Name}» добавлена в корзину.");
            }
        }

        private void ProcessPayment()
        {
            if (_customer.ShoppingCart.Count == 0)
            {
                _view.ShowMessage("Корзина пуста. Нечего оплачивать.");
                return;
            }

            // Проверяем взвешенность товаров перед оплатой
            CheckProductWeights();

            decimal total = _customer.GetCartTotal();
            decimal availableMoney = _customer.WalletBalance + _customer.CardBalance + _customer.BonusBalance;

            // Пока сумма в корзине больше доступных средств, предлагаем убрать товары
            while (total > availableMoney && _customer.ShoppingCart.Count > 0)
            {
                _view.ShowMessage($"\nНедостаточно средств! Нужно ещё {total - availableMoney} руб.");
                _view.ShowMessage("Доступные средства:");
                _view.ShowMessage($"- Наличные: {_customer.WalletBalance} руб.");
                _view.ShowMessage($"- Карта: {_customer.CardBalance} руб.");
                _view.ShowMessage($"- Бонусы: {_customer.BonusBalance} руб.");

                _view.ShowCart(_customer.ShoppingCart);
                int index = _view.GetItemToRemove(_customer.ShoppingCart);

                if (index >= 0 && index < _customer.ShoppingCart.Count)
                {
                    var removedItem = _customer.ShoppingCart[index];
                    _customer.ShoppingCart.RemoveAt(index);
                    total = _customer.GetCartTotal();
                    _view.ShowMessage($"Товар '{removedItem.Item.Name}' удалён из корзины. Новый итог: {total} руб.");
                }
                else
                {
                    _view.ShowMessage("Операция отменена.");
                    return;
                }
            }

            // Если после удаления товаров корзина пуста
            if (_customer.ShoppingCart.Count == 0)
            {
                _view.ShowMessage("В корзине не осталось товаров.");
                return;
            }

            // Основной процесс оплаты
            decimal remaining = total;
            _view.ShowMessage($"\n=== ОПЛАТА ===\nОбщая сумма: {total} руб.");

            while (remaining > 0)
            {
                _view.ShowMessage($"\nОсталось оплатить: {remaining} руб.");
                _view.DisplayPaymentOptions(_customer);

                int method = _view.GetPaymentChoice();
                IPaymentStrategy strategy = GetPaymentStrategy(method);

                if (strategy == null)
                {
                    _view.ShowMessage("Неверный выбор способа оплаты.");
                    continue;
                }

                decimal amount = _view.GetPaymentAmount(remaining, strategy, _customer);

                if (strategy.CanPay(amount, _customer))
                {
                    remaining -= amount;
                    _view.ShowMessage($"Оплачено {amount} руб. ({strategy.Name})");
                }
                else
                {
                    _view.ShowMessage($"Недостаточно средств на {strategy.Name}!");
                }
            }

            _view.ShowMessage("\nОплата прошла успешно! Спасибо за покупку!");
            _customer.ShoppingCart.Clear();
            if (File.Exists("Data/session.json"))
            {
                File.Delete("Data/session.json");
            }
        }

        private void CheckProductWeights()
        {
            foreach (var item in _customer.ShoppingCart)
            {
                if (item.Item is Product product && product.RequiresWeighing && product.Weight == null)
                {
                    _view.ShowMessage($"Требуется взвешивание товара: {product.Name}");
                    product.Weight = _view.GetProductWeight();
                }
            }
        }

        private IPaymentStrategy GetPaymentStrategy(int method)
        {
            switch (method)
            {
                case 1: return new CashPayment();
                case 2: return new CardPayment();
                case 3: return new BonusPayment();
                default: return null;
            }
        }
    }
}
