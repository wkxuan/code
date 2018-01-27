search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: "商户代码", key: 'MERCHANTID', width: 100 },
        { title: '商户名称', key: 'NAME', width: 200 },
        { title: '税号', key: 'SH', width: 200 },
        { title: '银行账号', key: 'BANK', width: 200 },
        { title: '银行', key: 'BANK_NAME', width: 200 },
        { title: '地址', key: 'ADRESS', width: 200 },
        { title: '联系人', key: 'CONTACTPERSON', width: 200 },
        { title: '联系人电话', key: 'PHONE', width: 200 },
        { title: '状态', key: 'STATUS', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 200 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 200 },
        { title: '审核人', key: 'VERIFY_NAME', width: 200 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 200 },
       // { title: '状态', key: 'STATUS', width: 200 },
        { title: '状态', key: 'STATUSMC', width: 200 },
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "ShglService";
    search.method = "GetMerchant";
}
//浏览双击跳转页面
search.browseHref = function (row, index) {
    _.OpenPage("SHGL/MERCHANT/Detail/" + row.MERCHANTID, function (data) {
    });
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("SHGL/MERCHANT/MerchantEdit/", function (data) {
    });
}
//修改跳转页面,并且要根据单号查出来相关的数据信息
search.modHref = function (row, index) {
    if (row.STATUS == 1) {
        _.OpenPage("SHGL/MERCHANT/MerchantEdit/" + row.MERCHANTID, function (data) {
        });
    }
}




