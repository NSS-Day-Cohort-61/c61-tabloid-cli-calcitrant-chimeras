using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class ColorManager : IUserInterfaceManager
    {

        private readonly IUserInterfaceManager _parentUI;

        public ColorManager(IUserInterfaceManager parentUI)
        {
            _parentUI = parentUI;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Background Color Menu");
            Console.WriteLine(" 1) Black");
            Console.WriteLine(" 2) Cyan");
            Console.WriteLine(" 3) Magenta");
            Console.WriteLine(" 4) Green");
            Console.WriteLine(" 5) Yellow");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return this;
                case "2":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.Clear();
                    return this;
                case "3":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.Clear();
                    return this;
                case "4":
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Clear();
                    return this;
                case "5":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.Clear();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
    }
}
