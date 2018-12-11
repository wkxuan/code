srch.beforeVue = function () {
    var col = [
        { title: '年月', key: 'YEARMONTH', width: 90 },
        { title: '业态编码', key: 'CATEGORYCODE', width: 90 },
        { title: '业态名称', key: 'CATEGORYNAME', width: 100 },
        { title: '楼层', key: 'FLOORCODE', width: 80 },
        { title: '铺位号', key: 'SHOPCODE', width: 90 },
        { title: '经营面积(平米)', key: 'AREA', width: 100, align: "right" },
        { title: '品牌', key: 'BRANDNAME', width: 120},
        { title: '租约号', key: 'CONTRACTID', width: 100},
        { title: '客户编码', key: 'MERCHANTID', width: 90 },
        { title: '客户名称', key: 'MERCHANTNAME', width: 120 },
        { title: '固定租金单价(元/平米/月)', key: 'RENTPRICE', width: 110, align: "right" },
        { title: '固定月租金(元)', key: 'RENTS', width: 110, align: "right" },
        { title: '含税销售额(元)', key: 'AMOUNT', width: 120, align: "right" },
        { title: '扣率租金(元)', key: 'TCRENTS', width: 100, align: "right" },
        { title: '差异(元)', key: 'CE', width: 100, align: "right" },
        { title: '应计提扣率租金(元)', key: 'YJT', width: 100, align: "right" },
        { title: '物业费单价(元/平米/月)', key: 'WYPRICE', width: 110, align: "right" },
        { title: '物业费(元)', key: 'WYJE', width: 95, align: "right" }

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