
//Genel Lookup Data 
window.mnLookup = function () {
    var self = {};

    self.list = null;

    self.listLoad = function (_opt) {

        var opt = $.extend({
            ViewName: "", //hangi view içinden çağrılıyor, bu durum loopup cache ayrımı içindir
            ViewFieldName: "",//hangi view içinden hangi filed için çağrılıyor, bu durum loopup cache ayrımı içindir
            TableName: "",
            ValueField: "Id",
            TextField: "Ad",
            OtherFields: "", // Masterid vb. alanlar için sorgulama yapılabilmesi için isteniyor
            Filters: [
                { Field: "Id" },
                { Operator: ">" },
                { Value: "0" },
                { ValueType: "Int" } //Int,Decimal,Float, String,DateTime
            ],
            Sorts: [
                { Field: "Id", Dir: "asc" }
            ],
            Reload: false
        }, _opt);

        //_dsName de replacelerde de uyumsuz olabilecek karekterlerin tamamını yoketmelisin
        //opt.TableName en başta olacak, refresh yapılıyor
        var _dsName = opt.TableName + opt.ViewName + opt.ViewFieldName + opt.TextField;

        for (var i in opt.Filters) {
            _dsName += opt.Filters[i].Field + mnApi.StrToHex(opt.Filters[i].Operator) + opt.Filters[i].Value;
        }

        for (var i in opt.Sorts) {
            _dsName += opt.Sorts[i].Dir + opt.Sorts[i].Field;
        }

        _dsName += opt.OtherFields;

        //replace ile A-Za-z hariç herşeyin kaldırılması gerçekleşiyor
        _dsName = _dsName.replace(/\W/g, "");
        
        //---------------------------------------------------------------------------------

        if (self.list.get(_dsName) !== undefined) {
            if (opt.Reload) {
                self.list.get(_dsName).read();
            }
        } else {
            
            //delete opt.ViewName;
            //delete opt.ViewFieldName;
            //delete opt.Reload;

            var dsTemp = new kendo.data.DataSource({
                transport: {
                    read: {
                        type: "POST", dataType: "json",
                        url: "/Lookup/Read",
                        data: opt
                    }
                },
                schema: {
                    errors: 'Errors', data: 'Data', total: 'Total', aggregates: 'Aggregates',
                },
                error: function (e) {
                    if (e.xhr === null) { alert(e.errors); } else { mnErrorHandler.Handle(e.xhr); }
                }
            });

            if (self.list.get(_dsName) !== undefined) {
                self.list = dsTemp;
            } else {
                self.list.set(_dsName, dsTemp);
            }
        }
        return self.list.get(_dsName);
    };

    //tablo ya göre data refresh için
    self.listRead = function (_tableName) {
        for (var item in self.list) {
            try {
                if (item.substring(0, _tableName.length) === _tableName) {
                    self.list.get(item).read();
                }
            }
            catch (err) {
                //
            }
        }
    };

    self.prepare = function () {

        if (self.list === null) {
            self.list = kendo.observable({
                //kullanımı => mnLookup.Data.dsEvetHayir şeklinde
                //Statik Data bu statik olanlar direk isimleri ile çağrılacak
                dsEvetHayir: [{ value: 'false', text: mnLang.TranslateWithWord("xLng.Hayir") }, { value: 'true', text: mnLang.TranslateWithWord("xLng.Evet") }],
                dsAktifPasif: [{ value: 'false', text: mnLang.TranslateWithWord("xLng.Pasif") }, { value: 'true', text: mnLang.TranslateWithWord("xLng.Aktif") }],
                dsDogruYanlis: [{ value: 'false', text: mnLang.TranslateWithWord("xLng.Yanlis") }, { value: 'true', text: mnLang.TranslateWithWord("xLng.Dogru") }],
                //cultures
                dsCultures: mnLang.getCulturesLookupData(),

                //OperationType
                dsCRUD: [
                    { value: "C", text: mnLang.TranslateWithWord("xLng.Create") },
                    { value: "R", text: mnLang.TranslateWithWord("xLng.Read") },
                    { value: "U", text: mnLang.TranslateWithWord("xLng.Update") },
                    { value: "D", text: mnLang.TranslateWithWord("xLng.Delete") }
                ],

                dsZamanTur: [
                    { value: 0, text: mnLang.TranslateWithWord("xLng.Gunluk") },
                    { value: 1, text: mnLang.TranslateWithWord("xLng.Haftalik") },
                    { value: 2, text: mnLang.TranslateWithWord("xLng.Aylik") }
                ],

                //
                dsHaftaninGunu: [
                    { value: "Monday", text: mnLang.TranslateWithWord("xLng.Gun.Monday") },
                    { value: "Tuesday", text: mnLang.TranslateWithWord("xLng.Gun.Tuesday") },
                    { value: "Wednesday", text: mnLang.TranslateWithWord("xLng.Gun.Wednesday") },
                    { value: "Thursday", text: mnLang.TranslateWithWord("xLng.Gun.Thursday") },
                    { value: "Friday", text: mnLang.TranslateWithWord("xLng.Gun.Friday") },
                    { value: "Saturday", text: mnLang.TranslateWithWord("xLng.Gun.Saturday") },
                    { value: "Sunday", text: mnLang.TranslateWithWord("xLng.Gun.Sunday") }
                ],

                //sunucudan alınması gerekebilir (log tabloları seçimi için)
                dsTables: [
                    { value: "User", text: "Kullanıcılar" },
                    { value: "Role", text: "Roller" },
                    { value: "EmailLetterhead", text: "MailAntet" },
                    { value: "EmailTemplate", text: "MailSablon" },
                    { value: "Parameter", text: "Parametreler" },
                ],

                // Kan grupları
                dsKanGrup: [
                    { value: "", text: "" },
                    { value: "AB Rh+", text: "AB Rh+" },
                    { value: "AB Rh-", text: "AB Rh-" },
                    { value: "A Rh+ ", text: "A Rh+ " },
                    { value: "A Rh- ", text: "A Rh- " },
                    { value: "B Rh+ ", text: "B Rh+ " },
                    { value: "B Rh- ", text: "B Rh- " },
                    { value: "O Rh+ ", text: "O Rh+ " },
                    { value: "O Rh- ", text: "O Rh- " }
                ],


                // number array
                dsSayi_1_3: [
                    { value: "1", text: "1" },
                    { value: "2", text: "2" },
                    { value: "3", text: "3" }
                ],

                dsSayi_0_9: [
                    { value: "0", text: "0" },
                    { value: "1", text: "1" },
                    { value: "2", text: "2" },
                    { value: "3", text: "3" },
                    { value: "4", text: "4" },
                    { value: "5", text: "5" },
                    { value: "6", text: "6" },
                    { value: "7", text: "7" },
                    { value: "8", text: "8" },
                    { value: "9", text: "9" }
                ],

                //Ayın günü
                dsAyinGunu: [
                    { value: "0", text: "" },
                    { value: "1", text: "1" },
                    { value: "2", text: "2" },
                    { value: "3", text: "3" },
                    { value: "4", text: "4" },
                    { value: "5", text: "5" },
                    { value: "6", text: "6" },
                    { value: "7", text: "7" },
                    { value: "8", text: "8" },
                    { value: "9", text: "9" },
                    { value: "10", text: "10" },
                    { value: "11", text: "11" },
                    { value: "12", text: "12" },
                    { value: "13", text: "13" },
                    { value: "14", text: "14" },
                    { value: "15", text: "15" },
                    { value: "16", text: "16" },
                    { value: "17", text: "17" },
                    { value: "18", text: "18" },
                    { value: "19", text: "19" },
                    { value: "20", text: "20" },
                    { value: "21", text: "21" },
                    { value: "22", text: "22" },
                    { value: "23", text: "23" },
                    { value: "24", text: "24" },
                    { value: "25", text: "25" },
                    { value: "26", text: "26" },
                    { value: "27", text: "27" },
                    { value: "28", text: "28" }
                ],

                // number array
                dsHaftaNoList: [
                    //{ value: "0", text: mnLang.TranslateWithWord("UyumHaftasi") },
                    { value: "1", text: "1." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "2", text: "2." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "3", text: "3." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "4", text: "4." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "5", text: "5." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "6", text: "6." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "7", text: "7." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "8", text: "8." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "9", text: "9." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "10", text: "10." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "11", text: "11." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "12", text: "12." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "13", text: "13." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "14", text: "14." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "15", text: "15." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "16", text: "16." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "17", text: "17." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "18", text: "18." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "19", text: "19." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "20", text: "20." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "21", text: "21." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "22", text: "22." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "23", text: "23." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "24", text: "24." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "25", text: "25." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "26", text: "26." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "27", text: "27." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "28", text: "28." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "29", text: "29." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "30", text: "30." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "31", text: "31." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "32", text: "32." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "33", text: "33." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "34", text: "34." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "35", text: "35." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "36", text: "36." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "37", text: "37." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "38", text: "38." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "39", text: "39." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "40", text: "40." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "41", text: "41." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "42", text: "42." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "43", text: "43." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "44", text: "44." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "45", text: "45." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "46", text: "46." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "47", text: "47." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "48", text: "48." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "49", text: "49." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "50", text: "50." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "51", text: "51." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "52", text: "52." + mnLang.TranslateWithWord("xLng.Hafta") },
                    { value: "99", text: mnLang.TranslateWithWord("xLng.Havuz") },
                ],

                //...
                //dsSehirIlce: new kendo.data.DataSource({
                //    transport: {
                //        read: {
                //            type: "POST", dataType: "json",contentType: "application/json; charset=utf-8",
                //            url: "/Panel/Api/SehirIlceRead"
                //        }
                //    },
                //    schema: {
                //        errors: 'Errors', data: 'Data', total: 'Total', aggregates: 'Aggregates'
                //    },
                //    error: function (e) {
                //        if (e.xhr === null) { alert(e.errors); } else { mnErrorHandler.Handle(e.xhr); }
                //    }
                //}),

                //...

            });
        }

        //ön yükle 1.şekil
        //self.list.dsSehirIlce.read();

        //ön yükle 2.şekil
        //self.listLoad({
        //    TableName: 'Cinsiyet',
        //    ValueField: "Id",
        //    TextField: 'Ad',
        //    OtherFields: '',
        //    Filters: [
        //        { Field: 'Id', Operator: '>=', Value: '0', ValueType: 'Int' },
        //        { Field: 'Durum', Operator: '=', Value: '1', ValueType: 'Boolean' }
        //    ],
        //    Sorts: [
        //        { Field: 'Sira', Dir: 'asc' },
        //        { Field: 'Ad', Dir: 'asc' }
        //    ]
        //}).read();

    };

    return self;
}();
