
class MyBaseComp extends HTMLElement {
    constructor() {
        super();
    }

    async fnBaseLoadFiles(_localName) {

        //add Dictionary
        const resultJson = await fetch(`./components/${_localName}/${_localName}.json?v.${window.myApp.version}`)
            .then((response) => response.json());
        window.myLang.addDictionary(resultJson);

        //html temp
        const resultHtml = await fetch(`./components/${_localName}/${_localName}.html?v.${window.myApp.version}`)
            .then((response) => response.text());
        this.innerHTML = resultHtml;
    }

}

class MyBaseCompShadow extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
    }

    async fnBaseLoadFiles(_localName) {
        //add Dictionary
        const resultJson = await fetch(`./components/${_localName}/${_localName}.json?v.${window.myApp.version}`)
            .then((response) => response.json());
        window.myLang.addDictionary(resultJson);

        //css+html temp
        const resultHtml = await fetch(`./components/${_localName}/${_localName}.html?v.${window.myApp.version}`)
            .then((response) => response.text());
        const template = document.createElement("template");
        template.innerHTML = resultHtml;
        this.shadowRoot.appendChild(template.content.cloneNode(true));
    }

}

