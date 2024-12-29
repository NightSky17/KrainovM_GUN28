using System;
using System.Collections.Generic;

namespace HomeWork
{
    internal class Program
    {
        // Задача 1: Работа со списком строк
        private class ListTask
        {
            private readonly List<string> _listOfStrings;

            public ListTask()
            {
                _listOfStrings = new List<string>
                {
                    "Первый элемент",
                    "Второй элемент",
                    "Третий элемент"
                };
            }

            public void TaskLoop()
            {
                Console.WriteLine("Содержимое списка:");
                DisplayList();

                Console.WriteLine("Введите строку для добавления в конец списка:");
                string newString = Console.ReadLine();
                _listOfStrings.Add(newString);
                DisplayList();

                Console.WriteLine("Введите строку для добавления в середину списка:");
                string middleString = Console.ReadLine();
                _listOfStrings.Insert(_listOfStrings.Count / 2, middleString);
                DisplayList();

                Console.WriteLine("Для завершения введите –exit");
                string exitCommand = Console.ReadLine();
                if (exitCommand.ToLower() == "-exit")
                {
                    Console.WriteLine("Завершение задачи 1.");
                }
            }

            private void DisplayList()
            {
                foreach (var item in _listOfStrings)
                {
                    Console.WriteLine(item);
                }
            }
        }

        // Задача 2: Работа с словарем студентов
        private class DictionaryTask
        {
            private readonly Dictionary<string, double> _students;

            public DictionaryTask()
            {
                _students = new Dictionary<string, double>();
            }

            public void TaskLoop()
            {
                while (true)
                {
                    Console.WriteLine("Введите имя студента:");
                    string name = Console.ReadLine();

                    Console.WriteLine("Введите оценку (от 2 до 5):");
                    if (double.TryParse(Console.ReadLine(), out double grade) && grade >= 2 && grade <= 5)
                    {
                        _students[name] = grade;
                    }
                    else
                    {
                        Console.WriteLine("Оценка должна быть от 2 до 5.");
                        continue;
                    }

                    Console.WriteLine("Хотите ввести еще одного студента? (y/n)");
                    string continueInput = Console.ReadLine();
                    if (continueInput.ToLower() != "y")
                    {
                        break;
                    }
                }

                Console.WriteLine("Введите имя студента для проверки оценки:");
                string checkName = Console.ReadLine();
                if (_students.ContainsKey(checkName))
                {
                    Console.WriteLine($"Оценка студента {checkName}: {_students[checkName]}");
                }
                else
                {
                    Console.WriteLine("Студент с таким именем не существует.");
                }

                Console.WriteLine("Для завершения введите –exit");
                string exitCommand = Console.ReadLine();
                if (exitCommand.ToLower() == "-exit")
                {
                    Console.WriteLine("Завершение задачи 2.");
                }
            }
        }

        // Задача 3: Двусвязный список
        private class LinkedListTask
        {
            private class Node
            {
                public string Data;
                public Node Next;
                public Node Prev;

                public Node(string data)
                {
                    Data = data;
                    Next = null;
                    Prev = null;
                }
            }

            private Node _head;

            public LinkedListTask()
            {
                _head = null;
            }

            public void TaskLoop()
            {
                Console.WriteLine("Введите от 3 до 6 элементов для двусвязного списка:");

                for (int i = 0; i < 3; i++)
                {
                    Console.Write($"Введите элемент {i + 1}: ");
                    string input = Console.ReadLine();
                    AddToEnd(input);
                }

                DisplayListForward();
                DisplayListBackward();

                Console.WriteLine("Для завершения введите –exit");
                string exitCommand = Console.ReadLine();
                if (exitCommand.ToLower() == "-exit")
                {
                    Console.WriteLine("Завершение задачи 3.");
                }
            }

            private void AddToEnd(string data)
            {
                Node newNode = new Node(data);
                if (_head == null)
                {
                    _head = newNode;
                    return;
                }

                Node temp = _head;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = newNode;
                newNode.Prev = temp;
            }

            private void DisplayListForward()
            {
                Console.WriteLine("Список в прямом порядке:");
                Node temp = _head;
                while (temp != null)
                {
                    Console.WriteLine(temp.Data);
                    temp = temp.Next;
                }
            }

            private void DisplayListBackward()
            {
                Console.WriteLine("Список в обратном порядке:");
                Node temp = _head;
                if (temp == null) return;

                while (temp.Next != null)
                {
                    temp = temp.Next;
                }

                while (temp != null)
                {
                    Console.WriteLine(temp.Data);
                    temp = temp.Prev;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите 1, 2 или 3 для выбора задания 1, 2 или 3");
            if (int.TryParse(Console.ReadLine(), out int task))
            {
                switch (task)
                {
                    case 1:
                        var listTask = new ListTask();
                        listTask.TaskLoop();
                        break;
                    case 2:
                        var dictionaryTask = new DictionaryTask();
                        dictionaryTask.TaskLoop();
                        break;
                    case 3:
                        var linkedListTask = new LinkedListTask();
                        linkedListTask.TaskLoop();
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
            }
        }
    }
}
