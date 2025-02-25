//
// Global method for setting input value from modal
//

var filePicker = {
	set: function (url, clientId) {
		const input = document.getElementById(clientId);
		const event = new Event('input');

		input.value = url;
		input.dispatchEvent(event);
	},

	close: function () {
		if (document.querySelector('.url-browser__modal') !== null) {
			$('.url-browser__modal').modal('hide');
		} else {
			$.colorbox.close();
		}
	}
};


//
// Advanced File Picker
//

(function (d) {
	const advancedFilePicker = d.querySelectorAll('.advanced-file-picker');

	//
	// Keep native validation from running
	//

	d.forms[0].setAttribute('novalidate', '');


	//
	// Set heading and iframe source of modal, then open it
	//

	const openFileManager = function (output, pickerType, startFolder) {
		const modal = d.querySelector('.url-browser__modal');
		const modalIframe = modal.querySelector('.url-browser__modal-iframe');
		const modalType = modal.querySelector('.url-browser__modal-type');
		const modalPath = systemKeys.fileBrowserUrl + '?editor=filepicker&type=' + pickerType + '&inputId=' + output.id;
		let modalStartFolder = '';

		if (startFolder !== '') {
			modalStartFolder = '&startFolder=' + startFolder;
		}

		modalType.textContent = pickerType.charAt(0).toUpperCase() + pickerType.slice(1);
		modalIframe.src = modalPath + modalStartFolder;

		modalIframe.addEventListener('load', function () {
			if (this.src !== '') {
				this.removeAttribute('style');
			}
		});

		$(modal).modal('show');
	};


	//
	// For Every File Picker
	//

	advancedFilePicker.forEach(function (picker) {

		//
		// Variables
		//

		const pickerType = picker.dataset.pickerType;
		const startFolder = picker.dataset.startFolder;
		const pickerDefaultText = picker.dataset.pickerDefaultText;
		const output = picker.parentNode.querySelector('.advanced-file-picker__output');
		const showToolsBtn = picker.querySelector('.advanced-file-picker__show-tools-btn');
		const tools = picker.querySelector('.advanced-file-picker__tools');
		const toolsInput = tools.querySelector('.advanced-file-picker__tools-input');
		const okToolsBtn = tools.querySelector('.advanced-file-picker__tools-btn--set');
		const clearToolsBtn = tools.querySelector('.advanced-file-picker__tools-btn--clear');
		const pickerLink = picker.querySelector('.advanced-file-picker__link');
		const imagePreview = picker.querySelector('.advanced-file-picker__image-preview');
		const pickerText = picker.querySelector('.advanced-file-picker__text');


		//
		// Functions
		//

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

		const setPickerText = function (str, title) {
			if (title === undefined) title = str;

			pickerText.classList.remove('text-danger');
			pickerText.title = title;
			pickerText.innerHTML = str;
		};

		const returnGifIfEmpty = function (str) {
			return str !== '' ? str : 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7';
		};

		const setImagePreview = function (url) {
			imagePreview.src = returnGifIfEmpty(url);
			previewState('outside');
		};

		const showTools = function (e) {
			tools.classList.remove('hide');
			toolsInput.focus();
		};

		const hideTools = function (e) {
			tools.classList.add('hide');
		};


		const failValidation = function (validationMessage) {
			pickerText.classList.add('text-danger');
			pickerText.textContent = validationMessage;
			previewState('inside');
		};

		const validateInput = function (mutationsList) {
			mutationsList.forEach(function (mutation) {
				if (mutation.type === 'attributes' && mutation.attributeName === 'style') {
					const validationMessage = mutation.target.dataset.valErrormessage;

					failValidation(validationMessage);
				}
			});
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

					failValidation(validationMessage);

					return false;
				}

				return true;
			}
		};

		const checkFile = function () {
			const url = output.value.startsWith('/') ?
				new URL(window.location.origin + output.value) :
				new URL(output.value);

			fetch(url, { method: 'HEAD' })
				.then(response => {
					if (!response.ok) {
						failValidation('That file does not exist, please choose another file.');
					}
				})
				.catch(e => {
					console.log(e);
				});
		};

		const bindEvents = function () {
			pickerLink.addEventListener('click', fileManagerEvent, false);
			output.addEventListener('input', updatePreviewEvent, false);
			showToolsBtn.addEventListener('click', toggleToolsEvent, false);
			toolsInput.addEventListener('keypress', enterKeyEvent, false);
			clearToolsBtn.addEventListener('click', clearFileEvent, false);
			okToolsBtn.addEventListener('click', submitUrlEvent, false);
			d.body.addEventListener('click', bodyToolsCloseEvent, false);
		};


		//
		// Event Handlers
		//

		const fileManagerEvent = function (e) {
			e.preventDefault();
			openFileManager(output, pickerType, startFolder);
		};

		const updatePreviewEvent = function (e) {
			e.preventDefault();

			if (this.value === '') {
				setPickerText(pickerDefaultText);
				previewState('inside');

				return;
			}

			let newPickerText = `<span class="trunc-center">${this.value}</span>`;
			let newPickerTextTitle = this.value;

			toolsInput.value = this.value;

			previewState('inside');

			if (this.value.includes('.')) {
				const fileExt = this.value.split('.').pop();
				let fileName = getFileName(this.value).split('.');
				fileName.pop();

				newPickerText = '<span class="trunc-center">' + fileName + '</span><span class="trunc-ext">.' + fileExt + '</span>';
				newPickerTextTitle = getFileName(this.value);

				if (picker.dataset.pickerType === 'image') {
					setImagePreview(this.value);
				}

				if (picker.dataset.pickerType === 'file') {
					const imageTypes = ['bmp', 'cod', 'gif', 'ief', 'jpe', 'jpeg', 'jpg', 'jfif', 'svg', 'tif', 'tiff', 'ras', 'cmx', 'ico', 'png', 'pnm', 'pbm', 'pgm', 'ppm', 'rgb', 'xbm', 'xpm', 'xwd'];

					let iconCssClass;

					if (fileExt) {
						switch (fileExt) {
							case 'pdf':
								iconCssClass = 'fa-file-pdf';
								break;
							case 'xls':
							case 'xlsx':
								iconCssClass = 'fa-file-excel';
								break;
							case 'doc':
							case 'docx':
								iconCssClass = 'fa-file-word';
								break;
							case 'ppt':
								iconCssClass = 'fa-file-powerpoint';
								break;
							default:
								iconCssClass = 'fa-file';
						}
					}
					else {
						iconCssClass = 'fa-link';
					}

					if (imageTypes.includes(fileExt)) {
						setImagePreview(this.value);
						iconCssClass = 'fa-image';
					}

					newPickerText = `<span class="trunc-icon fa-regular ${iconCssClass}"></span>
						<span class="trunc-center">${fileName}</span>
						<span class="trunc-ext">.${fileExt}</span>`;
				}
			}

			setPickerText(newPickerText, newPickerTextTitle);
			checkFile();
		};

		const toggleToolsEvent = function (e) {
			e.preventDefault();

			if (tools.classList.contains('hide')) {
				showTools();
			} else {
				hideTools();
			}
		};

		const submitUrlEvent = function (e) {
			e.preventDefault();

			const outputEvent = new Event('input');

			output.value = toolsInput.value;
			output.dispatchEvent(outputEvent);
			hideTools();
		};

		const enterKeyEvent = function (e) {
			if (e.key === 'Enter') {
				e.stopPropagation();
				e.preventDefault();

				okToolsBtn.click();
			}
		};

		const clearFileEvent = function (e) {
			e.preventDefault();

			const outputEvent = new Event('input');

			output.value = '';
			toolsInput.value = '';
			imagePreview.src = returnGifIfEmpty('');
			hideTools();
			output.dispatchEvent(outputEvent);
		};

		const bodyToolsCloseEvent = function (e) {
			if (
				!e.target.closest('.advanced-file-picker__show-tools-btn') &&
				!e.target.closest('.advanced-file-picker__tools')
			) {
				hideTools();
			}
		};


		//
		// Init
		//

		const init = function () {
			const outputEvent = new Event('input');
			const valid = pickerValidation();

			bindEvents();

			if (output.value === '') {
				if (valid !== false) {
					previewState('inside');
				}
			} else {
				output.dispatchEvent(outputEvent);
			}
		};

		init();
	});
})(document);
