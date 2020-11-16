using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class History_Actions
    {
        public string type;
        public string sum;
        public int current_balance;

        public History_Actions(string type, string sum, int current_balance)
        {
            this.type = type;
            this.sum = sum;
            this.current_balance = current_balance;
        }

        public override string ToString() => $"{type,15} | {sum,-7} | {current_balance}";
    }
    class BankAccount
    {
        private static int account_count = 0;
        private string user_name;
        private int Balance { get; set; }
        public List<History_Actions> History_Actions = new List<History_Actions>();


        // Инициализация
        public BankAccount(string user_name)
        {
            this.user_name = user_name;
            Balance = 0;
            account_count += 1;
        }

        // Пополнить счет add >= 0
        public void Money_Top_Up(int amount)
        {
            amount *= amount > 0 ? 1 : -1;
            Balance += amount;
            History_Actions.Add(new History_Actions("Cash Top Up  ", "+" + Convert.ToString(amount), Balance));
        }
        // Совершить перевод
        public void Money_Transfer(int amount)
        {
            amount *= amount > 0 ? -1 : 1; // Всегда отрицательное
            Balance += amount;
            History_Actions.Add(new History_Actions("Cash Transfer", Convert.ToString(amount), Balance));
        }
        // Совершить оплату
        public void Money_Payment(int amount)
        {
            amount *= amount > 0 ? -1 : 1;
            Balance += amount;
            History_Actions.Add(new History_Actions("Cash Payment ", Convert.ToString(amount), Balance));
        }
        public void Show_Balance() => Console.WriteLine(Balance);
        public void Show_History()
        {
            Console.WriteLine(
                $"----------------------------------------------" +
                $"\nUSER NAME : {user_name}\n" +
                $"----------------------------------------------"
                );
            foreach (var i in History_Actions) Console.WriteLine(i);
        }
        public static int Count() => account_count;

            
    }
}
