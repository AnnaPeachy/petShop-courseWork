using petShop_courseWork.Model;
using petShop_courseWork.View;

namespace petShop_courseWork.Commands
{
    // Команда оформления покупки
    public class MakePurchaseCommand : ICommand
    {
        private readonly Customer _customer;
        private readonly IShopView _view;

        public MakePurchaseCommand(Customer customer, IShopView view)
        {
            _customer = customer;
            _view = view;
        }

        public void Execute()
        {
            _view.ShowMessage("Оформляется покупка...");
            _customer.ShoppingCart.Clear(); // Очистка корзины после оплаты
            _view.ShowMessage("Корзина очищена. Оплата прошла успешно! Спасибо за покупку!");
        }
    }
}


