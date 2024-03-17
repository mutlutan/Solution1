
import { mnUtils } from "/modules/mnUtils.js"

export class mnUser {
    constructor() {
        this.isLogin = false;
        this.info = new Object();
    }

    async fnGetUserInfo() {
        const myUtils = new mnUtils();
        if (!this.isLogin) {
            this.info = await (await myUtils.fetch(myUtils.apiHost + "/Panel/Api/GetUserInfo")).json();
        }
        return this.info;
    }

    fnLogout() {
        sessionStorage.setItem("token", "");
		document.cookie = "Authorization=;path=/;SameSite=None; Secure;";                
        location.reload(location.pathname);
    }

    fnLogin(email, password) {
        let payload = {
            "Email": email,
            "Password": password
        }

        mnUtils.fetch(mnUtils.apiHost + "/Panel/Api/Login", {
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
    }
}