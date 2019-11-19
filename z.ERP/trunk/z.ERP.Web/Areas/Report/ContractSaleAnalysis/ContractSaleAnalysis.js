search.beforeVue = function () {
    search.service = "ReportService";
    search.method = "ContractSaleAnalysis";
    search.indexShow = false;
    search.selectionShow = false;
    search.pageShow = false;
    search.tableHeight = 600;

    search.screenParam.colDef = [
        { title: '楼层', key: 'FLOORNAME' },
        { title: '租约号', key: 'CONTRACTID' },
        { title: '面积', key: 'AREAR', width: 100, align: "right" },
        { title: '商户名称', key: 'MERCHANTNAME' },
        { title: '品牌', key: 'BRANDNAME' },
        { title: '店铺', key: 'SHOPNAME', width: 120 },
        { title: '本期', key: 'CP'},
        { title: '本期销售', key: 'CP_SALE', width: 120, align: "right" },
        { title: '本期坪效', key: 'BQPX', width: 120, align: "right" },
        { title: '上期', key: 'PP'},
        { title: '上期销售', key: 'PP_SALE', width: 120, align: "right" },
        { title: '销售环比(%)', key: 'HB', width: 100, align: "right" },
        { title: '上期坪效', key: 'SQPX', width: 120, align: "right" },
        { title: '坪效环比(%)', key: 'PXHB', width: 100, align: "right" },
        { title: '上年同期', key: 'SP' },
        { title: '上年同期销售', key: 'SP_SALE', width: 150, align: "right" },
        { title: '销售同比(%)', key: 'TB', width: 150, align: "right" },
        { title: '上年同期坪效', key: 'SNTQPX', width: 150, align: "right" },
        { title: '坪效同比(%)', key: 'PXTB', width: 100, align: "right" },
    ];
};

search.newCondition = function () {
    search.searchParam.SrchTYPE = 1;
    search.searchParam.FLOORID = [];    
    search.searchParam.BRANCHID = [];
    search.searchParam.CONTRACTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.CP = "";
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
search.IsValidSrch = function () {
    if (!search.searchParam.CP) {
        if (search.searchParam.SrchTYPE == 1) {
            iview.Message.info("请先确定日期!");
        } else {
            iview.Message.info("请先确定年月!");
        }
        return false;
    }
    return true;
};

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
    changeSrchType: function (value) {
        Vue.set(this, "data", []);
        Vue.set(this, "pagedataCount", 0);
        search.searchParam.CP = "";
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