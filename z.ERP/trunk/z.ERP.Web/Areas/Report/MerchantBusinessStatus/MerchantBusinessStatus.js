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
    search.searchParam.SrchTYPE = 1;
    search.searchParam.BRANCHID = [];
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.FLOORID = [];
    search.searchParam.CATEGORYCODE = "";

    search.screenParam.CATEGORY = [];
    search.screenParam.colDef = colD;
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
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    },
    SelBrand: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择品牌";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopBrandList/";
        search.popConfig.open = true;
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
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择商户":
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break;
                case "选择品牌":
                    search.searchParam.BRANDNAME = data.sj[i].NAME;
                    break;
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