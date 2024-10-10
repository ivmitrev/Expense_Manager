// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using ExpensesManager.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExpensesManager
{
    class Program
    {
       // private static double balance = 0;
        
        static void Main(string[] args)
        {
            // changing decimal separator from , to . to be valid when i enter my balance in my json
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            
            
            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Hello, this is your personal expense manager");
            Console.WriteLine(new string('-', 15));
            
            var path = Path.Combine(Environment.CurrentDirectory, "balance.json");
            if (!File.Exists(path))
            {
                Console.Write("Please add money: ");
                while (true) 
                {
                    string money = Console.ReadLine();
                    double moneyDouble;
                    if (!double.TryParse(money, out moneyDouble))
                    {
                        Console.Write("Please enter a valid number: ");
                        continue;
                    }
                    
                    string toInsertInJson = @"{""Balance"": " + $"{moneyDouble}" + @"}";
                    File.WriteAllText(path,toInsertInJson);
                    break;
                    
                }
                
            }
            
            //string jsonString = File.ReadAllText(path);
            //var result = JsonConvert.DeserializeObject<JToken>(jsonString);
            //var balance = result["Balance"]; //contains 5
            //Console.WriteLine(balance);
           /* string ser = JsonConvert.SerializeObject(result);
            Console.WriteLine(ser);
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(Directory.GetCurrentDirectory()); */
            
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
            ExpenseDbContext db = new ExpenseDbContext();
            foreach (var VARIABLE in db.Bills)
            {
                Console.WriteLine(VARIABLE.Name);
            }
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