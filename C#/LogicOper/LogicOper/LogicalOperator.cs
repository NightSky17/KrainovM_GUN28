using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeWork
{
    internal class LogicalOperator
    {
        static void Main(string[] args)
        {
         Console.WriteLine("Enter first number");
            if (int.TryParse(Console.ReadLine(), out int a))
            {
                // обработка
            }
            else
            {
                Console.WriteLine("Error!"); 
                return;
            }
            Console.WriteLine("Enter second number");
            if (int.TryParse(Console.ReadLine(), out int b))
            {
                // обработка
            }
            else
            {
                Console.WriteLine("Error!");
                return;
            }
            Console.WriteLine("Enter symbol (&, |, ^)");
            string input = Console.ReadLine();
            if (input != "&" && input != "|" && input != "^")
            {
                Console.WriteLine("Error!");
                return;
            }
            int result;
            switch (input)
            {
                case "&":
                    result = a & b;
                    break;
                case "|":
                    result = a | b;
                    break;
                case "^":
                    result = a ^ b;
                    break;
                default:
                    Console.WriteLine("Error!");
                    return;
            }
            Console.WriteLine("Result (Decimal): " + result);
            Console.WriteLine("Result (Binary): " + Convert.ToString(result, 2));
            Console.WriteLine("Result (Hexadecimal): " + result.ToString("X"));
        }
    }
}
