/*eslint eqeqeq: ["error", "smart"]*/
document.forms[0].setAttribute('novalidate', '');

var filePicker = {
	vars: {
		input: null,
		url: null
	},

	set: function (url, clientId) {
		var event = document.createEvent('Event');
		event.initEvent('input', true, true);

		this.vars.url = url;

		this.vars.input = document.getElementById(clientId);
		this.vars.input.value = url;
		this.vars.input.dispatchEvent(event);
	},

	close: function () {
		if (document.querySelector('.url-browser__modal') !== null) {
			$('.url-browser__modal').modal('hide');
			if (this.vars.input.dataset.fileType === 'image') {
				this.vars.input.parentNode.querySelector('.url-browser__image-preview').src = this.vars.url;

				var event = document.createEvent('Event');
				event.initEvent('input', true, true);
				this.vars.input.dispatchEvent(event);
			}
		} else {
			$.colorbox.close();
		}
	}
};

(function () {
	const d = document;
	const advancedFilePicker = d.querySelectorAll('.advanced-file-picker');

	const openFileManager = function (output, pickerType) {
		var modal = d.querySelector('.url-browser__modal'),
			modalIframe = modal.querySelector('.url-browser__modal-iframe'),
			modalType = modal.querySelector('.url-browser__modal-type'),
			modalPath = systemKeys.fileBrowserUrl + '?editor=filepicker&type=' + pickerType + '&inputId=' + output.id;

		modalType.textContent = pickerType.charAt(0).toUpperCase() + pickerType.slice(1);
		modalIframe.src = modalPath;
		modalIframe.addEventListener('load', function () {
			if (this.src != '') {
				this.removeAttribute('style');
			}
		});

		$(modal).modal('show');
	};

	advancedFilePicker.forEach(function (picker) {
		const pickerType = picker.dataset.pickerType;
		const output = picker.parentNode.querySelector('.advanced-file-picker__output');
		const showToolsBtn = picker.querySelector('.advanced-file-picker__show-tools-btn');
		const tools = picker.querySelector('.advanced-file-picker__tools');
		const toolsInput = tools.querySelector('.advanced-file-picker__tools-input');
		const okToolsBtn = tools.querySelector('.advanced-file-picker__tools-btn--ok');
		const cancelToolsBtn = tools.querySelector('.advanced-file-picker__tools-btn--cancel');
		const pickerLink = picker.querySelector('.advanced-file-picker__link');
		const imagePreview = picker.querySelector('.advanced-file-picker__image-preview');
		const pickerText = picker.querySelector('.advanced-file-picker__text');

		const previewState = function (state) {
			if (state === 'inside') {
				if (pickerText.parentNode === picker) {
					pickerLink.appendChild(pickerText);
					imagePreview.classList.add('hide');
				}
			} else if (state === 'outside') {
				if (pickerText.parentNode === pickerLink) {
					picker.appendChild(pickerText);
					imagePreview.classList.remove('hide');
				}
			}
		};

		const getFileName = function (str) {
			const splitString = str.split('/');
			return splitString[splitString.length - 1];
		};

		const setPickerText = function (pickerText, str, title = str) {
			pickerText.classList.remove('text-danger');
			pickerText.title = title;
			pickerText.innerHTML = str;
		};

		const createEvent = function (eventName) {
			var event = document.createEvent('Event');
			event.initEvent(eventName, true, true);
			return event;
		};

		const returnGifIfEmpty = function (str) {
			return str !== '' ? str : 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7';
		};

		const bindEvents = function () {
			const fileManagerEvent = function (e) {
				e.preventDefault();
				openFileManager(output, pickerType);
			};

			const updatePreviewEvent = function (e) {
				e.preventDefault();

				const formGroup = this.parentNode;
				const picker = formGroup.querySelector('.advanced-file-picker');
				const pickerText = formGroup.querySelector('.advanced-file-picker__text');
				const pickerDefaultText = picker.dataset.pickerDefaultText;
				const toolsInput = formGroup.querySelector('.advanced-file-picker__tools-input');
				const url = this.value;

				let newPickerText;
				let newPickerTextTitle;

				toolsInput.value = url;

				const setImagePreview = function () {
					const imagePreview = formGroup.querySelector('.advanced-file-picker__image-preview')

					imagePreview.src = returnGifIfEmpty(url);
					newPickerText = getFileName(url);
					newPickerTextTitle = getFileName(url);
					previewState('outside');
				};

				if (picker.dataset.pickerType === 'image') {
					setImagePreview();
				}

				if (picker.dataset.pickerType === 'file') {
					const imageTypes = ['bmp', 'cod', 'gif', 'ief', 'jpe', 'jpeg', 'jpg', 'jfif', 'svg', 'tif', 'tiff', 'ras', 'cmx', 'ico', 'png', 'pnm', 'pbm', 'pgm', 'ppm', 'rgb', 'xbm', 'xpm', 'xwd'];
					const fileExt = url.split('.').pop();

					if (imageTypes.includes(fileExt)) {
						setImagePreview();
					} else {
						let iconCssClass;

						switch (fileExt) {
							case 'pdf':
								iconCssClass = 'fa-file-pdf-o';
								break;
							case 'xls':
							case 'xlsx':
								iconCssClass = 'fa-file-excel-o';
								break;
							case 'doc':
							case 'docx':
								iconCssClass = 'fa-file-word-o';
								break;
							case 'ppt':
								iconCssClass = 'fa-file-powerpoint-o';
								break;
							default:
								iconCssClass = 'fa-file-o';
						};

						let fileName = getFileName(url).split('.');
						fileName.pop();

						newPickerText = '<span class="trunc-icon fa ' + iconCssClass + '">' + '</span>' +
							'<span class="trunc-center">' + fileName + '</span>' +
							'<span class="trunc-ext">.' + fileExt + '</span>' +
							'</div>';
						newPickerTextTitle = getFileName(url);

						previewState('inside');
					}
				}


				if (url === '') {
					setPickerText(pickerText, pickerDefaultText);
				} else {
					setPickerText(pickerText, newPickerText, newPickerTextTitle);
				}
			};

			const toggleToolsEvent = function (e) {
				e.preventDefault();

				const tools = this.closest('.advanced-file-picker').querySelector('.advanced-file-picker__tools');
				tools.classList.toggle('hide');

				if (!tools.classList.contains('hide')) {
					tools.querySelector('.advanced-file-picker__tools-input').focus();
				}
			};

			const submitUrlEvent = function (e) {
				e.preventDefault();

				const url = this.previousElementSibling.value;
				const output = this.closest('.settingrow').querySelector('.advanced-file-picker__output');
				const closeBtn = this.nextElementSibling;
				const inputEvent = createEvent('input');

				output.value = url;
				output.dispatchEvent(inputEvent);
				closeBtn.click();
			};

			const enterKeyEvent = function (e) {
				if (e.key === 'Enter') {
					e.stopPropagation();
					e.preventDefault();
					this.nextElementSibling.click();
				}
			};

			pickerLink.addEventListener('click', fileManagerEvent, false);
			output.addEventListener('input', updatePreviewEvent, false);
			showToolsBtn.addEventListener('click', toggleToolsEvent, false);
			toolsInput.addEventListener('keypress', enterKeyEvent, false);
			cancelToolsBtn.addEventListener('click', toggleToolsEvent, false);
			okToolsBtn.addEventListener('click', submitUrlEvent, false);
		};

		const failValidation = function (element, validationMessage) {
			element.classList.add('text-danger');
			element.textContent = validationMessage;
		};

		const validateInput = function (mutationsList) {
			for (var mutation of mutationsList) {
				if (mutation.type == 'attributes' && mutation.attributeName === 'style') {
					const validationMessage = mutation.target.dataset.valErrormessage;
					const formGroup = mutation.target.parentNode;
					const pickerText = formGroup.querySelector('.advanced-file-picker__text');

					failValidation(pickerText, validationMessage);
				}
			}
		};

		const pickerValidation = function () {
			const valElement = picker.parentNode.querySelector('[data-val="true"]');

			if (valElement !== null) {
				const observer = new MutationObserver(validateInput);
				const config = {
					attributes: true,
					childList: false,
					subtree: false
				};

				valElement.classList.add('hide');

				observer.observe(valElement, config);

				if (valElement.dataset.valIsvalid !== undefined && valElement.dataset.valIsvalid === 'False') {
					const validationMessage = valElement.dataset.valErrormessage;

					failValidation(pickerText, validationMessage);

					return false;
				}

				return true;
			}
		};

		const init = function () {
			const valid = pickerValidation();

			bindEvents();

			output.classList.add('hide');

			if (output.value === '') {
				if (valid !== false) {
					previewState('inside');
				}
			} else {
				const inputEvent = createEvent('input');
				output.dispatchEvent(inputEvent);
				previewState('outside');
			}
		};

		init();
	});
})();