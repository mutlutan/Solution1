
export class mnUtils {

    constructor() {
        this.apiHost = "https://localhost:44309";
    }

    //jwt token before send fech
    fetch(url, options) {
        if (options == undefined) {
            options = {};
        }
        if (options.headers == undefined) {
            options.headers = {};
        }
        options.headers.Authorization = 'Bearer ' + sessionStorage.getItem('token');
        return fetch(url, options);
    }

}
