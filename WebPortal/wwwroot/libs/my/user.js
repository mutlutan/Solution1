﻿window.myUser = function () {
    var self = {};
    self.info = new Object();

    self.init = async function () {
        const result = await (await window.myApp.fetch(myApp.origin + "/Panel/Api/GetUserInfo")).json();
        self.info = result;
        return self;
    };

    //window.myUser.login(email, password); //using
    self.login = function (email, password) {
        let payload = {
            "Email": email,
            "Password": password
        }

        window.myApp.fetch(myApp.origin + "/Panel/Api/Login", {
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify(payload)
        })
            .then((response) => response.json())
            .then((result) => {
                document.cookie = "Authorization=" + result.Data.UserToken + ";domain=." + myApp.host + ";path=/;SameSite=None; Secure;";
                sessionStorage.setItem("token", result.Data.UserToken);
                //eğer login ve ga validate başarılı ise girmiş demektir
                if (result.Data.IsUserLogin && result.Data.IsGoogleValidate) {
                    location.reload();
                } else if (result.Data.IsUserLogin && result.Data.IsGoogleValidate == false) {
                    //ga yüklenmesi için qr getirlmeli
                    location.reload();
                } else {
                    alert(result.Data.Messages);
                }
            })
            .catch(err => {
                alert(err);
            });
    };

    //window.myUser.logout();
    self.logout = function () {
        sessionStorage.setItem("token", "");
        location.reload(location.pathname);
    };

    return self;
}();