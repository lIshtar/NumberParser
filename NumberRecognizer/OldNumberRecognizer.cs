using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NumberRecognizer
{
	public class OldNumberRecognizer
	{
		public int GetNumberFromString(string numberText, out int recognizedNumber)
		{
			numberText = numberText.Trim().ToLower();
			int number = 0;
			int hundreds = CountHundreds(ref numberText);
			if (hundreds == 0)
			{
				// apparently, i need to recognize 0-99 too
				//recognizedNumber = 0;
				//return 0;
			}

			number += hundreds;
			if (numberText.Length > 0)
			{
				var tens = CountTens(ref numberText);
				if (tens == 0)
				{
					// i need to придумать error code for this 
					recognizedNumber = 0;
					return 0;
				}
				number += tens;
				if (tens > 20)
				{
					if (!(numberText == "" || numberText == "s" && tens == 80))
					{
						int foundUnit = 0;
						if (tens == 60 || tens == 80)
						{
							foundUnit = CountUnitsToTwenty(ref numberText, (SixtyEightyContext)tens);
						}
						else
						{
							foundUnit = CountUnitsToTen(ref numberText);
						}
						if (foundUnit == 0)
						{
							// i need to придумать error code for this 
							recognizedNumber = 0;
							return 0;
						}
						else
						{
							number += foundUnit;
						}
					}
					else if (numberText == "s" && tens == 80)
					{
						numberText = "";
					}
				}
			}
			if (numberText == "")
			{
				// i need to придумать success code for this 
				recognizedNumber = number;
				return 0;
			}
			else
			{
				// i need to придумать error code for this 
				recognizedNumber = 0;
				return 0;
			}
		}

		int CountHundreds(ref string numberText)
		{
			int number = 0;
			int centIndex = numberText.IndexOf(OneHundred.Key);

			if (centIndex == -1)
			{
				return 0;
			}
			else if (centIndex == 0)
			{
				number += OneHundred.Value;
			}
			else
			{
				foreach (var unit in new List<string>(units.Keys))
				{
					if (numberText.IndexOf(unit) == 0 && unit != "un")
					{
						number += OneHundred.Value * units[unit];
						numberText = numberText.Remove(0, unit.Length);

						numberText = numberText.Trim();
						centIndex = numberText.IndexOf(OneHundred.Key);
						if (centIndex != 0)
						{
							return 0;
						}
						break;
					}
				}
			}
			numberText = numberText.Remove(0, OneHundred.Key.Length);
			numberText = numberText.Trim();

			return number;
		}

		int CountTens(ref string numberText)
		{
			bool isFound = false;
			int number = 0;
			foreach (var tenNumber in Tens)
			{
				if (numberText.IndexOf(tenNumber.Key) == 0)
				{
					number = tenNumber.Value;
					numberText = numberText.Remove(0, tenNumber.Key.Length);
					isFound = true;
					break;
				}
			}
			if (!isFound)
			{
				foreach (var unit in units)
				{
					if (numberText.IndexOf(unit.Key) == 0)
					{
						number = unit.Value;
						numberText = numberText.Remove(0, unit.Key.Length);
						isFound = true;
						break;
					}
				}
			}
			if (!isFound)
			{
				foreach (var unit in TenToTwenty)
				{
					if (numberText.IndexOf(unit.Key) == 0)
					{
						number = unit.Value;
						numberText = numberText.Remove(0, unit.Key.Length);
						break;
					}
				}
			}
			numberText = numberText.Trim();
			return number;
		}

		int CountUnitsToTen(ref string numberText)
		{
			int number = 0;
			numberText = numberText.Trim();
			if (numberText.IndexOf("et") == 0)
			{
				numberText = numberText.Remove(0, "et".Length);
				numberText = numberText.Trim();
				if (numberText == "un")
				{
					number = 1;
					numberText = numberText.Remove(0, "un".Length);
				}
			}
			else
			{
				foreach (var unit in units)
				{
					if (numberText.IndexOf(unit.Key) == 0 || numberText.IndexOf($"-{unit.Key}") == 0)
					{
						if (unit.Key == "un")
							return 0;
						number = unit.Value;
						int deleteLength = unit.Key.Length;
						if (numberText.StartsWith('-'))
							deleteLength++;
						numberText = numberText.Remove(0, deleteLength);
						break;
					}
				}
			}
			numberText = numberText.Trim();
			return number;
		}

		int CountUnitsToTwenty(ref string numberText, SixtyEightyContext OneWordContext)
		{
			bool isFound = false;
			int number = 0;
			numberText = numberText.Trim();
			if (numberText.IndexOf("et") == 0 && OneWordContext == SixtyEightyContext.Sixty)
			{
				numberText = numberText.Remove(0, "et".Length);
				numberText = numberText.Trim();
				if (numberText == "un")
				{
					number = 1;
					numberText = numberText.Remove(0, "un".Length);
				}
				else if (numberText == "onze")
				{
					number = 11;
					numberText = numberText.Remove(0, "onze".Length);
				}
			}
			else
			{
				foreach (var unit in units)
				{
					if (numberText.IndexOf(unit.Key) == 0 || numberText.IndexOf($"-{unit.Key}") == 0)
					{
						if (unit.Key == "un" && OneWordContext == SixtyEightyContext.Sixty)
							return 0;
						number = unit.Value;
						int deleteLength = unit.Key.Length;
						if (numberText.StartsWith('-'))
							deleteLength++;
						numberText = numberText.Remove(0, deleteLength);
						isFound = true;
						break;
					}
				}
				if (!isFound)
				{
					foreach (var unit in TenToTwenty)
					{
						if (numberText.IndexOf(unit.Key) == 0 || numberText.IndexOf($"-{unit.Key}") == 0)
						{
							if (unit.Key == "onze" && OneWordContext == SixtyEightyContext.Sixty)
								return 0;
							number = unit.Value;
							int deleteLength = unit.Key.Length;
							if (numberText.StartsWith('-'))
								deleteLength++;
							numberText = numberText.Remove(0, deleteLength);
							break;
						}
					}
				}
			}
			return number;
		}

		enum SixtyEightyContext
		{
			Sixty = 60,
			Eighty = 80
		}

		KeyValuePair<string, int> OneHundred = new KeyValuePair<string, int>("cent", 100);

		Dictionary<string, int> units = new Dictionary<string, int>()
		{
			{ "un", 1},
			{ "deux", 2 },
			{ "trois", 3 },
			{ "quatre", 4 },
			{ "cinq", 5 },
			{ "six", 6 },
			{ "sept", 7},
			{ "huit", 8},
			{ "neuf", 9}
		};

		Dictionary<string, int> Tens = new Dictionary<string, int>()
		{
			{ "vingt", 20 },
			{ "trente", 30 },
			{ "quarante", 40 },
			{ "cinquante", 50 },
			{ "soixante", 60 },
            //{ "soixante-dix", 70},
            { "quatre-vingt", 80},
            //{ "quatre-vingt-dix", 90}
        };


		//
		Dictionary<string, int> TenToTwenty = new Dictionary<string, int>()
		{

			{ "onze", 11 },
			{ "douze", 12 },
			{ "treize", 13 },
			{ "quatorze", 14 },
			{ "quinze", 15 },
			{ "seize", 16 },
			{ "dix-sept", 17},
			{ "dix-huit", 18},
			{ "dix-neuf", 19},
			{ "dix", 10 }
		};
	}
}
