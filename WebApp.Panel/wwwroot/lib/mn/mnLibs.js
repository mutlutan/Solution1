
// String.Format() (IFormatProvider for .NET).
//"{0} is dead, but {1} is alive! {0} {2}".format("ASP", "ASP.NET")
//String.prototype.mnStringFormat = function () {
//    var args = arguments;
//    return this.replace(/{(\d+)}/g, function (match, number) {
//        return typeof args[number] !== undefined
//            ? args[number]
//            : match
//            ;
//    });
//};

// ilk char upper ediliyor
//String.prototype.mnToTitleCase = function () {
//    return this.replace(/^./, function (match) {
//        return match.toUpperCase();
//    });
//};

//Error handler
window.mnErrorHandler = function () {
    var self = {};

    self.Handle = function (xhr) {
        if (xhr.status === 400) {
            alert("(" + xhr.status + ") " + xhr.responseText);
        }
        else if (xhr.status === 401) {
            alert("(" + xhr.status + ") " + xhr.responseText);
            setTimeout(function () {
                window.location.replace("/");
            }, 1000);
        }
        else if (xhr.status === 0) {
            alert(mnLang.TranslateWithWord('xLng.IstekBasarisizOldu'));
        }
        else {
            alert(xhr.status + " " + xhr.statusText);
        }
    };

    return self;
}();

//Convert bin,dec,hex
window.mnConvertBase = function () {

    var ConvertBase = function (num) {
        return {
            from: function (baseFrom) {
                return {
                    to: function (baseTo) {
                        return parseInt(num, baseFrom).toString(baseTo);
                    }
                };
            }
        };
    };

    // binary to decimal
    ConvertBase.bin2dec = function (num) {
        return ConvertBase(num).from(2).to(10);
    };

    // binary to hexadecimal
    ConvertBase.bin2hex = function (num) {
        return ConvertBase(num).from(2).to(16);
    };

    // decimal to binary
    ConvertBase.dec2bin = function (num) {
        return ConvertBase(num).from(10).to(2);
    };

    // decimal to hexadecimal
    ConvertBase.dec2hex = function (num) {
        return ConvertBase(num).from(10).to(16);
    };

    // hexadecimal to binary
    ConvertBase.hex2bin = function (num) {
        return ConvertBase(num).from(16).to(2);
    };

    // hexadecimal to decimal
    ConvertBase.hex2dec = function (num) {
        return ConvertBase(num).from(16).to(10);
    };

    return ConvertBase;
}(); //useing mnConvertBase.bin2dec('111'); // '7'

