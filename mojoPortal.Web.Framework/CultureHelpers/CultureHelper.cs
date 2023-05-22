using System;
using System.Globalization;
using System.Reflection;
using System.Text;


namespace mojoPortal.Web.Framework
{
	/// <summary>
	/// A helper class for various cultures that are not as well supported in ASP.nET
	/// </summary>
	public static class CultureHelper
    {
        public static CultureInfo GetPersianCulture()
        {
            //*****************************************************************************
            //*****************************2008-04-02 By A. Samarian*****Begin*************
            //Modified:2010-09-18

            //Modified by: Asad Samarian
            //*****************************************************************************
            CultureInfo persianCulture = new CultureInfo("fa-IR");
            DateTimeFormatInfo info = persianCulture.DateTimeFormat;

            info.DayNames = new string[] { "یکشنبه", "دوشنبه", "ﺳﻪشنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
            info.AbbreviatedDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };

            info.MonthNames = new string[] { "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            info.AbbreviatedMonthNames = new string[] { "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            //It Seems .NET 4 use Genitive manes for months.
            info.MonthGenitiveNames = new string[] { "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            info.AbbreviatedMonthGenitiveNames = new string[] { "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };

            info.AMDesignator = "ق.ظ";
            info.PMDesignator = "ب.ظ";
            info.ShortDatePattern = "yyyy/MM/dd";
            info.LongDatePattern = "dddd dd MMMM yyyy";
            info.FullDateTimePattern = "dddd dd MMMM yyyy, HH:mm:ss";
            info.FirstDayOfWeek = DayOfWeek.Saturday;
            PersianCalendar cal = new PersianCalendar();

            // 2010-07-18 this part throws an error under .NET 4, Asad says it is still needed for 3.5
            if (Core.Configuration.ConfigHelper.GetBoolProperty("UseNet35PersianHelper", false)) //this setting is true in the Web.config for 3.5 .NET but not 4.0
            {
                typeof(DateTimeFormatInfo).GetField("calendar", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(info, cal);
                object obj = typeof(DateTimeFormatInfo).GetField("m_cultureTableRecord",

                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(info);

                obj.GetType().GetMethod("UseCurrentCalendar",
                    BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, new object[] { cal.GetType().GetProperty("ID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cal, null) });

                //typeof(DateTimeFormatInfo).GetField("calendar", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(info, cal);
                //typeof(CultureInfo).GetField("calendar", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(persianCulture, cal);
            }

            typeof(DateTimeFormatInfo).GetField("calendar", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(info, cal);
            typeof(CultureInfo).GetField("calendar", (BindingFlags.NonPublic | (BindingFlags.Public | BindingFlags.Instance))).SetValue(persianCulture, cal);

            persianCulture.DateTimeFormat = info;
            persianCulture.NumberFormat.CurrencyDecimalDigits = 0;
            return persianCulture;

            //*****************************************************************************
            //*************************2008-04-02 By A. Samarian*****End*******************
            //Modified:2010-09-18
            //Modified by: Asad Samarian
            //*****************************************************************************

        }


        #region Digit Substitution

        // .ToString() does not localize digits, typically the digits used are always the ones configured in the
        // Regional Settings of a computer
        // this is unfortunate for web apps where we want to localize digits but cannot depend on the user
        // always having the correct regional settings for the site language

        /// <summary>
        /// Example extension method for int, allows us to do digit substitution where needed.
        /// Current example is only for Arabic but the same technique can be used for other languages
        /// if we can add more conversion methods
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ToLocalString(this int i)
        {
            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "ar":
                    return SubstituteArabicDigits(i.ToString());

                default:
                    return i.ToString();

            }

        }

        /// <summary>
        /// based on http://weblogs.asp.net/abdullaabdelhaq/archive/2009/06/27/displaying-arabic-number.aspx
        /// seems like a fairly expensive method to call so not sure if its suitable to use this everywhere
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SubstituteArabicDigits(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            Encoding utf8 = new UTF8Encoding();
            Decoder utf8Decoder = utf8.GetDecoder();
            StringBuilder result = new StringBuilder();

            Char[] translatedChars = new Char[1];
            Char[] inputChars = input.ToCharArray();
            Byte[] bytes = { 217, 160 };

            foreach (Char c in inputChars)
            {
                if (Char.IsDigit(c))
                {
                    // is this the key to it all? does adding 160 change to the unicode char for Arabic?
                    //So we can do the same with other languages using a different offset?
                    bytes[1] = Convert.ToByte(160 + Convert.ToInt32(char.GetNumericValue(c)));
                    utf8Decoder.GetChars(bytes, 0, 2, translatedChars, 0);
                    result.Append(translatedChars[0]);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        #endregion


    }
}
