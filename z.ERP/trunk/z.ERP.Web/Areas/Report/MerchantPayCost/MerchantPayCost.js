var colD = [
    { title: '商户编码', key: 'MERCHANTID', width: 120,sortable:true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 250, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '债权发生月', key: 'NIANYUE', width: 150, sortable: true },
    { title: '收费项目', key: 'TRIMNAME', width: 150, sortable: true },
    { title: '应收金额', key: 'MUST_MONEY', width: 150, align: "right", sortable: true },
    { title: '已收金额', key: 'RECEIVE_MONEY', width: 150, align: "right", sortable: true },
    { title: '未付金额', key: 'UNPAID_MONEY', width: 150, align: "right", sortable: true },

];

var colM = [
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 250, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '收费项目', key: 'TRIMNAME', width: 150, sortable: true },
    { title: '应收金额', key: 'MUST_MONEY', width: 150, align: "right", sortable: true },
    { title: '已收金额', key: 'RECEIVE_MONEY', width: 150, align: "right", sortable: true },
    { title: '未付金额', key: 'UNPAID_MONEY', width: 150, align: "right", sortable: true },

];

srch.beforeVue = function () {
    srch.screenParam.colDef = colD;
    srch.service = "ReportService";
    srch.method = "MerchantPayCost";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
    srch.searchParam.SrchTYPE = 1;
};

srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.TRIMID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.ISpay = "";
    srch.searchParam.BRANDNAME = "";
    srch.searchParam.YEARMONTH_END = "";
    srch.searchParam.YEARMONTH_START = "";
};

srch.mountedInit = function () {
    _.Ajax('SearchKind', {
        Data: {}
    }, function (data) {
        Vue.set(srch.screenParam, "dataKind", data.treeorg.Obj);
    });
}

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    },
    SelBrand: function () {
        srch.screenParam.showPopBrand = true;
    },
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colD);
            Vue.set(srch, "method", "MerchantPayCost");
        } else {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colM);
            Vue.set(srch, "method", "MerchantPayCostS");
        }
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

    if (srch.screenParam.showPopBrand) {
        srch.screenParam.showPopBrand = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.BRANDID = data.sj[i].BRANDID;
            srch.searchParam.BRANDNAME = data.sj[i].NAME;
        }
    }
};

srch.IsValidSrch = function () {
    if (!srch.searchParam.SrchTYPE) {
        iview.Message.info("请选择查询类型!");
        return false;
    }

    return true;
}