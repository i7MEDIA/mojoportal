// Array.prototypr.includes() polyfill
Array.prototype.includes||(Array.prototype.includes=function(a){"use strict";if(null==this)throw new TypeError("Array.prototype.includes called on null or undefined");var b=Object(this),c=parseInt(b.length,10)||0;if(0===c)return!1;var e,d=parseInt(arguments[1],10)||0;d>=0?e=d:(e=c+d,e<0&&(e=0));for(var f;e<c;){if(f=b[e],a===f||a!==a&&f!==f)return!0;e++}return!1});
// String.prototype.includes() polyfill
String.prototype.includes||(String.prototype.includes=function(a,b){"use strict";return"number"!=typeof b&&(b=0),!(b+a.length>this.length)&&this.indexOf(a,b)!==-1});

$(document).ready(function() {

	// Add placeholders to categories if they have them
	$('.cats-wrap').each(function() {
		if ($(this).find('[data-placeholder]').length) {
			var placeholder = $(this).find('[data-placeholder]').data('placeholder');
			$(this).find('input[type="text"]').attr("placeholder", placeholder);
		}
	});

	// Convert Dynamic Cat List To Radio Functionality
	$(".cats-radio").each(function() {
		var $this = $(this);
		var titleList = [];
		var origValue = $this.find('input[type="checkbox"]:checked').next('label').text();

		$this.find('input[type="checkbox"]').each(function() {
			titleList.push($(this).next('label').text().toLowerCase());
		});
		if ($this.find('ul.hide-list').length) {
			$this.find('input[type="text"]').val(origValue);
		}

		$(this).find('input[type="checkbox"]').on('change', function() {
			$this.find('input[type="checkbox"]:checked').prop('checked', false);
			$(this).prop('checked', true)
		});

		$(this).find('input[type="text"]').on('keyup keypress blur change', function() {
			if ($(this).val() !== '') {
				$this.find('input[type="checkbox"]').prop('checked', false)
			}
		});

		if ($this.hasClass('distinct')) {
			$(this).find('input[type="text"]').on('keyup keypress change', function() {
				var currentVal = $this.find('input[type="text"]').val().toLowerCase();
				if (currentVal.toLowerCase() !== origValue.toLowerCase()) {
					if (titleList.indexOf(currentVal) > -1) {
						$this.addClass("has-error");
						if (!$this.find('input[type="text"]').next('p.text-danger').length) {
							$('<p class="text-danger" style="font-size: 16px; margin: 6px 0 0;">An item with this title already exists, please enter a different title.</p>').insertAfter($this.find('input[type="text"]'));
						}
					} else {
						$this.removeClass("has-error");
						$this.find('input[type="text"]').next('p.text-danger').remove();
					}
				}
			});
		}

	});

	document.forms[0].setAttribute('novalidate', '');

	var checkList = $('ul.checkbox-title-control input[type="checkbox"]'),
		titleList = [],
		checkInput = $('input.checkbox-title-control'),
		titleInput = $('#checkboxTitleInput'),
		checkTitle,
		checkedTitle,
		titleOk = false,
		errorOk = true,
		checkboxTitleError = $('#checkboxTitleError');

	checkList.find('+ label').each(function() {
		titleList.push($(this).text());
	});

	checkList.each(function(i) {
		if ($(this).prop('checked')) {
			checkTitle = i;
			checkedTitle = titleList[i];
		}
	});

	if (checkTitle != null) {
		titleInput.val(titleList[checkTitle]);
	}

	titleInput.on('input blur', function(e) {
		if (titleList.includes($.trim($(this).val())) == false) {
			checkInput.val($(this).val());
			checkList.removeProp('checked');
			$(this).parents('.form-group').removeClass('has-error');
			checkboxTitleError.text('');
			errorOk = true;
		} else {
			var checked = titleList.indexOf($.trim($(this).val()));

			checkInput.val('');

			if (checkedTitle == $.trim(titleInput.val())) {
				checkList.eq(checked).prop('checked', 'checked');
			} else {
				$(this).parents('.form-group').addClass('has-error');
				checkboxTitleError.text('An item with this title already exists, please enter a different title.');
				errorOk = false;
			}
		}

		checkOk();
	});

	function checkOk() {
		if (titleInput.val() != '') {
			titleOk = true;
		} else {
			titleOk = false;
		}

		if (titleOk == true && errorOk == true) {
			$('#ctl00_mainContent_updateButton').removeAttr('disabled');
		} else if (titleOk == false || fileOk == false || errorOk == false) {
			$('#ctl00_mainContent_updateButton').prop('disabled', true);
		}
	}

	checkOk();
});