/*
 * Side Bar Menu widget for Bootstrap 5
 * Version: 0.3
 * Mutlu MUTLUTAN
*/

(function ($) {

    $.fn.mnSideBarMenu = function (_opt) {

        var $this = $(this);
        var self = $this;
        self.opt = $.extend({
            dataSource: [],
            itemColor: "ghostwhite",
            itemBackgroundColor: "transparent",
            itemHoverColor: "dodgerblue",
            itemHoverBackgroundColor: "white"
        }, _opt);

        function fAddStyle() {
            var style = `                                                                                     
                <style>    
                    .mnSideBarMenu{
                       font-size: 1em;
                    }
                    .mnSideBarMenu i:first-child{                                                             
                       color: ${self.opt.itemColor};
                    }                                                                                         
                    .mnSideBarMenu ul li a {                                                                  
                        color: ${self.opt.itemColor};
                    }                                                                                         
                        .mnSideBarMenu ul {                                                                   
                            padding: 0px;                                                                     
                            margin: 0px;                                                                      
                        }                            
                            .mnSideBarMenu ul li {                                                            
                                list-style: none;                                                             
                            }                                                                                 
                                .mnSideBarMenu > ul li a {                                                    
                                    text-decoration: none;                                                    
                                    display: block;                                                           
                                    padding: 0px 10px 0px 10px;                                    
                                    margin: 0px;                                                              
                                    font-weight: bold;                                                        
                                    opacity: 0.8;                                                             
                                    font-size: 1em;  
                                    transition: all 0 !important;
                                }                                                                             
                                    .mnSideBarMenu ul li a:hover {                                            
                                        cursor: pointer;    
                                        color: ${self.opt.itemHoverColor};
                                        background-color: ${self.opt.itemHoverBackgroundColor};
                                    }
                                .mnSideBarMenu > ul > li > ul > li > ul{
                                    margin-bottom:5px;
                                }
                                .mnSideBarMenu > ul li ul li a {                                              
                                    padding: 5px 10px 0px 20px;                                               
                                    font-weight: 550;                                                         
                                    font-size: 1em;                                                           
                                }                                                                             
                                .mnSideBarMenu > ul li ul li ul li a{                                         
                                    padding: 0px 0px 0px 30px;                                                
                                    font-weight: 400;                                                         
                                    font-size: 1rem;                                                           
                                }                                                                             
                        .mnSideBarMenu i.mnCollapseButton {                                                   
                            margin-top: 3px;                                                                  
                        }                                                                                     
                        .mnSideBarMenu * ul {                           
                            transition: all 0.3s;                                                                 
                        }                                                                                     
                        .mnSideBarMenu [aria-expanded="true"] i.mnCollapseButton {                            
                            transform: rotate(-90deg);                                                        
                            transition: 0.3s;                                                                 
                        }                                                                                     
                </style>
            `;

            $(document.head).append(style);
        }

        function fGetMenuHtml() {
            var mainMenuUniqeName = "mainMenu_" + new Date().getTime();
            var menuHtmls = $('<ul id="' + mainMenuUniqeName + '" ></ul>');

            for (var i = 0; i < self.opt.dataSource.length; i++) {

                var level_0_name = mnApi.ReplaceAll(self.opt.dataSource[i].id, ".", "-");
                var level_0_item_length = (self.opt.dataSource[i].items === undefined ? 0 : self.opt.dataSource[i].items.length);
                var newLine = '';
                newLine += '<li style="padding-bottom:5px;">';
                newLine += '    <a name="' + level_0_name + '" data-item-length="' + level_0_item_length + '" data-parent="#' + mainMenuUniqeName + '" data-bs-toggle="collapse" data-bs-target="#' + level_0_name + '" aria-expanded="' + self.opt.dataSource[i].expanded + '">';
                newLine += '        <i class="' + self.opt.dataSource[i].cssClass + '"></i>';
                newLine += '        <span>' + self.opt.dataSource[i].text + '</span>';

                if (self.opt.dataSource[i].items) {
                    newLine += '        <i class="mnCollapseButton bi bi-chevron-left float-end"></i>';
                }
                newLine += '    </a>';

                if (self.opt.dataSource[i].items) {
                    newLine += '<ul class="multi-collapse collapse ' + (self.opt.dataSource[i].expanded === true ? " show " : "") + '" id="' + level_0_name + '">';
                    for (var k = 0; k < self.opt.dataSource[i].items.length; k++) {
                        var level_1_name = mnApi.ReplaceAll(self.opt.dataSource[i].items[k].id, ".", "-");
                        var level_1_item_length = (self.opt.dataSource[i].items[k].items === undefined ? 0 : self.opt.dataSource[i].items[k].items.length);
                        newLine += '<li>';
                        newLine += '    <a name="' + level_1_name + '" data-item-length="' + level_1_item_length + '" data-parent="#' + level_0_name + '" data-bs-toggle="collapse" data-bs-target="#' + level_1_name + '" aria-expanded="' + self.opt.dataSource[i].items[k].expanded + '" style="white-space: nowrap !important;" >';
                        newLine += '        <i class="' + self.opt.dataSource[i].items[k].cssClass + '"></i>';
                        newLine += '        <span>' + self.opt.dataSource[i].items[k].text + '</span>';
                        //--------level(2) için Collapse Button baş-----------------------
                        if (self.opt.dataSource[i].items[k].items) {
                            newLine += '        <i class="mnCollapseButton bi bi-chevron-left float-end"></i>';
                        }
                        //-----------level(2) için Collapse Button bit--------------------
                        newLine += '    </a>';
                        //level(2) yeri...bas
                        if (self.opt.dataSource[i].items[k].items) {
                            newLine += '<ul class="collapse ' + (self.opt.dataSource[i].items[k].expanded === true ? " show " : "") + '" id="' + level_1_name + '">';
                            for (var j = 0; j < self.opt.dataSource[i].items[k].items.length; j++) {
                                var level_2_name = mnApi.ReplaceAll(self.opt.dataSource[i].items[k].items[j].id, ".", "-");
                                var level_2_item_length = (self.opt.dataSource[i].items[k].items[j].items === undefined ? 0 : self.opt.dataSource[i].items[k].items[j].items.length);
                                newLine += '<li>';

                                var prms = self.opt.dataSource[i].items[k].items[j].params.length > 0 ? '?' + self.opt.dataSource[i].items[k].items[j].params : "";
                                var href = self.opt.dataSource[i].items[k].items[j].rout.length > 0 ? ' href="#/' + self.opt.dataSource[i].items[k].items[j].rout + prms + '" ' : '';

                                newLine += '    <a ' + href + ' name="' + level_2_name + '" data-item-length="' + level_2_item_length + '" >';
                                newLine += '        <i class="' + self.opt.dataSource[i].items[k].items[j].cssClass + '" style="vertical-align: middle;"></i>';
                                newLine += '        <div style="display:inline-block; vertical-align: middle;">';
                                newLine += '            <span style="display:block;">' + self.opt.dataSource[i].items[k].items[j].text + '</span>';
                                if (self.opt.dataSource[i].items[k].items[j].hint != undefined && self.opt.dataSource[i].items[k].items[j].hint != null && self.opt.dataSource[i].items[k].items[j].hint.length > 0) {
                                    if (self.opt.dataSource[i].items[k].items[j].text != self.opt.dataSource[i].items[k].items[j].hint) {
                                        newLine += '            <span style="display:block; opacity:0.5;">' + self.opt.dataSource[i].items[k].items[j].hint + '</span>';
                                    }
                                }
                                newLine += '        </div>';
                                newLine += '    </a>';
                                newLine += '</li>';
                            }
                            newLine += '</ul>';
                        }
                        //level(2) yeri...bit

                        newLine += '</li>';
                    }
                    newLine += '</ul>';
                }

                newLine += '</li>';

                menuHtmls.append($(newLine));
            }

            return menuHtmls;
        }

        self.fInit = function () {
            fAddStyle();
            $this.addClass("mnSideBarMenu");
            $this.empty().append(fGetMenuHtml());

            $this.on("click", "a[data-item-length=0]", function (e) {
                var yetkiId = $(this).attr("name");
                yetkiId = mnApi.ReplaceAll(yetkiId, "-", ".");
                //event bind
                if (self.opt.onClick !== undefined) {
                    self.opt.onClick({ "yetkiId": yetkiId });
                }
            });

        };

        self.fInit();

        return self;
    };

})(jQuery);
