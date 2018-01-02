search.beforeVue = function () {
    var col = [
        { title: "代码", key: 'ID', width: 100 },
        { title: '名称', key: 'NAME', width: 200 },
        { title: '业态', key: 'CATEGORYNAME', width: 200 },
        { title: '地址', key: 'ADRESS', width: 200 },
        { title: '联系人', key: 'CONTACTPERSON', width: 100 },
        { title: '电话', key: 'PHONENUM', width: 100 },
        { title: '邮编', key: 'PIZ', width: 100 },
        { title: '微信', key: 'WEIXIN', width: 100 },
        { title: 'QQ', key: 'QQ', width: 100 },
        { title: '描述', key: 'DESCRIPTION', width: 200 },
        { title: '状态', key: 'STATUS', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 200 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 200 },
        { title: '审核人', key: 'VERIFY_NAME', width: 200 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 200 },
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "XtglService";
    search.method = "GetBrandData";
}

search.browseHref = function (row, index) {
    _.OpenPage("PPGL/BRAND/BrandEdit/" + row.ID, function (data) {
    });
}

search.addHref = function (row) {
    _.OpenPage("PPGL/BRAND/BrandEdit/", function (data) {
    });
}