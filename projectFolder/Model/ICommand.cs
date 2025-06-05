using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop_courseWork.Commands
{
    // Интерфейс команды для реализации паттерна "Команда"
    public interface ICommand
    {
        void Execute();
    }
}

