/* ========================================================================
 * Framework: administration.js v3.3.6
 * ========================================================================
 * Copyright 2016 i7MEDIA LLC
 * ======================================================================== */


// Polyfil to support dispatchEvent in older browsers
function Event( event, params ) {
    params = params || { bubbles: false, cancelable: false, detail: undefined };
    var evt = document.createEvent( 'CustomEvent' );
    evt.initCustomEvent( event, params.bubbles, params.cancelable, params.detail );
    return evt;
}


//
// Toggle Edit Links
// --------------------------------------------------

;(function() {
	var toggleLinks = {
		links: document.querySelectorAll('.flexi-item-edit,.forumEdit,.modulelinks,.ModuleEditLink,.postEdit,.galleryedit,.feedlist .editlink, #editLink,.threadEdit,.forumpostusername a[id*="Hyperlink2"],.linksmodule [id*="editLink2"],.carousel-slide-edit-link, [id*="_rptEvents"][id*="_editLink"]'),
		linkBtn: document.querySelector('.edit-link-toggle'),
		linkSwitch: document.querySelector('.edit-link-toggle-button'),
		getCookie: function() {
			return Get_Cookie('fwEditLinks');
		},
		setCookie: function(fwValue) {
			Set_Cookie('fwEditLinks', fwValue, "", "/");
		},
		hide: function() {
			[].forEach.call(this.links, function(link) {
				link.style.display = 'none';
			});
			this.linkSwitch.classList.add('is-checked');
			this.setCookie('hide');
		},
		show: function() {
			[].forEach.call(this.links, function(link) {
				link.removeAttribute('style');
			});
			this.linkSwitch.classList.remove('is-checked');
			this.setCookie('show');
		},
		init: function() {
			var links = this;

			if (this.getCookie() == 'hide') {
				this.hide();
			}

			if (this.linkBtn) {
				this.linkBtn.addEventListener('click', function(e) {
					e.preventDefault();
					var cookie = links.getCookie();
					if (cookie === null || cookie == 'show') {
						links.hide();
					} else if (cookie == 'hide') {
						links.show();
					}
					return false;
				}, false);
			}
		}
	};

	if (document.querySelector('.admin-drawer')) {
		toggleLinks.init();
	}
})(); 


//
// For the workflow toggle
// --------------------------------------------------
;(function() {
	var workFlow = {
		link: document.querySelector('.admin-drawer .workflow-type > a'),
		select: document.querySelector('.admin-drawer .workflow-type select'),
		thumb: document.querySelector('.admin-drawer .slider-switch__thumb'),
		toggle: function() {
			var _this = this;

			if (_this.select.value == 'Live') {
				_this.thumb.classList.remove('active');

				setTimeout(function() {
					_this.select.value = 'WorkInProgress';
					_this.select.dispatchEvent(new Event('change'));
				}, 280);
			} else {
				_this.thumb.classList.add('active');

				setTimeout(function() {
					_this.select.value = 'Live';
					_this.select.dispatchEvent(new Event('change'));
				}, 280)
			}
		},
		init: function() {
			var _this = this;

			if (_this.select.value == 'Live') {
				_this.thumb.classList.add('active', 'refresh');

				setTimeout(function() {
					_this.thumb.classList.remove('refresh');
				}, 280);
			}

			_this.link.addEventListener('click', function(e) {
				e.preventDefault();

				_this.toggle();
			});

		}
	}
	if (document.querySelector('.admin-drawer .workflow-type')) {
		workFlow.init();
	}
});


//
// Administration Drawer
// --------------------------------------------------

;(function() {
	var adminDrawer = {
		openBtn: document.querySelector('.admin-drawer__open'),
		closeBtn: document.querySelector('.admin-drawer__close'),
		drawer: function() {
			var drawer = this.openBtn.getAttribute('data-admin-drawer');
			return document.querySelector(drawer);
		},
		getCookie: Get_Cookie('fwAdminDrawer'),
		setCookie: function(fwValue) {
			Set_Cookie('fwAdminDrawer', fwValue, "", "/");
		},
		open: function(fwClass, fwCookie) {
			var _this = this;
			if (typeof fwClass === 'object') {
				fwClass.forEach(function(element) {
					_this.drawer().classList.add(element);
				});
			} else if (typeof fwClass == 'string') {
				this.drawer().classList.add(fwClass);
			}
			this.setCookie(fwCookie);
		},
		close: function(fwClass, fwCookie) {
			this.drawer().classList.remove(fwClass);
			this.setCookie(fwCookie);
		},
		init: function() {
			var _this = this;

			if (this.getCookie === null || this.getCookie == 'open') {
				if ($(window).width() >= 768) {
					this.open(['active', 'refresh'], 'open');
				} else {
					_this.close('active', 'close');
				}

				setTimeout(function() {
					_this.drawer().classList.remove('refresh');
				}, 250);
			}

			if (hideAdminDrawerInitially === true && this.getCookie != 'open') {
				_this.close('active', 'close');
			}

			this.openBtn.addEventListener('click', function(e) {
				e.preventDefault();
				_this.open('active', 'open');
			}, false);

			this.closeBtn.addEventListener('click', function(e) {
				e.preventDefault();
				_this.close('active', 'close');
				return false;
			}, false);
		}
	};

	if (document.querySelector('.admin-drawer')) {
		adminDrawer.init();
	}
})();

// Slight stylistic improvement for the administration page, this was not directly changed in mojo to avoid conflicts with older versions of fontawesome in older skins
var secIcon = `<span class="fa-stack" style="margin: 10px 0;"><i class="fa fa-shield fa-stack-2x"></i><i class="fa fa-exclamation fa-inverse fa-stack-1x"></i></span>`;
if (document.querySelector('.adminlink-securityadvisor > a > .fa.fa-shield')) {
	document.querySelector('.adminlink-securityadvisor > a > .fa.fa-shield').insertAdjacentHTML('beforebegin', secIcon);
	document.querySelector('.adminlink-securityadvisor > a > .fa.fa-shield').remove();
}

// For Workflow Icons | Needs to be refactored into plain JS
// Places better content for styling that fowards the click to original input
$(document).ready(function() {
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
});
