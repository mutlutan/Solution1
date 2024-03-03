window.myLibs = function () {
    var self = {};

    self.stringFormat = function (sText, _params) {
        var args = _params;
        return sText.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== undefined
                ? args[number]
                : match
                ;
        });
    };

    self.generateQuerySelector = function (elm) {
        if (elm.tagName.toLowerCase() == "html")
            return "HTML";
        var str = elm.tagName;
        str += (elm.id != "") ? "#" + elm.id : "";
        if (elm.className) {
            var classes = elm.className.split(/\s/);
            for (var i = 0; i < classes.length; i++) {
                str += "." + classes[i]
            }
        }
        return self.generateQuerySelector(elm.parentNode) + " > " + str;
    };

    self.parseJWT = function (token) {
        let base64Url = token.split('.')[1];
        let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        let jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    };

    self.init = function () {
        return self;
    };

    return self.init();
}();