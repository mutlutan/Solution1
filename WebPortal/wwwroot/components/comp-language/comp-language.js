window.customElements.define('comp-language',
    class CompLanguage extends MyBaseCompShadow {
        constructor() {
            super();
            this.fnLoadFiles();
        }

        connectedCallback() { }

        attributeChangedCallback(name, oldValue, newValue) { }

        async fnLoadFiles() {
            await this.fnBaseLoadFiles(this.localName);

            //init
            this.fnInit();
        }

        fnInit() {
            let self = this;

            self.shadowRoot.querySelectorAll("img").forEach(elm => {
                elm.addEventListener("click", (event) => {
                    //img click de yapılacaklar
                    alert("dil seçildi ...");
                });
            });

            window.myLang.translateWithContainer(self.shadowRoot);
        }

        fnBeforeShow() {
            console.log("dil gösterildi");
        }
    }
);
