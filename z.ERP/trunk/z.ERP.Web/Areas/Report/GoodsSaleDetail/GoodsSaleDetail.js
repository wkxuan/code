var colD = [
    { title: '交易时间', key: 'SALE_TIME', width: 200,sortable:true },
    { title: '终端号', key: 'POSNO', width: 150, sortable: true },
    { title: '交易号', key: 'DEALID', width: 150, sortable: true },
    { title: '商品名称', key: 'GOODSNAME', width: 200, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '收款方式', key: 'NAME', width: 150, sortable: true },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right", sortable: true },
    { title: '原终端号', key: 'POSNO_OLD', width: 150, sortable: true },
    {
        title: '原交易号', key: 'DEALID_OLD', width: 150,
        render: function (h, params) {
            return h('div',this.row.DEALID_OLD==0? "": this.row.DEALID_OLD)
        }
    },
];
search.beforeVue = function () {
    search.screenParam.colDef = colD;
    search.service = "ReportService";
    search.method = "GoodsSaleDetail";
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
    search.searchParam.BRANCHID = [];
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.GOODSDM = "";
    search.searchParam.GOODSNAME = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.RQ_START = "";
    search.searchParam.RQ_END = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.YEARMONTH_START = "";
};

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
                case "选择品牌":
                    search.searchParam.BRANDNAME = data.sj[i].NAME;
                    break;
            }
        }
    }
};