//mnApi
window.mnApi = function () {
    var self = {};

    //Sleep
    self.sleep = function (_milliseconds) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++) {
            if (new Date().getTime() - start > _milliseconds) {
                break;
            }
        }
    };

    self.StringFormat = function (sText, _params) {
        var args = _params;
        return sText.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== undefined
                ? args[number]
                : match
                ;
        });
    };

    self.Trim = function (text) {
        var rV = "";
        if (text !== null) {
            text.replace(/^\s+|\s+$/g, '');
        }
        return rV;
    };

    self.ReplaceAll = function (text, search, replacement) {
        return text.split(search).join(replacement);
    };

    self.TurkceKarekterDegistir = function (_str) {
        try {
            var letters = { "İ": "I", "Ş": "S", "Ğ": "G", "Ü": "U", "Ö": "O", "Ç": "C", "Â": "A", "ı": "i", "ş": "s", "ğ": "g", "ü": "u", "ö": "o", "ç": "c", "â": "a" };
            if (_str !== null) {
                _str = _str.replace(/(([İŞĞÜÇÖÂışğüçöâ]))/g, function (letter) {
                    return letters[letter];
                });
            }
        }
        catch (ex) {
            var hata = ex;
        }

        return _str;
    };

    self.parseQueryString = function () {
        let result = [];
        const params = new URLSearchParams(window.location.href.split('?')[0]);
        for (const [key, value] of params) {
            result[key]=value;
        }
        return result;
    };

    self.GetURLParamByName = function (_sParamName) {
        _sParamName = _sParamName.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + _sParamName + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    };

    self.StrToHex = function (str) {
        var arr = [];
        for (var i = 0, l = str.length; i < l; i++) {
            var hex = Number(str.charCodeAt(i)).toString(16);
            arr.push(hex);
        }
        return arr.join(''); ////a2hex('2460'); //returns 32343630
    };

    self.HexToStr = function (hexx) {
        var hex = hexx.toString();//force conversion
        var str = '';
        for (var i = 0; i < hex.length; i += 2)
            str += String.fromCharCode(parseInt(hex.substr(i, 2), 16));
        return str; ////////hex2a('32343630'); // returns '2460'
    };

    //json date to Date
    self.JsonDateToDate = function (_s) {
        try {
            if (_s !== null) {
                _s = _s.replace('/Date(', '').replace(')/', '');
                return new Date(parseInt(_s));
            } else {
                return "";
            }
        }
        catch (err) {
            return "";
        }
    };

    // Control enable / disable
    self.controlEnable = function (_elm, _b) {
        if (_b) {
            _elm.removeAttr("disabled").css("pointer-events", "").removeClass("disabled");

        } else {
            _elm.attr("disabled", "disabled").css("pointer-events", "none").addClass("disabled");
        }
    };

    self.controlDisableWait = function (_elm) {
        self.controlEnable(_elm, false);
        setTimeout(function () { self.controlEnable(_elm, true); }, 500);
    };

    // Control show/hide
    self.controlShowHide = function (_elm, _b) {
        if (_b) {
            _elm.show();
        } else {
            _elm.hide();
        }
    };

    self.LocalDateTime = function () {
        var now = new Date();
        return now;
    }

    self.LocalDate = function () {
        var now = new Date();
        return new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
    }

    self.LocalTime = function () {
        var now = new Date();
        return new Date(0, 0, 0, now.getHours(), now.getMinutes(), now.getSeconds(), 0);
    }

    self.requestFullscreen = function (_elm) {
        if (_elm.requestFullscreen) {
            _elm.requestFullscreen();
        } else if (_elm.mozRequestFullScreen) { /* Firefox */
            _elm.mozRequestFullScreen();
        } else if (_elm.webkitRequestFullscreen) { /* Chrome, Safari & Opera */
            _elm.webkitRequestFullscreen();
        } else if (_elm.msRequestFullscreen) { /* IE/Edge */
            _elm.msRequestFullscreen();
        }
    };

    self.exitFullscreen = function () {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    };

    self.convertToRtf = function (plain) {
        plain = plain.replace(/\n/g, "\\par\n");
        return "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft Sans Serif;}}\n\\viewkind4\\uc1\\pard\\f0\\fs17 " + plain + "\\par\n}";
    };

    self.convertToPlain = function (rtf) {
        rtf = rtf.replace(/\\par[d]?/g, "");
        return rtf.replace(/\{\*?\\[^{}]+}|[{}]|\\\n?[A-Za-z]+\n?(?:-?\d+)?[ ]?/g, "").trim();
    };

    self.inFrame = function () {
        //window.frameElement ? 'embedded in iframe or object' : 'not embedded or cross-origin'
        try {
            return window.self !== window.top;
        } catch (e) {
            return true;
        }
    };

    self.validateEmail = function (email) {
        var re = /\S+@\S+\.\S+/;
        return re.test(email);
    };


    return self;
}();

