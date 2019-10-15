search.beforeVue = function () {
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
    ];
    search.screenParam.colDef = col;
    search.service = "ReportService";
    search.method = "ContractInfo";
    search.indexShow = true;
    search.selectionShow = false;
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopContract = false;
    search.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    search.screenParam.showPopBrand = false;
    search.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    search.screenParam.popParam = {};
    search.searchParam.CATEGORYCODE = "";
    search.screenParam.CATEGORY = [];
    _.Ajax('SearchFEE', {
        Data: {}
    }, function (data) {
        var list = [];
        for (var i = 0; i < data.length;i++) {
            list.push({ title: data[i].NAME, key: data[i].PYM, width: 130, align: "right" });
        }
        search.screenParam.colDef.push(list);
    });
};

search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.CATEGORYCODE = "";
    search.searchParam.FLOORID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.BRANDNAME = "";
};

search.mountedInit = function () {
    _.Ajax('SearchCate', {
        Data: {}
    }, function (data) {
        Vue.set(search.screenParam, "CATEData", data.treeOrg.Obj);
    });

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
        search.screenParam.showPopMerchant = true;
    },
    SelContract: function () {
        search.screenParam.showPopContract = true;
    },
    SelBrand: function () {
        search.screenParam.showPopBrand = true;
    },
    changeCate: function (value, selectedData) {
        search.searchParam.CATEGORYCODE = selectedData[selectedData.length - 1].code;
    }
}

search.popCallBack = function (data) {

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }

    if (search.screenParam.showPopContract) {
        search.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
        }
    }

    if (search.screenParam.showPopBrand) {
        search.screenParam.showPopBrand = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.BRANDID = data.sj[i].BRANDID;
            search.searchParam.BRANDNAME = data.sj[i].NAME;
        }
    }
};