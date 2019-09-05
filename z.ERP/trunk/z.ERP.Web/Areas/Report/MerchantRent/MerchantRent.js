srch.beforeVue = function () {
    var col = [
        { title: '年月', key: 'YEARMONTH', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '业态编码', key: 'CATEGORYCODE', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '业态名称', key: 'CATEGORYNAME', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '楼层', key: 'FLOORNAME', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '铺位号', key: 'SHOPCODE', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '经营面积(平米)', key: 'AREA', width: 120, align: "right",  ellipsis: true, tooltip: true },
        { title: '品牌', key: 'BRANDNAME', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '租约号', key: 'CONTRACTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '客户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '客户名称', key: 'MERCHANTNAME', width: 120, sortable: true, ellipsis: true, tooltip: true },
        { title: '固定租金单价(元/平米/月)', key: 'RENTPRICE', width: 120, align: "right",  ellipsis: true, tooltip: true },
        { title: '固定月租金(元)', key: 'RENTS', width: 120, align: "right",  ellipsis: true, tooltip: true },
        { title: '含税销售额(元)', key: 'AMOUNT', width: 120, align: "right",  ellipsis: true, tooltip: true },
        { title: '扣率租金(元)', key: 'TCRENTS', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
        { title: '差异(元)', key: 'CE', width: 120, align: "right", sortable: true, ellipsis: true, tooltip: true },
        { title: '应计提扣率租金(元)', key: 'YJT', width: 120, align: "right",  ellipsis: true, tooltip: true },
        { title: '物业费单价(元/平米/月)', key: 'WYPRICE', width: 120, align: "right",  ellipsis: true, tooltip: true },
        { title: '物业费(元)', key: 'WYJE', width: 120, align: "right", sortable: true, ellipsis: true, tooltip: true }

    ];
    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "MerchantRent";

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
srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.CATEGORY = "";
    srch.searchParam.FLOORID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.CONTRACTID = "";
    srch.searchParam.BRANDNAME = "";
    srch.searchParam.YEARMONTH_START = "";
    srch.searchParam.YEARMONTH_END = "";
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