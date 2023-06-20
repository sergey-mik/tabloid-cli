using System;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    class Program
    {
        static void Main(string[] args)
        {
                Console.WriteLine("Choose a background color:");
                Console.WriteLine("1. Red");
                Console.WriteLine("2. Gray");
                Console.WriteLine("3. Blue");
                Console.Write("Enter your choice (1, 2, or 3): ");
                string input = Console.ReadLine();
                int choice = int.Parse(input);

                switch (choice)
                {
                    case 1:
                        Console.BackgroundColor = ConsoleColor.Red;
                    break;
                    case 2:
                        Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                    case 3:
                        Console.BackgroundColor = ConsoleColor.Blue;

                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();

            // MainMenuManager implements the IUserInterfaceManager interface
            IUserInterfaceManager ui = new MainMenuManager();
            while (ui != null)
            {   
                // Each call to Execute will return the next IUserInterfaceManager we should execute
                // When it returns null, we should exit the program;
                ui = ui.Execute();
            }
        }
    }
}
