/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petShop_courseWork
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
*/

using petShop_courseWork.View;
using petShop_courseWork.Presenter;
using petShop_courseWork.ConsoleApp;
using petShop_courseWork.Model;
using System;

class Program
{
    static void Main()
    {
        try
        {
            Customer customer = new Customer();
            ConsoleShopView view = new ConsoleShopView(customer);
            ShopPresenter presenter = new ShopPresenter(view, customer);

            // Обработка закрытия приложения
            //AppDomain.CurrentDomain.ProcessExit += (s, e) => presenter.SaveBeforeExit();
            //Console.CancelKeyPress += (s, e) => presenter.SaveBeforeExit();


            presenter.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.WriteLine("Подробности: " + ex.StackTrace);
        }
        
        Console.WriteLine("\nНажмите Enter для выхода...");
        Console.ReadLine();
    }
}

