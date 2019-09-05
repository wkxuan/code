srch.beforeVue = function () {
    var col = [
        { title: '门店', key: 'BRANCHNAME'},
        { title: '商户编码', key: 'MERCHANTID' },
        { title: '商户名称', key: 'MERCHANTNAME'},
        { title: '收费单位', key: 'FEE_ACCOUNTNAME' },        
        { title: '商户预收款余额', key: 'BALANCE',align: "right" },
        { title: '已用金额', key: 'USED_MONEY', align: "right" }

    ];
    srch.screenParam.colDef = col;
    srch.service = "ShglService";
    srch.method = "GetMerchantAccount";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";

    srch.screenParam.popParam = {};
};

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    }
}

srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.FEE_ACCOUNT_ID = "";

};


srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};