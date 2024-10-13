// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using ExpensesManager.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager
{
    class Program
    {
        private static double balance = -1;
        private static string path = Path.Combine(Environment.CurrentDirectory, "balance.json");
        private static bool mainMenu = false;
        private static bool balanceMenu = false;
        static void Main(string[] args)
        {
            // changing decimal separator from , to . to be valid when i enter my balance in my json
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            
            ExpenseDbContext dbCon = new ExpenseDbContext();
            
            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Hello, this is your personal expense manager");
            Console.WriteLine(new string('-', 15));
            
            // if you start the program for the first time you will be asked to add balance and that creates balance.json in debug folder
            // that is how we will take the balance everytime we start the program
            if (!File.Exists(path))
            {
                Console.Write("Please add money: ");
                while (true)
                {
                    string money = Console.ReadLine();
                    double moneyDouble;
                    if (!double.TryParse(money, out moneyDouble) || moneyDouble < 0)
                    {
                        Console.Write("Please enter a valid number: ");
                        continue;
                    }


                    string toInsertInJson = @"{""Balance"": " + $"{moneyDouble}" + @"}";
                    File.WriteAllText(path, toInsertInJson);
                    balance = moneyDouble;
                    break;
                }
            }
            else
            {
                string jsonString = File.ReadAllText(path);
                var result = JsonConvert.DeserializeObject<JToken>(jsonString);
                balance = Convert.ToDouble(result["Balance"]);
            }

            Console.Clear();
           
           while (true)
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
                   mainMenu = true;
                   int commandNumber = WriteValidInputForMenus();
                   if (commandNumber == 9)
                   {
                       string toInsertInJson = @"{""Balance"": " + $"{balance}" + @"}";
                       File.WriteAllText(path,toInsertInJson);
                       return;
                   }
                   else if (commandNumber == 1)
                   {
                       balanceMenu = true;
                       BalanceMenu();
                       Console.Clear();
                       break;
                   }
                   else if (commandNumber == 2)
                   {
                       ExpenseMenu(dbCon);
                       Console.Clear();
                       break;
                   }
                   else if (commandNumber == 3)
                   {
                       BillsMenu(dbCon);
                       Console.Clear();
                       break;
                   }


               }
           }

        }

       


        private static int WriteValidInputForMenus()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (char.IsDigit(input[0]) && input.Length == 1)
                {
                    int menuNumber = input[0] - '0';
                    if (balanceMenu && menuNumber != 1 && menuNumber != 2)
                    {
                        Console.Write("Please write a valid number for a menu: ");
                        continue;
                    }

                    if (mainMenu && menuNumber == 0)
                    {
                        Console.Write("Please write a valid number for a menu: ");
                        continue;
                    }

                    mainMenu = false;
                    balanceMenu = false;
                    return input[0] - '0';
                }
                else
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }
            }
        }

        private static void BalanceMenu()
        {   
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Balance is: {0}", balance);
                Console.WriteLine("1.Add Balance");
                Console.WriteLine("2.Back");
                
                int commInt = WriteValidInputForMenus();

                if (commInt == 1)
                {
                    Console.Write("Write a balance you want to add: ");
                    while (true)
                    {
                        string money = Console.ReadLine();
                        if (!double.TryParse(money, out double doubleMoney) || doubleMoney < 0)
                        {
                            Console.Write("Write a valid number for money: ");
                            continue;
                        }
                        
                        balance += double.Parse(money);
                        break;
                    }
                }
                else if (commInt == 2)
                {
                    return;
                }
               
               
            }
        }
        
        private static void ExpenseMenu(ExpenseDbContext dbContext)
        {
            Console.Clear();
            Console.Clear();
            double totalSpent = 0;
            foreach (var bill in dbContext.Bills)
            {
                totalSpent += bill.Price;
            }

            foreach (var food in dbContext.Foods)
            {
                totalSpent += food.Price;
            }

            foreach (var transport in dbContext.Transports)
            {
                totalSpent += transport.Price;
            }

            foreach (var education in dbContext.Educations)
            {
                totalSpent += education.Price;
            }

            foreach (var leisure in dbContext.Leisures)
            {
                totalSpent += leisure.Price;
            }

            foreach (var other in dbContext.Others)
            {
                totalSpent += other.Price;
            }
            
            Console.WriteLine("Total spent: {0}", totalSpent);
            Console.WriteLine("1.Back");
            Console.WriteLine("All expenses:");

            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Bills:");
            foreach (var bill in dbContext.Bills)
            {
                Console.WriteLine($"Id: {bill.Id}, Name: {bill.Name}, Price: {bill.Price}, Date: {bill.Date} ");
            }

            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Food:");
            foreach (var food in dbContext.Foods)
            {
                Console.WriteLine($"Id: {food.Id}, Name: {food.Name}, Price: {food.Price}, Date: {food.Date} ");
            }

            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Transport:");
            foreach (var transport in dbContext.Transports)
            {
                Console.WriteLine($"Id: {transport.Id}, Name: {transport.Name}, Price: {transport.Price}, Date: {transport.Date} ");
            }

            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Education:");
            foreach (var education in dbContext.Educations)
            {
                Console.WriteLine($"Id: {education.Id}, Name: {education.Name}, Price: {education.Price}, Date: {education.Date} ");
            }

            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Leisure:");
            foreach (var leisure in dbContext.Leisures)
            {
                Console.WriteLine($"Id: {leisure.Id}, Name: {leisure.Name}, Price: {leisure.Price}, Date: {leisure.Date} ");
            }

            Console.WriteLine(new string('-', 15));
            Console.WriteLine("Other:");
            foreach (var other in dbContext.Others)
            {
                Console.WriteLine($"Id: {other.Id}, Name: {other.Name}, Price: {other.Price}, Date: {other.Date} ");
            }
            Console.WriteLine(new string('-', 15));


            while (true)
            {
                string command = Console.ReadLine();
                if (command == "1")
                {
                    break;  
                }
                Console.Write("Please write a valid number for a menu: ");
            }   
        }
        private static void BillsMenu(ExpenseDbContext dbCon)
        {
            
        }
    }
}