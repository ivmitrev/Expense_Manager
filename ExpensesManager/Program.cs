// See https://aka.ms/new-console-template for more information


namespace ExpensesManager
{
    class Program
    {
        private static double balance = 0;
        
        static void Main(string[] args)
        {
            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Hello, this is your personal expense manager");
            Console.WriteLine(new string('-', 15));
            
            
            
            Console.WriteLine("1.Balance");
            Console.WriteLine("2.All expenses");
            Console.WriteLine("3.Bills");
            Console.WriteLine("4.Food");
            Console.WriteLine("5.Transport");
            Console.WriteLine("6.Education");
            Console.WriteLine("7.Leisure");
            Console.WriteLine("8.Other");
            Console.WriteLine("9.Exit");

            Console.WriteLine(new string('-', 15));
            Console.Write("Choose a menu number: ");
           
            while (true)
            {
                string command = Console.ReadLine();
                int commandNumber;
                if (!int.TryParse(command, out commandNumber))
                {
                    Console.Write("Please write a number: ");
                    continue;
                }
                
                
                
                
            }
            
        }
    }
}