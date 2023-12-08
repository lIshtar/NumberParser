using NumberParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberParserTests
{
    public class NumberRecognizerTests
    {
        [Theory]
        [MemberData(nameof(NumbersFrom160To199))]
        void GetNumberFromString_NumberFrom160To199_Passed(string recognizedString, int actualNumber)
        {
            //Arrange
            var numberRecognizer = new NumberRecognizer();

            //Act
            int recognizedNumber;
            int errorcode = numberRecognizer.GetNumberFromString(recognizedString, out recognizedNumber);

            //Assert
            Assert.Equal(actualNumber, recognizedNumber);
        }
        public static IEnumerable<object[]> NumbersFrom160To199()
        {
            yield return new object[] { "cent Quatre-vingt-dix-neuf ", 199 };
            yield return new object[] { " cent Quatre-vingt-dix-huit", 198 };
            yield return new object[] { " cent Quatre-vingt-dix-sept", 197 };
            yield return new object[] { " cent Quatre-vingt-seize  ", 196 };
            yield return new object[] { " cent Quatre-vingt-quinze", 195 };
            yield return new object[] { "cent Quatre-vingt-quatorze ", 194 };
            yield return new object[] { " cent Quatre-vingt-treize ", 193 };
            yield return new object[] { " cent Quatre-vingt-douze", 192 };
            yield return new object[] { " cent Quatre-vingt-onze  ", 191 };
            yield return new object[] { " cent Quatre-vingt-dix", 190 };
            yield return new object[] { "cent Quatre-vingt-neuf ", 189 };
            yield return new object[] { " cent Quatre-vingt-huit", 188 };
            yield return new object[] { " cent Quatre-vingt-sept", 187 };
            yield return new object[] { " cent Quatre-vingt-six ", 186 };
            yield return new object[] { " cent Quatre-vingt-cinq ", 185 };
            yield return new object[] { "cent Quatre-vingt-quatre", 184 };
            yield return new object[] { " cent Quatre vingt trois", 183 };
            yield return new object[] { " cent Quatre-vingt-deux ", 182 };
            yield return new object[] { " cent Quatre-vingt-un  ", 181 };
            yield return new object[] { " cent Quatre-vingts", 180 };
            yield return new object[] { "cent Soixante-dix-neuf ", 179 };
            yield return new object[] { " cent Soixante-dix-huit", 178 };
            yield return new object[] { " cent Soixante-dix-sept", 177 };
            yield return new object[] { " cent Soixante-seize ", 176 };
            yield return new object[] { " cent Soixante-quinze", 175 };
            yield return new object[] { "cent Soixante-quatorze  ", 174 };
            yield return new object[] { " cent Soixante-treize", 173 };
            yield return new object[] { " cent Soixante-douze", 172 };
            yield return new object[] { " cent Soixante et onze ", 171 };
            yield return new object[] { " cent Soixante-dix", 170 };
            yield return new object[] { "cent Soixante-neuf  ", 169 };
            yield return new object[] { " cent Soixante-huit", 168 };
            yield return new object[] { " cent Soixante-sept", 167 };
            yield return new object[] { " cent Soixante-six ", 166 };
            yield return new object[] { " cent Soixante-cinq ", 165 };
            yield return new object[] { "cent Soixante-quatre ", 164 };
            yield return new object[] { " cent Soixante-trois", 163 };
            yield return new object[] { " cent Soixante-deux", 162 };
            yield return new object[] { " cent Soixante et un ", 161 };
            yield return new object[] { " cent Soixante ", 160 };
			yield return new object[] { "cents" };
			yield return new object[] { "Cinq cent q" };
			yield return new object[] { "Six cent dexa" };
			yield return new object[] { "Neuf a cent" };
			yield return new object[] { "a Huit cent" };
			yield return new object[] { "cent Trente", 130 };
			yield return new object[] { " cent Vingt", 120 };
			yield return new object[] { " cent Douze", 112 };
			yield return new object[] { " cent Sept", 107 };
			yield return new object[] { " cent Quatre", 104 };
			yield return new object[] { "cent", 100 };
			yield return new object[] { "Cinq cent", 500 };
			yield return new object[] { "Six cent", 600 };
			yield return new object[] { "Neuf cent", 900 };
			yield return new object[] { "Huit cent", 800 };
		}
    }
}
