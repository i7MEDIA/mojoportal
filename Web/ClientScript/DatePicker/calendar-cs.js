/* 
	calendar-cs-win.js
	language: Czech
	encoding: windows-1250
	author: Lubos Jerabek (xnet@seznam.cz)
	        Jan Uhlir (espinosa@centrum.cz)
*/

// ** I18N
Calendar._DN  = new Array
('Ned&#236;le',
'Pond&#236;l&#237;',
'&#218;ter&#253;',
'St&#248;eda',
'&#200;tvrtek',
'P&#225;tek',
'Sobota',
'Ned&#236;le');

Calendar._SDN = new Array
('Ne',
'Po',
'&#218;t',
'St',
'&#200;t',
'P&#225;',
'So',
'Ne');
Calendar._MN  = new Array
('Leden',
'&#218;nor',
'B&#248;ezen',
'Duben',
'Kv&#236;ten',
'&#200;erven',
'&#200;ervenec',
'Srpen',
'Z&#225;&#248;&#237;',
'&#216;&#237;jen',
'Listopad',
'Prosinec');

Calendar._SMN = new Array
('Led',
'&#218;no',
'B&#248;e',
'Dub',
'Kv&#236;',
'&#200;rv',
'&#200;vc',
'Srp',
'Z&#225;&#248;',
'&#216;&#237;j',
'Lis',
'Pro');

// tooltips
Calendar._TT = {};
Calendar._TT["INFO"] = "O komponent&#236; kalend&#225;&#248;";
Calendar._TT["TOGGLE"] = "Zm&#236;na prvn&#237;ho dne v t&#253;dnu";
Calendar._TT["PREV_YEAR"] = "P&#248;edchoz&#237; rok (p&#248;idrz pro menu)";
Calendar._TT["PREV_MONTH"] = "P&#248;edchoz&#237; m&#236;s&#237;c (p&#248;idrz pro menu)";
Calendar._TT["GO_TODAY"] = "Dnešn&#237; datum";
Calendar._TT["NEXT_MONTH"] = "Dals&#237; m&#236;s&#237;c (p&#248;idrz pro menu)";
Calendar._TT["NEXT_YEAR"] = "Dals&#237; rok (p&#248;idrž pro menu)";
Calendar._TT["SEL_DATE"] = "Vyber datum";
Calendar._TT["DRAG_TO_MOVE"] = "Chy a t&#225;hni, pro p&#248;esun";
Calendar._TT["PART_TODAY"] = " (dnes)";
Calendar._TT["MON_FIRST"] = "Ukaz jako prvn&#237; Pond&#236;l&#237;";
//Calendar._TT["SUN_FIRST"] = "Ukaž jako první Nedìli";

Calendar._TT["ABOUT"] =
"DHTML Date/Time Selector\n" +
"(c) dynarch.com 2002-2005 / Author: Mihai Bazon\n" + // don't translate this this ;-)
"For latest version visit: http://www.dynarch.com/projects/calendar/\n" +
"Distributed under GNU LGPL.  See http://gnu.org/licenses/lgpl.html for details." +
"\n\n" +
"V&#253;b&#236;r datumu:\n" +
"- Use the \xab, \xbb buttons to select year\n" +
"- Pouzijte tla&#232;&#237;tka " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) 
+ " k v&#253;b&#236;ru m&#236;s&#237;ce\n" +
"- Podrzte tla&#232;&#237;tko mysi na jak&#233;mkoliv z t&#236;ch tla&#232;&#237;tek pro rychlejs&#237; v&#253;b&#236;r.";

Calendar._TT["ABOUT_TIME"] = "\n\n" +
"V&#253;b&#236;r &#232;asu:\n" +
"- Klikn&#237;te na jakoukoliv z &#232;&#225;st&#237; v&#253;b&#236;ru &#232;asu pro zv&#253;sen&#237;.\n" +
"- nebo Shift-click pro sn&#237;zen&#237;\n" +
"- nebo klikn&#236;te a t&#225;hn&#236;te pro rychlejs&#237; v&#253;b&#236;r.";

// the following is to inform that "%s" is to be the first day of week
// %s will be replaced with the day name.
Calendar._TT["DAY_FIRST"] = "Zobraz %s prvn&#237;";

// This may be locale-dependent.  It specifies the week-end days, as an array
// of comma-separated numbers.  The numbers are from 0 to 6: 0 means Sunday, 1
// means Monday, etc.
Calendar._TT["WEEKEND"] = "0,6";

Calendar._TT["CLOSE"] = "Zav&#248;&#237;t";
Calendar._TT["TODAY"] = "Dnes";
Calendar._TT["TIME_PART"] = "(Shift-)Klikni nebo t&#225;hni pro zm&#236;nu hodnoty";

// date formats
Calendar._TT["DEF_DATE_FORMAT"] = "d.m.yy";
Calendar._TT["TT_DATE_FORMAT"] = "%a, %b %e";

Calendar._TT["WK"] = "wk";
Calendar._TT["TIME"] = "&#200;as:";

