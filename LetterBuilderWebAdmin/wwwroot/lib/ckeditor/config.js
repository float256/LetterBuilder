/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

function getRequestVerificationToken() {
	return $('[name=__RequestVerificationToken]')[0].value
}

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

	config.removeButtons = 'Underline,JustifyCenter,Form,Checkbox,Radio,TextField,Textarea,Select,ImageButton,Button,HiddenField,Language,Flash';
	config.language = 'ru';
	config.extraPlugins = 'textindent,uploadimage';
	config.uploadUrl = '/Admin/Picture/Upload';
	config.fileTools_requestHeaders = {
		'RequestVerificationToken': getRequestVerificationToken()
    }
};
