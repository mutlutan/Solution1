window.customElements.define("comp-login",
    class CompLogin extends MyBaseComp {
        constructor() {
            super();
            this.fnLoadFiles();
        }

        connectedCallback() { }

        async fnLoadFiles() {
            await this.fnBaseLoadFiles(this.localName);

            //init
            this.fnInit();
        }

        fnInit() {
            let self = this;

            this.querySelector("[name=btnLogin]").addEventListener("click", function () {
                if (self.fnValidate()) {
                    let email = self.querySelector('[name=email]').value;
                    let password = self.querySelector('[name=password]').value;
                    window.myUser.login(email, password);
                }
            });

            window.myLang.translateWithContainer(this);
        }

        fnBeforeShow() {
            console.log("fnBeforeShow");
        }

        fnValidate() {
            this.querySelectorAll('input').forEach(elm => {
                if (!elm.checkValidity()) {
                    elm.classList.add("is-invalid");
                    elm.closest('div').querySelector('.error').innerHTML = elm.validationMessage;
                } else {
                    elm.classList.remove("is-invalid");
                    elm.closest('div').querySelector('.error').innerHTML = '';
                }
            });

            return this.querySelectorAll('.is-invalid').length == 0;
        }




    }
);
