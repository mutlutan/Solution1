//Dialog
window.mnAlert = function () {
    var self = {};
    self.widget = null;

    self.prepare = function () {
    };

    //aşagıdakileri kendo dialog ile hazırlayabilirsin

    self.info = function (_content) {
        $('<div><div>').appendTo('body').kendoDialog({
            maxWidth: 500,
            buttonLayout: 'normal',
            title: mnLang.TranslateWithWord('xLng.Mesaj'),
            messages: { close: '' },
            content: '<div class="" style="padding:0px 30px 0px 15px;">'
                + '    <i class="bi bi-info-circle text-info" style="opacity:0.5; vertical-align: middle; font-size: 1.5em;"></i>'
                   + '    <span class="text-secondary pl-1" >' + _content + '</span>'
                   + '</div>',
            actions: [{
                text: mnLang.TranslateWithWord('xLng.Tamam')
            }],
            close: function (e) {
                this.destroy();
            }
        });
    };

    self.success = function (_content) {
        $('<div><div>').appendTo('body').kendoDialog({
            maxWidth: 500,
            buttonLayout: 'normal',
            title: mnLang.TranslateWithWord('xLng.Mesaj'),
            messages: { close: '' },
            content: '<div class="" style="padding:0px 30px 0px 15px;">'
                + '    <i class="bi bi-check-circle text-success" style="opacity:0.5; vertical-align: middle; font-size: 1.5em;"></i>'
                + '    <span class="text-secondary pl-1" >' + _content + '</span>'
                   + '</div>',
            actions: [{
                text: mnLang.TranslateWithWord('xLng.Tamam')
            }],
            close: function (e) {
                this.destroy();
            }
        });
    };

    self.warning = function (_content) {
        $('<div><div>').appendTo('body').kendoDialog({
            maxWidth: 500,
            buttonLayout: 'normal',
            title: mnLang.TranslateWithWord('xLng.Mesaj'),
            messages: { close: '' },
            content: '<div class="" style="padding:0px 30px 0px 15px;" >'
                + '    <i class="bi bi-exclamation-triangle text-warning" style="opacity:0.5; vertical-align: middle; font-size: 1.5em;"></i> '
                + '    <span class="text-secondary pl-1" >' + _content + '</span>'
                    + '</div>',
            actions: [{
                text: mnLang.TranslateWithWord('xLng.Tamam')
            }],
            close: function (e) {
                this.destroy();
            }
        });
    };

    self.error = function (_content) {
        $('<div><div>').appendTo('body').kendoDialog({
            maxWidth: 500,
            buttonLayout: 'normal',
            title: mnLang.TranslateWithWord('xLng.Mesaj'),
            messages: { close: '' },
            content: '<div class="" style="padding:0px 30px 0px 15px;" >'
                + '    <i class="bi bi-bug text-danger" style="opacity:0.5; vertical-align: middle; font-size: 1.5em;"></i>'
                + '    <span class="text-secondary pl-1" >' + _content + '</span>'
                   + '</div>',
            actions: [{
                text: mnLang.TranslateWithWord('xLng.Tamam')
            }],
            close: function (e) {
                this.destroy();
            }
        });
    };

    return self;
}();
