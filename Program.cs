using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank
{
    class Program
    {
        static readonly string DATE_PATH = @"C:\Users\firef\source\repos\Bank\USERS\DATE.txt";
        static List<BankAccount> Accounts = new List<BankAccount>();
        public static List<double> Currency = new List<double>();

        public static void Get_Currency()
        {
            string URL_Site = @"https://www.banki.ru/products/currency/cb/";

            System.Net.WebClient wc = new System.Net.WebClient();

            string input = wc.DownloadString(URL_Site);
            string pattern = @"<td>([0-9]+\.[0-9]+)</td>";

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
                Currency.Add(double.Parse(match.Groups[1].Value));
        }
        public static bool String_Is_Correct(ref string s)
        {
            int t = 0; //Количество ','
            bool correct = true;
            s = s.Replace('.', ',');
            if (s[0] == ',') correct = false;
            if (s[s.Length - 1] == ',') s += '0'; // Добавляем в конце 0 при таких ситуациях 3434,
            foreach (var elem in s)
            {
                if (elem == ',') t++;
                else if (!Char.IsDigit(elem)) correct = false; // Нет ли не цифр, кроме ','
            }

            return correct && t < 2;
        }
        public static void Set_Default_Theme()
        {
             Console.ForegroundColor = ConsoleColor.White;
             Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void Set_Theme(bool Dark_Theme)
        {
            if (Dark_Theme)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
        }
        public static void Sign_Up()
        {
            string Input_User_Name, Input_PASSWORD, Sec_Password;

            Console.Write("Create your User Name : ");
            Input_User_Name = Console.ReadLine();
            Input_User_Name = Input_User_Name.Replace(' ', '-');
            Input_User_Name = Input_User_Name.Replace('_', '-');

            bool Already_Exist = false;
            foreach (var user in Accounts) if (user.User_Name == Input_User_Name) Already_Exist = true;

            while (Already_Exist)
            {
                Already_Exist = false;
                Console.WriteLine("User with the same name already exists...");
                Console.Write("Create your User Name : ");
                Input_User_Name = Console.ReadLine();
            }
            Console.Write("Create a new password : ");
            Input_PASSWORD = Console.ReadLine();

            while (Input_PASSWORD.Length < 5 && Input_PASSWORD.Length > 10)
            {
                Console.Write("Password must be at least 5 characters long and no more than 8 : ");
                Input_PASSWORD = Console.ReadLine();
            }

            Console.Write("Create your password again : ");
            Sec_Password = Console.ReadLine();

            while (Sec_Password != Input_PASSWORD)
            {
                Console.Write("Passwords do not match... \nCreate again : ");
                Sec_Password = Console.ReadLine();
            }

            Accounts.Add(new BankAccount(Input_User_Name, Input_PASSWORD, 0, true));
            PUSH_DATE(Accounts);
        }
        public static void Sign_In(ref BankAccount User)
        {
            string Input_User_Name, Input_PASSWORD;
            User = null;

            SIGN_IN_START:
            Console.Write("Enter user name : ");
            Input_User_Name = Console.ReadLine();
            Console.Write("Enter password : ");
            Input_PASSWORD = Console.ReadLine();

            foreach (var user in Accounts)
            {
                if (user.User_Name == Input_User_Name && user.PASSWORD == Input_PASSWORD)
                {
                    User = user;
                    User.Get_Info();
                }
            }

            if (User == null)
            {
                Console.WriteLine("Your user name or password Incorrect.\nTry again ...");
                goto SIGN_IN_START;
            }
        }
        public static void Money_Top_Up(ref BankAccount User)
        {
            Console.Write("На какую сумму хотите пополнить счет ? : ");
            string add = Console.ReadLine();


            if (String_Is_Correct(ref add))
                User.Money_Top_Up(double.Parse(add));

            PUSH_DATE(Accounts);
        }
        public static void Money_Payment(ref BankAccount User)
        {
            Console.Write("Какая сумма списания ? ");
            string lose = Console.ReadLine();

            if (String_Is_Correct(ref lose))
                User.Money_Payment(double.Parse(lose));

            PUSH_DATE(Accounts);
        }
        public static void Settings(ref BankAccount User)
        {
            SETTING_STAT:
            Set_Theme(User.Theme_Color); // Ставим тему (светлая / темная)
            Console.Clear();
            Console.WriteLine("1 - Выбрать цвет темы");
            Console.WriteLine("2 - Выход");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        Console.WriteLine("1 - Сделать тему темной\n2 - Сделать тему светлой");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                {
                                    User.Theme_Color = true;
                                    Console.Clear();
                                    goto SETTING_STAT;
                                }
                            case "2":
                                {
                                    User.Theme_Color = false;
                                    goto SETTING_STAT;
                                }
                        }
                        Set_Theme(User.Theme_Color); // Ставим измененную тему (светлая / темная)
                        goto SETTING_STAT;
                    }
                case "2": break;
            }
        }
        public static void GET_DATE(ref List<BankAccount> Accounts)
        {

            if (!File.Exists(DATE_PATH) || File.ReadAllText(DATE_PATH).Length == 0)
            {
                File.WriteAllText(DATE_PATH, "");
            }
            else
            {
                StreamReader sr = new StreamReader(DATE_PATH);
                int line_counter = 0;
                while (sr.ReadLine() != null) //читаем по одной линии(строке) пока не вычитаем все из потока (пока не достигнем конца файла)
                {
                    line_counter++;
                }

                string[] Users_Date = File.ReadAllLines(DATE_PATH); // Массив из всех строк

                for (int i = 0; i < line_counter; i++)
                {
                    string[] One_User_Date = Users_Date[i].Split('_'); // Разделяем строку на подстроки знаком '_'
                    string User_Name = One_User_Date[0];
                    string Password = One_User_Date[1];
                    string Balance = One_User_Date[2];
                    bool Theme_Color = One_User_Date[3] == "True";

                    Accounts.Add(new BankAccount(User_Name, Password, double.Parse(Balance), Theme_Color));
                }
                sr.Close();
            }
        }
        public static void PUSH_DATE(List<BankAccount> Accounts)
        {
            StreamWriter sr = new StreamWriter(DATE_PATH, false);

            foreach(var i in Accounts)
            {
                sr.WriteLine(String.Join("_",i.ToString().Split(' '))); // %$#*&@
            }
            sr.Close();
        }
        public static void Show_All_Date()
        {
            Console.WriteLine("\n____________________________________________");
            foreach (var i in Accounts)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("____________________________________________|\n");
        }
        public static void LOGIN_TO_THE_SYSTEM(ref BankAccount User)
        {
            Start: // CHECK POINT
            Console.Clear();
            Show_All_Date();
            Console.WriteLine("Вы уже заригистрированны ?(Yes / No / Exit) : ");
            switch (Console.ReadLine())
            {
                case "Yes":
                    {
                        Sign_In(ref User);
                    }
                    break;
                case "No":
                    {
                        Sign_Up();
                        goto Start;
                    }
                case "Exit":
                    {
                        PUSH_DATE(Accounts);
                        Environment.Exit(0);
                    }break;
                default:
                    {
                        goto Start;
                    }
            }
        } 
        public static void TRADING_PLATFFORM(ref BankAccount User)
        {
            Get_Currency();
            char selected_currency = Console.ReadLine()[0];
            switch (selected_currency)
            {
                case '1':
                    {
                        break;
                    }
            }
        }
        public static void ACIONS_IN_THE_SYSTEM(ref BankAccount User)
        {
            ACTION_START:
            Set_Theme(User.Theme_Color); // Ставим тему (светлая / темная)
            User.Get_Info();
            switch (Console.ReadLine())
            {
                case "1": // Пополнить счет
                    {
                        Money_Top_Up(ref User);
                        goto ACTION_START;
                    }
                case "2": // Оплата
                    {
                        Money_Payment(ref User);
                        goto ACTION_START;
                    }
                case "3": // Перевод
                    {
                        if (BankAccount.account_count < 2)
                        {
                            Console.WriteLine("Функция пока что не доступна. Количество пользователей мнеьше 2.");
                            Console.ReadKey();
                            goto ACTION_START;
                        }

                        Console.Write("Введите ID пользователя (5 знаков) : ");
                        string ID = Console.ReadLine();

                        bool ID_Exist = false;
                        BankAccount Reciever = null;
                        foreach (var reciever in Accounts)
                        {
                            if (reciever.ID == ID)
                            {
                                Reciever = reciever;
                                ID_Exist = true;
                            }
                        }

                        if (!ID_Exist)
                        {
                            Console.WriteLine("Такого пользователя не сушествует ...");
                            Console.ReadKey();
                            goto ACTION_START;
                        }
                        Console.Write("Введите сумму перевода : ");
                        string Trans = Console.ReadLine();
                        

                        if (User.Balance < double.Parse(Trans) && String_Is_Correct(ref Trans))
                        {
                            Console.WriteLine("Не хвататет средств на счету ... ");
                            Console.ReadKey();
                            goto ACTION_START;
                        }

                        if (String_Is_Correct(ref Trans) && ID_Exist)
                        {
                            User.Money_Transfer_To(double.Parse(Trans), ref Reciever);
                            Reciever.Money_Transfer_From(double.Parse(Trans), ref User);
                        }

                        PUSH_DATE(Accounts);
                        goto ACTION_START;
                    }
                case "4":
                    {
                        //LOGIN_TO_THE_TRADING_PLATFORM();
                        goto ACTION_START;
                    }
                case "5":
                    {
                        Settings(ref User);
                        goto ACTION_START;
                    }
                case "6": // История
                    {
                        Console.Clear();
                        User.Show_History();
                        Console.ReadKey();
                        goto ACTION_START;
                    }
                case "7": // выход
                    {
                        Set_Default_Theme();
                        LOGIN_TO_THE_SYSTEM(ref User);
                        goto ACTION_START;
                    }
                default:
                    {
                        goto ACTION_START;
                    }
            }
        }
        static void Main(string[] args)
        {
            BankAccount User = null;
            GET_DATE(ref Accounts);

            LOGIN_TO_THE_SYSTEM(ref User);

            ACIONS_IN_THE_SYSTEM(ref User);

            PUSH_DATE(Accounts);
            Console.ReadKey();
        }
    }
}
