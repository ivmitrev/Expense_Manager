// See https://aka.ms/new-console-template for more information
/*
using System.Threading.Channels;
using ExpensesManager.Data;
using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager
{
    class Program
    {
        private static double balance = 0;
        private static double totalSpent = 0;
        private static string path = Path.Combine(Environment.CurrentDirectory, "balance.json");
        private static bool mainMenu = false;
        private static bool balanceAndAddMenu = false;
        private static bool expenseAndRemoveMenu = false;
        

        static void Main(string[] args)
        {
            // changing decimal separator from , to . to be valid when i enter my balance in my json
            System.Globalization.CultureInfo customCulture =
                (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

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
                using (var dbContext = new ExpenseDbContext())
                {
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
                }
            }

            Console.Clear();

            while (true)
            {
                Console.WriteLine(new string('-', 15));
                Console.WriteLine("Hello, this is your personal expense manager");
                Console.WriteLine(new string('-', 15));
                Console.WriteLine("1.Add Balance/Add new expense");
                Console.WriteLine("2.All expenses/Remove expenses");
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
                        File.WriteAllText(path, toInsertInJson);
                        return;
                    }
                    else if (commandNumber == 1)
                    {
                        balanceAndAddMenu = true;
                        BalanceAndAddMenu();
                        Console.Clear();
                        break;
                    }
                    else if (commandNumber == 2)
                    {
                        expenseAndRemoveMenu = true;
                        ExpenseAndRemoveMenu();
                        Console.Clear();
                        break;
                    }
                    else if (commandNumber == 3)
                    {
                        BillsMenu();
                        Console.Clear();
                        break;
                    }
                    else if (commandNumber == 4)
                    {
                        FoodMenu();
                        Console.Clear();
                        break;
                    }
                    else if (commandNumber == 5)
                    {
                        TransportMenu();
                        Console.Clear();
                        break;
                        
                    }
                    else if (commandNumber == 6)
                    {
                        EducationMenu();
                        Console.Clear();
                        break;
                    }
                    else if (commandNumber == 7)
                    {
                        LeisureMenu();
                        Console.Clear();
                        break;
                    }
                    else if (commandNumber == 8)
                    {
                        OtherMenu();
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
                    if (balanceAndAddMenu && menuNumber != 1 && menuNumber != 2 && menuNumber != 3)
                    {
                        Console.Write("Please write a valid number for a menu: ");
                        continue;
                    }

                    if (mainMenu && menuNumber == 0)
                    {
                        Console.Write("Please write a valid number for a menu: ");
                        continue;
                    }

                    if (expenseAndRemoveMenu && menuNumber != 1 && menuNumber != 2)
                    {
                        Console.Write("Please write a valid number for a menu: ");
                        continue;
                    }

                    mainMenu = false;
                    balanceAndAddMenu = false;
                    expenseAndRemoveMenu = false;
                    return input[0] - '0';
                }
                else
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }
            }
        }

        private static void BalanceAndAddMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Balance is: {0:f2}   |   Total spent is: {1:f2}", balance, totalSpent);
                Console.WriteLine("1.Add Balance");
                Console.WriteLine("2.Add new expense");
                Console.WriteLine("3.Back");

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
                    if (balance == 0)
                    {
                        Console.Write("Please add balance first!");
                        Thread.Sleep(2500);
                        continue;
                    }

                    Console.WriteLine("Categories: Bills | Education | Food | Leisure | Transport | Other");
                    Console.Write("Write the category of your expense: ");
                    while (true)
                    {
                        string category = Console.ReadLine();
                        category = category.ToLower();
                        if (category != "bills" && category != "education" && category != "food" &&
                            category != "other" && category != "leisure" &&
                            category != "transport")
                        {
                            Console.Write("Write a valid category: ");
                            continue;
                        }

                        Console.Write("Name of your expense: ");
                        string name = Console.ReadLine();
                        Console.Write("Price of your expense: ");
                        double price = 0;
                        while (true)
                        {
                            if (!double.TryParse(Console.ReadLine(), out price))
                            {
                                Console.Write("Write a valid price for your expense: ");
                                continue;
                            }

                            break;
                        }

                        if (price > balance)
                        {
                            Console.Write("You don't have enough money.Please add balance first!");
                            Thread.Sleep(2500);
                            break;
                        }

                        using (var dbContext = new ExpenseDbContext())
                        {
                            if (category == "bills")
                            {
                                Bill bill = new Bill()
                                    { Name = name, Price = price, Date = DateOnly.FromDateTime(DateTime.Now) };
                                dbContext.Add(bill);
                            }
                            else if (category == "education")
                            {
                                Education education = new Education()
                                    { Name = name, Price = price, Date = DateOnly.FromDateTime(DateTime.Now) };
                                dbContext.Add(education);
                            }
                            else if (category == "food")
                            {
                                Food food = new Food()
                                    { Name = name, Price = price, Date = DateOnly.FromDateTime(DateTime.Now) };
                                dbContext.Add(food);
                            }
                            else if (category == "other")
                            {
                                Other other = new Other()
                                    { Name = name, Price = price, Date = DateOnly.FromDateTime(DateTime.Now) };
                                dbContext.Add(other);
                            }
                            else if (category == "leisure")
                            {
                                Leisure leisure = new Leisure()
                                    { Name = name, Price = price, Date = DateOnly.FromDateTime(DateTime.Now) };
                                dbContext.Add(leisure);
                            }
                            else if (category == "transport")
                            {
                                Transport transport = new Transport()
                                    { Name = name, Price = price, Date = DateOnly.FromDateTime(DateTime.Now) };
                                dbContext.Add(transport);
                            }

                            totalSpent += price;
                            balance -= price;
                            Console.WriteLine("Succesfully added: {0}", name);
                            dbContext.SaveChanges();
                        }

                        Thread.Sleep(2500);
                        break;
                    }
                }
                else if (commInt == 3)
                {
                    return;
                }
            }
        }

        private static void ExpenseAndRemoveMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1.Remove an expense");
                Console.WriteLine("2.Back");

                using (var dbContext = new ExpenseDbContext())
                {
                    Console.WriteLine(new string('-', 15));
                    Console.WriteLine("Total spent: {0:f2}", totalSpent);
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
                        Console.WriteLine(
                            $"Id: {transport.Id}, Name: {transport.Name}, Price: {transport.Price}, Date: {transport.Date} ");
                    }

                    Console.WriteLine(new string('-', 15));
                    Console.WriteLine("Education:");
                    foreach (var education in dbContext.Educations)
                    {
                        Console.WriteLine(
                            $"Id: {education.Id}, Name: {education.Name}, Price: {education.Price}, Date: {education.Date} ");
                    }

                    Console.WriteLine(new string('-', 15));
                    Console.WriteLine("Leisure:");
                    foreach (var leisure in dbContext.Leisures)
                    {
                        Console.WriteLine(
                            $"Id: {leisure.Id}, Name: {leisure.Name}, Price: {leisure.Price}, Date: {leisure.Date} ");
                    }

                    Console.WriteLine(new string('-', 15));
                    Console.WriteLine("Other:");
                    foreach (var other in dbContext.Others)
                    {
                        Console.WriteLine(
                            $"Id: {other.Id}, Name: {other.Name}, Price: {other.Price}, Date: {other.Date} ");
                    }

                    Console.WriteLine(new string('-', 15));
                }

                int commInt = WriteValidInputForMenus();

                if (commInt == 1)
                {
                    using (var dbContext = new ExpenseDbContext())
                    {
                        if (!dbContext.Bills.Any() && !dbContext.Foods.Any() && !dbContext.Transports.Any() &&
                            !dbContext.Educations.Any()
                            && !dbContext.Leisures.Any() && !dbContext.Others.Any())
                        {
                            Console.WriteLine("There is nothing to remove! You should add expenses.");
                            continue;
                        }
                    }

                    Console.WriteLine("Categories: Bills | Education | Food | Leisure | Transport | Other");
                    Console.Write("Write the category of the expense you want to remove: ");
                    while (true)
                    {
                        string category = Console.ReadLine();
                        category = category.ToLower();
                        if (category != "bills" && category != "education" && category != "food" &&
                            category != "other" && category != "leisure" &&
                            category != "transport")
                        {
                            Console.Write("Write a valid category: ");
                            continue;
                        }

                        using (var dbContext = new ExpenseDbContext())
                        {
                            bool flag = false;
                            if (category == "transport" && !dbContext.Transports.Any())
                            {
                                flag = true;
                            }
                            else if (category == "bills" && !dbContext.Bills.Any())
                            {
                                flag = true;
                            }
                            else if (category == "education" && !dbContext.Educations.Any())
                            {
                                flag = true;
                            }
                            else if (category == "food" && !dbContext.Foods.Any())
                            {
                                flag = true;
                            }
                            else if (category == "leisure" && !dbContext.Leisures.Any())
                            {
                                flag = true;
                            }
                            else if (category == "other" && !dbContext.Others.Any())
                            {
                                flag = true;
                            }

                            if (flag)
                            {
                                Console.WriteLine("Nothing to remove in this category!");
                                Thread.Sleep(2500);
                                break;
                            }
                        }

                        Console.Write("Write the id of the expense you want to remove in {0} category: ", category);
                        while (true)
                        {
                            int id = 0;
                            if (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
                            {
                                Console.Write("Write a valid integer for id in the chosen category: ");
                                continue;
                            }


                            using (var dbContext = new ExpenseDbContext())
                            {
                                if (category == "bills")
                                {
                                    if (dbContext.Bills.All(bill => bill.Id != id))
                                    {
                                        Console.Write("Write a valid integer for id in the chosen category: ");
                                        continue;
                                    }
                                    

                                    Bill bill = dbContext.Bills.First(a => a.Id == id);
                                    totalSpent -= bill.Price;
                                    balance += bill.Price;
                                    dbContext.Bills.Attach(bill);
                                    dbContext.Bills.Remove(bill);
                                    dbContext.SaveChanges();
                                    
                                }
                                else if (category == "food")
                                {
                                    if (dbContext.Foods.All(food => food.Id != id))
                                    {
                                        Console.Write("Write a valid integer for id in the chosen category: ");
                                        continue;
                                    }
                                    
                                    Food food = dbContext.Foods.First(a => a.Id == id);
                                    totalSpent -= food.Price;
                                    balance += food.Price;
                                    dbContext.Foods.Attach(food);
                                    dbContext.Foods.Remove(food);
                                    dbContext.SaveChanges();
                                    
                                }
                                else if (category == "education")
                                {
                                    if (dbContext.Educations.All(ed => ed.Id != id))
                                    {
                                        Console.Write("Write a valid integer for id in the chosen category: ");
                                        continue;
                                    }
                                    
                                  

                                    Education education = dbContext.Educations.First(a => a.Id == id);
                                    totalSpent -= education.Price;
                                    balance += education.Price;
                                    dbContext.Educations.Attach(education);
                                    dbContext.Educations.Remove(education);
                                    dbContext.SaveChanges();
                                    
                                }
                                else if (category == "transport")
                                {
                                    if (dbContext.Transports.All(tr => tr.Id != id))
                                    {
                                        Console.Write("Write a valid integer for id in the chosen category: ");
                                        continue;
                                    }
                                    
                                    Transport transport = dbContext.Transports.First(a => a.Id == id);
                                    totalSpent -= transport.Price;
                                    balance += transport.Price;
                                    dbContext.Transports.Attach(transport);
                                    dbContext.Transports.Remove(transport);
                                    dbContext.SaveChanges();
                                    
                                }
                                else if (category == "leisure")
                                {
                                    if (dbContext.Leisures.All(l => l.Id != id))
                                    {
                                        Console.Write("Write a valid integer for id in the chosen category: ");
                                        continue;
                                    }
                                    
                                    Leisure leisure = dbContext.Leisures.First(a => a.Id == id);
                                    totalSpent -= leisure.Price;
                                    balance += leisure.Price;
                                    dbContext.Leisures.Attach(leisure);
                                    dbContext.Leisures.Remove(leisure);
                                    dbContext.SaveChanges();
                                    
                                }
                                else if (category == "other")
                                {
                                    if (dbContext.Others.All(o => o.Id != id))
                                    {
                                        Console.Write("Write a valid integer for id in the chosen category: ");
                                        continue;
                                    }
                                    
                                    Other other = dbContext.Others.First(a => a.Id == id);
                                    totalSpent -= other.Price;
                                    balance += other.Price;
                                    dbContext.Others.Attach(other);
                                    dbContext.Others.Remove(other);
                                    dbContext.SaveChanges();
                                    
                                }
                                Console.WriteLine($"Successfully removed an expense!");
                                Thread.Sleep(2500);
                            }

                            Console.Clear();
                            break;
                        }

                        break;
                    }
                }
                else if (commInt == 2)
                {
                    return;
                }
            }
        }

        private static void BillsMenu()
        {
            Console.Clear();
            Console.WriteLine("Bills: ");
            Console.WriteLine(new string('-',15));
            using (var dbContext = new ExpenseDbContext())
            {
                foreach (var bill in dbContext.Bills)
                {
                    Console.WriteLine($"Id: {bill.Id}, Name: {bill.Name}, Price: {bill.Price}, Date: {bill.Date} ");
                }
            }
            Console.WriteLine(new string('-',15));
            Console.WriteLine("1.Back");
            while (true)
            {
                string command = Console.ReadLine();
                if (command != "1")
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }

                break;
            }
            
        }
        
        private static void FoodMenu()
        {
            Console.Clear();
            Console.WriteLine("Food: ");
            Console.WriteLine(new string('-',15));
            using (var dbContext = new ExpenseDbContext())
            {
                foreach (var food in dbContext.Foods)
                {
                    Console.WriteLine($"Id: {food.Id}, Name: {food.Name}, Price: {food.Price}, Date: {food.Date} ");
                }
            }
            Console.WriteLine(new string('-',15));
            Console.WriteLine("1.Back");
            while (true)
            {
                string command = Console.ReadLine();
                if (command != "1")
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }

                break;
            }
        }
        
        private static void TransportMenu()
        {
            Console.Clear();
            Console.WriteLine("Transport: ");
            Console.WriteLine(new string('-',15));
            using (var dbContext = new ExpenseDbContext())
            {
                foreach (var transport in dbContext.Transports)
                {
                    Console.WriteLine($"Id: {transport.Id}, Name: {transport.Name}, Price: {transport.Price}, Date: {transport.Date} ");
                }
            }
            Console.WriteLine(new string('-',15));
            Console.WriteLine("1.Back");
            while (true)
            {
                string command = Console.ReadLine();
                if (command != "1")
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }

                break;
            }
        }
        
        private static void EducationMenu()
        {
            Console.Clear();
            Console.WriteLine("Education: ");
            Console.WriteLine(new string('-',15));
            using (var dbContext = new ExpenseDbContext())
            {
                foreach (var education in dbContext.Educations)
                {
                    Console.WriteLine($"Id: {education.Id}, Name: {education.Name}, Price: {education.Price}, Date: {education.Date} ");
                }
            }
            Console.WriteLine(new string('-',15));
            Console.WriteLine("1.Back");
            while (true)
            {
                string command = Console.ReadLine();
                if (command != "1")
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }

                break;
            }
        }
        
        
        private static void LeisureMenu()
        {
            Console.Clear();
            Console.WriteLine("Leisure: ");
            Console.WriteLine(new string('-',15));
            using (var dbContext = new ExpenseDbContext())
            {
                foreach (var leisure in dbContext.Leisures)
                {
                    Console.WriteLine($"Id: {leisure.Id}, Name: {leisure.Name}, Price: {leisure.Price}, Date: {leisure.Date} ");
                }
            }
            Console.WriteLine(new string('-',15));
            Console.WriteLine("1.Back");
            while (true)
            {
                string command = Console.ReadLine();
                if (command != "1")
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }

                break;
            }
        }
        
        private static void OtherMenu()
        {
            Console.Clear();
            Console.WriteLine("Other: ");
            Console.WriteLine(new string('-',15));
            using (var dbContext = new ExpenseDbContext())
            {
                foreach (var other in dbContext.Others)
                {
                    Console.WriteLine($"Id: {other.Id}, Name: {other.Name}, Price: {other.Price}, Date: {other.Date} ");
                }
            }
            Console.WriteLine(new string('-',15));
            Console.WriteLine("1.Back");
            while (true)
            {
                string command = Console.ReadLine();
                if (command != "1")
                {
                    Console.Write("Please write a valid number for a menu: ");
                    continue;
                }

                break;
            }
        }
        
        
        
        
    }
} */