srch.beforeVue = function () {
    var col = [
        { title: '门店', key: 'BRANCHNAME', width: 150},
        { title: '商户编码', key: 'MERCHANTID', width: 90 },
        { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
        { title: '收费单位', key: 'FEE_ACCOUNTNAME', width: 200 },        
        { title: '商户预收款余额', key: 'BALANCE', width: 150, align: "right" },
        { title: '已用金额', key: 'USED_MONEY', width:150, align: "right" }

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

srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};