using Microsoft.VisualStudio.TestTools.UnitTesting;
using petShop_courseWork.Model;
using petShop_courseWork.Model.Payment;

namespace petShop_courseWork.Tests
{
    /// <summary>
    /// “естирует поведение товара, который продаЄтс€ по весу.
    /// </summary>
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void GetTotalPrice_WeightedProduct_ReturnsCorrectPrice()
        {
            // Arrange Ч создаЄм товар с ценой за кг 100 и весом 2.5 кг
            var product = new Product
            {
                Name = " орм",
                PricePerUnit = 100,
                RequiresWeighing = true,
                Weight = 2.5m
            };

            // Act Ч вызываем метод подсчЄта цены
            var total = product.GetTotalPrice();

            // Assert Ч провер€ем, что результат равен 250
            Assert.AreEqual(250m, total);
        }
    }

    /// <summary>
    /// “естирует услугу с фиксированной ценой.
    /// </summary>
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void GetTotalPrice_Service_ReturnsFixedPrice()
        {
            // Arrange Ч создаЄм услугу с фиксированной ценой 300
            var service = new Service
            {
                Name = "—трижка когтей",
                Price = 300,
                Description = "Ѕыстра€ услуга"
            };

            // Act Ч вызываем метод подсчЄта
            var total = service.GetTotalPrice();

            // Assert Ч провер€ем, что результат равен 300
            Assert.AreEqual(300m, total);
        }
    }

    /// <summary>
    /// “естирует оплату бонусами и корректное списание.
    /// </summary>
    [TestClass]
    public class BonusPaymentTests
    {
        [TestMethod]
        public void CanPay_BonusPayment_SuccessfullyDeductsAmount()
        {
            // Arrange Ч создаЄм клиента с 100 бонусами
            var customer = new Customer
            {
                BonusBalance = 100
            };

            var payment = new BonusPayment();

            // Act Ч пытаемс€ оплатить 60 бонусами
            bool result = payment.CanPay(60, customer);

            // Assert Ч провер€ем, что оплата успешна и остаток равен 40
            Assert.IsTrue(result);
            Assert.AreEqual(40, customer.BonusBalance);
        }
    }

    /// <summary>
    /// “естирует оплату с карты при недостаточном балансе.
    /// </summary>
    [TestClass]
    public class CardPaymentTests
    {
        [TestMethod]
        public void CanPay_CardPayment_NotEnoughFunds_ReturnsFalse()
        {
            // Arrange Ч создаЄм клиента с балансом карты 100
            var customer = new Customer
            {
                CardBalance = 100
            };

            var payment = new CardPayment();

            // Act Ч пытаемс€ оплатить 150
            bool result = payment.CanPay(150, customer);

            // Assert Ч провер€ем, что оплата невозможна и баланс не изменилс€
            Assert.IsFalse(result);
            Assert.AreEqual(100, customer.CardBalance);
        }
    }

}
