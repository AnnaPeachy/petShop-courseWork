using Microsoft.VisualStudio.TestTools.UnitTesting;
using petShop_courseWork.Model;
using petShop_courseWork.Model.Payment;

namespace petShop_courseWork.Tests
{
    /// <summary>
    /// ��������� ��������� ������, ������� �������� �� ����.
    /// </summary>
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void GetTotalPrice_WeightedProduct_ReturnsCorrectPrice()
        {
            // Arrange � ������ ����� � ����� �� �� 100 � ����� 2.5 ��
            var product = new Product
            {
                Name = "����",
                PricePerUnit = 100,
                RequiresWeighing = true,
                Weight = 2.5m
            };

            // Act � �������� ����� �������� ����
            var total = product.GetTotalPrice();

            // Assert � ���������, ��� ��������� ����� 250
            Assert.AreEqual(250m, total);
        }
    }

    /// <summary>
    /// ��������� ������ � ������������� �����.
    /// </summary>
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void GetTotalPrice_Service_ReturnsFixedPrice()
        {
            // Arrange � ������ ������ � ������������� ����� 300
            var service = new Service
            {
                Name = "������� ������",
                Price = 300,
                Description = "������� ������"
            };

            // Act � �������� ����� ��������
            var total = service.GetTotalPrice();

            // Assert � ���������, ��� ��������� ����� 300
            Assert.AreEqual(300m, total);
        }
    }

    /// <summary>
    /// ��������� ������ �������� � ���������� ��������.
    /// </summary>
    [TestClass]
    public class BonusPaymentTests
    {
        [TestMethod]
        public void CanPay_BonusPayment_SuccessfullyDeductsAmount()
        {
            // Arrange � ������ ������� � 100 ��������
            var customer = new Customer
            {
                BonusBalance = 100
            };

            var payment = new BonusPayment();

            // Act � �������� �������� 60 ��������
            bool result = payment.CanPay(60, customer);

            // Assert � ���������, ��� ������ ������� � ������� ����� 40
            Assert.IsTrue(result);
            Assert.AreEqual(40, customer.BonusBalance);
        }
    }

    /// <summary>
    /// ��������� ������ � ����� ��� ������������� �������.
    /// </summary>
    [TestClass]
    public class CardPaymentTests
    {
        [TestMethod]
        public void CanPay_CardPayment_NotEnoughFunds_ReturnsFalse()
        {
            // Arrange � ������ ������� � �������� ����� 100
            var customer = new Customer
            {
                CardBalance = 100
            };

            var payment = new CardPayment();

            // Act � �������� �������� 150
            bool result = payment.CanPay(150, customer);

            // Assert � ���������, ��� ������ ���������� � ������ �� ���������
            Assert.IsFalse(result);
            Assert.AreEqual(100, customer.CardBalance);
        }
    }

}
