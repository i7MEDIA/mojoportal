CKEDITOR.editorConfig = function(config) {
	config.entities = false;
	config.scayt_autoStartup = false;
	config.disableNativeSpellChecker = false;
	config.justifyClasses = ['text-left', 'text-center', 'text-right', 'text-justify'];
	config.indentClasses = ['text-indent-1', 'text-indent-2', 'text-indent-3'];
	config.removePlugins = 'stylesheetparser';
	config.extraPlugins = 'codemirror';
	config.sourceAreaTabSize = 8;
	//config.oembed_maxWidth = '560';
	//config.oembed_maxHeight = '315';
	config.embed_provider = '//ckeditor.iframe.ly/api/oembed?url={url}&callback={callback}';
	config.allowedContent = true;

	//A comma separated list of elements attributes to be removed when executing the remove format command.
	config.removeFormatAttributes = 'class,style,lang,width,height,align,hspace,valign,border,cellspacing,cellpadding';


	// ALLOW empty <i></i> elements, these are used for font awesome
	//config.protectedSource.push(/<i[\s\S]*?\>/g); //allows beginning <i> tag
	//config.protectedSource.push(/<\/i[\s\S]*?\>/g); //allows ending </i> tag

	// We're not using the "Stylesheet Parser" right now. It doesn't style some of the rules very well while in the dropdown,
	// they work fine but they look like plaintext in the editor's selector dropdown.
	//config.stylesheetParser_skipSelectors = /(^body\.|\.high|^input\.|^textarea\.|^button\.|^fieldset\.|^span\.fa|^a\.thumbnail|^a\.btn|\.has-submenu|\.collapse|^select\.|^br\.|\.cartnav|\.treecommands|\.forminput|\.scroll-|\.blogdate|\.jp-|\.cke_|\.sr-|\.jqtree|^\.)/i;
	
	config.image2_alignClasses = ['image-left', 'image-center', 'image-right'];
	config.image2_captionedClass = 'image-captioned';
	config.image2_altRequired = true;

	config.fontSize_sizes = 'X-Small/font-xsmall;Small/font-small;Normal/font-normal;Large/font-large;X-Large/font-xlarge';
	config.fontSize_style = {
		element: 'span',
		attributes: {
			'class': '#(size)'
		},
		overrides: [{
			element: 'font',
			attributes: {
				'size': null
			}
		}]
	};
	//config.colorButton_colors = 'muted/777777,primary/337ab7,success/3C763D,info/31708F,warning/8A6D3B,danger/A94442';

	//config.colorButton_foreStyle = {
	//	element: 'span',
	//	attributes: { 'class': 'text-#(colorName)' }
	//};

	//config.colorButton_backStyle = {
	//	element: 'span',
	//	attributes: { 'class': 'bg-#(colorName)' }
	//};

	//config.stylesSet = 'mojo';

	//config.format_tags = 'p;h1;h2;h3;h4';
	
	config.format_h1 = { element: 'h3' };
	config.format_h2 = { element: 'h4' };
	config.format_h3 = { element: 'h5' };
	config.format_h4 = { element: 'h6' };

	config.toolbar_CKFull =
		[
			['Source', '-','Save', 'NewPage', 'Preview', '-', 'Templates'],
		['autoFormat', 'CommentSelectedRange', 'UncommentSelectedRange', 'AutoComplete'],
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
			['Source', 'Maximize'],
		['autoFormat', 'CommentSelectedRange', 'UncommentSelectedRange', 'AutoComplete'],
			['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
			['Undo', 'Redo', '-', 'Find', 'Replace', 'Bold', 'Italic', 'Underline', '-', 'Strike', 'Superscript'],
			'/', ['Blockquote', 'Format', 'Styles', 'FontSize'],
			['TextColor', 'BGColor'],
			['NumberedList', 'BulletedList'],
			['Link', 'Unlink', 'Anchor'],
			['Image', 'oembed', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar']

		];

	config.toolbar_Newsletter =
		[
			['Source'],
		['autoFormat', 'CommentSelectedRange', 'UncommentSelectedRange', 'AutoComplete'],
			['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
			['Undo', 'Redo', '-', 'Find', 'Replace'],
			'/', ['Blockquote', 'Bold', 'Italic', 'Underline', 'Strike', 'Superscript'],
			['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
			['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
			['Link', 'Unlink', 'Anchor'],
			['Image', 'Table', 'HorizontalRule', 'SpecialChar'],
			'/', ['Format', 'Font', 'FontSize'],
			['TextColor', 'BGColor'],
			['Maximize', 'Preview']

		];

	config.toolbar_FullWithTemplates =
		[
			['Source', 'Maximize', 'ShowBlocks'],
		['autoFormat', 'CommentSelectedRange', 'UncommentSelectedRange','AutoComplete'],
			['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
			['Undo', 'Redo'],
			['Find', 'Replace'],
			['Bold', 'Italic', 'Underline', 'Strike', 'Superscript'],
			'/', ['Blockquote', 'Format', 'Styles', 'FontSize'],
			//['TextColor', 'BGColor'],
			['NumberedList', 'BulletedList', 'Outdent', 'Indent'],
			['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
			['Link', 'Unlink', 'Anchor'],
			['Templates', 'Image', 'oembed', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar']

		];

	config.toolbar_Forum =
		[
			['Cut', 'Copy', 'PasteText', '-'],
			['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
			['Blockquote', 'Bold', 'Italic', 'Underline'],
			['NumberedList', 'BulletedList'],
			['Link', 'Unlink'],
			['SpecialChar', 'Smiley']
		];

	config.toolbar_ForumWithImages =
		[
			['Cut', 'Copy', 'PasteText', '-'],
			['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
			['Blockquote', 'Bold', 'Italic', 'Underline', 'Image'],
			['NumberedList', 'BulletedList'],
			['Link', 'Unlink'],
			['SpecialChar', 'Smiley']
		];

	config.toolbar_SimpleWithSource =
		[
			['Source', 'Cut', 'Copy', 'PasteText'],
		['autoFormat', 'CommentSelectedRange', 'UncommentSelectedRange', 'AutoComplete'],
			['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'Smiley']
		];

	config.toolbar_AnonymousUser =
		[
			['Cut', 'Copy', 'PasteText'],
			['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'Smiley']
		];

	config.toolbar_Custom1 =
		[
			['Cut', 'Copy', 'PasteText'],
			['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'Smiley']
		];

	config.toolbar_Custom2 =
		[
			['Cut', 'Copy', 'PasteText'],
			['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'Smiley']
		];
	///
	/// CodeMirror Syntax Highlighting for CK Editor
	/// https://github.com/w8tcha/CKEditor-CodeMirror-Plugin
	///
	config.codemirror = {

		// Whether or not you want Brackets to automatically close themselves
		autoCloseBrackets: true,

		// Whether or not you want tags to automatically close themselves
		autoCloseTags: true,

		// Whether or not to automatically format code should be done when the editor is loaded
		autoFormatOnStart: true,

		// Whether or not to automatically format code which has just been uncommented
		autoFormatOnUncomment: true,

		// Whether or not to continue a comment when you press Enter inside a comment block
		continueComments: true,

		// Whether or not you wish to enable code folding (requires 'lineNumbers' to be set to 'true')
		enableCodeFolding: true,

		// Whether or not to enable code formatting
		enableCodeFormatting: true,

		// Whether or not to enable search tools, CTRL+F (Find), CTRL+SHIFT+F (Replace), CTRL+SHIFT+R (Replace All), CTRL+G (Find Next), CTRL+SHIFT+G (Find Previous)
		enableSearchTools: true,

		// Whether or not to highlight all matches of current word/selection
		highlightMatches: true,

		// Whether, when indenting, the first N*tabSize spaces should be replaced by N tabs
		indentWithTabs: true,

		// Whether or not you want to show line numbers
		lineNumbers: true,

		// Whether or not you want to use line wrapping
		lineWrapping: false,

		// Define the language specific mode 'htmlmixed' for html  including (css, xml, javascript), 'application/x-httpd-php' for php mode including html, or 'text/javascript' for using java script only 
		mode: 'htmlmixed',

		// Whether or not you want to highlight matching braces
		matchBrackets: true,

		// Whether or not you want to highlight matching tags
		matchTags: true,

		// Whether or not to show the showAutoCompleteButton   button on the toolbar
		showAutoCompleteButton: true,

		// Whether or not to show the comment button on the toolbar
		showCommentButton: true,

		// Whether or not to show the format button on the toolbar
		showFormatButton: true,

		// Whether or not to show the search Code button on the toolbar
		showSearchButton: true,

		// Whether or not to show Trailing Spaces
		showTrailingSpace: true,

		// Whether or not to show the uncomment button on the toolbar
		showUncommentButton: true,

		// Whether or not to highlight the currently active line
		styleActiveLine: true,

		// Set this to the theme you wish to use (codemirror themes)
		theme: 'material',

		// "Whether or not to use Beautify for auto formatting On start
		useBeautifyOnStart: false
	};
};