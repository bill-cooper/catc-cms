var mediaPlugins = ",|";

if (mediaPickerEnabled) {
    mediaPlugins += ",mediapicker";
}

if (mediaLibraryEnabled) {
    mediaPlugins += ",medialibrary";
}
tinyMCE.init({
    theme: "advanced",
    schema: "html5",
    mode: "specific_textareas",
    editor_selector: "tinymce",
    plugins: "fullscreen,autoresize,searchreplace,inlinepopups,pasteimage,table" + mediaPlugins.substr(2),
    theme_advanced_toolbar_location: "top",
    theme_advanced_toolbar_align: "left",
    theme_advanced_buttons1: "search,replace,|,cut,copy,paste,|,undo,redo" + mediaPlugins + ",|,link,unlink,charmap,emoticon,codeblock,|,bold,italic,|,numlist,bullist,formatselect,|,code,fullscreen,",
    theme_advanced_buttons2: "tablecontrols",
    theme_advanced_buttons3: "",
    table_styles : "Header 1=header1;Header 2=header2;Header 3=header3",
    table_cell_styles : "Header 1=header1;Header 2=header2;Header 3=header3;Table Cell=tableCel1",
    table_row_styles : "Header 1=header1;Header 2=header2;Header 3=header3;Table Row=tableRow1",
    table_cell_limit : 100,
    table_row_limit : 50,
    table_col_limit : 5,
    convert_urls: false,
    valid_elements: "+*[*]",
    // shouldn't be needed due to the valid_elements setting, but TinyMCE would strip script.src without it.
    extended_valid_elements: "script[type|defer|src|language]",
    browser_spellcheck: true,
    setup: function(ed) {
        ed.onKeyDown.add(function(ed, evt) {
            console.debug('Key up event: ' + evt.keyCode);
            if (evt.keyCode == 9) { // tab pressed
                ed.execCommand('mceInsertContent', false, '&emsp;&emsp;'); // inserts tab
                evt.preventDefault();
                evt.stopPropagation();
                return false;
            }
        });
    }
});
