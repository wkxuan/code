srch.beforeVue = function () {
    var col = [
        { title: '租约号', key: 'CONTRACTID', width: 150, sortable: true, ellipsis: true, tooltip: true },
        { title: '楼层', key: 'FLOORCODE', width: 80, sortable: true, ellipsis: true, tooltip: true },
        { title: '铺位号', key: 'SHOPCODE', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '品牌', key: 'BRANDNAME', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '客户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '客户名称', key: 'MERCHANTNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
        { title: '经营面积(平米)', key: 'AREAR', width: 110, align: "right", ellipsis: true, tooltip: true },
        {
            title: '合同有效期起', key: 'CONT_START', width: 110, ellipsis: true, tooltip: true
        },
        {
            title: '合同有效期止', key: 'CONT_END', width: 110, ellipsis: true, tooltip: true
        },
        { title: '租金收取方式', key: 'RENTWAY', width: 150, sortable: true, ellipsis: true, tooltip: true },
        { title: '固定租金标准(年:元/平米/月)', key: 'RENTPRICE', width: 200, ellipsis: true, tooltip: true },
        { title: '固定租金收费规则', key: 'RENTRULE', width: 150, ellipsis: true, tooltip: true },
        //{ title: '物业费收费规则', key: 'WYFRULE', width: 110 },
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
    _.Ajax('SearchFEE', {
        Data: {}
    }, function (data) {
        var list = [];
        for (var i = 0; i < data.length;i++) {
            debugger
            list.push({ title: data[i].NAME, key: data[i].PYM, width: 130, align: "right" });
        }
        debugger
        srch.screenParam.colDef.push(list);
    });
};

srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.CATEGORYCODE = "";
    srch.searchParam.FLOORID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.CONTRACTID = "";
    srch.searchParam.BRANDNAME = "";
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