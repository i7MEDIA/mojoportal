(function() {
	// This code assumes that jQuery and the Bootstrap 3.4.1 Modal plugin are on the page.
	function addModal(e) {
		const target = e.target.closest('a[data-modal]');

		if (!target) {
			return;
		}

		e.preventDefault();

		const template = document.getElementById('mojoModalTemplate');
		const clone = document.importNode(template.content, true);
		const modal = clone.querySelector('.modal');
		const modalTitle = modal.querySelector('.modal-title');
		const modalBody = modal.querySelector('.modal-body');
		const modalFooter = modal.querySelector('.modal-footer');
		const closeBtn = modal.querySelector('.close');
		const customCloseBtn = modal.querySelector('.modal-footer .btn[data-dismiss="modal"]');
	
		modalTitle.textContent = target.title ?? target.dataset.title;
		closeBtn.title = target.dataset.closeText;
		customCloseBtn.textContent = target.dataset.closeText;

		if (typeof(target.dataset.size) !== 'undefined' && target.dataset.size.trim() !== '') {
			const modalDialog = modal.querySelector('.modal-dialog');

			switch (target.dataset.size.toLowerCase()) {
				default:
					break;
				case 'large':
					modalDialog.classList.add('modal-lg');
					modalDialog.classList.remove('modal-sm','modal-xl','modal-fluid','modal-fluid-lg','modal-fluid-xl');
					break;
				case 'xlarge':
					modalDialog.classList.add('modal-xl');
					modalDialog.classList.remove('modal-sm','modal-lg','modal-fluid','modal-fluid-lg','modal-fluid-xl');
					break;
				case 'small':
					modalDialog.classList.add('modal-sm');
					modalDialog.classList.remove('modal-lg','modal-xl','modal-fluid','modal-fluid-lg','modal-fluid-xl');
					break;	
				case 'fluid':
					modalDialog.classList.add('modal-fluid');
					modalDialog.classList.remove('modal-sm','modal-lg','modal-xl','modal-fluid-lg','modal-fluid-xl');
					break;										
				case 'fluid-large':
					modalDialog.classList.add('modal-fluid-lg');
					modalDialog.classList.remove('modal-sm','modal-lg','modal-xl','modal-fluid','modal-fluid-xl');
					break;										
				case 'fluid-xlarge':
					modalDialog.classList.add('modal-fluid-xl');
					modalDialog.classList.remove('modal-sm','modal-lg','modal-xl','modal-fluid','modal-fluid-lg');
					break;																							
			}
		}

		if (typeof (target.dataset.height) !== 'undefined' && target.dataset.height.trim() !== '') {
			switch (target.dataset.height.toLowerCase()) {
				default:
					break;
				case 'full':
					modal.classList.add('modal-full-height');
					break;
			}
		}

		if (target.dataset.modalType === 'iframe' && target.href?.trim() !== '') {
			const iframe = document.createElement('iframe');
	
			iframe.src = target.href;
			iframe.setAttribute('frameborder', 0);
			iframe.title = target.title;
			iframe.style.width = '100%';
			//iframe.style.minHeight = '400px';
	
			modalBody.innerHTML = '';
			modalBody.append(iframe);
		}
		else if (target.dataset.modalType === 'encodedHtml') {
			modalBody.innerHTML = target.dataset.content;
		}
		else {
			modalBody.append(target.dataset.content);
		}
		
		document.body.append(modal);
	
		$(modal).modal('show');

		if (target.dataset.callback) {
			$(modal).on('hidden.bs.modal', function () {
				eval(target.dataset.callback);
			});
		}

		$(modal).on('hidden.bs.modal', e => e.target.remove());
	}

	document.body.addEventListener('click', addModal);
})();
