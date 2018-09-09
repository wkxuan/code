search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '核算方式', key: 'STYLEMC', width: 100 },
        { title: "商户代码", key: 'MERCHANTID', width: 90 },
        { title: '商户名称', key: 'MERNAME', width: 100 },
        { title: '合同号', key: 'CONTRACTID', width: 100 },
        { title: "分店代码", key: 'BRANCHID', width: 90 },
        { title: '分店名称', key: 'NAME', width: 100 },
        { title: '描述', key: 'DESCRIPTION', width: 200 },
        /*{
            title: '变更', key: 'action', width: 80,
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
        }*/
    ];
    search.searchParam.STYLE = "2";
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "HtglService";
    search.method = "GetContract";
}

search.browseHref = function (row, index) {
    if (row.HTLX == 1) {
        _.OpenPage({
            id: 10600102,
            title: '联营租约详情',
            url: "HTGL/LYHT/HtDetail/" + row.CONTRACTID
        });
    }
    else {
        _.OpenPage({
            id: 10600102,
            title: '联营变更租约详情',
            url: "HTGL/LYHT_BG/LyHt_BgDetail/" + row.CONTRACTID
        });
    };
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
                        title: '修改联营租约',
                        url: "HTGL/LYHT/HtEdit/" + row.CONTRACTID
                    });
                };
                if (row.HTLX == 2) {
                    _.OpenPage({
                        id: 10600103,
                        title: '变更联营租约',
                        url: "HTGL/LYHT_BG/LyHt_BgEdit/" + row.CONTRACTID
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
                id: 10600103,
                title: '变更联营租约',
                url: "HTGL/LYHT_BG/LyHt_BgEdit/" + row.CONTRACTID
            });
        }
    }
}




