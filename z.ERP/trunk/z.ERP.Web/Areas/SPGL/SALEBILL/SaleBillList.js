search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 100 },
        { title: "记账日期", key: "ACCOUNT_DATE", width: 150 },
        { title: "收银员", key: "SYYMC", width: 100 },
        { title: "营业员", key: "YYYMC", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100 },
        { title: "分店名称", key: "BRANCHMC", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "SpglService";
    search.method = "GetSaleBillList";

    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopBrand = false;
    search.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";
    search.screenParam.popParam = {};
    search.screenParam.KINDID = [];
    search.searchParam.TYPE = 3;
}

search.mountedInit = function () {
    _.Ajax('SearchKind', {
        Data: {}
    }, function (data) {
        Vue.set(search.screenParam, "dataKind", data.treeorg.Obj);
    });
}

search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    },
    SelBrand: function () {
        search.screenParam.showPopBrand = true;
    },
    changeKind: function (value, selectedData) {
        search.screenParam.KINDID = value[value.length - 1];
    },
}

search.popCallBack = function (data) {

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
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

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 105004,
        title: '浏览销售补录单',
        url: "SPGL/SALEBILL/SaleBillDetail/" + row.BILLID
    })
}

search.modHref = function (row, index) {
    if (row.STATUS == 1){
        _.OpenPage({
            id: 105004,
            title: '编辑销售补录单',
            url: "SPGL/SALEBILL/SaleBillEdit/" + row.BILLID
        })
    }else{
        iview.Message.info('当前销售补录单已审核,不能编辑!');
        return;
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 105004,
        title: '新增销售补录单',
        url: "SPGL/SALEBILL/SaleBillEdit/"
    })
}


