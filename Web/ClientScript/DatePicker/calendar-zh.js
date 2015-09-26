
// ** I18N

// Calendar ZH language
// Author: muziq, <muziq@sina.com>
// Updated by: Aimin, <eman_lee@hotmail.com>
// Updated on: 05/11/2008
// Encoding: GB2312 or GBK
// Distributed under the same terms as the calendar itself.


// full day names
Calendar._DN = new Array
("&#26143;&#26399;&#26085;",
 "&#26143;&#26399;&#19968;",
 "&#26143;&#26399;&#20108;",
 "&#26143;&#26399;&#19977;",
 "&#26143;&#26399;&#22235;",
 "&#26143;&#26399;&#20116;",
 "&#26143;&#26399;&#20845;",
 "&#26143;&#26399;&#26085;");



// Please note that the following array of short day names (and the same goes
// for short month names, _SMN) isn't absolutely necessary.  We give it here
// for exemplification on how one can customize the short day names, but if
// they are simply the first N letters of the full name you can simply say:
//
//   Calendar._SDN_len = N; // short day name length
//   Calendar._SMN_len = N; // short month name length
//
// If N = 3 then this is not needed either since we assume a value of 3 if not
// present, to be compatible with translation files that were written before
// this feature.

// short day names
Calendar._SDN = new Array
("&#26085;",
 "&#19968;",
 "&#20108;",
 "&#19977;",
 "&#22235;",
 "&#20116;",
 "&#20845;",
 "&#26085;");

// First day of the week. "0" means display Sunday first, "1" means display
// Monday first, etc.
Calendar._FD = 0;

// full month names
Calendar._MN = new Array
("&#19968;&#26376;",
 "&#20108;&#26376;",
 "&#19977;&#26376;",
 "&#22235;&#26376;",
 "&#20116;&#26376;",
 "&#20845;&#26376;",
 "&#19971;&#26376;",
 "&#20843;&#26376;",
 "&#20061;&#26376;",
 "&#21313;&#26376;",
 "&#21313;&#19968;&#26376;",
 "&#21313;&#20108;&#26376;");

// short month names
Calendar._SMN = new Array
("&#19968;&#26376;",
 "&#20108;&#26376;",
 "&#19977;&#26376;",
 "&#22235;&#26376;",
 "&#20116;&#26376;",
 "&#20845;&#26376;",
 "&#19971;&#26376;",
 "&#20843;&#26376;",
 "&#20061;&#26376;",
 "&#21313;&#26376;",
 "&#21313;&#19968;&#26376;",
 "&#21313;&#20108;&#26376;");

// tooltips
Calendar._TT = {};
Calendar._TT["INFO"] = "&#24110;&#21161;";

Calendar._TT["ABOUT"] =
"DHTML Date/Time Selector\n" +
"(c) dynarch.com 2002-2005 / Author: Mihai Bazon\n" + // don't translate this this ;-)
"For latest version visit: http://www.dynarch.com/projects/calendar/\n" +
"Distributed under GNU LGPL.  See http://gnu.org/licenses/lgpl.html for details." +
"\n\n" +
"&#36873;&#25321;&#26085;&#26399;:\n" +
"- &#28857;&#20987; \xab, \xbb &#25353;&#38062;&#36873;&#25321;&#24180;&#20221;\n" +
"- &#28857;&#20987; " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) + " &#25353;&#38062;&#36873;&#25321;&#26376;&#20221;\n" +
"- &#38271;&#25353;&#20197;&#19978;&#25353;&#38062;&#21487;&#20174;&#33756;&#21333;&#20013;&#24555;&#36895;&#36873;&#25321;&#24180;&#20221;&#25110;&#26376;&#20221;";
Calendar._TT["ABOUT_TIME"] = "\n\n" +
"&#36873;&#25321;&#26102;&#38388;:\n" +
"- &#28857;&#20987;&#23567;&#26102;&#25110;&#20998;&#38047;&#21487;&#20351;&#25913;&#25968;&#20540;&#21152;&#19968;\n" +
"- &#25353;&#20303;Shift&#38190;&#28857;&#20987;&#23567;&#26102;&#25110;&#20998;&#38047;&#21487;&#20351;&#25913;&#25968;&#20540;&#20943;&#19968;\n" +
"- &#28857;&#20987;&#25302;&#21160;&#40736;&#26631;&#21487;&#36827;&#34892;&#24555;&#36895;&#36873;&#25321;";

Calendar._TT["PREV_YEAR"] = "&#19978;&#19968;&#24180; (&#25353;&#20303;&#20986;&#33756;&#21333;)";
Calendar._TT["PREV_MONTH"] = "&#19978;&#19968;&#26376; (&#25353;&#20303;&#20986;&#33756;&#21333;)";
Calendar._TT["GO_TODAY"] = "&#36716;&#21040;&#20170;&#26085;";
Calendar._TT["NEXT_MONTH"] = "&#19979;&#19968;&#26376; (&#25353;&#20303;&#20986;&#33756;&#21333;)";
Calendar._TT["NEXT_YEAR"] = "&#19979;&#19968;&#24180; (&#25353;&#20303;&#20986;&#33756;&#21333;)";
Calendar._TT["SEL_DATE"] = "&#36873;&#25321;&#26085;&#26399;";
Calendar._TT["DRAG_TO_MOVE"] = "&#25302;&#21160;";
Calendar._TT["PART_TODAY"] = " (&#20170;&#26085;)";

// the following is to inform that "%s" is to be the first day of week
// %s will be replaced with the day name.
Calendar._TT["DAY_FIRST"] = "&#26368;&#24038;&#36793;&#26174;&#31034;%s";

// This may be locale-dependent.  It specifies the week-end days, as an array
// of comma-separated numbers.  The numbers are from 0 to 6: 0 means Sunday, 1
// means Monday, etc.
Calendar._TT["WEEKEND"] = "0,6";

Calendar._TT["CLOSE"] = "&#20851;&#38381;";
Calendar._TT["TODAY"] = "&#20170;&#26085;";
Calendar._TT["TIME_PART"] = "(Shift-)&#28857;&#20987;&#40736;&#26631;&#25110;&#25302;&#21160;&#25913;&#21464;&#20540;";

// date formats
Calendar._TT["DEF_DATE_FORMAT"] = "%Y-%m-%d";
Calendar._TT["TT_DATE_FORMAT"] = "%A, %b %e&#26085;";

Calendar._TT["WK"] = "&#21608;";
Calendar._TT["TIME"] = "&#26102;&#38388;:";
