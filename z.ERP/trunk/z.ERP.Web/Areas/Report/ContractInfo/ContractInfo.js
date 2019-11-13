search.beforeVue = function () {
    var col = [
        { title: '租约号', key: 'CONTRACTID', width: 150, sortable: true, ellipsis: true, tooltip: true },
        { title: '状态', key: 'STATUSMC', width: 100, sortable: true, ellipsis: true, tooltip: true },
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

    search.popConfig = {
        title: "",
        src: "",
        width: 800,
        height: 550,
        open: false
    };
    search.screenParam.popParam = {};

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
    search.searchParam.BRANCHID = [];
    search.searchParam.FLOORID = [];  
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.CATEGORYCODE = "";
    search.searchParam.CATEGORY = [];
    search.searchParam.STATUS = "";
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