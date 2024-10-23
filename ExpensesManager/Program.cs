// See https://aka.ms/new-console-template for more information

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
            Runner runner = new Runner(path,balance,totalSpent,mainMenu,balanceAndAddMenu,expenseAndRemoveMenu);
            runner.Run();
        }
        
    }
}