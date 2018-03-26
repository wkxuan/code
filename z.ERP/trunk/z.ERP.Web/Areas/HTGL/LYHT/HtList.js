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

        
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "HtglService";
    search.method = "GetContract";
}

search.browseHref = function (row, index) {
    if (row.STYLE == 2) {
        _.OpenPage({
            id: 10600101,
            title: '联营租约详情',
            url: "HTGL/LYHT/HtDetail/" + row.CONTRACTID
        });
    };
    if (row.STYLE == 1) {
        _.OpenPage({
            id: 10600201,
            title: '租赁租约详情',
            url: "HTGL/ZLHT/HtDetail/" + row.CONTRACTID
        })
    };
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10600102,
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
                if (data.rows[0].STYLE == 2) {
                    _.OpenPage({
                        id: 10600103,
                        title: '修改联营租约',
                        url: "HTGL/LYHT/HtEdit/" + row.CONTRACTID
                    });
                };
                if (data.rows[0].STYLE == 1) {
                    _.OpenPage({
                        id: 10600203,
                        title: '修改租赁租约',
                        url: "HTGL/ZLHT/HtEdit/" + row.CONTRACTID
                    });
                }
            } else {
                iview.Message.info('当前租约信息不是未审核状态,不能编辑!');
                return;
            }
        }
    })
}




