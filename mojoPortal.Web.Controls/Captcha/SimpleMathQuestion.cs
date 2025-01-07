using System;

namespace mojoPortal.Web.Controls.Captcha;


[Serializable]
public class SimpleMathQuestion
{
	#region Private Properties

	private readonly int firstNumber;
	private readonly int secondNumber;
	private readonly string operatorDisplay = " + ";

	#endregion


	#region Public Properties

	public int FirstNumber => firstNumber;

	public int SecondNumber => secondNumber;

	public string QuestionExpression => $"{firstNumber}{operatorDisplay}{secondNumber} =";

	#endregion


	#region Constructors

	public SimpleMathQuestion()
	{
		var randomGenerator = new Random();

		firstNumber = randomGenerator.Next(1, 10);
		secondNumber = randomGenerator.Next(1, 10);
	}

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


	#region Private Methods

	private int GetCorrectAnswer() => firstNumber + secondNumber;

	#endregion
}
