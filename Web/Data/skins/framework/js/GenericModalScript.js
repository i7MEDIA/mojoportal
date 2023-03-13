(function() {
	// This code assumes that jQuery and the Bootstrap 3.4.1 Modal plugin are on the page.
	function addModal(e) {
		e.preventDefault();
		const link = e.target.closest('a[data-modal]');
		const template = document.getElementById('mojoModalTemplate');
		const clone = document.importNode(template.content, true);
		const modal = clone.querySelector('.modal');
		const modalTitle = modal.querySelector('.modal-title');
		const modalBody = modal.querySelector('.modal-body');
		const modalFooter = modal.querySelector('.modal-footer');
		const closeBtn = modal.querySelector('.close');
		const customCloseBtn = modal.querySelector('.modal-footer .btn[data-dismiss="modal"]');
	
		modalTitle.textContent = link.title;
		closeBtn.title = link.dataset.closeText;
		customCloseBtn.textContent = link.dataset.closeText;

		if (typeof(link.dataset.size) !== 'undefined' && link.dataset.size.trim() !== '') {
			const modalDialog = modal.querySelector('.modal-dialog');

			switch (link.dataset.size.toLowerCase()) {
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

		if (typeof (link.dataset.height) !== 'undefined' && link.dataset.height.trim() !== '') {
			switch (link.dataset.height.toLowerCase()) {
				default:
					break;
				case 'full':
					modal.classList.add('modal-full-height');
					break;
			}
		}

		if (link.dataset.modalType === 'iframe' && link.href?.trim() !== '') {
			const iframe = document.createElement('iframe');
	
			iframe.src = link.href;
			iframe.setAttribute('frameborder', 0);
			iframe.title = link.title;
			iframe.style.width = '100%';
			//iframe.style.minHeight = '400px';
	
			modalBody.innerHTML = '';
			modalBody.append(iframe);
		}
		else if (link.dataset.modalType === 'encodedHtml') {
			modalBody.innerHTML = link.dataset.content;
		}
		else if (link.dataset.modalType === 'customFunction') {
			new Function('modalBody', 'modalFooter', link.dataset.content)(modalBody, modalFooter);
		}
		else {
			modalBody.append(link.dataset.content);
		}
		
		document.body.append(modal);
	
		$(modal).modal('show');
		$(modal).on('hidden.bs.modal', e => e.target.remove());
	}

	const modalLinks = document.querySelectorAll('[data-modal]');

	for (const modalLink of modalLinks) {
		modalLink.addEventListener('click', addModal);
	}
})();