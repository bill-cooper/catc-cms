(function () {
    tinymce.create('tinymce.plugins.ceenq.com.assetpicker', {
        /**
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function (ed, url) {
            // Register the command so that it can be invoked by using tinyMCE.activeEditor.execCommand('mceAssetPicker');
            ed.addCommand('mceAssetPicker', function () {
                ed.focus();
                var url = "/assetpicker.html";
                var height = $('html').height();
                $.colorbox({
                    href: url,
                    iframe: true,
                    reposition: true,
                    width: "90%",
                    height: "800px",
                    top: "100px",
                    onLoad: function () {
                        // hide the scrollbars from the main window
                        $('html, body').css('overflow', 'hidden');
                        $('html').css('height', height + 'px');
                    },
                    onClosed: function () {
                        $('html, body').css('overflow', '');

                        var selectedData = $.colorbox.selectedData;

                        if (selectedData == null) // Dialog cancelled, do nothing
                            return;

                        var newContent = '<img asset-ref="' + selectedData.id + '" src="/api/files/' + selectedData.id + '">';

                        // reassign the src to force a refresh
                        tinyMCE.execCommand('mceReplaceContent', false, newContent);
                    }
                });
            });

            ed.addButton('assetpicker', {
                title: 'Insert Asset',
                cmd: 'mceAssetPicker',
                image: url + '/img/picture_add.png'
            });
        },

        /**
        * Creates control instances based in the incomming name. This method is normally not
        * needed since the addButton method of the tinymce.Editor class is a more easy way of adding buttons
        * but you sometimes need to create more complex controls like listboxes, split buttons etc then this
        * method can be used to create those.
        *
        * @param {String} n Name of the control to create.
        * @param {tinymce.ControlManager} cm Control manager to use inorder to create new control.
        * @return {tinymce.ui.Control} New control instance or null if no control was created.
        */
        createControl: function (n, cm) {
            return null;
        },

        /**
        * Returns information about the plugin as a name/value array.
        * The current keys are longname, author, authorurl, infourl and version.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                author: 'cnq.io',
                authorurl: 'http://www.cnq.io',
                version: '1.0'
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('assetpicker', tinymce.plugins.ceenq.com.assetpicker);
})();