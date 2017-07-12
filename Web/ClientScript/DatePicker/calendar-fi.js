// ** I18N

// Modified by  8/21/2005 replaced special chars with numeric entity references
// this fixed errors I was getting during testing of localization

// Calendar FI language (Finnish, Suomi)
// Author: Jarno Käyhkö, <gambler@phnet.fi>
// Encoding: UTF-8
// Distributed under the same terms as the calendar itself.

// full day names
Calendar._DN = new Array
("Sunnuntai",
 "Maanantai",
 "Tiistai",
 "Keskiviikko",
 "Torstai",
 "Perjantai",
 "Lauantai",
 "Sunnuntai");

// short day names
Calendar._SDN = new Array
("Su",
 "Ma",
 "Ti",
 "Ke",
 "To",
 "Pe",
 "La",
 "Su");
 
 // First day of the week. "0" means display Sunday first, "1" means display
// Monday first, etc.
Calendar._FD = 0;

// full month names
Calendar._MN = new Array
("Tammikuu",
 "Helmikuu",
 "Maaliskuu",
 "Huhtikuu",
 "Toukokuu",
 "Kes&#228;kuu",
 "Hein&#228;kuu",
 "Elokuu",
 "Syyskuu",
 "Lokakuu",
 "Marraskuu",
 "Joulukuu");

// short month names
Calendar._SMN = new Array
("Tam",
 "Hel",
 "Maa",
 "Huh",
 "Tou",
 "Kes",
 "Hei",
 "Elo",
 "Syy",
 "Lok",
 "Mar",
 "Jou");

// tooltips
Calendar._TT = {};
Calendar._TT["INFO"] = "Tietoja kalenterista";

Calendar._TT["ABOUT"] =
"DHTML Date/Time Selector\n" +
"(c) dynarch.com 2002-2005 / Author: Mihai Bazon\n" + // don't translate this this ;-)
"Uusin versio osoitteessa: http://www.dynarch.com/projects/calendar/\n" +
"Julkaistu GNU LGPL lisenssin alaisuudessa. Lisätietoja osoitteessa http://gnu.org/licenses/lgpl.html" +
"\n\n" +
"P&#228;iv&#228;m&#228;&#228;r&#228; valinta:\n" +
"- K&#228;yt&#228; \xab, \xbb painikkeita valitaksesi vuosi\n" +
"- K&#228;yt&#228; " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) + " painikkeita valitaksesi kuukausi\n" +
"- Pit&#228;m&#228;ll&#228; hiiren painiketta mink&#228; tahansa yll&#228; olevan painikkeen kohdalla, saat n&#228;kyviin valikon nopeampaan siirtymiseen.";
Calendar._TT["ABOUT_TIME"] = "\n\n" +
"Ajan valinta:\n" +
"- Klikkaa kellonajan numeroita lisätäksesi aikaa\n" +
"- tai pit&#228;m&#228;ll&#228; Shift-n&#228;pp&#228;int&#228; pohjassa saat aikaa taaksep&#228;in\n" +
"- tai klikkaa ja pidä hiiren painike pohjassa sek&#228; liikuta hiirt&#228; muuttaaksesi aikaa nopeasti eteen- ja taaksep&#228;in.";

Calendar._TT["PREV_YEAR"] = "Edell. vuosi (paina hetki, n&#228;et valikon)";
Calendar._TT["PREV_MONTH"] = "Edell. kuukausi (paina hetki, n&#228;et valikon)";
Calendar._TT["GO_TODAY"] = "Siirry t&#228;h&#228;n p&#228;iv&#228;&#228;n";
Calendar._TT["NEXT_MONTH"] = "Seur. kuukausi (paina hetki, n&#228;et valikon)";
Calendar._TT["NEXT_YEAR"] = "Seur. vuosi (paina hetki, n&#228;et valikon)";
Calendar._TT["SEL_DATE"] = "Valitse p&#228;iv&#228;m&#228;&#228;r&#228;";
Calendar._TT["DRAG_TO_MOVE"] = "Siirr&#228; kalenterin paikkaa";
Calendar._TT["PART_TODAY"] = " (t&#228;n&#228;&#228;n)";
//Calendar._TT["MON_FIRST"] = "Näytä maanantai ensimmäisenä";
//Calendar._TT["SUN_FIRST"] = "Näytä sunnuntai ensimmäisenä";

// the following is to inform that "%s" is to be the first day of week
// %s will be replaced with the day name.
Calendar._TT["DAY_FIRST"] = "Display %s first";

// This may be locale-dependent.  It specifies the week-end days, as an array
// of comma-separated numbers.  The numbers are from 0 to 6: 0 means Sunday, 1
// means Monday, etc.
Calendar._TT["WEEKEND"] = "0,6";


Calendar._TT["CLOSE"] = "Sulje";
Calendar._TT["TODAY"] = "T&#228;n&#228;&#228;n";
Calendar._TT["TIME_PART"] = "(Shift-) Klikkaa tai liikuta muuttaaksesi aikaa";

// date formats
Calendar._TT["DEF_DATE_FORMAT"] = "%d.%m.%Y";
Calendar._TT["TT_DATE_FORMAT"] = "%d.%m.%Y";

Calendar._TT["WK"] = "Vko";
Calendar._TT["TIME"] = "Time:";
