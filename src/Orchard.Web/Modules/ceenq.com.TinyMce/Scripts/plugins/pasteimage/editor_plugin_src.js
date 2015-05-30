(function () {


    tinymce.create('tinymce.plugins.Orchard.PasteImage', {

        init: function (ed, url) {

            ed.onPaste.add(function (ed, ev) {
                var file = ev.clipboardData.items[0].getAsFile();
                var reader = new FileReader();
                reader.onload = function (evt) {
                    var result = evt.target.result;
                    var arr = result.split(",");
                    var data = arr[1]; // raw base64
                    var contentType = arr[0].split(";")[0].split(":")[1];

                    // this needs to post to a server route that can accept raw base64 content and save to a file            
                    $.post("/echo/html/", {
                        contentType: contentType,
                        data: data
                    });
                    ed.execCommand('mceInsertContent', false, "<img src='" + result + "' />");
                };

                reader.readAsDataURL(file);
            });



        },


        createControl: function (n, cm) {
            return null;
        },

        getInfo: function () {
            return {
                longname: 'Orchard PasteImage Plugin',
                author: 'Ceenq Software Solutions',
                authorurl: 'http://www.ceenq.com',
                infourl: 'http://www.ceenq.com',
                version: '1.0'
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('pasteimage', tinymce.plugins.Orchard.PasteImage);
})();