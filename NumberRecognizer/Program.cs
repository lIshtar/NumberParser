using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NumberRecognizer
{
	internal class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			int number;
			Error error;
			var recognizer = new ModifiedNumberRecognizer();
			while (true)
			{
				
				string? input = "cent " + Console.ReadLine().Trim(new char[] { ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }).ToLower().Replace("-", " ");
				if (input != null)
				{
					number = recognizer.TryGetNumberFromString(input, out error);
					if (number == 0)
						Console.WriteLine(error.errorCode.ToString());
					else
						Console.WriteLine(number);
				}
			}
		}
	}
}
