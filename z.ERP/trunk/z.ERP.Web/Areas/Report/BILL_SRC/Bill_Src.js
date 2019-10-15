search.beforeVue = function () {
    var col = [
        { title: '门店', key: 'BRANCHNAME', width: 180 },
        { title: '商户名称', key: 'MERCHANTNAME', width: 120 },
        { title: '账单号', key: 'BILLID', width: 100 },
        { title: '收费项目', key: 'FEENAME', width: 120 },
        { title: '租约号', key: 'CONTRACTID', width: 100 },
        { title: '债权发生月', key: 'NIANYUE', width: 100 },
        { title: '收付实现月', key: 'YEARMONTH', width: 100 },
        { title: '账单类型', key: 'TYPEMC', width: 100 },
        { title: '账单状态', key: 'STATUSMC', width: 100 },
        { title: '核算单位', key: 'UNITNAME', width: 120 },
        { title: '应收金额', key: 'MUST_MONEY', width: 100, align: "right" },
        { title: '已收金额', key: 'RECEIVE_MONEY', width: 100, align: "right" },
        { title: '返还金额', key: 'RETURN_MONEY', width: 100, align: "right" },
        { title: '描述', key: 'DESCRIPTION', width: 200 }
    ];
    search.indexShow = true;
    search.selectionShow = false;
    search.screenParam.colDef = col;
    search.service = "ReportService";
    search.method = "Bill_Src";
    search.screenParam.popParam = {};
    search.searchParam.CATEGORYCODE = "";
    search.screenParam.CATEGORY = [];
    search.searchParam.Bill_Src = "";
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    search.screenParam.showPopContract = false;
    search.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
};

search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.BILLID = "";
    search.searchParam.TYPE = "";
    search.searchParam.STATUS = "";
    search.searchParam.TRIMID = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.YEARMONTH_START = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.FLOORID = "";
    search.searchParam.CATEGORY = "";
    search.searchParam.CATEGORYCODE = "";
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

};