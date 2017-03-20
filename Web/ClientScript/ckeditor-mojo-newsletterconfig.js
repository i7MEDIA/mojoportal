CKEDITOR.editorConfig = function(config) {
	config.fullPage = true;
	config.allowedContent = true;
	config.toolbar_CKFull =
		[
			['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
			['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
			['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
			['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
			'/', ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
			['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
			['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
			['Link', 'Unlink', 'Anchor'],
			['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
			'/', ['Styles', 'Format', 'Font', 'FontSize'],
			['TextColor', 'BGColor'],
			['Maximize', 'ShowBlocks', '-', 'About']
		];


	config.toolbar_Full =
		[
			['Source'],
			['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteWord', '-', 'Print'],
			['Undo', 'Redo', '-', 'Find', 'Replace'],
			'/', ['Blockquote', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
			['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
			['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyFull'],
			['Link', 'Unlink', 'Anchor'],
			['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'],
			'/', ['Format', 'Styles'],
			['Maximize']

		];

	config.toolbar_Newsletter =
		[
			['Source'],
			['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteWord', '-', 'Print'],
			['Undo', 'Redo', '-', 'Find', 'Replace'],
			'/', ['Blockquote', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
			['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
			['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyFull'],
			['Link', 'Unlink', 'Anchor'],
			['Image', 'Table', 'HorizontalRule', 'SpecialChar'],
			'/', ['Format', 'Font', 'FontSize'],
			['TextColor', 'BGColor'],
			['Maximize']

		];

	config.toolbar_FullWithTemplates =
		[
			['Source'],
			['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteWord', '-', 'Print'],
			['Undo', 'Redo', '-', 'Find', 'Replace'],
			'/', ['Blockquote', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
			['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
			['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyFull'],
			['Link', 'Unlink', 'Anchor'],
			['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'],
			'/', ['Templates', 'Format', 'Styles'],
			['Maximize']

		];

	config.toolbar_Forum =
		[
			['Cut', 'Copy', 'PasteText', '-'],
			['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
			['Blockquote', 'Bold', 'Italic', 'Underline'],
			['OrderedList', 'UnorderedList'],
			['Link', 'Unlink'],
			['SpecialChar', 'Smiley']
		];

	config.toolbar_ForumWithImages =
		[
			['Cut', 'Copy', 'PasteText', '-'],
			['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
			['Blockquote', 'Bold', 'Italic', 'Underline', 'Image'],
			['OrderedList', 'UnorderedList'],
			['Link', 'Unlink'],
			['SpecialChar', 'Smiley']
		];

	config.toolbar_SimpleWithSource =
		[
			['Source', 'Cut', 'Copy', 'PasteText'],
			['Blockquote', 'Bold', 'Italic', '-', 'OrderedList', 'UnorderedList', '-', 'Link', 'Unlink', 'Smiley']
		];

	config.toolbar_AnonymousUser =
		[
			['Cut', 'Copy', 'PasteText'],
			['Blockquote', 'Bold', 'Italic', '-', 'OrderedList', 'UnorderedList', '-', 'Link', 'Unlink', 'Smiley']
		];

};