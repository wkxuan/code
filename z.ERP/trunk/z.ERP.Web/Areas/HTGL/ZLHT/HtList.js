﻿search.beforeVue = function () {
    search.service = "HtglService";
    search.method = "GetContract";
    search.screenParam.colDef = [
        { title: '租约号', key: 'CONTRACTID', sortable: true },
        { title: '状态', key: 'STATUSMC' },
        { title: '资产代码', key: 'SHOPDM', sortable: true },
        { title: '品牌名称', key: 'BRANDNAME' },
        { title: '核算方式', key: 'STYLEMC' },
        { title: "商户代码", key: 'MERCHANTID', sortable: true },
        { title: '商户名称', key: 'MERNAME', width: 200 },
        { title: '合同员', key: 'SIGNER_NAME' },
        { title: '登记人', key: 'REPORTER_NAME' },
        { title: '登记时间', key: 'REPORTER_TIME', cellType: "datetime", width: 160, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', },
        { title: '审核时间', key: 'VERIFY_TIME', cellType: "datetime", width: 160, sortable: true },
        { title: "门店代码", key: 'BRANCHID' },
        { title: '门店名称', key: 'NAME' },
        {
            title: '操作', key: 'operate', authority: "10600200", onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10600200,
                    title: '租赁租约详情',
                    url: "HTGL/ZLHT/HtEdit/" + row.CONTRACTID
                });
            }
        }
    ];
}

search.newCondition = function () {
    search.searchParam.STYLE = "1";  //只查询租赁合同
    search.searchParam.HTLX = 1; //默认查询原始合同
    search.searchParam.BRANCHID = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.SHOPDM = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.SIGNER_NAME = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.VERIFY_NAME = "";
    search.searchParam.STATUS = "";
}

search.otherMethods = {
    SelSigner: function () {
        search.screenParam.popParam = { USER_TYPE: "7" };
        search.popConfig.title = "选择合同员";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelReporter: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择登记人";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelVerify: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择审核人";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择合同员":
                    search.searchParam.SIGNER_NAME = data.sj[i].USERNAME;
                    break;
                case "选择登记人":
                    search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
                    break
                case "选择商户":
                    search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break;
            }
        }
    }
};

search.addHref = function (row) {
    _.OpenPage({
        id: 10600201,
        title: '增加租赁租约',
        url: "HTGL/ZLHT/HtEdit/"
    });

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
        authority: ""
    }, {
        id: "del",
        authority: ""
    }, {
        id: "export",
        authority: "",
        fun: function () {
            let selection = search.vueObj.$refs.selectData.getSelection();
            if (selection.length == 0) {
                iview.Message.info("请选中要导出的合同信息!");
            } else {
                _.Ajax('Output', {
                    Data: selection
                }, function (data) {
                    if (data) {
                        window.location.href = __BaseUrl + data;
                    }
                });
            }
        },
    }];
}