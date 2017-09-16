document.forms[0].setAttribute('novalidate', '');

var filePicker = {
	vars: {
		input: null,
		url: null
	},

	set: function(url, clientId) {
		this.vars.input = $('#' + clientId);
		this.vars.url = url;
		this.vars.input.val(url).trigger('input');
	},

	close: function() {
		if ($('.url-browser__modal').length) {
			$('.url-browser__modal').modal('hide');

			if (this.vars.input.data('filetype') === 'image' && /[^\s]+\.(jpe?g|png|gif|bmp)$/.test(this.vars.url)) {
				this.vars.input.siblings('.url-browser__image-preview').attr('src', this.vars.url).trigger('input');
			}
		} else {
			if ($('.url-browser__modal').length) {
				$('.url-browser__modal').modal('hide');

				if (this.vars.input.data('filetype') === 'image' && /[^\s]+\.(jpe?g|png|gif|bmp)$/.test(this.vars.url)) {
					this.vars.input.siblings('.url-browser__image-preview').attr('src', this.vars.url).trigger('input');
				}
			} else {
				$.colorbox.close();
			} 
		}
	}
};

function openUrlBrowser(buttonObject) {
	var urlBrowserGroup = $(buttonObject).parent(),
		fileInput = urlBrowserGroup.find('input'),
		fileType = fileInput.data("filetype"),
		modal = $('.url-browser__modal'),
		modalPath = systemKeys.fileBrowserUrl + '?editor=filepicker&type=' + fileType + '&inputId=' + fileInput.attr('id');

	modal.find('.url-browser__modal-iframe').attr('src', modalPath);

	modal.find('.url-browser__modal-type').text(fileType.charAt(0).toUpperCase() + fileType.slice(1));

	modal.find('iframe').on('load', function() {
		var iframeHeight = $(this).contents().find('html').height();

		if ($(this).attr('src') != '') {
			$(this).attr('style', '');
		}
	});

	modal.modal('show');
};

$(document).ready(function() {
	function updatePreview($this) {
		if ($this.length > 1) {
			$this.each(function() {
				console.log($(this));
				if ($(this).val() !== '') {
					$(this).siblings('.url-browser__image-preview').attr('src', $(this).val())
				}
			});
		} else {
			if ($this.val() !== '') {
				$this.siblings('.url-browser__image-preview').attr('src', $this.val());
			}
		}
	}

	updatePreview($('.url-browser__browser-input'));

	$('.url-browser__modal').on('hidden.bs.modal', function() {
		$(this).find('.url-browser__modal-iframe').attr('src', '').hide();
	});

	$('.url-browser__browser-input').on('input', function() {
		updatePreview($(this));
	});
});