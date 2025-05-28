using System.Collections.Generic;
using petShop_courseWork.Model;

namespace petShop_courseWork.View
{
    /// <summary>
    /// Интерфейс представления магазина — всё, что отображается пользователю.
    /// </summary>
    public interface IShopView
    {
        // События / команды пользователя
        void ShowMainMenu();
        int GetMainMenuChoice();

        void DisplayProducts(List<Product> products);
        void DisplayServices(List<Service> services);
        int GetProductSelection(List<Product> products);
        int GetServiceSelection(List<Service> services);

        void ShowCart(List<CartItem> cartItems);
        void ShowMessage(string message);

        int GetPaymentChoice();
        decimal GetPartialPaymentAmount();

        void DisplayCustomerInfo(Customer customer);
        bool ConfirmPurchase();

        decimal GetProductWeight();

        void RequestInitialCustomer();
    }
}
