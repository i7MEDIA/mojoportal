using System;
using System.Globalization;

namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// A helper class for Persian language
    /// </summary>
    public class PersianDateHelper
    {
        

        public PersianDateHelper()
        {
        }

        #region  this.DateManager.AddDays
        public string ChangeDate(string PersionDate, int Days)
        {
            DateTime aDate = ShtoM(PersionDate);
            if (Days >= 0)
            {

                aDate = aDate.AddDays(Days);
            }
            else
            {
                System.TimeSpan days = new TimeSpan(Days, 0, 0, 0, 0);
                aDate = aDate.Subtract(-days);
            }
            PersionDate = this.MtoSh(aDate);
            return PersionDate;
        }
        #endregion
        public string ToDay()
        {
            return MtoSh(DateTime.Today);
        }
        public DateTime StringShToDateM(string Ddate)
        {
            DateTime dt = new DateTime();
            try
            {
                //string M=ShtoM(Ddate);
                //dt=new DateTime(int.Parse(M.Substring(4)),int.Parse(M.Substring(5,2)),int.Parse(M.Substring(8,2)));
                return dt;
            }
            catch
            {
                return dt;
            }


        }
        public DateTime ShtoM(string strd)
        {
            System.DateTime dt = new DateTime();
            try
            {

                if (strd.Length == 6)
                    strd = "13" + strd;
                if (strd.Length == 8)
                    strd = strd.Trim().Substring(0, 4) + "/" + strd.Trim().Substring(4, 2) + "/" + strd.Trim().Substring(6, 2);
                string tt1, tt2, tt3;
                int bt, at, ct, ST = 0;

                if (strd == "    /  /  " || strd == "13  /  /  ")
                    return dt;
                tt1 = strd.Trim().Substring(0, 4);
                tt2 = strd.Trim().Substring(5, 2);
                tt3 = strd.Trim().Substring(8, 2);
                int tmptt;
                tmptt = (int.Parse(tt1) + 621);
                tt1 = tmptt.ToString();
                if (int.Parse(tt1) > 1995 && int.Parse(tt1) % 4 == 0)
                    dt = new DateTime(int.Parse(tt1), 3, 20);
                else
                    dt = new DateTime(int.Parse(tt1), 3, 21);
                at = int.Parse(tt1);
                bt = int.Parse(tt2);
                ct = int.Parse(tt3);
                switch (bt)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        ST = ((bt - 1) * 31) + ct;
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        ST = (6 * 31) + ((bt - 7) * 30) + ct;
                        break;
                }

                dt = dt.AddDays(ST - 1);

                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public string MtoSh(System.DateTime ddat)
        {
            try
            {
                int da, mo, ye, ld;
                string tt1, tt2, tt3;

                int[] buf1, buf2;
                buf1 = new int[12];
                buf2 = new int[12];
                buf1[0] = 0;
                buf1[1] = 31;
                buf1[2] = 59;
                buf1[3] = 90;
                buf1[4] = 120;
                buf1[5] = 151;
                buf1[6] = 181;
                buf1[7] = 212;
                buf1[8] = 243;
                buf1[9] = 273;
                buf1[10] = 304;
                buf1[11] = 334;

                buf2[0] = 0;
                buf2[1] = 31;
                buf2[2] = 60;
                buf2[3] = 91;
                buf2[4] = 121;
                buf2[5] = 152;
                buf2[6] = 182;
                buf2[7] = 213;
                buf2[8] = 244;
                buf2[9] = 274;
                buf2[10] = 305;
                buf2[11] = 335;

                //				if (ddat==null)
                //					return ("        ");
                if ((ddat.Year % 4) != 0)
                {
                    da = buf1[ddat.Month - 1] + ddat.Day;
                    if (da > 79)
                    {
                        da = da - 79;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        if (ddat.Year > 1996 && (ddat.Year % 4) == 1)

                            ld = 11;
                        else
                            ld = 10;
                        da = da + ld;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                else
                {
                    da = buf2[ddat.Month - 1] + ddat.Day;
                    if (ddat.Year >= 1996)
                        ld = 79;
                    else
                        ld = 80;
                    if (da > ld)
                    {
                        da = da - ld;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        da = da + 10;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                tt1 = ye.ToString().Trim();
                tt2 = mo.ToString().Trim();
                if (tt2.Length == 1)
                    tt2 = "0" + tt2;
                tt3 = da.ToString().Trim();
                if (tt3.Length == 1)
                    tt3 = "0" + tt3;

                return (tt1 + "/" + tt2 + "/" + tt3);
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }

        public int GetPersianMonth(System.DateTime ddat)
        {
            try
            {
                int da, mo, ye, ld;
                
                int[] buf1, buf2;
                buf1 = new int[12];
                buf2 = new int[12];
                buf1[0] = 0;
                buf1[1] = 31;
                buf1[2] = 59;
                buf1[3] = 90;
                buf1[4] = 120;
                buf1[5] = 151;
                buf1[6] = 181;
                buf1[7] = 212;
                buf1[8] = 243;
                buf1[9] = 273;
                buf1[10] = 304;
                buf1[11] = 334;

                buf2[0] = 0;
                buf2[1] = 31;
                buf2[2] = 60;
                buf2[3] = 91;
                buf2[4] = 121;
                buf2[5] = 152;
                buf2[6] = 182;
                buf2[7] = 213;
                buf2[8] = 244;
                buf2[9] = 274;
                buf2[10] = 305;
                buf2[11] = 335;

                //				if (ddat==null)
                //					return ("        ");
                if ((ddat.Year % 4) != 0)
                {
                    da = buf1[ddat.Month - 1] + ddat.Day;
                    if (da > 79)
                    {
                        da = da - 79;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        if (ddat.Year > 1996 && (ddat.Year % 4) == 1)

                            ld = 11;
                        else
                            ld = 10;
                        da = da + ld;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                else
                {
                    da = buf2[ddat.Month - 1] + ddat.Day;
                    if (ddat.Year >= 1996)
                        ld = 79;
                    else
                        ld = 80;
                    if (da > ld)
                    {
                        da = da - ld;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        da = da + 10;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                //tt1 = ye.ToString().Trim();
                //tt2 = mo.ToString().Trim();
                //if (tt2.Length == 1)
                //    tt2 = "0" + tt2;
                //tt3 = da.ToString().Trim();
                //if (tt3.Length == 1)
                //    tt3 = "0" + tt3;

                //return (tt1 + "/" + tt2 + "/" + tt3);

                return mo;
            }
            catch (System.Exception)
            {
                return ddat.Month;
            }
        }

        public int GetPersianYear(System.DateTime ddat)
        {
            try
            {
                int da, mo, ye, ld;

                int[] buf1, buf2;
                buf1 = new int[12];
                buf2 = new int[12];
                buf1[0] = 0;
                buf1[1] = 31;
                buf1[2] = 59;
                buf1[3] = 90;
                buf1[4] = 120;
                buf1[5] = 151;
                buf1[6] = 181;
                buf1[7] = 212;
                buf1[8] = 243;
                buf1[9] = 273;
                buf1[10] = 304;
                buf1[11] = 334;

                buf2[0] = 0;
                buf2[1] = 31;
                buf2[2] = 60;
                buf2[3] = 91;
                buf2[4] = 121;
                buf2[5] = 152;
                buf2[6] = 182;
                buf2[7] = 213;
                buf2[8] = 244;
                buf2[9] = 274;
                buf2[10] = 305;
                buf2[11] = 335;

                //				if (ddat==null)
                //					return ("        ");
                if ((ddat.Year % 4) != 0)
                {
                    da = buf1[ddat.Month - 1] + ddat.Day;
                    if (da > 79)
                    {
                        da = da - 79;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        if (ddat.Year > 1996 && (ddat.Year % 4) == 1)

                            ld = 11;
                        else
                            ld = 10;
                        da = da + ld;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                else
                {
                    da = buf2[ddat.Month - 1] + ddat.Day;
                    if (ddat.Year >= 1996)
                        ld = 79;
                    else
                        ld = 80;
                    if (da > ld)
                    {
                        da = da - ld;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        da = da + 10;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                //tt1 = ye.ToString().Trim();
                //tt2 = mo.ToString().Trim();
                //if (tt2.Length == 1)
                //    tt2 = "0" + tt2;
                //tt3 = da.ToString().Trim();
                //if (tt3.Length == 1)
                //    tt3 = "0" + tt3;

                //return (tt1 + "/" + tt2 + "/" + tt3);

                return ye;
            }
            catch (System.Exception)
            {
                return ddat.Month;
            }
        }

        public string MtoSh(string strdat)
        {
            try
            {
                System.DateTime ddat = new DateTime(int.Parse(strdat.Substring(0, 4)), int.Parse(strdat.Substring(5, 2)), int.Parse(strdat.Substring(8, 2)));
                int da, mo, ye, ld;
                string tt1, tt2, tt3;

                int[] buf1, buf2;
                buf1 = new int[12];
                buf2 = new int[12];
                buf1[0] = 0;
                buf1[1] = 31;
                buf1[2] = 59;
                buf1[3] = 90;
                buf1[4] = 120;
                buf1[5] = 151;
                buf1[6] = 181;
                buf1[7] = 212;
                buf1[8] = 243;
                buf1[9] = 273;
                buf1[10] = 304;
                buf1[11] = 334;

                buf2[0] = 0;
                buf2[1] = 31;
                buf2[2] = 60;
                buf2[3] = 91;
                buf2[4] = 121;
                buf2[5] = 152;
                buf2[6] = 182;
                buf2[7] = 213;
                buf2[8] = 244;
                buf2[9] = 274;
                buf2[10] = 305;
                buf2[11] = 335;

                //				if (ddat.ToString()==null)
                //					return ("        ");
                if ((ddat.Year % 4) != 0)
                {
                    da = buf1[ddat.Month - 1] + ddat.Day;
                    if (da > 79)
                    {
                        da = da - 79;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        if (ddat.Year > 1996 && (ddat.Year % 4) == 1)

                            ld = 11;
                        else
                            ld = 10;
                        da = da + ld;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                else
                {
                    da = buf2[ddat.Month - 1] + ddat.Day;
                    if (ddat.Year >= 1996)
                        ld = 79;
                    else
                        ld = 80;
                    if (da > ld)
                    {
                        da = da - ld;
                        if (da <= 186)
                        {
                            switch (da % 31)
                            {
                                case 0:
                                    mo = da / 31;
                                    da = 31;
                                    break;
                                default:
                                    mo = (int)(da / 31) + 1;
                                    da = da % 31;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                        else
                        {
                            da = da - 186;
                            switch (da % 30)
                            {
                                case 0:
                                    mo = (da / 30) + 6;
                                    da = 30;
                                    break;
                                default:
                                    mo = (int)(da / 30) + 7;
                                    da = da % 30;
                                    break;
                            }
                            ye = ddat.Year - 621;
                        }
                    }
                    else
                    {
                        da = da + 10;
                        switch (da % 30)
                        {
                            case 0:
                                mo = (da / 30) + 9;
                                da = 30;
                                break;
                            default:
                                mo = (int)(da / 30) + 10;
                                da = da % 30;
                                break;
                        }
                        ye = ddat.Year - 622;
                    }
                }
                tt1 = ye.ToString().Trim();
                tt2 = mo.ToString().Trim();
                if (tt2.Length == 1)
                    tt2 = "0" + tt2;
                tt3 = da.ToString().Trim();
                if (tt3.Length == 1)
                    tt3 = "0" + tt3;

                return (tt1 + "/" + tt2 + "/" + tt3);
            }
            catch
            {
                return "";
            }
        }
        public int GetWeek(string strdat)
        {
            int W = 1;
            int dys = 0;
            int Week1 = 0;
            string[] str = strdat.Split('/');
            DateTime FirstDayOfYear = this.ShtoM(str[0].ToString() + "/01/01");
            switch (FirstDayOfYear.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    Week1 = 7;
                    break;
                case DayOfWeek.Sunday:
                    Week1 = 6;
                    break;
                case DayOfWeek.Monday:
                    Week1 = 5;
                    break;
                case DayOfWeek.Tuesday:
                    Week1 = 4;
                    break;
                case DayOfWeek.Wednesday:
                    Week1 = 3;
                    break;
                case DayOfWeek.Thursday:
                    Week1 = 2;
                    break;
                case DayOfWeek.Friday:
                    Week1 = 1;
                    break;
            }
           

            int m = int.Parse(str[1].ToString());
            int d = int.Parse(str[2].ToString());
            if (m < 7)
            {
                m--;
                dys = m * 31 + d;
            }
            else
            {

                if (m == 12)
                {
                    dys = 5 * 30 + 6 * 31 + d;
                }
                else
                {
                    m--;
                    dys = m * 30 + d;
                }
            }
            int Rem = 0;
            Math.DivRem((dys - Week1), 7, out Rem);
            if (Rem > 0)
            {
                W = ((dys - Week1) / 7) + 2;
            }
            else
            {
                W = ((dys - Week1) / 7) + 1;
            }
            return W;
        }
        public int GetYear(string strdat)
        {
            string[] str = new string[3];
            str = strdat.Split('/');
            return int.Parse(str[0].ToString());
        }
        public string GetDate(int Year, int Week, int DayInWeek)
        {
            int Day = 1;
            int DayInMoth = 1;
            int month = 1;
            int Week1 = 0;
            string startDate ="";
            DateTime FirstDayOfYear = this.ShtoM(Year.ToString()+"/01/01");
            switch (FirstDayOfYear.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    Week1 =7;
                    break;
                case DayOfWeek.Sunday:
                    Week1 = 6;
                    break;
                case DayOfWeek.Monday:
                    Week1 = 5;
                    break;
                case DayOfWeek.Tuesday:
                    Week1 = 4;
                    break;
                case DayOfWeek.Wednesday:
                    Week1 = 3;
                    break;
                case DayOfWeek.Thursday:
                    Week1 = 2;
                    break;
                case DayOfWeek.Friday:
                    Week1 = 1;
                    break;
            }
            Day = (Week-2) * 7 + DayInWeek+Week1;
            if (Day <= 186)
            {
                month = (Day / 31) + 1;
                DayInMoth = Day % 31;
                if (DayInMoth == 0)
                {
                    month--;
                    DayInMoth = 31;
                }
            }
            else
            {
                Day -= 186;
                month = (Day / 30) + 7;
                DayInMoth = Day % 30;
                if (DayInMoth == 0)
                {
                    month--;
                    DayInMoth = 30;
                }
            }
            if (month > 9 && DayInMoth > 9)
                startDate = Year.ToString() + "/" + month.ToString() + "/" + DayInMoth.ToString();
            if (month < 10 && DayInMoth > 9)
                startDate = Year.ToString() + "/0" + month.ToString() + "/" + DayInMoth.ToString();
            if (month > 9 && DayInMoth < 10)
                startDate = Year.ToString() + "/" + month.ToString() + "/0" + DayInMoth.ToString();
            if (month < 10 && DayInMoth < 10)
                startDate = Year.ToString() + "/0" + month.ToString() + "/0" + DayInMoth.ToString();
            return startDate;
        }
    }
}

