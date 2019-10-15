search.beforeVue = function () {
    var col = [
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
    search.screenParam.colDef = col;
    search.service = "ShglService";
    search.method = "GetMerchantAccountDetail";
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.popParam = {};
    search.indexShow = true;
    search.selectionShow = false;
};
search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
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

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};