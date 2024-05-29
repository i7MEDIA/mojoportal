using System;

namespace mojoPortal.Web.Framework;

[Serializable]
public class SpamPreventionQuestion
{
	#region Constructors

	public SpamPreventionQuestion()
	{
		var randomGenerator = new Random();

		FirstNumber = randomGenerator.Next(1, 10);
		SecondNumber = randomGenerator.Next(1, 10);
	}

	#endregion

	#region Public Properties

	public int FirstNumber
	{
		get; private set;
	}

	public int SecondNumber
	{
		get; private set;
	}

	public string QuestionExpression => $"{FirstNumber} + {SecondNumber} = ";

	#endregion


	private int GetCorrectAnswer() => FirstNumber + SecondNumber;
	

	public bool IsCorrectAnswer(string answerInput)
	{
		var result = false;
		if (int.TryParse(answerInput, out int answerToTest))
		{
			result = (answerToTest == GetCorrectAnswer());
		}

		return result;
	}	
}