
window.AppAuthority =
{
    id: "Menu.", text: mnLang.f("xLng.Authority.Menu"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-list", expanded: true, prefix: false, menu: true, yetkiGrups: "11,21", items: [
        {
            id: "Menu.Islemler.", text: mnLang.f("xLng.Authority.Menu.Islemler"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-menu-button-wide", expanded: true, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Islemler.Map.", text: mnLang.f("xLng.ViewMap.Title"), hint: "", rout: "Map", params: "", showType: "Page", header: true, viewFolder: "_Maps", viewName: "ViewMap", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Islemler.Map.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.Map.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                    ]
                },
                {
                    id: "Menu.Islemler.User.", text: mnLang.f("xLng.User.Title"), hint: "", rout: "User", params: "", showType: "Page", header: true, viewFolder: "User", viewName: "UserForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Islemler.User.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.User.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.A_ResetPassword.", text: mnLang.f("xLng.Authority.SifreSifirlayabilir"), hint: "", rout: "", params: "", showType: "", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-person-lock", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Islemler.Role.", text: mnLang.f("xLng.Role.Title"), hint: "", rout: "Role", params: "", showType: "Page", header: true, viewFolder: "Role", viewName: "RoleForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Islemler.Role.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Ayarlar.", text: mnLang.f("xLng.Authority.Menu.Ayarlar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-menu-button-wide", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Ayarlar.Parameter.", text: mnLang.f("xLng.Parameter.Title"), hint: "", rout: "Parameter", params: "Id=1", showType: "Form", header: true, viewFolder: "Parameter", viewName: "ParameterForForm", cssClass: "bi bi-wrench", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.Parametre.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Parametre.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.Job.", text: mnLang.f("xLng.Job.Title"), hint: "", rout: "Job", params: "", showType: "Page", header: true, viewFolder: "Job", viewName: "JobForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.Job.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Job.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Job.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Job.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Job.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.EmailLetterhead.", text: mnLang.f("xLng.EmailLetterhead.Title"), hint: "", rout: "EmailLetterhead", params: "", showType: "Page", header: true, viewFolder: "EmailLetterhead", viewName: "EmailLetterheadForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.EmailLetterhead.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.EmailTemplate.", text: mnLang.f("xLng.EmailTemplate.Title"), hint: "", rout: "EmailTemplate", params: "", showType: "Page", header: true, viewFolder: "EmailTemplate", viewName: "EmailTemplateForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.EmailTemplate.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailTemplate.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailTemplate.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.Dosyalar.", text: mnLang.f("xLng.viewDosyalar.Title"), hint: "", rout: "Dosyalar", params: "", showType: "Popup", header: true, viewFolder: "_Dosyalar", viewName: "viewDosyalar", cssClass: "bi bi-circle", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.Dosyalar.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Dosyalar.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Dosyalar.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Tanimlamalar.", text: mnLang.f("xLng.Authority.Menu.Tanimlamalar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-menu-button-wide", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Tanimlamalar.Currency.", text: mnLang.f("xLng.Currency.Title"), hint: "", rout: "Currency", params: "Id=1", showType: "Page", header: true, viewFolder: "Currency", viewName: "CurrencyForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.Currency.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Currency.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Currency.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Currency.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Currency.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Tanimlamalar.Country.", text: mnLang.f("xLng.Country.Title"), hint: "", rout: "Country", params: "Id=1", showType: "Page", header: true, viewFolder: "Country", viewName: "CountryForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.Country.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Country.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Country.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Country.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Country.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Tanimlamalar.Customer.", text: mnLang.f("xLng.Customer.Title"), hint: "", rout: "Customer", params: "", showType: "Page", header: true, viewFolder: "Customer", viewName: "CustomerForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Customer.Country.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Customer.Country.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Customer.Country.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Customer.Country.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Customer.Country.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Bildirimler.", text: mnLang.f("xLng.Authority.Menu.Bildirimler"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-menu-button-wide", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Bildirimler.EmailPool.", text: mnLang.f("xLng.EmailPool.Title"), hint: "", rout: "EmailPool", params: "", showType: "EmailPool", header: true, viewFolder: "EmailPool", viewName: "EmailPoolForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Bildirimler.EmailPool.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-plus", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-pencil", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-trash", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Loglar.", text: mnLang.f("xLng.Authority.Menu.Loglar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-menu-button-wide", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Loglar.VwAuditLog.", text: mnLang.f("xLng.VwAuditLog.Title"), hint: "", rout: "VwAuditLog", params: "", showType: "Page", header: true, viewFolder: "VwAuditLog", viewName: "VwAuditLogForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwAuditLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwAuditLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwUserLog.", text: mnLang.f("xLng.VwUserLog.Title"), hint: "", rout: "VwUserLog", params: "", showType: "Page", header: true, viewFolder: "VwUserLog", viewName: "VwUserLogForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwUserLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwUserLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwSystemLog.", text: mnLang.f("xLng.VwSystemLog.Title"), hint: "", rout: "VwSystemLog", params: "", showType: "Page", header: true, viewFolder: "VwSystemLog", viewName: "VwSystemLogForGrid", cssClass: "bi bi-circle", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwSystemLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-search", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwSystemLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "bi bi-file-earmark-excel", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        }

    ]
};
