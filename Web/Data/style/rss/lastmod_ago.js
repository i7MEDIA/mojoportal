// -*-coding: latin-1;-*-
// Time-stamp: "2006-01-09 19:54:44 AST" sburke@cpan.org
/*
   This is a dodad to say how long ago the lastmod time is.  I think
   that that's useful, given that the lastmod time is in GMT, which is
   unfriendly for most people.
	 sburke@cpan.org, Sean M. Burke.
   I hereby release this JavaScript code into the public domain.
*/

var month2num = {
	Jan:0, Feb:1, Mar:2, Apr:3, May:4, Jun:5,
	Jul:6, Aug:7, Sep:8, Oct:9, Nov:10, Dec:11
};

function RFC822_to_date (s) {
   var m = s.match(
    /^\w+, (\d\d) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d\d\d\d) (\d\d):(\d\d):(\d\d) GMT$/
   );   // like: "Mon, 23 May 2005 07:04:59 GMT"
        //             1   2   3   4  5  6
   if(!m) return undefined;

   var moment = new Date();
   moment.setUTCFullYear( parseInt( m[3],10));
   moment.setUTCMonth(    month2num[m[2]] );
   moment.setUTCDate(     parseInt( m[1],10));
   moment.setUTCHours(    parseInt( m[4],10));
   moment.setUTCMinutes(  parseInt( m[5],10));
   moment.setUTCSeconds(  parseInt( m[6],10));
   moment.setUTCMilliseconds(0);
   if( isNaN( moment.getTime() ) ) return undefined;
   return moment;
}

function seconds_ago (dateobj) {
  return Math.round(
	( new Date().getTime() - dateobj.getTime() ) / 1000
  );
}

function explicate_lastBuildDate () {
  if( ! document.getElementById ) return;  // sanity-check

  var el = document.getElementById('lastBuildDate');  if(!el     ) return;
  var text    = el.firstChild;                        if(!text   ) return;
  var lastmod = text.data;                            if(!lastmod) return;
  var mtime   = RFC822_to_date(lastmod);              if(!mtime  ) return;
  var s       = seconds_ago(mtime);                   if(isNaN(s)) return;

  el.setAttribute( 'title', mtime.toLocaleString() + "  (" + lastmod + ")" );
  s =
    (s<  -120)? undefined  // More than two minutes in the apparent future!?
   :(s<     1)? "just now"
   :(s<    90)?("about " +            s       .toString() + " seconds ago")
   :(s<  5400)?("about " + Math.round(s/   60).toString() + " minutes ago")
   :(s<129600)?("about " + Math.round(s/ 3600).toString() + " hours ago"  )
   :           (           Math.round(s/86400).toString() + " days ago"   )
  ;
  if(s) text.data = s;
  return;
}

explicate_lastBuildDate();

// End.
