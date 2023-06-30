
window.AppAuthority =
{
    id: "Menu.", text: mnLang.f("xLng.Authority.Menu"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-th fa-fw", expanded: true, prefix: false, menu: true, yetkiGrups: "11,21", items: [
        {
            id: "Menu.Islemler.", text: mnLang.f("xLng.Authority.Menu.Islemler"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: true, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Islemler.Map.", text: mnLang.f("xLng.ViewMap.Title"), hint: "", rout: "Map", params: "", showType: "Page", header: true, viewFolder: "_Maps", viewName: "ViewMap", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Islemler.Map.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.Map.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                    ]
                },
                {
                    id: "Menu.Islemler.User.", text: mnLang.f("xLng.User.Title"), hint: "", rout: "User", params: "", showType: "Page", header: true, viewFolder: "User", viewName: "UserForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Islemler.User.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.User.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21" },
                        { id: "Menu.Islemler.User.A_ResetPassword.", text: mnLang.f("xLng.Authority.SifreSifirlayabilir"), hint: "", rout: "", params: "", showType: "", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-refresh fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Islemler.Role.", text: mnLang.f("xLng.Role.Title"), hint: "", rout: "Role", params: "", showType: "Page", header: true, viewFolder: "Role", viewName: "RoleForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Islemler.Role.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Islemler.Role.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Uyeler.", text: mnLang.f("xLng.Authority.Menu.Uyeler"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: true, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Uyeler.UyeGrup.", text: mnLang.f("xLng.UyeGrup.Title"), hint: "", rout: "UyeGrup", params: "", showType: "Page", header: true, viewFolder: "UyeGrup", viewName: "UyeGrupForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Uyeler.UyeGrup.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeGrup.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeGrup.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeGrup.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeGrup.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Uyeler.Uye.", text: mnLang.f("xLng.Uye.Title"), hint: "", rout: "Uye", params: "", showType: "Page", header: true, viewFolder: "Uye", viewName: "UyeForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Uyeler.Uye.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.Uye.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.Uye.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.Uye.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.Uye.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Uyeler.UyeKaraListe.", text: mnLang.f("xLng.UyeKaraListe.Title"), hint: "", rout: "UyeKaraListe", params: "", showType: "Page", header: true, viewFolder: "UyeKaraListe", viewName: "UyeKaraListeForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Uyeler.UyeKaraListe.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeKaraListe.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeKaraListe.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeKaraListe.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Uyeler.UyeKaraListe.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Ayarlar.", text: mnLang.f("xLng.Authority.Menu.Ayarlar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Ayarlar.Parameter.", text: mnLang.f("xLng.Parameter.Title"), hint: "", rout: "Parameter", params: "Id=1", showType: "Form", header: true, viewFolder: "Parameter", viewName: "ParameterForForm", cssClass: "fa fa-wrench fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.Parametre.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Parametre.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.EmailLetterhead.", text: mnLang.f("xLng.EmailLetterhead.Title"), hint: "", rout: "EmailLetterhead", params: "", showType: "Page", header: true, viewFolder: "EmailLetterhead", viewName: "EmailLetterheadForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.EmailLetterhead.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailLetterhead.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.EmailTemplate.", text: mnLang.f("xLng.EmailTemplate.Title"), hint: "", rout: "EmailTemplate", params: "", showType: "Page", header: true, viewFolder: "EmailTemplate", viewName: "EmailTemplateForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.EmailTemplate.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailTemplate.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.EmailTemplate.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Ayarlar.Dosyalar.", text: mnLang.f("xLng.viewDosyalar.Title"), hint: "", rout: "Dosyalar", params: "", showType: "Popup", header: true, viewFolder: "_Dosyalar", viewName: "viewDosyalar", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Ayarlar.Dosyalar.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Dosyalar.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Ayarlar.Dosyalar.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
                //{
                //    id: "Menu.Ayarlar.Sehir.", text: mnLang.f("xLng.Sehir.Title"), hint: "", rout: "Sehir", params: "", showType: "Page", header: true, viewFolder: "Sehir", viewName: "SehirForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: false, yetkiGrups: "11,21", items: [
                //        { id: "Menu.Ayarlar.Sehir.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        { id: "Menu.Ayarlar.Sehir.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        { id: "Menu.Ayarlar.Sehir.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        { id: "Menu.Ayarlar.Sehir.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        { id: "Menu.Ayarlar.Sehir.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        {
                //            id: "Menu.Ayarlar.Ilce.", text: mnLang.f("xLng.Ilce.Title"), hint: "", rout: "Ilce", params: "", showType: "Page", header: true, viewFolder: "Ilce", viewName: "IlceForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: false, yetkiGrups: "11,21", items: [
                //                { id: "Menu.Ayarlar.MenuIlce.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //                { id: "Menu.Ayarlar.MenuIlce.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //                { id: "Menu.Ayarlar.MenuIlce.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //                { id: "Menu.Ayarlar.MenuIlce.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //                { id: "Menu.Ayarlar.MenuIlce.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                //            ]
                //        }
                //    ]
                //},
            ]
        },
        {
            id: "Menu.Tanimlamalar.", text: mnLang.f("xLng.Authority.Menu.Tanimlamalar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Tanimlamalar.Bolge.", text: mnLang.f("xLng.Bolge.Title"), hint: "", rout: "Bolge", params: "Id=1", showType: "Page", header: true, viewFolder: "Bolge", viewName: "BolgeForForm", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.Bolge.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Bolge.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Bolge.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Bolge.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Bolge.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Tanimlamalar.Arac.", text: mnLang.f("xLng.Arac.Title"), hint: "", rout: "Arac", params: "", showType: "Page", header: true, viewFolder: "Arac", viewName: "AracForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.Arac.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Arac.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Arac.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Arac.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Arac.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Tanimlamalar.SarjIstasyonu.", text: mnLang.f("xLng.SarjIstasyonu.Title"), hint: "", rout: "SarjIstasyonu", params: "", showType: "Page", header: true, viewFolder: "SarjIstasyonu", viewName: "SarjIstasyonuForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.SarjIstasyonu.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.SarjIstasyonu.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.SarjIstasyonu.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.SarjIstasyonu.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.SarjIstasyonu.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Tanimlamalar.Tarife.", text: mnLang.f("xLng.Tarife.Title"), hint: "", rout: "Tarife", params: "", showType: "Page", header: true, viewFolder: "Tarife", viewName: "TarifeForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.Tarife.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Tarife.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Tarife.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Tarife.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Tarife.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Tanimlamalar.Fiyat.", text: mnLang.f("xLng.Fiyat.Title"), hint: "", rout: "Fiyat", params: "", showType: "Page", header: true, viewFolder: "Fiyat", viewName: "FiyatForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Tanimlamalar.Fiyat.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Fiyat.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Fiyat.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Fiyat.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Tanimlamalar.Fiyat.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Hareketler.", text: mnLang.f("xLng.Authority.Menu.Hareketler"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Hareketler.AracHareket.", text: mnLang.f("xLng.AracHareket.Title"), hint: "", rout: "AracHareket", params: "", showType: "AracHareket", header: true, viewFolder: "AracHareket", viewName: "AracHareketForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Hareketler.AracHareket.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Hareketler.AracHareket.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Hareketler.AracHareket.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Hareketler.AracHareket.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Hareketler.AracHareket.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Bildirimler.", text: mnLang.f("xLng.Authority.Menu.Bildirimler"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Bildirimler.MobilBildirim.", text: mnLang.f("xLng.MobilBildirim.Title"), hint: "", rout: "MobilBildirim", params: "", showType: "MobilBildirim", header: true, viewFolder: "MobilBildirim", viewName: "MobilBildirimForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Bildirimler.MobilBildirim.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.MobilBildirim.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.MobilBildirim.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.MobilBildirim.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.MobilBildirim.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Bildirimler.SmsBildirim.", text: mnLang.f("xLng.SmsBildirim.Title"), hint: "", rout: "SmsBildirim", params: "", showType: "SmsBildirim", header: true, viewFolder: "SmsBildirim", viewName: "SmsBildirimForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Bildirimler.SmsBildirim.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.SmsBildirim.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.SmsBildirim.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.SmsBildirim.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.SmsBildirim.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Bildirimler.EmailPool.", text: mnLang.f("xLng.EmailPool.Title"), hint: "", rout: "EmailPool", params: "", showType: "EmailPool", header: true, viewFolder: "EmailPool", viewName: "EmailPoolForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Bildirimler.EmailPool.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Bildirimler.EmailPool.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Kampanyalar.", text: mnLang.f("xLng.Authority.Menu.Kampanyalar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Kampanyalar.Kampanya.", text: mnLang.f("xLng.Kampanya.Title"), hint: "", rout: "Kampanya", params: "", showType: "Kampanya", header: true, viewFolder: "Kampanya", viewName: "KampanyaForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Kampanyalar.Kampanya.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Kampanyalar.Kampanya.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Kampanyalar.Kampanya.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Kampanyalar.Kampanya.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Kampanyalar.Kampanya.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
            ]
        },
        {
            id: "Menu.Loglar.", text: mnLang.f("xLng.Authority.Menu.Loglar"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                {
                    id: "Menu.Loglar.VwAuditLog.", text: mnLang.f("xLng.VwAuditLog.Title"), hint: "", rout: "VwAuditLog", params: "", showType: "Page", header: true, viewFolder: "VwAuditLog", viewName: "VwAuditLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwAuditLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwAuditLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwUserLog.", text: mnLang.f("xLng.VwUserLog.Title"), hint: "", rout: "VwUserLog", params: "", showType: "Page", header: true, viewFolder: "VwUserLog", viewName: "VwUserLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwUserLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwUserLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwSystemLog.", text: mnLang.f("xLng.VwSystemLog.Title"), hint: "", rout: "VwSystemLog", params: "", showType: "Page", header: true, viewFolder: "VwSystemLog", viewName: "VwSystemLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwSystemLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwSystemLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwAracStatuLog.", text: mnLang.f("xLng.VwAracStatuLog.Title"), hint: "", rout: "VwAracStatuLog", params: "", showType: "Page", header: true, viewFolder: "VwAracStatuLog", viewName: "VwAracStatuLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwAracStatuLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwAracStatuLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwSmsLog.", text: mnLang.f("xLng.VwSmsLog.Title"), hint: "", rout: "VwSmsLog", params: "", showType: "Page", header: true, viewFolder: "VwSmsLog", viewName: "VwSmsLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwSmsLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwSmsLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                },
                {
                    id: "Menu.Loglar.VwMobilBildirimLog.", text: mnLang.f("xLng.VwMobilBildirimLog.Title"), hint: "", rout: "VwMobilBildirimLog", params: "", showType: "Page", header: true, viewFolder: "VwMobilBildirimLog", viewName: "VwMobilBildirimLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                        { id: "Menu.Loglar.VwMobilBildirimLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                        { id: "Menu.Loglar.VwMobilBildirimLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    ]
                }
                //{
                //    id: "Menu.Loglar.EmailPool.", text: mnLang.f("xLng.EmailPool.Title"), hint: "", rout: "EmailPool", params: "", showType: "Page", header: true, viewFolder: "EmailPool", viewName: "EmailPoolForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                //        { id: "Menu.Loglar.EmailPool.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        { id: "Menu.Loglar.EmailPool.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                //        { id: "Menu.Loglar.EmailPool.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", rout: "", params: "", showType: "Page", header: false, viewFolder: "", viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                //    ]
                //}
            ]
        }

    ]
};
