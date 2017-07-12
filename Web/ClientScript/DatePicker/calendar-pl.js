// ** I18N

// Modified by  8/21/2005 replaced special chars with numeric entity references
// this fixed errors I was getting during testing of localization


// Calendar PL language
// Author: Artur Filipiak, <imagen@poczta.fm>
// January, 2004
// Encoding: UTF-8
Calendar._DN = new Array
("Niedziela", "Poniedzia&#322;ek", "Wtorek", "&#348;roda", "Czwartek", "Pi&#261;tek", "Sobota", "Niedziela");

Calendar._SDN = new Array
("N", "Pn", "Wt", "&#348;r", "Cz", "Pt", "So", "N");

// First day of the week. "0" means display Sunday first, "1" means display
// Monday first, etc.
Calendar._FD = 0;

Calendar._MN = new Array
("Stycze&#324;", "Luty", "Marzec", "Kwiecie&#324;", "Maj", "Czerwiec", "Lipiec", "Sierpie&#324;", "Wrzesie&#324;", "Pa&#378;dziernik", "Listopad", "Grudzie&#324;");

Calendar._SMN = new Array
("Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Pa&#378;", "Lis", "Gru");

// tooltips
Calendar._TT = {};
Calendar._TT["INFO"] = "O kalendarzu";

Calendar._TT["ABOUT"] =
"DHTML Date/Time Selector\n" +
"(c) dynarch.com 2002-2005 / Author: Mihai Bazon\n" + // don't translate this this ;-)
"For latest version visit: http://www.dynarch.com/projects/calendar/\n" +
"Distributed under GNU LGPL.  See http://gnu.org/licenses/lgpl.html for details." +
"\n\n" +
"Wyb&#243;r daty:\n" +
"- aby wybra&#263; rok u&#380;yj przycisk&#243;w \xab, \xbb\n" +
"- aby wybra&#263; miesi&#261;c u&#380;yj przycisk&#243;w " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) + "\n" +
"- aby przyspieszy&#263; wyb&#243;r przytrzymaj wci&#347;ni&#281;ty przycisk myszy nad ww. przyciskami.";
Calendar._TT["ABOUT_TIME"] = "\n\n" +
"Wyb&#243;r czasu:\n" +
"- aby zwi&#281;kszy&#263; warto&#347;&#263; kliknij na dowolnym elemencie selekcji czasu\n" +
"- aby zmniejszy&#263; warto&#347;&#263; u&#380;yj dodatkowo klawisza Shift\n" +
"- mo&#380;esz r&#243;wnie&#380; porusza&#263; myszk&#281; w lewo i prawo wraz z wci&#347;ni&#281;tym lewym klawiszem.";

Calendar._TT["PREV_YEAR"] = "Poprz. rok (przytrzymaj dla menu)";
Calendar._TT["PREV_MONTH"] = "Poprz. miesi&#261;c (przytrzymaj dla menu)";
Calendar._TT["GO_TODAY"] = "Poka&#380; dzi&#347;";
Calendar._TT["NEXT_MONTH"] = "Nast. miesi&#261;c (przytrzymaj dla menu)";
Calendar._TT["NEXT_YEAR"] = "Nast. rok (przytrzymaj dla menu)";
Calendar._TT["SEL_DATE"] = "Wybierz dat&#281;";
Calendar._TT["DRAG_TO_MOVE"] = "Przesu&#324; okienko";
Calendar._TT["PART_TODAY"] = " (dzi&#347;)";
//Calendar._TT["MON_FIRST"] = "Poka&#380; Poniedzia&#322;ek jako pierwszy";
//Calendar._TT["SUN_FIRST"] = "Poka&#380; Niedziel&#281; jako pierwsz&#261;";
Calendar._TT["CLOSE"] = "Zamknij";
Calendar._TT["TODAY"] = "Dzi&#347;";
Calendar._TT["TIME_PART"] = "(Shift-)klik | drag, aby zmieni&#263; warto&#347;&#263;";

// the following is to inform that "%s" is to be the first day of week
// %s will be replaced with the day name.
Calendar._TT["DAY_FIRST"] = "Display %s first";

// This may be locale-dependent.  It specifies the week-end days, as an array
// of comma-separated numbers.  The numbers are from 0 to 6: 0 means Sunday, 1
// means Monday, etc.
Calendar._TT["WEEKEND"] = "0,6";

// date formats
Calendar._TT["DEF_DATE_FORMAT"] = "%Y.%m.%d";
Calendar._TT["TT_DATE_FORMAT"] = "%a, %b %e";

Calendar._TT["WK"] = "wk";