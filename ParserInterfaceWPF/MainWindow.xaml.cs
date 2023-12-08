using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NumberRecognizer;

namespace ParserInterfaceWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void NumberConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			//var numberRecognizer = new NumberParser.NumberRecognizer();
			var numberRecognizer = new ModifiedNumberRecognizer();
			var error = new Error();
			string? input = NumberTextBlock.Text;
			input = input.ToLower();
			if (input == null || input == "")
			{
				NumberDisplayLabel.Content = "Input string can't be empty";
				NumberDisplayLabel.Foreground = Brushes.Red;
			}
			else
			{
				int recognizedNumber;
				recognizedNumber = numberRecognizer.TryGetNumberFromString(input, out error);
				if (recognizedNumber != 0)
				{
					NumberDisplayLabel.Content = $"{recognizedNumber}";
					NumberDisplayLabel.Foreground = Brushes.Black;
					//ShowErrorText("");
				}
				else
				{
					switch (error.errorCode)
					{
						case ErrorCode.IncorrectWord:
							{
								ShowErrorText($"Неправильное слово: {error.errorWord}");
								break;
							}
						case ErrorCode.EndOfConsequence:
							{
								ShowErrorText("Неожиданное окончание числа");
								break;
							}
						case ErrorCode.UnexpectedSequence:
							{
								ShowErrorText("Неправильная последовательность символов");
								break;
							}
						case ErrorCode.UnexpectedSequenceAfterEndOfNumber:
							{
								ShowErrorText(error.errorText);
								break;
							}
						case ErrorCode.EtUnError | ErrorCode.EtOnzeError:
							{
								ShowErrorText("Ошибка с сочетанием Et Un (Et Onze)");
								break;
							}
						default:
							{
								ShowErrorText(error.errorCode.ToString());
								break;
							}
					}
				}
			}
		}

		private void ShowErrorText(string errorText)
		{
			NumberDisplayLabel.Content = errorText;
			NumberDisplayLabel.Foreground = Brushes.Red;
		}
	}
}
