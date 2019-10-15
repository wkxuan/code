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
    search.screenParam.showPopUser = false;
    search.screenParam.srcPopUser = __BaseUrl + "/Pop/Pop/PopSysuserList/";
    search.screenParam.showPopShop = false;
    search.screenParam.srcPopShop = __BaseUrl + "/Pop/Pop/PopShopList/";
    search.screenParam.popParam = {};
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
        search.screenParam.showPopUser = true;
    },
    SelShop: function () {
        search.screenParam.showPopShop = true;
    },
};
search.popCallBack = function (data) {
    if (search.screenParam.showPopUser) {
        search.screenParam.showPopUser = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.USERID = data.sj[i].USERID;
            search.searchParam.USERNAME = data.sj[i].USERNAME;
        }
    }
    if (search.screenParam.showPopShop) {
        search.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.SHOPID = data.sj[i].SHOPID;
            search.searchParam.SHOPCODE = data.sj[i].SHOPCODE;
        }
    }
};