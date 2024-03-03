window.customElements.define("comp-section",
    class CompSection extends MyBaseComp {
        constructor() {
            super();
            this.fnLoadFiles();
        }

        connectedCallback() {
        }

        async fnLoadFiles() {
            await this.fnBaseLoadFiles(this.localName);

            //init
            this.fnInit();
        }

        fnInit() {
            let self = this;
            window.myLang.translateWithContainer(this);
        }

        fnBeforeShow() {
        }
    }
);
