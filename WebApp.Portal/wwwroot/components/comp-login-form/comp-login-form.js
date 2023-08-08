window.customElements.define("comp-login-form",
    class CompLoginForm extends HTMLElement {
        //class propertileri
        selector = ".divLoginForm";

        constructor() {
            super();

            let html = `
                <div class="divLoginForm">
				    <div class="form-group row">
				    	<label for="username" class="col-md-4 col-form-label pr-0 font-weight-light">User Name</label>
				    	<div class="col-md-12">
				    		<input name='username' type="text" class='form-control font-weight-light' required autocomplete="username" />
				    	</div>
				    </div>

                    <div class="form-group row">
				    	<label for="password" class="col-md-4 col-form-label pr-0 font-weight-light">Password</label>
				    	<div class="col-md-12">
				    		<input name='password' type="text" class='form-control font-weight-light' required autocomplete="password" />
				    	</div>
				    </div>

                    <div class="form-group">
                        <button id="btnLogin" type="button" class="btn font-weight-light rounded">
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
                    this.querySelector(this.selector + " input[name=username]").value = newValue;
                    break;
                case "password":
                    this.querySelector(this.selector + " input[name=password]").value = newValue;
                    break;
            }
        }

        fnInit() {
            //taným

        }

    }

);