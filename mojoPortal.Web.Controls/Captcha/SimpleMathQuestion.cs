using System;

namespace mojoPortal.Web.Controls.Captcha;


[Serializable]
public class SimpleMathQuestion
{
	#region Constructors

	public SimpleMathQuestion()
	{
		var randomGenerator = new Random();

		firstNumber = randomGenerator.Next(1, 10);
		secondNumber = randomGenerator.Next(1, 10);
	}

	#endregion


	#region Private Properties

	private int firstNumber;
	private int secondNumber;
	private string operatorDisplay = " + ";

	#endregion


	#region Public Properties

	public int FirstNumber => firstNumber;

	public int SecondNumber => secondNumber;

	public string QuestionExpression => $"{firstNumber}{operatorDisplay}{secondNumber} =";

	#endregion


	#region Private Methods

	public int GetCorrectAnswer() => firstNumber + secondNumber;

	#endregion


	#region Public Methods

	public bool IsCorrectAnswer(string answerInput)
	{
		var result = false;

		if (int.TryParse(answerInput, out int answerToTest))
		{
			result = answerToTest == GetCorrectAnswer();
		}

		return result;
	}

	#endregion
}
