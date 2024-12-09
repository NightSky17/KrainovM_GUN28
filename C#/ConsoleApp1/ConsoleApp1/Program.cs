using System;

namespace HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                                                                    ЭТО ЗАДАНИЕ №1
                    int prev = 1;
                    int curr = 1;
                    Console.WriteLine("1\n1");
                            for (int i = 1; i <= 8; i++)
                    {
                        int next = prev + curr;
                        Console.WriteLine(prev + curr);
                        prev = curr;
                        curr = next;
                    }
                                                                    ЭТО ЗАДАНИЕ №2
                    for (int i = 2; i <= 20; i=i+2)
                    {
                        Console.WriteLine(i);
                    }
                                                                    ЭТО ЗАДАНИЕ №3
                    int n = 1;
                    for (int i = 1; i < 10; i++)
                    {
                        for (int j = 1; j < 5; j++)
                        Console.Write(j + "*" + i + "=" + j * i + "    ");
                        Console.WriteLine();
                                }
                                                                    ЭТО ЗАДАНИЕ №4*/
            string password = "qwerty";
            string a = Console.ReadLine();
            while (a != password)
            {
                a = Console.ReadLine();
            }
            Console.WriteLine("Добро пожаловать!");
        }
    }
}