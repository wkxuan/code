search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "商户", key: "MERCHANTNAME", width: 200},
        { title: "发票号码", key: "INVOICENUMBER", width: 120},
        { title: "发票类型", key: "TYPENAME", width: 100 },
        { title: "开票日期", key: "INVOICEDATE", width: 115, cellType: "date"},
        { title: "不含税金额", key: "NOVATAMOUNT", width: 100 },
        { title: "增值税金额", key: "VATAMOUNT", width: 100 },
        { title: "发票金额", key: "INVOICEAMOUNT", width: 100 },
    ];
    search.service = "JsglService";
    search.method = "GetInvoiceList";
}
search.initSearchParam = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.INVOICENUMBER = "";
    search.searchParam.TYPE = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.INVOICEDATE = "";
}