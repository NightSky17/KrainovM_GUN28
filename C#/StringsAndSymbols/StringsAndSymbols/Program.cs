using System;
using System.Text;

namespace HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Проверка метода 1
            Console.WriteLine(ConcatenateStrings("Hello", " World"));

            // Проверка метода 2
            Console.WriteLine(GreetUser("Максим", 25));

            // Проверка метода 3
            Console.WriteLine(AnalyzeString("Hello World"));

            // Проверка метода 4
            Console.WriteLine(GetFirstFiveCharacters("UnityGame"));

            // Проверка метода 5
            string[] words = { "This", "is", "a", "test" };
            Console.WriteLine(BuildSentence(words));

            // Проверка метода 6
            Console.WriteLine(ReplaceWords("Hello world", "world", "universe"));
        }

        // Задание 1: Конкатенация строк
        public static string ConcatenateStrings(string str1, string str2)
        {
            return str1 + str2;
        }

        // Задание 2: Приветствие пользователя
        public static string GreetUser(string name, int age)
        {
            return $"Hello, {name}!\nYou are {age} years old.";
        }

        // Задание 3: Анализ строки
        public static string AnalyzeString(string input)
        {
            return $"Length: {input.Length}\nUppercase: {input.ToUpper()}\nLowercase: {input.ToLower()}";
        }

        // Задание 4: Первые 5 символов строки
        public static string GetFirstFiveCharacters(string input)
        {
            if (input.Length < 5)
                return input;
            return input.Substring(0, 5);
        }

        // Задание 5: Объединение строк в предложение
        public static StringBuilder BuildSentence(string[] words)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var word in words)
            {
                sb.Append(word + " ");
            }
            return sb;
        }

        // Задание 6: Замена слова в строке
        public static string ReplaceWords(string inputString, string wordToReplace, string replacementWord)
        {
            return inputString.Replace(wordToReplace, replacementWord);
        }
    }
}
