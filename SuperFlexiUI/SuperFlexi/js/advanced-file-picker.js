/*eslint eqeqeq: ["error", "smart"]*/
// Element.closest() Polyfill
Element.prototype.matches||(Element.prototype.matches=Element.prototype.msMatchesSelector||Element.prototype.webkitMatchesSelector),Element.prototype.closest||(Element.prototype.closest=function(e){var t=this;do{if(t.matches(e))return t;t=t.parentElement||t.parentNode}while(null!==t&&1===t.nodeType);return null});
// NodeList.prototype.forEach() Polyfill
window.NodeList&&!NodeList.prototype.forEach&&(NodeList.prototype.forEach=function(o,t){t=t||window;for(var i=0;i<this.length;i++)o.call(t,this[i],i,this)});
// https://tc39.github.io/ecma262/#sec-array.prototype.includes
Array.prototype.includes||Object.defineProperty(Array.prototype,"includes",{value:function(r,e){if(null==this)throw new TypeError('"this" is null or not defined');var t=Object(this),n=t.length>>>0;if(0===n)return!1;var i,o,a=0|e,u=Math.max(0<=a?a:n-Math.abs(a),0);for(;u<n;){if((i=t[u])===(o=r)||"number"==typeof i&&"number"==typeof o&&isNaN(i)&&isNaN(o))return!0;u++}return!1}});



//
// Global method for setting input value from modal
//

var filePicker = {
	set: function(url, clientId) {
		const input = document.getElementById(clientId);
		const event = document.createEvent('Event');

		event.initEvent('input', true, true);
		input.value = url;
		input.dispatchEvent(event);
	},

	close: function() {
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

(function(d) {
	const advancedFilePicker = d.querySelectorAll('.advanced-file-picker');


	//
	// Keep native validation from running
	//

	d.forms[0].setAttribute('novalidate', '');


	//
	// Set heading and iframe source of modal, then open it
	//

	const openFileManager = function(output, pickerType) {
		const modal = d.querySelector('.url-browser__modal');
		const modalIframe = modal.querySelector('.url-browser__modal-iframe');
		const modalType = modal.querySelector('.url-browser__modal-type');
		const modalPath = systemKeys.fileBrowserUrl + '?editor=filepicker&type=' + pickerType + '&inputId=' + output.id;

		modalType.textContent = pickerType.charAt(0).toUpperCase() + pickerType.slice(1);
		modalIframe.src = modalPath;
		modalIframe.addEventListener('load', function() {
			if (this.src !== '') {
				this.removeAttribute('style');
			}
		});

		$(modal).modal('show');
	};


	//
	// For Every File Picker
	//

	advancedFilePicker.forEach(function(picker) {

		//
		// Variables
		//

		const pickerType = picker.dataset.pickerType;
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
		const outputEvent = document.createEvent('Event');


		//
		// Functions
		//

		const previewState = function(state) {
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

		const getFileName = function(str) {
			const splitString = str.split('/');
			return splitString[splitString.length - 1];
		};

		const setPickerText = function(str, title) {
			if (title === undefined) title = str;

			pickerText.classList.remove('text-danger');
			pickerText.title = title;
			pickerText.innerHTML = str;
		};

		const returnGifIfEmpty = function(str) {
			return str !== '' ? str : 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7';
		};

		const setImagePreview = function(url) {
			imagePreview.src = returnGifIfEmpty(url);
			previewState('outside');
		};

		const showTools = function(e) {
			tools.classList.remove('hide');
			toolsInput.focus();
		};

		const hideTools = function(e) {
			tools.classList.add('hide');
		};


		const failValidation = function(validationMessage) {
			pickerText.classList.add('text-danger');
			pickerText.textContent = validationMessage;
			previewState('inside');
		};

		const validateInput = function(mutationsList) {
			mutationsList.forEach(function (mutation) {
				if (mutation.type == 'attributes' && mutation.attributeName === 'style') {
					const validationMessage = mutation.target.dataset.valErrormessage;

					failValidation(validationMessage);
				}
			});
		};

		const pickerValidation = function() {
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

		const checkFile = function() {
			const request = new XMLHttpRequest();

			function listener() {
				if (this.status !== 200) {
					failValidation('That file does not exist, please choose another file.');
				}
			}

			request.open('HEAD', output.value);
			request.send();
			request.addEventListener('load', listener);
		};

		const bindEvents = function() {
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
		
		const fileManagerEvent = function(e) {
			e.preventDefault();
			openFileManager(output, pickerType);
		};

		const updatePreviewEvent = function(e) {
			e.preventDefault();

			const url = this.value;
			const fileExt = url.split('.').pop();

			let fileName = getFileName(url).split('.');
			fileName.pop();
			
			let newPickerText = '<span class="trunc-center">' + fileName + '</span><span class="trunc-ext">.' + fileExt + '</span>';
			let newPickerTextTitle = getFileName(url);

			toolsInput.value = url;

			if (picker.dataset.pickerType === 'image') {
				setImagePreview(url);
			}

			if (picker.dataset.pickerType === 'file') {
				const imageTypes = ['bmp','cod','gif','ief','jpe','jpeg','jpg','jfif','svg','tif','tiff','ras','cmx','ico','png','pnm','pbm','pgm','ppm','rgb','xbm','xpm','xwd'];

				if (imageTypes.includes(fileExt)) {
					setImagePreview(url);
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
					}

					newPickerText = '<span class="trunc-icon fa ' + iconCssClass + '">' + '</span>' +
						'<span class="trunc-center">' + fileName + '</span>' +
						'<span class="trunc-ext">.' + fileExt + '</span>';

					previewState('inside');
				}
			}


			if (url === '') {
				setPickerText(pickerDefaultText);
				previewState('inside');
			} else {
				setPickerText(newPickerText, newPickerTextTitle);
				checkFile();
			}
		};

		const toggleToolsEvent = function(e) {
			e.preventDefault();

			if (tools.classList.contains('hide')) {
				showTools();
			} else {
				hideTools();
			}
		};

		const submitUrlEvent = function(e) {
			e.preventDefault();

			output.value = toolsInput.value;
			output.dispatchEvent(outputEvent);
			hideTools();
		};

		const enterKeyEvent = function(e) {
			if (e.key === 'Enter') {
				e.stopPropagation();
				e.preventDefault();

				okToolsBtn.click();
			}
		};

		const clearFileEvent = function(e) {
			e.preventDefault();

			output.value = '';
			toolsInput.value = '';
			imagePreview.src = returnGifIfEmpty('');
			hideTools();
			output.dispatchEvent(outputEvent);
		};

		const bodyToolsCloseEvent = function(e) {
			if (!e.target.closest('.advanced-file-picker__show-tools-btn') && !e.target.closest('.advanced-file-picker__tools')) {
				hideTools();
			}
		};


		//
		// Init
		//

		const init = function() {
			outputEvent.initEvent('input', true, true);

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