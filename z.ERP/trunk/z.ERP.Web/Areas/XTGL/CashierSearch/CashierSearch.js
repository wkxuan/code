srch.beforeVue = function () {
    srch.screenParam.colDef = [
    { title: '终端号', key: 'STATIONBH', width: 130, sortable: true },
    { title: '门店编码', key: 'BRANCHID', width: 130, sortable: true },
    { title: '门店名称', key: 'BRANCHNAME',sortable: true },
    { title: '店铺', key: 'SHOPCODE', width: 130, sortable: true },
    { title: '收银员编码', key: 'USERCODE', width: 150, sortable: true },
    { title: '收银员名称', key: 'USERNAME', sortable: true }
    ];
    srch.service = "XtglService";
    srch.method = "CashierSearch";
    srch.screenParam.showPopUser = false;
    srch.screenParam.srcPopUser = __BaseUrl + "/Pop/Pop/PopSysuserList/";
    srch.screenParam.showPopShop = false;
    srch.screenParam.srcPopShop = __BaseUrl + "/Pop/Pop/PopShopList/";
    srch.screenParam.popParam = {};
};

srch.newCondition = function () {
    srch.searchParam.STATIONBH = "";
    srch.searchParam.BRANCHID = "";
    srch.searchParam.USERNAME = "";
    srch.searchParam.USERID = "";
    srch.searchParam.SHOPID = "";
    srch.searchParam.SHOPCODE = "";
};

srch.mountedInit = function () {
    
}

srch.otherMethods = {
    srchUser: function () {
        srch.screenParam.showPopUser = true;
    },
    SelShop: function () {
        srch.screenParam.showPopShop = true;
    },
};

srch.popCallBack = function (data) {
    if (srch.screenParam.showPopUser) {
        srch.screenParam.showPopUser = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.USERID = data.sj[i].USERID;
            srch.searchParam.USERNAME = data.sj[i].USERNAME;
        }
    }
    if (srch.screenParam.showPopShop) {
        srch.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.SHOPID = data.sj[i].SHOPID;
            srch.searchParam.SHOPCODE = data.sj[i].SHOPCODE;
        }
    }
};