window.myUser = function () {
    var self = {};
    self.info = new Object();

    self.init = async function () {
        const result = await window.myApp.fetch(myApp.origin + "/Account/GetUserInfo")
            .then((response) => response.json());

        self.info = result.Data;

        return self;
    };

    //window.myUser.login(userName, userpassword);
    self.login = function (userName, userpassword) {
        let payload = {
            "UserName": userName,
            "UserPassword": userpassword
        }

        window.myApp.fetch(myApp.origin + "/Account/Login", {
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify(payload)
        })
            .then((response) => response.json())
            .then((result) => {
                console.log("Login response", result);
                //document.cookie = "Authorization=" + result.Data.UserToken + ";domain=." + myApp.host +";path=/";
                sessionStorage.setItem("token", result.Data.Token);
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