//mnApp.areas
window.mnApp = function () {
    var self = {};

    self.appName = "";
    self.version = ""; //App version
    self.hostAddress = "";
    self.areas = [];

    //kendo
    self.validatorErrorTemplateIconMsg = '<span class="text-warning col-form-label" style="position:absolute; right:4px; top:0px;" title="#=message#"> <i class="fa fa-exclamation"></i> </span>';
    self.validatorErrorTemplateTextMsg = '<span class="text-warning" style="font-size: 0.8em;">#=message#</span>';
    self.validatorErrorTemplateNoMsg = '<span class="text-warning"></span>';

    //kendo grid pageSizes
    self.gridPageSizes_mini = [1, 5, 10];
    self.gridPageSizes_small = [5, 10, 25, 50];
    self.gridPageSizes_default = [10, 50, 100, 250, 500, 1000, 2000];
    self.gridPageSizes_large = [10, 100, 250, 500, 1000, 2500];
    self.gridPageSizes_xlarge = [10, 500, 1000, 2000, 3000, 4000, 5000, 10000];

    //kendo date time piker ComponentType
    self.kendoDatePiker_ComponentType = 'classic'; //modern,classic
    self.kendoTimePiker_ComponentType = 'classic'; //modern,classic
    self.kendoDateTimePiker_ComponentType = 'classic'; //modern,classic

    //kendo date time piker  dateInput
    self.kendoDatePiker_DateInput = true;
    self.kendoTimePiker_DateInput = true; //modern,classic
    self.kendoDateTimePiker_DateInput = true; //modern,classic

    //kendo Editor default
    self.kendoEditor_tools_mini = ['bold', 'italic', 'underline', 'strikethrough', 'justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'insertUnorderedList', 'insertOrderedList', 'createTable', 'createLink', 'insertImage', 'viewHtml', 'formatting', 'cleanFormatting', 'print'];
    self.kendoEditor_tools_default = ['bold', 'italic', 'underline', 'strikethrough', 'justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'insertUnorderedList', 'insertOrderedList', 'indent', 'outdent', 'createLink', 'unlink', 'insertImage', 'insertFile', 'subscript', 'superscript', 'tableWizard', 'createTable', 'addRowAbove', 'addRowBelow', 'addColumnLeft', 'addColumnRight', 'deleteRow', 'deleteColumn', 'viewHtml', 'formatting', 'cleanFormatting', 'fontName', 'fontSize', 'foreColor', 'backColor', 'print'];

    self.createValidator = function (_form, _errorTemplate) {
        //validator
        var validator = _form.kendoValidator({
            errorTemplate: _errorTemplate,
            messages: {
                required: mnLang.TranslateWithWord('xLng.LutfenBuAlaniDoldurunuz'),
                validmask: mnLang.TranslateWithWord('xLng.Gecersiz'),
                email: mnLang.TranslateWithWord('xLng.LutfenGecerliBirMailAdresGiriniz'),
                datepicker: "Lütfen geçerli bir tarih giriniz!"
            },
            validate: function (e) {
                //genel olarak validate çağrıldığında
                if (window.location.host.startsWith('localhost')) {
                    //console.log('validate :' + e.valid, e);
                }
            },
            validateInput: function (e) {
                //Her eleman için validate edilince, console.log('validateInput ' + e.input.attr('name') + ' changed to valid: ' + e.valid);
                var $elm = $(e.input);

                if ($elm.attr('type') === 'hidden') {
                    $elm = $elm.closest('.k-textbox');
                } else if ($elm.closest('.k-textbox').hasClass('mnSearchEdit')) {
                    $elm = $elm.closest('.k-textbox');
                } else if ($elm.closest('.k-widget')) {
                    $elm = $elm.closest('.k-widget');
                } else if ($elm.hasClass('k-textbox')) {
                    $elm = $(e.input);
                }

                if (e.valid) {
                    $elm.removeClass('mn-invalid').addClass('mn-valid');
                } else {
                    $elm.addClass('mn-invalid').removeClass('mn-valid');
                }
            },
            rules: {
                validmask: function (input) {
                    if (input.is("[data-role='maskedtextbox']") && input.val() != "") {
                        var maskedtextbox = input.data("kendoMaskedTextBox");
                        return maskedtextbox.value().indexOf(maskedtextbox.options.promptChar) === -1;
                    }
                    return true;
                },
                custom: function (input) {
                    //Get the MultiSelect instance
                    var ms = input.data('kendoMultiSelect');
                    if (ms !== undefined) {
                        if (input.is('[min]') && ms.value().length < $(input).attr('min')) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }
                },
                datepicker: function (input) {
                    if (input.is("[data-role=datetimepicker]") && input.is('[required]')) {
                        return input.data("kendoDateTimePicker").value();
                    } else {
                        return true;
                    }
                }
            }
        }).getKendoValidator();

        return validator;
    };

    self.find_options_ToFilterList = function (_modul) {
        var filterList = { logic: 'and', filters: [] };

        $(_modul.opt.filters).each(function (index, item) {
            filterList.filters.push({ field: item.filterColumnName, operator: item.filterOperator, value: item.filterValue });
        });

        //data-find_option = 'auto' olarlar burada
        $(_modul.selector).find('[data-find_option=auto]').each(function (index, element) {
            var find_field = $(element).attr('data-find_field');
            var find_operator = $(element).attr('data-find_operator');
            var find_type = $(element).attr('data-find_type');
            var kendoWidgetInstance = kendo.widgetInstance($(element));

            var field_value = null;
            if (kendoWidgetInstance) {
                field_value = kendoWidgetInstance.value();
            } else {
                if ($(element).attr('type') == 'radio') {
                    if ($(element).is(':checked')) {
                        field_value = $(element).val();
                    }
                } else {
                    field_value = $(element).val();
                }
            }

            if (field_value != null) {
                if (find_type == 'System.String') {
                    if (field_value.length > 0) {
                        filterList.filters.push({ field: find_field, operator: find_operator, value: field_value });
                    }
                }
                else if (find_type == 'System.Int32') {
                    if (field_value.toString().length > 0) {
                        filterList.filters.push({ field: find_field, operator: find_operator, value: parseInt(field_value) });
                    }
                }
                else if (find_type == 'System.Decimal') {
                    if (field_value.toString().length > 0) {
                        filterList.filters.push({ field: find_field, operator: find_operator, value: parseFloat(field_value) });
                    }
                }
                else if (find_type == 'System.Boolean') {
                    if (field_value.length > 0) {
                        filterList.filters.push({ field: find_field, operator: find_operator, value: (field_value == 'true') });
                    }
                }
                else if (find_type == 'System.TimeSpan') {
                    filterList.filters.push({ field: find_field, operator: find_operator, value: field_value });
                }
                else if (find_type == 'System.DateTime') {
                    filterList.filters.push({ field: find_field, operator: find_operator, value: field_value });
                }
                else if (find_type == 'StringArray') {
                    //(multiselect için burası )
                    for (var i in field_value) {
                        var sValue = field_value[i];
                        if (sValue.toString().length > 0) {
                            filterList.filters.push({ field: find_field, operator: find_operator, value: sValue });
                        }
                    }
                }
            }
        });

        if (filterList.filters.length == 0) {
            filterList = {};
        }

        return filterList;
    };

    self.setKendoDefaults = function (_culture) {
        if (kendo) {
            kendo.culture(_culture);
            /* kendo Validator messages */
            if (kendo.ui.Validator) {
                kendo.ui.Validator.prototype.options.messages =
                    $.extend(true, kendo.ui.Validator.prototype.options.messages, {
                        "required": "{0} " + mnLang.TranslateWithWord('xLng.Gerekli'),
                        "pattern": "{0} " + mnLang.TranslateWithWord('xLng.Gecersiz'),
                        "min": "{0} should be greater than or equal to {1}",
                        "max": "{0} should be smaller than or equal to {1}",
                        "step": "{0} is not valid",
                        "email": "{0} is not valid email",
                        "url": "{0} is not valid URL",
                        "date": "{0} is not valid date",
                        "dateCompare": "End date should be greater than or equal to the start date"
                    });
            }
        }
    };

    self.prepare = function () {
        getAppInfo();
        registerEditorImageUrlSelector();
    };

    function getAppInfo() {
        $.ajax({
            url: "/Panel/Api/GetAppInfo",
            type: "GET", dataType: "json",
            async: false,
            success: function (result, textStatus, jqXHR) {
                self.appName = result.appName;
                self.version = result.version;
                self.hostAddress = result.hostAddress;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    function registerEditorImageUrlSelector() {

        var style = `                                                                                     
            <style>                                                                                       
                .k-editor-dialog .k-edit-label label[for=k-editor-image-url]:after {
                    content: " ▶";
                }                                                                                         
            </style>
        `;

        $(document.head).append(style);

        $("body").on("click", ".k-editor-dialog .k-edit-label label[for=k-editor-image-url]", function (evnt) {
            $elm = $(evnt.currentTarget).closest(".k-editor-dialog").find("#k-editor-image-url");

            mnPopupView.create({
                viewFolder: '_Dosyalar',
                viewName: 'viewDosyalar',
                subTitle: mnLang.TranslateWithWord('xLng.AramaIslemleri'),
                onShow: function (e) {
                    e.beforeShow({ 'ownerViewName': 'Bos' });
                },
                onClose: function (e) {
                    if (e.opt.isSelected) {
                        $elm.val(self.hostAddress + e.opt.selectedDataItem.FileUrl);
                    }
                }
            });
        });
    }

    self.exportGridWithTemplatesContentForKendo = function (e) {
        var data = e.data;
        var gridColumns = e.sender.columns;
        var sheet = e.workbook.sheets[0];
        var visibleGridColumns = [];
        var columnTemplates = [];
        var dataItem;
        // Create element to generate templates in.
        var elem = document.createElement('div');

        // Get a list of visible columns
        for (var i = 0; i < gridColumns.length; i++) {
            if (!gridColumns[i].hidden && !gridColumns[i].command) {
                visibleGridColumns.push(gridColumns[i]);
            }
        }

        // Create a collection of the column templates, together with the current column index
        for (var i = 0; i < visibleGridColumns.length; i++) {
            if (visibleGridColumns[i].template) {
                columnTemplates.push({ cellIndex: i, template: kendo.template(visibleGridColumns[i].template) });
            }
        }

        // Traverse all exported rows.
        for (var i = 1; i < sheet.rows.length; i++) {
            var row = sheet.rows[i];
            // Traverse the column templates and apply them for each row at the stored column position.

            // Get the data item corresponding to the current row.
            var dataItem = data[i - 1];
            for (var j = 0; j < columnTemplates.length; j++) {
                var columnTemplate = columnTemplates[j];
                // Generate the template content for the current cell.
                elem.innerHTML = columnTemplate.template(dataItem);
                if (row.cells[columnTemplate.cellIndex] != undefined)
                    // Output the text content of the templated cell into the exported cell.
                    row.cells[columnTemplate.cellIndex].value = elem.textContent || elem.innerText || "";
            }
        }

        kendo.ui.progress(e.sender.wrapper, false); //progress off
    };

    self.globalQueryFilterSet = function () {
        const params = new URLSearchParams(window.location.href.split('?')[1]);



        for (const [key, value] of params) {
            console.log(key, value);
            var element = $('.mnFindPanel:visible').find('[data-find_field=' + key + ']');
            if (element.attr('data-role') == 'dropdownlist' && element.attr('data-find_type') == 'System.Int32') {



                kendo.widgetInstance(element).value(value);
            }
            else {
                element.val(value);
            }
        }
        if (params.size > 0) {
            $('.mnFindPanel:visible').find('#btnFlitreUygula').click();
        }
    };

    return self;
}();

//mnUser 
window.mnUser = function () {
    var self = {};
    self.logoutForce = false;
    //User Bilgileri
    self.Info = null;

    self.setInfo = function (_info) {
        self.Info = $.extend({
            IsAuthenticated: false,
            IsAdmin: false,
            Culture: "tr-TR",
            UserId: 0,
/*            UserPhoto: "",*/
            UserName: "",
            NameSurname: "",
            nYetkiGrup: 0,
            UserAuthorities: ""
        }, _info);
    };

    function fnGetUserInfo() {
        $.ajax({
            url: "/Panel/Api/GetUserInfo",
            type: "GET", dataType: "json",
            async: false,
            success: function (result) {
                self.setInfo(result);
            },
            error: function (xhr, status) {
                //mnErrorHandler.Handle(xhr);
            }
        });
    }

    function fnRefreshTimeout() {
        $.ajax({
            url: "/Panel/Api/RefreshTimeOut",
            type: "GET", dataType: "json",
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status === 401) {
                    alert("(" + jqXHR.status + ") " + jqXHR.responseText);
                    setTimeout(function () {
                        window.location.replace("/");
                    }, 250);
                }
            }
        });
    }


    self.prepare = function () {
        self.setInfo(null);
        fnGetUserInfo();
    };

    self.refresh = function () {
        fnGetUserInfo();
    };

    // yetkiyi kontrol ederken kullanılacak fonksiyon
    self.isYetkili = function (_sYetkiKey) {
        //admin vey a yönetici değil ise normal user ...
        if (self.Info.IsAdmin) {
            return true;
        } else {
            var yetkiler = self.Info.UserAuthorities.split(',').filter(function (word) {
                return word.indexOf(_sYetkiKey) > -1;
            });

            return yetkiler.length > 0;
        }
    };

    //logout
    self.logout = function () {
        document.cookie = "Authorization=" + "logout" + ";path=/";
        window.location = "/";
    };

    self.idleTimeout = function (ms) {
        //server session time out olmasın diye
        setInterval(function () {
            fnRefreshTimeout();
        }, ms);
    };

    self.idleLogout = function (ms) {
        // screensaver gibi belli bir zamanda logine düşer
        var interval = 1000; // nekadar sürede bir eksiltileceği
        var sayac = ms;

        var timer = setInterval(function () {
            sayac -= interval;
            if (sayac < 0) {
                console.log("idleLogout : mnUser => logout");
                mnUser.logout(true);

            }
        }, interval);

        function reset() {
            sayac = ms;
        }

        var events = ['mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart'];

        events.forEach(function (name) {
            document.addEventListener(name, reset, true);
        });
    };

    return self;
}();

