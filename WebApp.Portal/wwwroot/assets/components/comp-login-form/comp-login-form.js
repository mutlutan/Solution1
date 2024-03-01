window.customElements.define("comp-login-form",
    class CompLoginForm extends HTMLElement {
        //class propertileri
        useGA = false;
        useCaptcha = false;

        constructor() {
            super();

            let html = `
                <div class="divLoginForm">
				    <div class="form-group row pb-1">
				    	<label for="username" class="col-md-4 col-form-label pr-0 font-weight-light">User Name</label>
				    	<div class="col-md-12">
				    		<input name='username' type="text" class='form-control font-weight-light' required autocomplete="username" />
				    	</div>
				    </div>

                    <div class="form-group row pb-1">
				    	<label for="password" class="col-md-4 col-form-label pr-0 font-weight-light">Password</label>
				    	<div class="col-md-12">
				    		<input name='password' type="text" class='form-control font-weight-light' required autocomplete="password" />
				    	</div>
				    </div>

                    <div class="form-group pb-1">
                        <button id="btnLogin" type="button" class="btn btn-success font-weight-light rounded">
							<i class="fa fa-user-o"></i>
							<span>Login</span>
							<span id="loader"></span>
						</button>
                    </div>
                </div>
            `;

            let css = `
                <style>
                    .divLoginForm {
                        
                    }
                </style>
            `;


            this.innerHTML = html + css;
            this.fnInit();
        }

        connectedCallback() {
            //console.log("connectedCallback");
        }

        static get observedAttributes() {
            return ['username', 'password'];
        }

        attributeChangedCallback(property, oldValue, newValue) {
            //console.log("attributeChangedCallback:", property, oldValue, newValue);
            if (oldValue === newValue) return;

            switch (property) {
                case "username":
                    this.querySelector("input[name=username]").value = newValue;
                    break;
                case "password":
                    this.querySelector(" input[name=password]").value = newValue;
                    break;
            }
        }

        fnFormValidate() {
            let result = { isSuccess: false, messages: [] };

            const elements = this.querySelectorAll("input,select");

            result.isSuccess = true; //ilk deðer true
            elements.forEach((elm) => {

                elm.classList.remove("is-invalid");
                if (elm.checkVisibility() && !elm.checkValidity()) {
                    result.isSuccess = false;
                    let elmCaption = this.querySelector("[for=" + elm.name + "]").innerHTML;
                    result.messages.push(elmCaption + " : " + elm.validationMessage);
                    elm.classList.add("is-invalid");
                } 

            });

            return result;
        }

        fnSetEvents() {
            this.querySelector("#btnLogin").addEventListener("click", (event) => {
                let resultValidate = this.fnFormValidate();
                if (resultValidate.isSuccess) {
                    alert("ok. gönder");
                } else {
                    alert(resultValidate.messages.join("\n"));
                }
            });
        }

        fnInit() {
            //taným
            this.fnSetEvents();
        }

    }

);