
tinymce.PluginManager.load('assetpicker', '/Modules/ceenq.com.Layouts/Scripts/tinymceplugins/assetpicker/plugin.js');

tinymce.init({
selector: "textarea.tinymce",
theme: "modern",
skin: 'orchardlightgray',
schema: "html5",
    plugins: [
            "assetpicker advlist autolink link lists charmap hr anchor pagebreak",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime",
            "table contextmenu directionality textcolor paste textcolor colorpicker textpattern"
    ],

    toolbar1: "searchreplace | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | formatselect fontselect fontsizeselect | forecolor backcolor | assetpicker",
    toolbar2: "cut copy paste | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor insertdatetime | table | hr removeformat | subscript superscript | charmap | visualblocks | code",

    menubar: false,
    toolbar_items_size: 'small',
    browser_spellcheck: true,
    convert_urls: false,
    valid_elements: "*[*]",
    // shouldn't be needed due to the valid_elements setting, but TinyMCE would strip script.src without it.
    extended_valid_elements: "script[type|defer|src|language]",
    directionality: directionality


});