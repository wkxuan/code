var col = [
        { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '商户名称', key: 'MERCHANTNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
];
search.beforeVue = function () {
    search.screenParam.colDef = col;
    search.service = "ReportService";
    search.method = "MerchantSaleCollect";
    search.indexShow = true;
    search.selectionShow = false;

    search.popConfig = {
        title: "",
        src: "",
        width: 800,
        height: 550,
        open: false
    };
    search.screenParam.popParam = {};   
};

search.newCondition = function () {
    search.screenParam.colDef = [
        { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '商户名称', key: 'MERCHANTNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    ];
    search.searchParam.BRANCHID = [];
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.Pay = [];
    search.searchParam.CONTRACTID = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.RQ_START = null;
    search.searchParam.RQ_END = null;
};
search.searchDataAfter = function (data) {
    if (data.length) {
        var paystr = "";
        if (search.searchParam.Pay.length>0) {
            for (let pay in search.searchParam.Pay) {
                paystr += search.searchParam.Pay[pay] + ",";
            }
            paystr = paystr.substring(0, paystr.lastIndexOf(','));
        }
        _.Ajax('GetPay', {
            item: { Values: { Pay: paystr } }
        }, function (data) {
            var list = [];
            for (var i = 0; i < data.length; i++) {
                list.push({ title: data[i].NAME, key: "PAYID" + data[i].PAYID, width: 130, align: "right", sortable: true });
            }
            col = [
            { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
            { title: '商户名称', key: 'MERCHANTNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
            ];
            col.push.apply(col, list);
            col.push({ title: '总计金额', key: 'SUMPAY', width: 150, sortable: true, ellipsis: true, tooltip: true });
            search.screenParam.colDef = col;
        });
    }
}
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "export",
        authority: ""
    }];
}

search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    },
    SelContract: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择租约";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopContractList/";
        search.popConfig.open = true;
    },
    SelBrand: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择品牌";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopBrandList/";
        search.popConfig.open = true;
    },
}

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择商户":
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break;
                case "选择租约":
                    search.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
                    break;
                case "选择品牌":
                    search.searchParam.BRANDNAME = data.sj[i].NAME;
                    break;
            }
        }
    }
};