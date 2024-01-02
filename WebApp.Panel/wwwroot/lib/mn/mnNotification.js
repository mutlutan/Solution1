//Notification
window.mnNotification = function () {
    var self = {};
    self.widget = null; //k-animation-container

    self.prepare = function (_opt) {

        self.opt = $.extend({
            autoHideAfter: 2000,
            opacityInfo: 0.90,
            opacitySuccess: 0.90,
            opacityWarning: 0.90,
            opacityError: 0.90,
        }, _opt);

        $("head").append('<style> .k-notification {} </style>');

        $("head").append('<style> .k-notification.k-notification-info {opacity: ' + self.opt.opacityInfo + ' !important;} </style>');
        $("head").append('<style> .k-notification.k-notification-success {opacity: ' + self.opt.opacitySuccess + ' !important;} </style>');
        $("head").append('<style> .k-notification.k-notification-warning {opacity: ' + self.opt.opacityWarning + ' !important;} </style>');
        $("head").append('<style> .k-notification.k-notification-error {opacity: ' + self.opt.opacityError + ' !important;} </style>');

        $(document.body).append('<span id="notificationContainer" style="display:none;"></span>');

        //widget
        self.widget = $("#notificationContainer").kendoNotification({
            autoHideAfter: self.opt.autoHideAfter,
            stacking: "up",
            position: { bottom: 50, right: 50 },
            width: "35em",
            templates: [
                {
                    type: "info",
                    template: '<div style="padding:10px 20px; font-size:1.8em;"> <i class="bi bi-info-circle" style="vertical-align:middle;"></i> <span class="h6 text-break">#=msg#</span> </div>'
                },
                {
                    type: "success",
                    template: '<div style="padding:10px 20px; font-size:1.8em;"> <i class="bi bi-check-circle" style="vertical-align:middle;"></i> <span class="h6 text-break">#=msg#</span> </div>'
                },
                {
                    type: "warning",
                    template: '<div style="padding:10px 20px; font-size:1.8em;"> <i class="bi bi-exclamation-triangle" style="vertical-align:middle;"></i> <span class="h6 text-break">#=msg#</span> </div>'
                },
                {
                    type: "error",
                    template: '<div style="padding:10px 20px; font-size:1.8em;"> <i class="bi bi-bug" style="vertical-align:middle;"></i> <span class="h6 text-break">#=msg#</span> </div>'
                }
            ],
            show: function (e) {
                e.element.parent().css({ zIndex: 11001 });
                // center için
                //e.element.parent().css({ left: Math.floor($(window).width() / 2 - e.element.parent().width() / 2) });
            }
        }).getKendoNotification();
    };


    self.info = function (_Message) {
        self.widget.info({ msg: _Message });
    };

    self.success = function (_Message) {
        self.widget.success({ msg: _Message });
    };

    self.warning = function (_Message) {
        self.widget.warning({ msg: _Message });
    };

    self.error = function (_Message) {
        self.widget.error({ msg: _Message });
    };

    return self;
}();