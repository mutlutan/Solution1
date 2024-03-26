
import { mnUtils } from "/modules/mnUtils.js"
import { mnUser } from "/modules/mnUser.js"

export class app {

    abc = "zzz";
    constructor() {
        this.name = "Portal2024";
        this.root = document.getElementById('root');
        this.router = new SPARouter({ historyMode: true, caseInsensitive: false });
        this.myUser = new mnUser();

        this.fnInit();
    }

    fnInit() {

        const root = document.getElementById('root');
        this.router = new SPARouter({ historyMode: true, caseInsensitive: false });

        //bu kısmı sunucudan alalım
        this.routes = [
            { url: "/", name: "home", html: "<comp-home-page>home-page</comp-home-page>" },
            { url: "/login", name: "login", html: "<comp-login-page>login-page  <comp-login></comp-login>   </comp-login-page>" },
            { url: "/about", name: "about", html: "<comp-about-page>about-page</comp-about-page>" },
            { url: "/wrong", name: "wrong", html: "<comp-wrong-page>wrong-page</comp-wrong-page>" }
        ];

        this.routes.forEach((item) => {
            //this.router.get('/', this.fnRouterCallback).setName('home');
            //this.router.get('/login', this.fnRouterCallback).setName('home');
            this.router.get(item.url, (req, router) => { this.fnRouterCallback(req, router) }).setName(item.name);
        });

        this.router.notFoundHandler(function () {
            // if user navigates to /wrong-page outputs: oops! the page you are looking for is probably eaten by a snake
            this.router._goTo("/wrong?p=" + "404 : oops! the page you are looking for is probably eaten by a snake.");
        });

        this.router.init();
    }

    fnRouterCallback(req, router) {
        //console.log(this.argument); // outputs "A stored argument from my class" to the console
        console.log(req.uri); // outputs "/some-page-name" to the console
        console.log(req, router); // outputs "/some-page-name" to the console

        this.myUser.fnGetUserInfo().then((result) => {
            if (result.IsLogin) {
                this.root.innerHTML = this.routes.find(c => c.url == req.uri).html;
            } else {
                this.root.innerHTML = this.routes.find(c => c.url == "/login").html;
            }
        });

    }


}
