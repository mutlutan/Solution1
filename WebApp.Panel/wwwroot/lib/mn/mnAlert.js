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
                + '    <i class="fa fa-info-circle fa-3x text-info" style="opacity:0.5; vertical-align: middle; "></i>'
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
                + '    <i class="fa fa-check-circle fa-2x text-success" style="opacity:0.5; vertical-align: middle;"></i>'
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
                + '    <i class="fa fa-exclamation-triangle fa-2x text-warning" style="opacity:0.5; vertical-align: middle;"></i> '
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
                + '    <i class="fa fa-bug fa-2x text-danger" style="opacity:0.5; vertical-align: middle;"></i>'
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
