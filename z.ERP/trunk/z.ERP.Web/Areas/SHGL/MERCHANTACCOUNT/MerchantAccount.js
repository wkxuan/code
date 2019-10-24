search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '门店', key: 'BRANCHNAME'},
        { title: '商户编码', key: 'MERCHANTID' },
        { title: '商户名称', key: 'MERCHANTNAME'},
        { title: '收费单位', key: 'FEE_ACCOUNTNAME' },        
        { title: '商户预收款余额', key: 'BALANCE',align: "right" },
        { title: '已用金额', key: 'USED_MONEY', align: "right" }
    ];
    search.service = "ShglService";
    search.method = "GetMerchantAccount";
    search.indexShow = true;
    search.selectionShow = false;
};
search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    }
}
search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.FEE_ACCOUNT_ID = "";
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
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择商户":
                    search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break
            }
        }
    }
};