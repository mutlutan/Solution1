window.myLang = function () {
    var self = {};
    self.language = "";
    self.autoTranslate = true;
    self.wordList = new Object();

    //translate google single
    self.translateWithWeb = async function (_text, _source, _target) {
        var rV = "";

        const result = await fetch("https://translate.googleapis.com/translate_a/single?client=gtx&dt=t"
            + "&sl=" + _source + "&tl=" + _target + "&q=" + _text
        ).then((response) => response.json());
        rV = result[0][0][0];

        return rV;
    };

    self.translate = function (_key) {
        var rValue = _key;
        if (self.wordList[_key]) {
            rValue = self.wordList[_key][self.language];
        }
        return rValue;
    };

    //using params.. myLanguage.translateWithParams("xTest",["bir","iki","üç"]) --
    self.translateWithParams = function (_sKey, _params) {
        return mnLibs.stringFormat(self.translateWithWord(_sKey), _params);
    };

    self.translateWithContainer = function (_container) {
        //innerText
        _container.querySelectorAll("[data-langkey-text]").forEach(function (elm, i) {
            var str = "";
            elm.getAttribute("data-langkey-text").split(',').forEach(function (line) {
                if (str.length > 0) { str += " - "; }
                str += self.translate(line);
            });
            elm.innerText = str;
        });
        //innerHtml
        _container.querySelectorAll("[data-langkey-html]").forEach(function (elm, i) {
            var str = "";
            elm.getAttribute("data-langkey-html").split(',').forEach(function (line) {
                if (str.length > 0) { str += " - "; }
                str += self.translate(line);
            });
            elm.innerHtml = str;
        });
    };

    self.addDictionary = function (data) {
        data.forEach(async function (row) {
            self.wordList[row.key] = new Object();
            //burası propertilere bakıp kaç dil gelir ise ona göre dinamik doldurabilir, belki yaparsın
            self.wordList[row.key].tr = row.value["tr"];
            self.wordList[row.key].en = row.value["en"];
            //self.wordList[row.key].fr = row.value["fr"];
            //self.wordList[row.key].de = row.value["de"];

            if (self.autoTranslate == true) {
                if (self.wordList[row.key].en == "") {
                    self.wordList[row.key].en = await self.translateWithWeb(self.wordList[row.key].en, "tr", "en");
                }
            }
        });
    };

    self.init = async function (_language) {
        self.language = _language;
        return self;
    };

    return self;
}();