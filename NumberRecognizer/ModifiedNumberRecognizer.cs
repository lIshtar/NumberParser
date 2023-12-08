using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberRecognizer
{
	public class ModifiedNumberRecognizer
	{
		public int TryGetNumberFromString(string inputString, out Error error)
		{
			error = new Error();
			int wordNumber = 0;
			int recognizedNumber = 0;
			List<string> inputWords = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
			if (inputWords.Count == 0)
			{
				error = new Error(ErrorCode.NoElements);
				return 0;
			}

			int centCounter = 0;
			foreach (string word in inputWords)
			{
				if (!correctWords.Contains(word))
				{
					error = new Error(ErrorCode.IncorrectWord, word);
					return 0;
				}
				else if (word == "cent")
				{
					centCounter++;
				}
			}
			if (centCounter > 1)
			{
				error = new Error(ErrorCode.UnexpectedSequenceAfterEndOfNumber);
				error.errorText = "не может использоваться два и более слова cent";
				return 0;
			}

			if (inputWords[0] == "et")
			{
				error = new Error(ErrorCode.BeginWithEt);
				return 0;
			}

			int recognizedIntermediate = TryRecognizeUnits(inputWords, ref wordNumber, out error);

			// слово un может быть только числом 1 на первой позиции
			if (recognizedIntermediate == 1)
			{
				if (inputWords.Count > 1)
				{
					error = new Error(ErrorCode.UnexpectedSequenceAfterEndOfNumber);
					error.errorText = "слово un может быть только числом 1 на первой позиции";
					return 0;
				}
			}

			// если распознанные единицы явлюятся всем числом
			if (recognizedIntermediate != 0 && inputWords.Count == wordNumber)
			{
				return recognizedIntermediate;
			}

			// проверка на особый случай вследствие особенности написания числа 80 
			if (recognizedIntermediate == 4 && inputWords.Count > wordNumber && inputWords[wordNumber] != "cent")
			{
				recognizedIntermediate = TryRecognizeEighty(inputWords, ref wordNumber, out error);

			}

			if (inputWords.Count > wordNumber && inputWords[wordNumber] == "cent")
			{
				if (wordNumber == 0)
					recognizedNumber = 100;
				else
					recognizedNumber = recognizedIntermediate * 100;
				wordNumber++;
			}
			else if (wordNumber != 0)
			{
				recognizedNumber = recognizedIntermediate;
			}

			if (wordNumber < inputWords.Count)
			{
				if (recognizedNumber >= 100 && (recognizedIntermediate = TryRecognizeUnits(inputWords, ref wordNumber, out error)) != 0)
				{
					if (recognizedIntermediate == 4 && inputWords.Count > wordNumber)
					{
						recognizedNumber += TryRecognizeEighty(inputWords, ref wordNumber, out error);
					}
					else
					{
						recognizedNumber += recognizedIntermediate;
					}
						
				}
				else if (Tens.ContainsKey(inputWords[wordNumber]) && (recognizedNumber >= 100 || recognizedNumber == 0))
				{
					int recognizedTens = Tens[inputWords[wordNumber]];
					recognizedNumber += recognizedTens;
					wordNumber++;
					if (wordNumber != inputWords.Count)
					{
						if (recognizedTens == 60)
						{
							// проверка на 1-19
							int recognizedOneTwenty = TryRecognizeTenTwenty(inputWords, ref wordNumber, out error, true);
							if (recognizedOneTwenty == 0)
							{
								recognizedOneTwenty = TryRecognizeUnits(inputWords, ref wordNumber, out error, true);
							}
							if (wordNumber != inputWords.Count)
							{
								error = new Error(ErrorCode.UnexpectedSequenceAfterEndOfNumber);
								error.errorText = ErrorHandler(inputWords, wordNumber);
								return 0;
							}
							else
							{
								return (recognizedNumber += recognizedOneTwenty);
							}
						}
						else
						{
							recognizedIntermediate = TryRecognizeUnits(inputWords, ref wordNumber, out error, true);
							if (recognizedIntermediate == 0)
							{
								error = new Error(ErrorCode.UnexpectedSequenceAfterEndOfNumber);
								error.errorText = ErrorHandler(inputWords, wordNumber);
								return 0;
							}
							else
							{
								recognizedNumber += recognizedIntermediate;
							}
						}
					}
				}
				else if (TenToTwenty.ContainsKey(inputWords[wordNumber]))
				{
					// проверка на 10 - 19
					recognizedIntermediate = TryRecognizeTenTwenty(inputWords, ref wordNumber, out error);
					recognizedNumber += recognizedIntermediate;
				}
			}
			if (wordNumber != inputWords.Count)
			{
				error = new Error(ErrorCode.UnexpectedSequenceAfterEndOfNumber);
				error.errorText = ErrorHandler(inputWords, wordNumber);
				return 0;
			}
			return recognizedNumber;
		}

		string ErrorHandler(List<string> inputWords, int wordNumber)
		{
			if (Tens.ContainsKey(inputWords[wordNumber - 1]) && Tens.ContainsKey(inputWords[wordNumber]))
			{
				return "десятки не могут стоять после десятков";
			}
			else if (units.ContainsKey(inputWords[wordNumber - 1]) && Tens.ContainsKey(inputWords[wordNumber]))
			{
				return "единицы не могут стоять перед десятками";
			}
			else if (TenToTwenty.ContainsKey(inputWords[wordNumber - 1]) && Tens.ContainsKey(inputWords[wordNumber]))
			{
				return "десятки не могут стоять после чисел десять-девятнадцать";
			}
			else if (units.ContainsKey(inputWords[wordNumber - 1]) && TenToTwenty.ContainsKey(inputWords[wordNumber]))
			{
				return "числа десять-девятнадцать не могут стоять после единиц";
			}
			else if (TenToTwenty.ContainsKey(inputWords[wordNumber - 1]) && units.ContainsKey(inputWords[wordNumber]))
			{
				return "единицы не могут стоять после чисел десять-девятнадцать";
			}
			else if (Tens.ContainsKey(inputWords[wordNumber - 1]) && TenToTwenty.ContainsKey(inputWords[wordNumber]))
			{
				return "числа десять-девятнадцать не могут стоять после десятков";
			}
			else if (TenToTwenty.ContainsKey(inputWords[wordNumber - 1]) && TenToTwenty.ContainsKey(inputWords[wordNumber]))
			{
				return "числа десять-девятнадцать не могут стоять после числел десять-девятнадцать";
			}
			else if (units.ContainsKey(inputWords[wordNumber - 1]) && units.ContainsKey(inputWords[wordNumber]))
			{
				return "единицы не могут стоять после единиц";
			}
			else if (TenToTwenty.ContainsKey(inputWords[wordNumber - 1]) && inputWords[wordNumber] == "cent")
			{
				return "сотни не могут стоять после чисел десять-девятнадцать";
			}
			else if (Tens.ContainsKey(inputWords[wordNumber - 1]) && inputWords[wordNumber] == "cent")
			{
				return "сотни не могут стоять после десятков";
			}
			else if (units.ContainsKey(inputWords[wordNumber - 1]) && inputWords[wordNumber] == "cent")
			{
				return "сотни не могут стоять после единиц, не стоящих на первом месте";
			}
			else if (inputWords.Contains("cent") && (
				Tens.Select(t => inputWords.IndexOf(t.Key)).Where(x => x >= 0) is null ?
				Tens.Select(t => inputWords.IndexOf(t.Key)).Where(x => x >= 0).Min() < inputWords.IndexOf("cent") : false
				 )
			)
			//else if (inputWords.Contains("cent") && (
			//	Tens.Select(t => inputWords.IndexOf(t.Key)).Where(x => x >= 0)?.Min(x => x < inputWords.IndexOf("cent")) < inputWords.IndexOf("cent") : false
			//	 )
			//)
			{
				return "сотни не могут стоять после десятков";
			}
			else
			{
				return "";
			}
		}

		int TryRecognizeUnits(List<string> inputWords, ref int wordNumber, out Error error, bool EtUnEnable = false)
		{
			error = new Error();
			int recognizedNumber = 0;
			if (wordNumber >= inputWords.Count)
			{
				error = new Error(ErrorCode.EndOfConsequence, wordNumber);
				return 0;
			}

			if (units.ContainsKey(inputWords[wordNumber]))
			{
				if (EtUnEnable && inputWords[wordNumber] == "un")
				{
					error = new Error(ErrorCode.EtUnError, wordNumber);
					return 0;
				}
				recognizedNumber = units[inputWords[wordNumber]];
			}
			else if (EtUnEnable && inputWords[wordNumber] == "et")
			{
				wordNumber++;
				if (wordNumber >= inputWords.Count)
				{
					error = new Error(ErrorCode.EndOfConsequence, wordNumber);
					return 0;
				}
				if (inputWords[wordNumber] == "un")
				{
					recognizedNumber = 1;
				}
				else
				{
					error = new Error(ErrorCode.EtUnError, wordNumber);
					return 0;
				}
			}
			if (recognizedNumber != 0)
				wordNumber++;
			return recognizedNumber;
		}

		int TryRecognizeEighty(List<string> inputWords, ref int wordNumber, out Error error)
		{
			int recognizedNumber = 0;
			error = new Error();
			if (inputWords[wordNumber] == "vingt")
			{
				wordNumber++;

				// тут теперь надо опознать числа от 1 до 19
				int recognizedOneTwenty = TryRecognizeTenTwenty(inputWords, ref wordNumber, out error);
				if (recognizedOneTwenty == 0)
				{
					recognizedOneTwenty = TryRecognizeUnits(inputWords, ref wordNumber, out error);
				}

				return (recognizedNumber = 80 + recognizedOneTwenty);
			}
			return 0;
		}

		int TryRecognizeTenTwenty(List<string> inputWords, ref int wordNumber, out Error error, bool EtOnzeEnable = false)
		{
			error = new Error();
			int recognizedNumber = 0;
			if (wordNumber < inputWords.Count)
			{
				if (TenToTwenty.ContainsKey(inputWords[wordNumber]))
				{
					recognizedNumber = TenToTwenty[inputWords[wordNumber]];
					if (EtOnzeEnable && inputWords[wordNumber] == "onze")
					{
						error = new Error(ErrorCode.EtOnzeError, wordNumber);
						return 0;
					}
					if (recognizedNumber == 10)
					{	
						if (wordNumber < inputWords.Count)
						{
							wordNumber++;
							int checkedUnit = TryRecognizeUnits(inputWords, ref wordNumber, out error);
							if (checkedUnit == 7 || checkedUnit == 8 || checkedUnit == 9)
							{
								recognizedNumber += checkedUnit;
							}
							if (checkedUnit > 0 && checkedUnit < 7) wordNumber--;
							wordNumber--;
						}
					}
					wordNumber++;
				}
				else if (EtOnzeEnable && inputWords[wordNumber] == "et")
				{
					wordNumber++;
					if (inputWords[wordNumber] == "onze")
					{
						recognizedNumber = 11;
						wordNumber++;
					}
					else
					{
						error = new Error(ErrorCode.EtOnzeError, wordNumber);
						wordNumber--;
						return 0;
					}
					
				}
			}
			else
			{
				error = new Error(ErrorCode.EndOfConsequence);
				return 0;
			}
			//if (recognizedNumber != 0)
			//	wordNumber++;
			return recognizedNumber;
		}

		Dictionary<string, int> units = new()
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

		Dictionary<string, int> Tens = new()
		{
			{ "vingt", 20 },
			{ "trente", 30 },
			{ "quarante", 40 },
			{ "cinquante", 50 },
			{ "soixante", 60 },
            //{ "soixante-dix", 70},
            { "quatre", 80},
            //{ "quatre-vingt-dix", 90}
        };

		Dictionary<string, int> TenToTwenty = new()
		{
			{ "onze", 11 },
			{ "douze", 12 },
			{ "treize", 13 },
			{ "quatorze", 14 },
			{ "quinze", 15 },
			{ "seize", 16 },
			{ "dix", 10 }
		};

		List<string> correctWords = new()
		{
			"un",
			"et",
			"deux",
			"trois",
			"quatre",
			"cinq",
			"six",
			"sept",
			"huit",
			"neuf",
			"dix",
			"onze",
			"douze",
			"treize",
			"quatorze",
			"quinze",
			"seize",
			"sept",
			"huit",
			"neuf",
			"vingt",
			"trente",
			"quarante",
			"cinquante",
			"soixante",
			"cent"
		};
	}
}
