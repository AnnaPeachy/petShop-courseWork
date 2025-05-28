using System;
using System.Collections.Generic;
using petShop_courseWork.Model;
using petShop_courseWork.Model.Payment;
using petShop_courseWork.View;
using petShop_courseWork.Services;

namespace petShop_courseWork.Presenter
{
    public class ShopPresenter
    {
        private readonly IShopView _view;
        private readonly Customer _customer;
        private readonly List<Product> _products;
        private readonly List<Service> _services;

        public ShopPresenter(IShopView view, Customer customer)
        {
            _view = view;
            _customer = customer;


            // путь к файлам
            _products = ProductLoader.LoadProducts("Data/products.json");
            _services = ProductLoader.LoadServices("Data/services.json");
        }

        public void Start()
        {
            bool running = true;

            while (running)
            {
                _view.RequestInitialCustomer();
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
            _view.DisplayProducts(_products);
            int index = _view.GetProductSelection(_products);

            if (index >= 0 && index < _products.Count)
            {
                Product selected = _products[index];

                if (selected.RequiresWeighing)
                {
                    decimal? weight = selected.Weight > 0 ? selected.Weight : 0.5m;
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
            decimal total;
            try
            {
                total = _customer.GetCartTotal(); // Место, где может возникнуть исключение
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowMessage($"Ошибка при расчёте итоговой суммы: {ex.Message}");
                return;
            }

            _view.ShowMessage($"Итоговая сумма к оплате: {total} руб.");

            if (!_view.ConfirmPurchase())
                return;

            decimal remaining = total;

            while (remaining > 0)
            {
                _view.ShowMessage($"Осталось оплатить: {remaining} руб.");
                int method = _view.GetPaymentChoice();

                IPaymentStrategy strategy = null;

                if (method == 1)
                    strategy = new CashPayment();
                else if (method == 2)
                    strategy = new CardPayment();
                else if (method == 3)
                    strategy = new BonusPayment();

                if (strategy == null)
                {
                    _view.ShowMessage("Неверный выбор способа оплаты.");
                    continue;
                }

                decimal amount = method == 1 ? remaining : _view.GetPartialPaymentAmount();

                if (strategy.CanPay(amount, _customer))
                {
                    remaining -= amount;
                    _view.ShowMessage($"Оплачено {amount} руб. способом {strategy.Name}.");
                }
                else
                {
                    _view.ShowMessage("Недостаточно средств для оплаты этой части.");
                }
            }

            _view.ShowMessage("Оплата прошла успешно. Спасибо за покупку!");
            _customer.ShoppingCart.Clear();
        }

    }
}
