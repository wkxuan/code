search.beforeVue = function () {
    search.searchParam.STYLE = "1";  //只查询租赁合同
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
        { title: "分店代码", key: 'BRANCHID' },
        { title: '分店名称', key: 'NAME' },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10600201,
                    title: '编辑租赁租约',
                    url: "HTGL/ZLHT/HtEdit/" + row.CONTRACTID
                });
            }
        }
    ];

    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};
}

search.otherMethods = {
    SelSigner: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "SIGNER";
        search.screenParam.popParam = { USER_TYPE: "7" };
    },
    SelReporter: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
        search.screenParam.popParam = {};
    },
    SelVerify: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
        search.screenParam.popParam = {};
    },
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "SIGNER") {
                search.searchParam.SIGNER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "REPORTER") {
                search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            }
        };
    }

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};

search.browseHref = function (row, index) {
    if (row.HTLX == 1) {
        _.OpenPage({
            id: 10600200,
            title: '浏览租赁租约详情',
            url: "HTGL/ZLHT/HtDetail/" + row.CONTRACTID
        })
    }
    else {
        _.OpenPage({
            id: 10600200,
            title: '浏览租赁租约变更详情',
            url: "HTGL/ZLHT_BG/ZlHt_BgDetail/" + row.CONTRACTID
        })
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10600201,
        title: '新增租赁租约',
        url: "HTGL/ZLHT/HtEdit/"
    });

}

search.modHref = function (row, index) {
    _.Search({
        Service: search.service,
        Method: search.method,
        Data: { CONTRACTID: row.CONTRACTID },
        Success: function (data) {
            if (data.rows[0].STATUS == 1) {
                if (row.HTLX == 1) {
                    _.OpenPage({
                        id: 10600201,
                        title: '编辑租赁租约',
                        url: "HTGL/ZLHT/HtEdit/" + row.CONTRACTID
                    });
                };
                if (row.HTLX == 2) {
                    _.OpenPage({
                        id: 10600203,
                        title: '变更租赁租约',
                        url: "HTGL/ZLHT_BG/ZlHt_BgEdit/" + row.CONTRACTID
                    });
                };
            } else {
                iview.Message.info('当前租约信息不是未审核状态,不能编辑!');
                return;
            }
        }
    })
}

search.bgHref = function (row, index) {
    if (row.HTLX == 2) {
        iview.Message.info('当前租约已经是变更后的，不能在进行变更!');
        return;
    }
    else {
        if (row.STATUS == 1) {
            iview.Message.info('当前租约尚未审核，要修改请直接编辑!');
            return;
        }
        else {
            _.OpenPage({
                id: 10600203,
                title: '变更租赁租约',
                url: "HTGL/ZLHT_BG/ZlHt_BgEdit/" + row.CONTRACTID
            });
        };
    };
};