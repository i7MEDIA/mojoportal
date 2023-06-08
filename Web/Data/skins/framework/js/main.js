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

	// Administration Drawer | Needs to be refoctored and moved to administration.js
	if ($('.workflow-type select').length > 0) {
		$('.workflow-type').addClass('show');
		var startingValue = $('.admin-drawer .workflow-type select').val();

		if (startingValue == 'Live') {
			$('.slider-switch__thumb').addClass('active refresh');

			setTimeout(function() {
				$('.slider-switch__thumb').removeClass('refresh');
			}, 280);
		}

		$('.workflow-type > a').click(function(e) {
			e.preventDefault();

			if (startingValue == 'Live') {
				$('.slider-switch__thumb').removeClass('active');

				setTimeout(function() {
					$('.admin-drawer .workflow-type select').val('WorkInProgress').change();
				}, 280);
			} else {
				$('.slider-switch__thumb').addClass('active');

				setTimeout(function() {
					$('.admin-drawer .workflow-type select').val('Live').change();
				}, 280);
			}
		});
	}


	// For Workflow Icons | Needs to be refactored and moved to administration.js
	// Places better content for styling that fowards the click to original input
	$('a.ModuleRejectContentLink').html('<i class="fa fa-ban"></i>').addClass('workflow-icon');

	$('input.ModuleCancelChangesLink').each(function() {
		$('<a class="workflow-icon" href="#"><i class="fa fa-times-circle"></i></a>').attr({
			title: this.title
		}).insertBefore(this).uiTooltip().click(function() {
			$(this).next().click();
			return false;
		});
	}).hide();

	$('input.ModulePostDraftForApprovalLink, input.ModuleApproveContentLink').each(function() {
		$('<a class="workflow-icon" href="#"><i class="fa fa-check-circle"></i></a>').attr({
			title: this.title
		}).insertBefore(this).uiTooltip().click(function() {
			$(this).next().click();
			return false;
		});
	}).hide();

	$('.modulelinks img[src="/Data/SiteImages/info.gif"]').each(function() {
		$('<a class="workflow-icon" href="#"><i class="fa fa-info-circle"></i></a>').attr({
			title: this.title
		}).insertBefore(this).uiTooltip().css('cursor', 'pointer').click(function(e) {
			e.preventDefault();
		});;
		return false;
	}).remove();

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
