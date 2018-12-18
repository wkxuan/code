srch.beforeVue = function () {
    var col = [
        { title: '租约号', key: 'CONTRACTID', width: 90 },
        { title: '楼层', key: 'FLOORCODE', width: 80 },
        { title: '铺位号', key: 'SHOPCODE', width: 90 },
        { title: '品牌', key: 'BRANDNAME', width: 120},
        { title: '客户编码', key: 'MERCHANTID', width: 90 },
        { title: '客户名称', key: 'MERCHANTNAME', width: 150 },
        { title: '经营面积(平米)', key: 'AREAR', width: 100, align: "right" },
        {
            title: '合同有效期起', key: 'CONT_START', width: 110
        },
        {
            title: '合同有效期止', key: 'CONT_END', width: 110
        },
        { title: '租金收取方式', key: 'RENTWAY', width: 120 },
        { title: '固定租金标准(年:元/平米/月)', key: 'RENTPRICE', width: 120 },
        { title: '固定租金收费规则', key: 'RENTRULE', width: 120 },
        { title: '物业费收费规则', key: 'WYFRULE', width: 110 },
        { title: '物业管理费(元/平米/月)', key: 'WYFPRICE', width: 110, align: "right" },
        { title: '履约保证金', key: 'LYBZJ', width: 100, align: "right" },
        { title: '装修保证金', key: 'ZXBZJ', width: 100, align: "right" },
        { title: 'POS押金', key: 'POSYJ', width: 100, align: "right" }
    ];
    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "ContractInfo";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopContract = false;
    srch.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
    srch.searchParam.CATEGORYCODE = "";
    srch.screenParam.CATEGORY = [];
};

srch.mountedInit = function () {
    _.Ajax('SearchCate', {
        Data: {}
    }, function (data) {
        Vue.set(srch.screenParam, "CATEData", data.treeOrg.Obj);
    });
}

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    },
    SelContract: function () {
        srch.screenParam.showPopContract = true;
    },
    SelBrand: function () {
        srch.screenParam.showPopBrand = true;
    },
    changeCate: function (value, selectedData) {
        srch.searchParam.CATEGORYCODE = selectedData[selectedData.length - 1].code;
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