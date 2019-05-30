search.beforeVue = function () {
    var col = [
        { title: "商户", key: "MERCHANTNAME", width: 200},
        { title: "发票号码", key: "INVOICENUMBER", width: 120},
        { title: "发票类型", key: "TYPENAME", width: 100 },
        {
            title: "开票日期", key: "INVOICEDATE", width: 115,
            render: function (h, params) {
                return h('div',
                    this.row.INVOICEDATE.substr(0, 10));
            }
        },
        { title: "不含税金额", key: "NOVATAMOUNT", width: 100 },
        { title: "增值税金额", key: "VATAMOUNT", width: 100 },
        { title: "发票金额", key: "INVOICEAMOUNT", width: 100 },
        //{ title: "创建人", key: "CREATENAME", width: 150 },
        //{ title: "创建时间", key: "CREATEDATE", width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetInvoiceList";
}
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.MERCHANTID = data.MERCHANTID;
    }
}