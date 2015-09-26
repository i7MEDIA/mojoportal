//////////////////////////////////////////////////////////////////////////////////////////////
//	Turkish Translation by Nuri AKMAN
//	Location: Ankara/TURKEY
//	e-mail	: nuriakman@hotmail.com
//	Date	: April, 9 2003
//
//	Note: if Turkish Characters does not shown on you screen
//		  please include falowing line your html code:
//
//		  <meta http-equiv="Content-Type" content="text/html; charset=windows-1254">
//
//////////////////////////////////////////////////////////////////////////////////////////////

Calendar._DN = new Array
("Pazar",
 "Pazartesi",
 "Sal&#253;",
 "&#199;ar&#222;amba",
 "Per&#222;embe",
 "Cuma",
 "Cumartesi",
 "Pazar");
 
Calendar._SDN = new Array
("P",
 "P",
 "S",
 "&#199;",
 "P",
 "C",
 "C",
 "P");

 
Calendar._MN = new Array
("Ocak",
 "&#222;ubat",
 "Mart",
 "Nisan",
 "May&#253;s",
 "Haziran",
 "Temmuz",
 "A&#240;ustos",
 "Eyl&#252;l",
 "Ekim",
 "Kas&#253;m",
 "Aral&#253;k");
 
 Calendar._SMN = new Array
("Ocak",
 "&#222;ubat",
 "Mart",
 "Nisan",
 "May&#253;s",
 "Haziran",
 "Temmuz",
 "A&#240;ustos",
 "Eyl&#252;l",
 "Ekim",
 "Kas&#253;m",
 "Aral&#253;k");

// tooltips
Calendar._TT = {};
Calendar._TT["INFO"] = "About the calendar";
Calendar._TT["ABOUT"] =
"DHTML Date/Time Selector\n" +
"(c) dynarch.com 2002-2005 / Author: Mihai Bazon\n" + 
"For latest version visit: http://www.dynarch.com/projects/calendar/\n" +
"Distributed under GNU LGPL.  See http://gnu.org/licenses/lgpl.html for details." +
"\n\n" +
"Date selection:\n" +
"- Use the \xab, \xbb buttons to select year\n" +
"- Use the " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) + " buttons to select month\n" +
"- Hold mouse button on any of the above buttons for faster selection.";

Calendar._TT["ABOUT_TIME"] = "\n\n" +
"Time selection:\n" +
"- Click on any of the time parts to increase it\n" +
"- or Shift-click to decrease it\n" +
"- or click and drag for faster selection.";


Calendar._TT["PREV_YEAR"] = "&#214;nceki Y&#253;l (Men&#252; i&#231;in bas&#253;l&#253; tutunuz)";
Calendar._TT["PREV_MONTH"] = "&#214;nceki Ay (Men&#252; i&#231;in bas&#253;l&#253; tutunuz)";
Calendar._TT["GO_TODAY"] = "Bug&#252;n  e git";
Calendar._TT["NEXT_MONTH"] = "Sonraki Ay (Men&#252; i&#231;in bas&#253;l&#253; tutunuz)";
Calendar._TT["NEXT_YEAR"] = "Sonraki Y&#253;l (Men&#252; i&#231;in bas&#253;l&#253; tutunuz)";
Calendar._TT["SEL_DATE"] = "Tarih se&#231;iniz";
Calendar._TT["DRAG_TO_MOVE"] = "Ta&#222;&#253;mak i&#231;in s&#252;r&#252;kleyiniz";
Calendar._TT["PART_TODAY"] = " (bug&#252;n)";
Calendar._TT["DAY_FIRST"] = "Display %s first";

Calendar._TT["WEEKEND"] = "0,6";

Calendar._TT["CLOSE"] = "Kapat";
Calendar._TT["TODAY"] = "Bug&#252;n";
Calendar._TT["TIME_PART"] = "(Shift-)Click or drag to change value";

// date formats
//Calendar._TT["DEF_DATE_FORMAT"] = "%d-%m-%Y";
//Calendar._TT["DEF_DATE_FORMAT"] = "%Y-%m-%d";
Calendar._TT["DEF_DATE_FORMAT"] = "dd.mm.yy";
//Calendar._TT["TT_DATE_FORMAT"] = "%A, %e de %B de %Y";
Calendar._TT["TT_DATE_FORMAT"] = "%a, %b %e";

Calendar._TT["WK"] = "Hafta";
Calendar._TT["TIME"] = "Time:";
