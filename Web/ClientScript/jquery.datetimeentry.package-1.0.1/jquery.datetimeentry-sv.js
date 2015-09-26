/* http://keith-wood.name/datetimeEntry.html
   Swedish initialisation for the jQuery date/time entry extension.
   Written by Anders Ekdahl ( anders@nomadiz.se). */
(function($) {
	$.datetimeEntry.regional['sv'] = {datetimeFormat: 'Y-O-D H:M',
        monthNames: ['Januari','Februari','Mars','April','Maj','Juni',
        'Juli','Augusti','September','Oktober','November','December'],
        monthNamesShort: ['Jan','Feb','Mar','Apr','Maj','Jun',
        'Jul','Aug','Sep','Okt','Nov','Dec'],
		dayNames: ['Söndag','Måndag','Tisdag','Onsdag','Torsdag','Fredag','Lördag'],
		dayNamesShort: ['Sön','Mån','Tis','Ons','Tor','Fre','Lör'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Nu', 'Förra fältet', 'Nästa fält', 'öka', 'minska'],
		isRTL: false};
    $.datetimeEntry.setDefaults($.datetimeEntry.regional['sv']); 
})(jQuery);
