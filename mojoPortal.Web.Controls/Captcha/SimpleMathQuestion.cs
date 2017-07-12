// Author:		        
// Created:            2007-08-15
// Last Modified:      2007-08-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web.Controls.Captcha
{
    
    [Serializable]
    public class SimpleMathQuestion
    {
        #region Constructors

        public SimpleMathQuestion()
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
