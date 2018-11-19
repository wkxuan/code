var colD = [
    {
        title: '日期', key: 'RQ', width: 100,
        render: function (h, params) {
            return h('div',
                this.row.RQ.substr(0, 10));
            //    new Date(this.row.RQ).Format('yyyy-MM-dd'));
        }
    },
    { title: '租约号', key: 'CONTRACTID', width: 95 },
    { title: '商户编码', key: 'MERCHANTID', width: 90 },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
    { title: '店铺编号', key: 'SHOPCODE', width: 120 },
    { title: '店铺名称', key: 'SHOPNAME', width: 120 },
    { title: '分类编码', key: 'KINDCODE', width: 100 },
    { title: '分类名称', key: 'KINDNAME', width: 100 },
    { title: '品牌', key: 'BRANDNAME', width: 100 },
    { title: '销售金额', key: 'AMOUNT', width: 120, align: "right" },
    { title: '销售成本', key: 'COST', width: 120, align: "right" },
    { title: '折扣金额', key: 'DIS_AMOUNT', width: 100, align: "right" },
    { title: '优惠金额', key: 'PER_AMOUNT', width: 100, align: "right" },

];

var colM = [
    { title: '年月', key: 'YEARMONTH', width: 100 },
    { title: '租约号', key: 'CONTRACTID', width: 95 },
    { title: '商户编码', key: 'MERCHANTID', width: 90 },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
    { title: '店铺编号', key: 'SHOPCODE', width: 120 },
    { title: '店铺名称', key: 'SHOPNAME', width: 120 },
    { title: '分类编码', key: 'KINDCODE', width: 100 },
    { title: '分类名称', key: 'KINDNAME', width: 100 },
    { title: '品牌', key: 'BRANDNAME', width: 100 },
    { title: '销售金额', key: 'AMOUNT', width: 120, align: "right" },
    { title: '销售成本', key: 'COST', width: 120, align: "right" },
    { title: '折扣金额', key: 'DIS_AMOUNT', width: 100, align: "right" },
    { title: '优惠金额', key: 'PER_AMOUNT', width: 100, align: "right" },

];

srch.beforeVue = function () {
    srch.screenParam.colDef = colD;
    srch.service = "ReportService";
    srch.method = "ContractSale";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopContract = false;
    srch.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
    srch.screenParam.KINDID = [];
    srch.searchParam.SrchTYPE = 1;
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
    SelContract: function () {
        srch.screenParam.showPopContract = true;
    },
    SelBrand: function(){
        srch.screenParam.showPopBrand = true;
    },
    changeKind: function (value, selectedData) {
        srch.screenParam.KINDID = value[value.length - 1];
    },
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(srch.screenParam, "colDef", colD);
            Vue.set(srch, "method", "ContractSale");
        } else {
            Vue.set(srch.screenParam, "colDef", colM);
            Vue.set(srch, "method", "ContractSaleM");
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

    if (srch.screenParam.showPopContract) {
        srch.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
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