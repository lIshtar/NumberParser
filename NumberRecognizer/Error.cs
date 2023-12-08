using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberRecognizer
{
	public class Error
	{
		public Error (ErrorCode errorCode, int position)
		{
			this.position = position;
			this.errorCode = errorCode;
		}

		public Error(ErrorCode errorCode, string errorWord)
		{
			this.errorWord = errorWord;
			this.errorCode = errorCode;
		}

		public Error(ErrorCode errorCode)
		{
			this.errorCode = errorCode;
		}

		public Error()
		{
			this.errorCode = ErrorCode.NoError;
		}

		int position = -1;
		public string errorWord = string.Empty;
		public string errorText = string.Empty;

		public ErrorCode errorCode;
	}

	public enum ErrorCode
	{
		NoError = 0,
		EndOfConsequence = 1,
		EtUnError = 2,
		EtOnzeError = 3,
		NoElements = 4,
		IncorrectWord = 5,
		UnexpectedSequence = 6,
		UnexpectedSequenceAfterEndOfNumber = 7,
		BeginWithEt = 8
	}
}
