//sweetalert2 required
window.mnAlert = function () {
    var self = {};
    
    self.alert = function (opt) {
        return Swal.fire(opt);
    };

    self.info = function (_content) {
        self.alert({ title: _content, icon: "info" });
    };

    self.success = function (_content) {
        self.alert({ title: _content, icon: "success" });
    };

    self.warning = function (_content) {
        self.alert({ title: _content, icon: "warning" });
    };

    self.error = function (_content) {
        self.alert({ title: _content, icon: "error" });
    };

    return self;
}();
