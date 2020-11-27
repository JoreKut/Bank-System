using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank
{
    class BankAccount : Program
    {   

        public static int account_count = 1;
        public string User_Name;
        public string PASSWORD;
        public string ID;
        public string Message_Type;
        public double Balance;
        public string History_PATH = @"C:\Users\firef\source\repos\Bank\USERS\Users_History\";
        public bool Theme_Color = true; //true == Black  
        // Initialization
        public BankAccount(string User_Name, string PASSWORD, double Balance, bool Theme_Color)
        {
            this.User_Name = User_Name;
            this.PASSWORD = PASSWORD;
            this.Balance = Balance;
            this.Theme_Color = Theme_Color;
            ID = Next_ID(account_count);

            History_PATH += ID + @".txt";

            if (!File.Exists(History_PATH))
            {
                using (StreamWriter w = new StreamWriter(History_PATH, true, Encoding.GetEncoding(1251)))
                {
                    w.WriteLine($"Date of creation : {DateTime.Now} \nBalance : {Balance}\nOPERATION HISTORY : ");
                    w.WriteLine("-----------------------------------------------------------------------------------------");
                }
            }

            account_count += 1;

        }
        public string Next_ID(int count)
        {
            string ID = "";

            int ID_LEN = 5;
            int C_Len = count == 0 ? 1 : 0;
            int tmp = count;
            while(tmp > 0)
            {
                C_Len += 1;
                tmp /= 10;
            }

            for (int i = 0; i < ID_LEN-C_Len; i++)
            {
                ID += "0";
            }

            return ID + Convert.ToString(count);
        }

        // Top Up Cash
        public void Money_Top_Up(double amount)
        {
            amount = Math.Abs(amount);
            Balance += amount;
            Message_Type = "Cash Top  Up           ";
            using (StreamWriter w = new StreamWriter(History_PATH, true, Encoding.GetEncoding(1251)))
            {
                w.WriteLine($"{Message_Type,-37} | +{amount,-7} | {Balance,-8} | {DateTime.Now}");
                w.WriteLine("-----------------------------------------------------------------------------------------");
            }
        }

        // Make a payment
        public void Money_Payment(double amount)
        {
            amount = Math.Abs(amount);
            Balance -= amount;
            Message_Type = "Cash Payment           ";
            using (StreamWriter w = new StreamWriter(History_PATH, true, Encoding.GetEncoding(1251)))
            {
                w.WriteLine($"{Message_Type,-37} | -{amount,-7} | {Balance,-8} | {DateTime.Now}");
                w.WriteLine("-----------------------------------------------------------------------------------------");
            }
        }

        // Make a transfer
        public void Money_Transfer_From(double amount,  ref BankAccount Sender)
        {
            Message_Type = "Cash Transfer FROM ";
            amount = Math.Abs(amount);
            Balance += amount;
            string who = '<' + Sender.User_Name + "(" + Sender.ID + ")>";

            using (StreamWriter w = new StreamWriter(History_PATH, true, Encoding.GetEncoding(1251)))
            {
                w.WriteLine($"{Message_Type,15}  {who,-16} | +{amount,-7} | {Balance,-8} | {DateTime.Now}");
                w.WriteLine("-----------------------------------------------------------------------------------------");
            }
        }
        public void Money_Transfer_To(double amount, ref BankAccount Reciever)
        {
            amount = Math.Abs(amount);
            Balance -= amount;
            Message_Type = "Cash Transfer TO   ";
            string who = '<' + Reciever.User_Name + "(" + Reciever.ID + ")>";

            using (StreamWriter w = new StreamWriter(History_PATH, true, Encoding.GetEncoding(1251)))
            {
                w.WriteLine($"{Message_Type,15}  {who,-16} | -{amount,-7} | {Balance,-8} | {DateTime.Now}");
                w.WriteLine("-----------------------------------------------------------------------------------------");
            }
        }
        public void Show_History()
        {
            bool has_changed = false;

            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------" +
                $"\nUSER NAME : {User_Name}\n" +
                $"-----------------------------------------------------------------------------------------"
                );
            string[] History_Ac = File.ReadAllLines(History_PATH);
            foreach (var line in History_Ac)
            {
                bool in_breackets = false;
                for (int i = 0; i < line.Length ; i++)
                {
                    if (line[i] == '<') in_breackets = true;
                    if (line[i] == '>')
                    {
                        Console.Write(line[i]);
                        in_breackets = false;
                        continue;
                    }
                    if (in_breackets)
                    {
                        if (!has_changed)
                        {
                            Theme_Color = Theme_Color == true ? false : true;
                            has_changed = true;
                        }
                        Set_Theme(Theme_Color); // Ставим тему (светлая)
                    }
                    else
                    {
                        if (has_changed)
                        {
                            Theme_Color = Theme_Color == false ? true : false;
                            has_changed = false;
                        }
                        Set_Theme(Theme_Color); // Ставим тему (темная)
                    }
                    Console.Write(line[i]);
                }
                Console.WriteLine();
            }
        }
        public void Get_Info()
        {
            Console.Clear();
            Console.WriteLine("-------------------------");
            Console.WriteLine($"NAME : {User_Name}\nBALANCE : {Balance} \nID: {ID}");
            Console.WriteLine("Действия : ");
            Console.WriteLine("1 - Пополнить счет.");
            Console.WriteLine("2 - Совершить оплату.");
            Console.WriteLine("3 - Совершить перевод.");
            Console.WriteLine("4 - Войти на Трейд-площадку");
            Console.WriteLine("5 - Настройки");
            Console.WriteLine("6 - Посмотреть историю.");
            Console.WriteLine("7 - Выход из аккаунта.");
            Console.WriteLine("-------------------------");
        }
        // Count of users
        public override string ToString() => $"{User_Name} {PASSWORD} {Balance} {ID} {Theme_Color}";
        public static int Count() => account_count;
    }
}
