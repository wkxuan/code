srch.beforeVue = function () {
    var col = [
        { title: '商户编码', key: 'MERCHANTID', width: 90 },
        { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
        { title: '收费单位', key: 'FEE_ACCOUNTNAME', width: 200 },
        { title: '单号', key: 'REFERID', width: 80 },
        { title: '类型', key: 'REFERTYPENAME', width: 120 },
        { title: '变更时间', key: 'CHANGE_TIME', width: 150 },
        { title: '收款金额', key: 'SAVE_MONEY', width: 100, align: "right" },
        { title: '付款金额', key: 'USE_MONEY', width: 100, align: "right" },
        { title: '变更后余额', key: 'ACCOUNT', width: 120, align: "right" },
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