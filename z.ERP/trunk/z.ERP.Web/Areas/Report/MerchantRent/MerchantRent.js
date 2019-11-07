search.beforeVue = function () {
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
    search.screenParam.colDef = col;
    search.service = "ReportService";
    search.method = "MerchantRent";
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
    search.searchParam.CATEGORYCODE = "";
    search.searchParam.FLOORID = [];
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.YEARMONTH_START = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.CATEGORY = [];
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
    changeCate: function (value, selectedData) {
        var data = selectedData[selectedData.length - 1];
        if (data) {
            search.searchParam.CATEGORYCODE = data.code;
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