search.beforeVue = function () {
    var col = [
        { title: "发票号码", key: "INVOICENUMBER", width: 120, sortable: true },
        { title: "商户", key: "MERCHANTNAME", width: 200, sortable: true },
        { title: "发票类型", key: "TYPENAME", width: 100 },
        {
            title: "开票日期", key: "INVOICEDATE", width: 115, sortable: true,
            render: function (h, params) {
                return h('div',
                    this.row.INVOICEDATE.substr(0, 10));
            }
        },
        { title: "不含税金额", key: "NOVATAMOUNT", width: 100 },
        { title: "增值税金额", key: "VATAMOUNT", width: 100 },
        { title: "发票金额", key: "INVOICEAMOUNT", width: 100 },
        { title: "创建人", key: "CREATENAME", width: 150 },
        { title: "创建时间", key: "CREATEDATE", width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetInvoiceList";
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 10700800,
        title: '浏览开票记录',
        url: "JSGL/Invoice/InvoiceDetail/" + row.INVOICEID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10700801,
        title: '新增开票记录',
        url: "JSGL/Invoice/InvoiceEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 10700801,
        title: '编辑开票记录',
        url: "JSGL/Invoice/InvoiceEdit/" + row.INVOICEID
    });
}

search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    },
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
