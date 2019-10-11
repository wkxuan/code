search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "商户编号", key: "MERCHANTID" },
        { title: "商户名称", key: "MERCHANTNAME" },
        {title: "编号", key: 'PAYMENTID'},
        { title: '银行卡号', key: 'CARDNO'},
        { title: '银行名称', key: 'BANKNAME'},
        { title: '开户人', key: 'HOLDERNAME'},
        { title: '身份证号', key: 'IDCARD' },
    ];
    search.service = "ShglService";
    search.method = "GetMerchantPayment";
}
search.initSearchParam = function () {
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.PAYMENTID = "";
}