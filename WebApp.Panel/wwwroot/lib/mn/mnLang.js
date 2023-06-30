
//modul Language 
window.mnLang = function () {
    var self = {};
    self.WidgetSelector = "";
    self.Dictionary = new Array();
    self.wordList = new Object();
    self.CurrentCulture = null;

    self.CultureList = [
        { enable: false, culture: "tr-TR", dispalyName: "Türkçe", direction: "", imgUrl: "/img/flag/tr-TR.png" },
        { enable: false, culture: "en-US", dispalyName: "English-US", direction: "", imgUrl: "/img/flag/en-GB.png" },
        { enable: false, culture: "en-GB", dispalyName: "English-GB", direction: "", imgUrl: "/img/flag/en-GB.png" },
        { enable: false, culture: "de-DE", dispalyName: "Deutsch", direction: "", imgUrl: "/img/flag/de-DE.png" },
        { enable: false, culture: "fr-FR", dispalyName: "Français", direction: "", imgUrl: "/img/flag/fr-FR.png" },
        { enable: false, culture: "ar-SA", dispalyName: "العربية", direction: "rtl", imgUrl: "/img/flag/ar-SA.png" }
    ];

    self.CreateWidget = function (_selector) {
        //style append
        $("head").append("<style> .imgLangFlag {cursor: pointer;width: 24px;height: 24px;margin-right: 6px;opacity: 0.3 !important;transition: all .35s ease-in-out;} </style>");
        $("head").append("<style> .imgLangFlag:hover {opacity: 1 !important; transform: scale(1.2);} </style>");
        $("head").append("<style> .imgLangFlag-active {opacity: 0.9 !important;} </style>");

        self.WidgetSelector = _selector;
        for (var itemIndex in self.CultureList) {
            var item = self.CultureList[itemIndex];
            if (item.enable) {
                $(self.WidgetSelector).append('<img class="imgLangFlag" data-culture="' + item.culture + '" data-direction="' + item.direction + '" title="' + item.dispalyName + '" src="' + item.imgUrl + '" />');
            }
        }

        //aktif dil widget e set ediliyr
        self.setWidgetCulture(self.CurrentCulture.culture);

        $(self.WidgetSelector).find(".imgLangFlag").click(function (e) {
            setCulture($(e.currentTarget).data("culture"));
        });
    };

    self.setWidgetCulture = function (_sCulture) {
        $(self.WidgetSelector).find(".imgLangFlag").removeClass("imgLangFlag-active");
        $(self.WidgetSelector).find("img[data-culture='" + _sCulture + "']").addClass("imgLangFlag-active");
    };

    //translate google single
    self.translateWithWeb = async function (_text, _source, _target) {
        var rV = "";

        const result = await fetch("https://translate.googleapis.com/translate_a/single?client=gtx&dt=t"
            + "?v." + window.myApp.version + "&sl=" + _source + "&tl=" + _target + "&q=" + _text
        ).then((response) => response.json());
        rV = result[0][0][0];

        return rV;
    };

    //fonksiyon Translate Kısa
    self.f = function (_sKey) {
        return self.TranslateWithWord(_sKey);
    };

    self.TranslateWithWord = function (_key) {
        var rValue = _key;
        if (self.wordList[_key]) {
            var sLang = self.CurrentCulture.culture.substring(0, 2);
            rValue = self.wordList[_key][sLang];
        }
        return rValue;
    };

    //using params.. mnLang.TranslateWithParams("xTest",["bir","iki","üç"]) --
    self.TranslateWithParams = function (_sKey, _params) {
        return mnApi.StringFormat(self.TranslateWithWord(_sKey), _params);
    };

    self.TranslateWithSelector = function (_selector) {
        $(_selector).find("[data-langkey-text]").each(function (index, element) {
            var lines = $(element).attr("data-langkey-text").split('|');
            var str = "";
            $.each(lines, function (key, line) {
                if (str.length > 0) { str += " - "; }
                str += self.TranslateWithWord(line);
            });
            $(element).text(str);
        });

        $(_selector).find("[data-langkey-html]").each(function (index, element) {
            var lines = $(element).attr("data-langkey-html").split('|');
            var str = "";
            $.each(lines, function (key, line) {
                if (str.length > 0) { str += " - "; }
                str += self.TranslateWithWord(line);
            });
            $(element).html(str);
        });

        $(_selector).find("[data-langkey-title]").each(function (index, element) {
            var lines = $(element).attr("data-langkey-title").split('|');
            var str = "";
            $.each(lines, function (key, line) {
                if (str.length > 0) { str += " - "; }
                str += self.TranslateWithWord(line);
            });
            $(element).attr("title", str);
        });

        $(_selector).find("[data-langkey-placeholder]").each(function (index, element) {
            var lines = $(element).attr("data-langkey-placeholder").split('|');
            var str = "";
            $.each(lines, function (key, line) {
                if (str.length > 0) { str += " - "; }
                str += self.TranslateWithWord(line);
            });
            $(element).attr("placeholder", str + "...");
        });

        $(_selector).find("[data-langkey-validationMessage]").each(function (index, element) {
            var lines = $(element).attr("data-langkey-validationMessage").split('|');
            var str = "";
            $.each(lines, function (key, line) {
                if (str.length > 0) { str += " - "; }
                str += self.TranslateWithWord(line);
            });
            $(element).attr("validationMessage", str);
        });


        //$(_selector).css("direction", self.CurrentCulture.Direction); //sistemde kendo kullanılmıyorsa bu satırı aç
        if (self.CurrentCulture.direction === "rtl") {
            $(_selector).addClass("k-rtl");
        } else {
            $(_selector).removeClass("k-rtl");
        }

    };

    function setActiveCulture(_culture) {
        for (var index in self.CultureList) {
            if (self.CultureList[index].culture === _culture) {
                self.CurrentCulture = self.CultureList[index];
            }
        }
    }

    function setCulture(_culture) {

        document.documentElement.setAttribute('lang', _culture);

        //current oluyor
        for (var index in self.CultureList) {
            if (self.CultureList[index].culture === _culture) {
                self.CurrentCulture = self.CultureList[index];
            }
        }

        //aktif dil widget e set ediliyr
        self.setWidgetCulture(self.CurrentCulture.culture);

        // Language
        self.TranslateWithSelector("body");
    }

    self.getCulturesLookupData = function () {
        var rCulturesData = [];
        for (var itemIndex in self.CultureList) {
            var item = self.CultureList[itemIndex];
            if (item.enable) {
                rCulturesData.push({ value: self.CultureList[itemIndex].culture, text: self.CultureList[itemIndex].dispalyName });
            }
        }
        return rCulturesData;
    };

    self.addDictionary = function (data) {
        data.dictionary.forEach(async function (row) {
            self.wordList[row.Key] = new Object();
            self.wordList[row.Key].tr = row.Value["Tr"];

            self.wordList[row.Key].en = row.Value["En"];

            //self.wordList[row.Key].fr = row.Value["Fr"];

            if (self.autoTranslate == true) {
                if (self.wordList[row.Key].en == "") {
                    self.wordList[row.Key].en = await self.translateWithWeb(self.wordList[row.Key].en, "tr", "en");
                }
                //if (self.wordList[row.Key].fr == "") {
                //    self.wordList[row.Key].fr = await self.translateWithWeb(self.wordList[row.Key].en, "tr", "fr");
                //}
            }
        });
    };

    function getDictionary(_culture) {
        $.ajax({
            url: "/Panel/Api/GetDictionary",
            type: "GET", dataType: "json",
            async: false,
            success: function (result, textStatus, jqXHR) {
                //enable/disable culterlist item
                for (var itemIndex in self.CultureList) {
                    var item = self.CultureList[itemIndex];
                    item.enable = (result.cultures.split(",").indexOf(item.culture) >= 0);
                }

                //set CurrentCulture
                setActiveCulture(_culture);

                self.addDictionary(result);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    self.prepare = function (_culture) {
        getDictionary(_culture);
    };

    return self;
}();