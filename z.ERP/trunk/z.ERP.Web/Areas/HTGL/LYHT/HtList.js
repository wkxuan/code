search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '租约号', key: 'CONTRACTID', width: 95, sortable: true },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '资产代码', key: 'SHOPDM', width: 110, sortable: true },
        { title: '品牌名称', key: 'BRANDNAME', width: 110, ellipsis: true },
        { title: '核算方式', key: 'STYLEMC', width: 95 },
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'MERNAME', width: 200, ellipsis: true },
        { title: '合同员', key: 'SIGNER_NAME', width: 90 },
        { title: '登记人', key: 'REPORTER_NAME', width: 90 },
        { title: '登记时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90, },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: "分店代码", key: 'BRANCHID', width: 90 },
        { title: '分店名称', key: 'NAME', width: 150 },
      /*  {
            title: '变更', key: 'action', width: 70,
            align: 'center', fixed: 'right',
            render: function (h, params) {
                if (!CanBg) {
                    return h('div',
                        []
                    );
                }
                else {
                    return h('div',
                        [
                            h('Button',
                            {

                                props: { type: 'primary', size: 'small', disabled: false },
                                style: { marginRight: '5px' },

                                on: { click: function (event) { search.bgHref(params.row, params.index) } },

                            }, '变更')
                        ]
                    );
                }
            }
        } */

    ];
    search.searchParam.STYLE = "2"; //只查询联营合同
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "HtglService";
    search.method = "GetContract";

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
            id: 10600100,
            title: '浏览联营租约详情',
            url: "HTGL/LYHT/HtDetail/" + row.CONTRACTID
        })
    }
    else {
        _.OpenPage({
            id: 10600100,
            title: '浏览联营租约变更详情',
            url: "HTGL/LYHT_BG/LyHt_BgDetail/" + row.CONTRACTID
        })
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10600101,
        title: '新增联营租约',
        url: "HTGL/LYHT/HtEdit/"
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
                        id: 10600101,
                        title: '编辑联营租约',
                        url: "HTGL/LYHT/HtEdit/" + row.CONTRACTID
                    });
                };
                if (row.HTLX == 2) {
                    _.OpenPage({
                        id: 10600103,
                        title: '变更联营租约',
                        url: "HTGL/LYHT_BG/LyHt_BgDetail/" + row.CONTRACTID
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
                url: "HTGL/LYHT_BG/LyHt_BgEdit/" + row.CONTRACTID
            });
        };
    };
};