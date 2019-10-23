search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '终端号', key: 'STATIONBH', width: 130, sortable: true },
        { title: '门店编码', key: 'BRANCHID', width: 130, sortable: true },
        { title: '门店名称', key: 'BRANCHNAME',sortable: true },
        { title: '店铺', key: 'SHOPCODE', width: 130, sortable: true },
        { title: '收银员编码', key: 'USERCODE', width: 150, sortable: true },
        { title: '收银员名称', key: 'USERNAME', sortable: true }
    ];
    search.service = "XtglService";
    search.method = "CashierSearch";
    search.indexShow = true;
    search.selectionShow = false;
};
search.newCondition = function () {
    search.searchParam.STATIONBH = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.USERNAME = "";
    search.searchParam.USERID = "";
    search.searchParam.SHOPID = "";
    search.searchParam.SHOPCODE = "";
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }];
}
search.otherMethods = {
    srchUser: function () {
        search.screenParam.popParam = { };
        search.popConfig.title = "选择收银员";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelShop: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择店铺";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopShopList/";
        search.popConfig.open = true;
    },
};
search.popCallBack = function (data) {
    search.popConfig.open = false;
    if (search.popConfig.title == "选择收银员") {
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.USERID = data.sj[i].USERID;
            search.searchParam.USERNAME = data.sj[i].USERNAME;
        };
    }
    if (search.popConfig.title == "选择店铺") {
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.SHOPID = data.sj[i].SHOPID;
            search.searchParam.SHOPCODE = data.sj[i].SHOPCODE;
        };
    }
};