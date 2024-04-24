//
// Event Calendar - Clean out styles and BR elements
// --------------------------------------------------

;(function() {
	var cal = document.querySelector('.event-cal');

	if (cal) {
		var calChild = cal.querySelectorAll('*'),
			calBr = cal.querySelectorAll('.ec-day br');

		var removeYuck = function(yuck, removeType) {
			if (removeType == 'attr') {
				for (var i = 0; i < calChild.length; i++) {
					calChild[i].removeAttribute(yuck);
				}
			} else if (removeType == 'el') {
				for (var j = 0; j < calBr.length; j++) {
					calBr[j].parentNode.removeChild(calBr[j]);
				}
			}
		};

		removeYuck('style', 'attr');
		removeYuck('align', 'attr');
		removeYuck(calBr, 'el');
	}
})();


//
// jQuery Scripts
// --------------------------------------------------
$(document).ready(function() {
	// Add caret to menu items with drop-down
	$('.navbar-site .dropdown-toggle > a').append(' <span class="caret"></span>');

	// Remove classes from certain elements
	$(".pollchoose .buttonlink, input[id$='_btnShowResults']").removeClass("buttonlink");
	$(".altfile").parent(".breadcrumbs").removeClass("breadcrumbs");

	// Placeholders for form wizard
	if ($('.formwizard.labels-as-placeholders')) {
		$('.formwizard.labels-as-placeholders [id$="_pnlQuestions"] .settingrow').each(function() {
			var label = $(this).find('label').text();
			if ($(this).hasClass('require')) {
				label = label+" *";
			}
			$(this).find('input[type="text"]').prop('placeholder', label);
		});
	}
});
