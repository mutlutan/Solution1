
window.myApp = function () {
    var self = {};
    self.name = "Temp2022";
    self.version = window.getVersion(); //index.html en başında
    self.protocol = "https:";
    self.host = "localhost:44351";
    self.origin = self.protocol + "//" + self.host;

    self.init = async function () {
        try {
            await window.myLang.init("tr");
            await window.myUser.init();
        }
        catch (ex) {
            console.log(ex);
        }
        return self;
    };

    //jwt token göndermek için ayarlandı
    self.fetch = function (url, options) {
        if (options == undefined) {
            options = {};
        }
        if (options.headers == undefined) {
            options.headers = {};
        }
        options.headers.Authorization = 'Bearer ' + sessionStorage.getItem('token');
        return fetch(url, options);
    }

    return self;
}();


