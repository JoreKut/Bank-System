﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            BankAccount user1 = new BankAccount("JOHN");
            user1.Money_Top_Up(5000);
            user1.Money_Payment(1359);
            user1.Money_Transfer(2300);
            user1.Show_History();
            Console.ReadKey();
        }
    }
}
