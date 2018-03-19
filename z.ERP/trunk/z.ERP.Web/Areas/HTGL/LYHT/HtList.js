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
//浏览双击跳转页面
search.browseHref = function (row, index) {
    _.OpenPage({
        id: 1,
        title: '合同详情',
        url: "HTGL/LYHT/HtDetail/" + row.CONTRACTID
    });
}
//添加跳转页面
search.addHref = function (row) {
    //联营
    _.OpenPage("HTGL/LYHT/HtEdit/", function (data) {
    });
}

//修改跳转页面,并且要根据单号查出来相关的数据信息
search.modHref = function (row, index) {
    _.Search({
        Service: search.service,
        Method: search.method,
        Data: { CONTRACTID: row.CONTRACTID },
        Success: function (data) {
            if (data.rows[0].STATUS == 1) {
                if (data.rows[0].STYLE == 2) {
                    _.OpenPage("HTGL/LYHT/HtEdit/" + row.CONTRACTID, function (data) {
                    });
                };
                if (data.rows[0].STYLE == 1) {
                    _.OpenPage("HTGL/ZLHT/HtEdit/" + row.CONTRACTID, function (data) {
                    });
                }
            } else {
                iview.Message.info('当前商户信息不是未审核状态,不能编辑!');
                return;
            }
        }
    })
}




