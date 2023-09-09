using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NumberParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var numberRecognizer = new NumberRecognizer();
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (input == null || input == "")
                {
                    Console.WriteLine("> Input string can't be empty");
                    continue;
                }

                int recognizedNumber = numberRecognizer.GetNumberFromString(input);
                if (recognizedNumber != 0)
                {
                    Console.WriteLine($"> {recognizedNumber}");
                }
                else
                {
                    Console.WriteLine("> Incorrect format of input string");
                }
            }
        }
    }
}