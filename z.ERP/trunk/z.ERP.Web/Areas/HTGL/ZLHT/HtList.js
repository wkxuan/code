search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '合同号', key: 'CONTRACTID', width: 95, sortable: true },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '资产代码', key: 'SHOPDM', width: 110, sortable: true },
        { title: '品牌名称', key: 'BRANDNAME', width: 110, ellipsis: true },
        { title: '核算方式', key: 'STYLEMC', width: 95 },
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'MERNAME', width: 200, ellipsis:true },
        { title: '录入员', key :'REPORTER_NAME',width:90},
        { title: '录入时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90, },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: "分店代码", key: 'BRANCHID', width: 90 },
        { title: '分店名称', key: 'NAME', width: 150},
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
    search.searchParam.STYLE = "1";
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul); 
    search.service = "HtglService";
    search.method = "GetContract";
}

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




