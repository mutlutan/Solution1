window.customElements.define("comp-header",
    class CompHeader extends MyBaseComp {
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
            this.querySelector("[name=btnLogout]").addEventListener("click", function () {
                window.myUser.logout();
            });

            window.myLang.translateWithContainer(this);
        }

        fnBeforeShow() {
            console.log('customMethod called', this);
        }
    }

);
