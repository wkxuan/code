search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID", sortable: true },
        { title: "记账日期", key: "ACCOUNT_DATE", sortable: true, cellType: "date" },
        { title: "收银员", key: "SYYMC", sortable: true },
        { title: "营业员", key: "YYYMC", sortable: true },
        { title: "状态", key: "STATUSMC" },
        { title: "门店名称", key: "BRANCHMC", width: 250 },
        { title: "登记人", key: "REPORTER_NAME" },
        { title: "登记时间", key: "REPORTER_TIME" },
        { title: "审核人", key: "VERIFY_NAME" },
        { title: "审核时间", key: "VERIFY_TIME" },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10500401,
                    title: '销售补录单详情',
                    url: "SPGL/SALEBILL/SaleBillEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "SpglService";
    search.method = "GetSaleBillList";
    search.uploadName = "销售补录单";
}

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        authority: "10500401"
    }, {
        id: "del",
        authority: "10500401"
    }, {
        id: "upload",
        authority: "10500401"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10500402",
        fun: function () {
            let _this = search.vueObj;
            let selectton = _this.$refs.selectData.getSelection();
            if (selectton.length == 0) {
                iview.Message.info("请选中要审核的数据!");
                return;
            }
            var sh = true;
            for (var i = 0; i < selectton.length;i++) {   //循环选择数据，当有未审核状态时执行审核
                if (selectton[i].STATUS == 1) {
                    sh = false;
                    break
                }
            }
            if (sh == false) {
                search.vueObj.spinShow = true;
                _.Ajax('ExecDataList', {
                    DataList: selectton,
                }, function (data) {
                    search.vueObj.spinShow = false;
                    iview.Message.info("审核成功");
                    search.searchList();
                }, function () {
                    search.vueObj.spinShow = false;
                    iview.Message.info("审核失败");
                });
            } else {
                iview.Message.info("选中的数据已审核!");
                return;
            }
        },
        enabled: function (disabled, data) {
            return true;
        },
        isNewAdd: true
    }];
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.ACCOUNT_DATE_START = "";
    search.searchParam.ACCOUNT_DATE_END = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY_NAME = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";

    search.searchParam.TYPE = 3;
};
search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.title = "选择商户";
        search.popConfig.open = true;
    },
    SelBrand: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopBrandList/";
        search.popConfig.title = "选择品牌";
        search.popConfig.open = true;
    },
    SelSysuser: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.title = "选择登记人";
        search.popConfig.open = true;
    },
    SelSysuser_sh: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.title = "选择审核人";
        search.popConfig.open = true;
    },
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
                case "选择登记人":
                    search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
                    break;
            }
        }
    }
};

search.addHref = function (row) {
    _.OpenPage({
        id: 105004,
        title: '添加销售补录单',
        url: "SPGL/SALEBILL/SaleBillEdit/"
    })
}