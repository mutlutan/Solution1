
//mnPopupView - kendo window dan create
window.mnPopupView = function () {
    var self = {};

    self.Container = "#mnPopupViewContainer";

    self.create = function (_opt) {

        var opt = $.extend({
            viewFolder: "",
            viewName: "",
            title: "", subTitle: "",
            minWidth: 400, width: 1000,
            showTitle: true,
            modal: true, resizable: true, draggable: true,
            actions: ["Close"],
            showCenter: true,
            showMaximize: false
        }, _opt);

        var popup = $(self.Container).find("#" + opt.viewName).getKendoWindow();
        //console.log(popup);
        var isCreate = popup !== null && popup !== undefined;

        if (isCreate) {
            return fShow(opt, isCreate, popup);
        } else {
            var htmlUrl = "/client/views/" + opt.viewFolder + "/" + opt.viewName + ".html?" + mnApp.version;
            $.ajax({
                url: htmlUrl,
                type: "GET",
                success: function (resultViewContent) {
                    $(self.Container).append(resultViewContent);
                    eval(opt.viewName).prepare(); // burada önceden yüklenmiş olan js dosyası html e etki ediyor

                    popup = $(self.Container).find("#" + opt.viewName).kendoWindow({
                        appendTo: self.Container, //bu nu açarsan mdi form gibi beyaz alanda kalır
                        title: opt.title,
                        modal: opt.modal, content: opt.content, actions: opt.actions, draggable: opt.draggable, resizable: opt.resizable,
                        minWidth: opt.minWidth, width: opt.width, height: opt.height,
                        animation: { open: { effects: "fadeIn", duration: 100 }, close: { effects: "fadeOut", duration: 100 } },
                        visible: false,
                        error: function (e) {
                            kendo.ui.progress(popup.wrapper, false); //progress Off
                            if (e.xhr === null) {
                                alert(e.errors);
                            } else {
                                mnErrorHandler.Handle(e.xhr);
                            }
                        }
                    }).getKendoWindow();

                    return fShow(opt, isCreate, popup);
                },
                error: function (xhr, status) {
                    alert(xhr.statusText);
                    if (xhr.status === 401) {
                        window.location.replace("");
                    }
                }
            });
        }
    };

    function fShow(opt, isCreate, popup) {

        //form title
        if (opt.title.length === 0) {
            opt.title = eval(opt.viewName).title;
        }

        var tempWindowTitle = ''
            + '<span class="text-nowrap">'
            + '    <span data-langkey-text="' + opt.title + '" ></span>'
            + '    <small data-langkey-text="' + opt.subTitle + '" style="padding-left:15px; opacity:0.5; font-style:italic; font-size:0.8em;"></small>'
            + '</span>';

        $(popup.wrapper).find(".k-window-titlebar .k-window-title").html(tempWindowTitle);
        $(popup.wrapper).find(".k-window-titlebar");//.addClass("mn-default-bg-color").addClass("mn-default-text-color");
        $(popup.wrapper).find(".k-window-content").css("padding", "15px");

        //kendo.ui.progress(popup.wrapper, true); //progress On

        //event bind
        if (opt.onClose !== undefined) {
            popup.bind("close", function (e) {
                var canClose = opt.onClose(eval(opt.viewName));
                if (canClose !== undefined & canClose === false) {
                    e.preventDefault();//window kapanmaz
                } else {
                    popup.unbind("close"); // bind edilen event siliniyor
                }
            });
        }

        if (!isCreate) {
            //ajax ile window url yükleniyor ise burada show olur
            popup.bind("refresh", function () {
                popup.unbind("refresh");
                if (opt.onShow !== undefined) {
                    opt.onShow(eval(opt.viewName));
                }
                kendo.ui.progress(popup.wrapper, false); //progress Off
            });
            //ajax ile window url yüklenmiyor ise burada show olur
            popup.bind("open", function () {
                popup.unbind("open");
                if (opt.onShow !== undefined) {
                    opt.onShow(eval(opt.viewName));
                }
                kendo.ui.progress(popup.wrapper, false); //progress Off
            });

        } else {
            ////ajax ile window url yüklenmiyor ise burada show olur
            popup.bind("open", function () {
                popup.unbind("open");
                if (opt.onShow !== undefined) {
                    opt.onShow(eval(opt.viewName));
                }
                kendo.ui.progress(popup.wrapper, false); //progress Off
            });
        }

        if (opt.reload !== undefined) {
            if (opt.reload === true) {
                if (isCreate) {
                    if (opt.reloadUrl === undefined) {
                        popup.refresh();
                    }
                    else {
                        popup.refresh({ url: opt.reloadUrl }); //win.refresh({ url: "../controller/action", data: { id: 0 } });
                    }
                }
            }
        }

        mnLang.TranslateWithSelector(popup.wrapper);

        if (opt.showCenter == true) {
            popup.center();
        }

        popup.open();

        if (opt.showMaximize == true) {
            popup.maximize();
        } else {
            if (popup.isMaximized()) {
                popup.toggleMaximization();
            }
        }

        return popup;
    }

    return self;
}();

