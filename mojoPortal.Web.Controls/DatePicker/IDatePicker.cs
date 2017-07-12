// Author:		        
// Created:            2007-11-07
// Last Modified:      2011-08-14
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.DatePicker
{
    
    public interface IDatePicker
    {
        Control GetControl();
        string ControlID { get;set;}
        string Text { get;set;}
        bool ShowTime { get;set;}
        string ClockHours { get;set;}
        Unit Width { get; set; }
        string ButtonImageUrl { get; set; }

        // *** added by ghalib ghniem Aug-14-2011 ChangeMonth: bool ,ChangeYear: bool, YearRange: string
        bool ShowMonthList { get; set; }
        bool ShowYearList { get; set; }
        string YearRange { get; set; }

        string CalculateWeek { get; set; }
        bool ShowWeek { get; set; }
        int FirstDay { get; set; }
        

    }
}
