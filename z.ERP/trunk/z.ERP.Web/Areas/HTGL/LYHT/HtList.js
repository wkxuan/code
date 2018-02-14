search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: "商户代码", key: 'MERCHANTID', width: 90 },
        { title: '商户名称', key: 'NAME', width: 100 },
        { title: '合同号', key: 'CONTRACTID', width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "HtglService";
    search.method = "GetContract";
}
//浏览双击跳转页面
search.browseHref = function (row, index) {
    _.OpenPage("HTGL/LYHT/HtDetail/" + row.CONTRACTID, function (data) {
    });
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("HTGL/LYHT/HtEdit/", function (data) {
    });
}
//修改跳转页面,并且要根据单号查出来相关的数据信息
search.modHref = function (row, index) {

    //1最好是先控制按钮状态,后面想办法模板处理
    //2这块是不是需要发请求去判断处理(根据需要,商户是审核之后也可以修改在保存,所以如何不用发请求)

    //_.Search({
    //    Service: search.service,
    //    Method: search.method,
    //    Data: { MERCHANTID: row.MERCHANTID},
    //    Success: function (data) {
    //        if (data.rows[0].STATUS == 1) {
    //            _.OpenPage("SHGL/MERCHANT/MerchantEdit/" + row.MERCHANTID, function (data) {
    //            });
    //        } else {
    //            iview.Message.info('当前商户信息不是未审核状态,不能编辑!');
    //        }
    //    }
    //})
    _.OpenPage("HTGL/LYHT/HtEdit/" + row.CONTRACTID, function (data) {
    });
}




