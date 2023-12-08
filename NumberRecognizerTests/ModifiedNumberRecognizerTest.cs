using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using NumberRecognizer;

namespace NumberRecognizerTests
{
	public class ModifiedNumberRecognizerTest
	{
		[Theory]
		[MemberData(nameof(TestFileManager))]
		public void Test1(int resultNumber, string numberInWords)
		{
			// arrange
			var modifiedNumberRecognizer = new ModifiedNumberRecognizer();
			Error error;

			// act
			int result = modifiedNumberRecognizer.TryGetNumberFromString(numberInWords, out error);

			// assert
			Assert.Equal(resultNumber, result);
		}

		public static IEnumerable<object[]> TestFileManager()
		{
			string path = "C:\\Users\\kamis\\Source\\Repos\\lIshtar\\NumberParser\\NumberRecognizerTests\\Test.txt";

			using (StreamReader reader = new StreamReader(path))
			{
				string? line;
				while ((line = reader.ReadLine()) != null)
				{
					string resultString = Regex.Match(line, @"\d+").Value;
					int parsedNumber = Int32.Parse(resultString);
					string parsedNumberAsString = line.Trim(new char[] { ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }).ToLower();
					parsedNumberAsString = parsedNumberAsString.Replace("-", " ");
					//if (parsedNumber == 1)
					//{
						foreach (var unit in units)
						{
							int resultNumber = parsedNumber + 100 * unit.Value;
							string numberAsString = unit.Key + " cent " + parsedNumberAsString;
							yield return new object[] { resultNumber, numberAsString };
						}
						yield return new object[] { parsedNumber, parsedNumberAsString };
						yield return new object[] { 100 + parsedNumber, " cent " + parsedNumberAsString };
					//}
					
				}
			}
		}

		//public static IEnumerable<object[]> TestFileManager()
		//{
		//	yield return new object[] { 101, "cent un" };
		//}

		static Dictionary<string, int> units = new()
		{
			{ "deux", 2 },
			{ "trois", 3 },
			{ "quatre", 4 },
			{ "cinq", 5 },
			{ "six", 6 },
			{ "sept", 7},
			{ "huit", 8},
			{ "neuf", 9}
		};
	}
}