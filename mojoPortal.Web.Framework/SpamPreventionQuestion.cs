using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// Author:             
    /// Created:            3/10/2006
    /// Last Modified:      3/10/2007
    /// 
    /// </summary>

    [Serializable]
    public class SpamPreventionQuestion
    {
        #region Constructors

        public SpamPreventionQuestion()
        {
            Random randomGenerator = new Random();


            firstNumber = randomGenerator.Next(1, 10);
            secondNumber = randomGenerator.Next(1, 10);

        }


        #endregion

        #region Private Properties

        private int firstNumber;
        private int secondNumber;
        //private bool useSubtraction = false;
        private string operatorDisplay = " + ";

        #endregion

        #region Public Properties

        public int FirstNumber
        {
            get { return firstNumber; }
        }

        public int SecondNumber
        {
            get { return secondNumber; }
        }

        public string QuestionExpression
        {
            get
            {
                string result = firstNumber.ToString()
                    + operatorDisplay + secondNumber.ToString()
                    + " = ";

                return result;

            }
        }

        #endregion


        #region Private Methods

        public int GetCorrectAnswer()
        {
            return firstNumber + secondNumber;

        }

        #endregion

        #region Public Methods

        public bool IsCorrectAnswer(string answerInput)
        {
            bool result = false;
            int answerToTest;
            if (int.TryParse(answerInput, out answerToTest))
            {
                result = (answerToTest == GetCorrectAnswer());

            }

            return result;

        }


        #endregion


    }
}
