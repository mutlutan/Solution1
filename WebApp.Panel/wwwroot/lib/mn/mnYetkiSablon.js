window.mnYetkiSablon = function () {
    var self = {};
    self.authorityList = [];

    //For MenuData
    self.getYetkiSablonForMenuData = function () {
        //burada menü işareti olanlar ve user yetkisinde olanlar geri dönecek, diğerleri dönmesin
        var menuNodes = [];
        fGetMenuNodesForMenuData(self.authorityList, menuNodes);
        return menuNodes;
    };
    function fGetMenuNodesForMenuData(_nodes, _menuNodes) {
        for (var i = 0; i < _nodes.length; i++) {
            if (_nodes[i].menu && mnUser.isYetkili(_nodes[i].id)) {
                if (_nodes[i].yetkiGrups.split(',').indexOf(mnUser.Info.nYetkiGrup.toString()) >= 0) {
                    var newLine = {
                        id: _nodes[i].id,
                        text: _nodes[i].text,
                        hint: _nodes[i].hint,
                        area: _nodes[i].area,
                        rout: _nodes[i].rout,
                        params: _nodes[i].params,
                        showType: _nodes[i].showType,
                        header: _nodes[i].header,
                        viewFolder: _nodes[i].viewFolder,
                        viewName: _nodes[i].viewName,
                        cssClass: _nodes[i].cssClass,
                        expanded: _nodes[i].expanded,
                        menu: _nodes[i].menu,
                        value: _nodes[i].value
                    };
                    _menuNodes.push(newLine);

                    if (_nodes[i].items) {
                        newLine.items = [];
                        fGetMenuNodesForMenuData(_nodes[i].items, newLine.items);
                        if (newLine.items.length === 0) {
                            delete newLine.items;
                        }
                    }
                }
            }
        }
    }

    //For Prefix
    self.getYetkiSablonForPrefix = function () {
        //burada yetki Prefix olanlar geri dönecek, diğerleri dönmesin
        var prefixNodes = [];
        fGetPrefixNodes(self.authorityList, prefixNodes);
        return prefixNodes;
    };
    function fGetPrefixNodes(_nodes, _prefixNodes) {
        for (var i = 0; i < _nodes.length; i++) {
            if (_nodes[i].prefix === true) {
                _prefixNodes.push(_nodes[i].id);
            }
            if (_nodes[i].items) {
                fGetPrefixNodes(_nodes[i].items, _prefixNodes);
            }
        }
    }

    //For Rout
    self.getYetkiSablonForRout = function () {
        //burada rout olanlar geri dönecek, diğerleri dönmesin
        var routNodes = [];
        fGetRoutNodes(self.authorityList, routNodes);
        return routNodes;
    };
    function fGetRoutNodes(_nodes, _routNodes) {
        for (var i = 0; i < _nodes.length; i++) {
            if (_nodes[i].rout && _nodes[i].rout.length > 0) {

                var newLine = {
                    id: _nodes[i].id,
                    text: _nodes[i].text,
                    hint: _nodes[i].hint,
                    area: _nodes[i].area,
                    //parentRout: _nodes.rout,
                    rout: _nodes[i].rout,
                    params: _nodes[i].params,
                    showType: _nodes[i].showType,
                    header: _nodes[i].header,
                    viewFolder: _nodes[i].viewFolder,
                    viewName: _nodes[i].viewName,
                    cssClass: _nodes[i].cssClass,
                    expanded: _nodes[i].expanded,
                    menu: _nodes[i].menu,
                    value: _nodes[i].value
                };
                _routNodes.push(newLine);
            }
            if (_nodes[i].items) {
                fGetRoutNodes(_nodes[i].items, _routNodes);
            }
        }
    }

    //Authority
    function getAuthority() {
        $.ajax({
            url: "/client/authority/authority.js?" + mnApp.version,
            dataType: 'script', async: false,
            success: function (result, textStatus, jqXHR) {
                var yetki = eval("AppAuthority");
                self.authorityList.push(yetki);
            }
        });
    }

    self.prepare = function () {
        getAuthority();
    };

    return self;
}();