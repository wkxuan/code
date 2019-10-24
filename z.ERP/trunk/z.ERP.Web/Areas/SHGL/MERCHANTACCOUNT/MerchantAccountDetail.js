search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '商户编码', key: 'MERCHANTID'},
        { title: '商户名称', key: 'MERCHANTNAME'},
        { title: '收费单位', key: 'FEE_ACCOUNTNAME' },
        { title: '单号', key: 'REFERID' },
        { title: '类型', key: 'REFERTYPENAME'},
        { title: '变更时间', key: 'CHANGE_TIME' },
        { title: '收款金额', key: 'SAVE_MONEY',  align: "right" },
        { title: '付款金额', key: 'USE_MONEY',  align: "right" },
        { title: '变更后余额', key: 'ACCOUNT',align: "right" },
    ];
    search.service = "ShglService";
    search.method = "GetMerchantAccountDetail";
    search.indexShow = true;
    search.selectionShow = false;
};
search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.FEE_ACCOUNT_ID = "";
    search.searchParam.REFERTYPE = "";
    search.searchParam.STARTTIME = "";
    search.searchParam.ENDTIME = "";
    search.searchParam.REFERID = "";
};
search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    }
}
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