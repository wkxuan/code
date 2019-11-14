search.beforeVue = function () {
    search.screenParam.colDef = [
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
        { title: "发票状态", key: "STATUSNAME", width: 150 },
        { title: "创建人", key: "REPORTER_NAME", width: 150 },
        { title: "创建时间", key: "REPORTER_TIME", width: 150 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10700801,
                    title: '开票记录详情',
                    url: "JSGL/Invoice/InvoiceEdit/" + row.INVOICEID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetInvoiceList";
}
search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.INVOICENUMBER = "";
    search.searchParam.TYPE = "";
    search.searchParam.INVOICEDATE = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 10700801,
        title: '添加开票记录',
        url: "JSGL/Invoice/InvoiceEdit/"
    });
}
search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    },
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
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10700800"
    }, {
        id: "clear",
        authority: "10700800"
    }, {
        id: "add",
        authority: "10700801"
    }, {
        id: "del",
        authority: "10700801",
        name:"作废"
    }];
};
