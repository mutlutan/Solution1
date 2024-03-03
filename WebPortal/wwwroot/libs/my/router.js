window.myRouter = function () {
    var self = {};

    const route = (event) => {
        event = event || window.event;
        event.preventDefault();
        window.history.pushState({}, "", event.target.href);
        handleLocation();
    };

    self.routes = {
        404: "/pages/404.html",
        "/index.html": "/index.html",
        "/about": "/pages/about.html",
        "/lorem": "/pages/lorem.html",
    };

    const handleLocation = async () => {
        const path = window.location.pathname;
        if (path != "/index.html") {
            const route = self.routes[path] || self.routes[404];
            const html = await fetch(route).then((data) => data.text());
            document.body.innerHTML = html;
        }
    };

    window.onpopstate = handleLocation;
    window.route = route;

    self.init = function () {
        //handleLocation();
        return self;
    };

}();