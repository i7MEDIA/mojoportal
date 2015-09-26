/* http://keith-wood.name/datetimeEntry.html
   Lithuanian initialisation for the jQuery date/time entry extension
   Written by Andrej Andrejev and Arturas Paleicikas <arturas@avalon.lt>. */
(function($) {
	$.datetimeEntry.regional['lt'] = {datetimeFormat: 'Y-O-D H:M',
		monthNames: ['Sausis','Vasaris','Kovas','Balandis','Gegužė','Birželis',
		'Liepa','Rugpjūtis','Rugsėjis','Spalis','Lapkritis','Gruodis'],
		monthNamesShort: ['Sau','Vas','Kov','Bal','Geg','Bir',
		'Lie','Rugp','Rugs','Spa','Lap','Gru'],
		dayNames: ['sekmadienis','pirmadienis','antradienis','trečiadienis','ketvirtadienis','penktadienis','šeštadienis'],
		dayNamesShort: ['sek','pir','ant','tre','ket','pen','šeš'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Dabar', 'Ankstesnis laukas', 'Kitas laukas', 'Daugiau', 'Mažiau'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['lt']);
})(jQuery);
