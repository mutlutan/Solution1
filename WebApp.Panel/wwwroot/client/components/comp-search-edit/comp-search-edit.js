window.customElements.define("comp-search-edit",
    class CompSearchEdit extends HTMLElement {
        //class propertileri
        //...
        constructor() {
            super();

            let html = `
                <div class="divSearchEdit input-group">
                  <input type="hidden">
                  <input type="text" class="form-control">
                  <button class="btn btn-outline-secondary" type="button"> <i class="fa fa-close"></i> </button>
                  <button class="btn btn-outline-secondary" type="button"> <i class="fa fa-search"></i> </button>
                </div>
            `;

            let css = `
                <style>
                    .divSearchEdit {
                        
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
            return ['placeholder', 'value'];
        }

        attributeChangedCallback(property, oldValue, newValue) {
            //console.log("attributeChangedCallback:", property, oldValue, newValue);
            if (oldValue === newValue) return;

            switch (property) {
                case "placeholder":
                    this.querySelector("input[type=text]").placeholder = newValue;
                    break;
                case "value":
                    this.querySelector("input[type=hidden]").value = newValue;
                    break;
            }
        }

        fnInit() {
            //taným

        }

    }

);