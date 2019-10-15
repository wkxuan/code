var colD = [
    { title: '商户编码', key: 'MERCHANTID', width: 120,sortable:true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '年月', key: 'YEARMONTH', width: 120, sortable: true },
    { title: '铺位面积', key: 'AREA', width: 120, align: "right", sortable: true },
    { title: '基础租金', key: 'JCZJ', width: 120, align: "right", sortable: true },
    { title: '抽成租金', key: 'TCZJ', width: 120, align: "right", sortable: true },
    { title: '实际租金', key: 'SJZJ', width: 120, align: "right", sortable: true },
    { title: '销售额', key: 'AMOUNT', width: 120, align: "right", sortable: true },
    { title: '坪效', key: 'BX', width: 120, align: "right", sortable: true },
    { title: '租售比', key: 'ZSB', width: 120, align: "right", sortable: true },
];

var colGD = [
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '年月', key: 'YEARMONTH', width: 120, sortable: true },
    { title: '铺位面积', key: 'AREA', width: 120, align: "right", sortable: true },
    { title: '基础租金', key: 'JCZJ', width: 120, align: "right", sortable: true },
    { title: '销售额', key: 'AMOUNT', width: 120, align: "right", sortable: true },
    { title: '坪效', key: 'BX', width: 120, align: "right", sortable: true },
    { title: '租售比', key: 'ZSB', width: 120, align: "right", sortable: true },

];

search.beforeVue = function () {
    search.screenParam.colDef = colD;
    search.service = "ReportService";
    search.method = "MerchantBusinessStatus";
    search.searchParam.SrchTYPE = 1;
    search.screenParam.showPop = false;
    search.screenParam.srcPop = "";
    search.screenParam.title = "";
    search.screenParam.popParam = {};
    search.indexShow = true;
    search.selectionShow = false;
};

search.newCondition = function () {
    search.searchParam.SrchTYPE = 1;
    search.searchParam.BRANCHID = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.BRANDID = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.FLOORID = null;
    search.searchParam.CATEGORYCODE = null;

    search.screenParam.CATEGORY = [];
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
        search.screenParam.title = "选择商户";
        search.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.screenParam.showPop = true;
    },
    SelBrand: function () {
        search.screenParam.title = "选择品牌";
        search.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopBrandList/";
        search.screenParam.showPop = true;
    },
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(search.screenParam, "colDef", colD);
        } else {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(search.screenParam, "colDef", colGD);
        }
    },
    changeCate: function (value, selectedData) {
        search.searchParam.CATEGORYCODE = selectedData[selectedData.length - 1].code;
    }
}

search.popCallBack = function (data) {
    if (search.screenParam.showPop) {
        search.screenParam.showPop = false;
        if (search.screenParam.title == "选择商户") {
            for (var i = 0; i < data.sj.length; i++) {
                search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                search.searchParam.MERCHANTNAME = data.sj[i].NAME;
            }
        }
        if (search.screenParam.title == "选择品牌") {
            for (var i = 0; i < data.sj.length; i++) {
                search.searchParam.BRANDID = data.sj[i].BRANDID;
                search.searchParam.BRANDNAME = data.sj[i].NAME;
            }
        }
    }
};

search.IsValidSrch = function () {
    if (!search.searchParam.SrchTYPE) {
        iview.Message.info("请选择查询类型!");
        return false;
    }

    return true;
}