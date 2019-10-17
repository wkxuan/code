var colD = [
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 250, sortable: true, ellipsis: true, tooltip: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '债权发生月', key: 'NIANYUE', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '收付实现月', key: 'YEARMONTH', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '收费项目', key: 'TRIMNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '应收金额', key: 'MUST_MONEY', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
    { title: '已收金额', key: 'RECEIVE_MONEY', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
    { title: '未付金额', key: 'UNPAID_MONEY', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },

];

var colM = [
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 250, sortable: true, ellipsis: true, tooltip: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '收费项目', key: 'TRIMNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '应收金额', key: 'MUST_MONEY', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
    { title: '已收金额', key: 'RECEIVE_MONEY', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
    { title: '未付金额', key: 'UNPAID_MONEY', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },

];

search.beforeVue = function () {
    search.screenParam.colDef = colD;
    search.service = "ReportService";
    search.method = "MerchantPayCost";
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
    search.searchParam.SrchTYPE = 1;
    search.searchParam.BRANCHID = [];
    search.searchParam.TRIMID = []; 
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.ISpay = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.YEARMONTH_START = "";

    search.screenParam.colDef = colD;
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
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(search.screenParam, "colDef", colD);
        } else {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(search.screenParam, "colDef", colM);
        }
    }
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

search.IsValidSrch = function () {
    if (!search.searchParam.SrchTYPE) {
        iview.Message.info("请选择查询类型!");
        return false;
    }

    return true;
}