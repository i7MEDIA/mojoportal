/* ========================================================================
 * Framework: administration.js v3.3.6
 * ========================================================================
 * Copyright 2016 i7MEDIA LLC
 * ======================================================================== */





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