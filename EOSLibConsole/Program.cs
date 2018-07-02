using EOSLib;
using System;

namespace EOSLibConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 class1 = new Class1();
            class1.testp();
            class1.test = "xxxxxxxx";
            Console.WriteLine("Hello World!" + class1.test);
            Console.ReadLine();
        }
    }
}
