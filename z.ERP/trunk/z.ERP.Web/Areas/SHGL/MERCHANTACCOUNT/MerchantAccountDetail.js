srch.beforeVue = function () {
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
    srch.screenParam.colDef = col;
    srch.service = "ShglService";
    srch.method = "GetMerchantAccountDetail";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";

    srch.screenParam.popParam = {};
};

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    }
}


srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};