search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "代码", key: 'ID', width: 90, sortable: true },
        { title: '名称', key: 'NAME', width: 200 },
        { title: '业态', key: 'CATEGORYNAME', width: 150 },
        { title: '地址', key: 'ADRESS', width: 200 },
        { title: '联系人', key: 'CONTACTPERSON', width: 100 },
        { title: '电话', key: 'PHONENUM', width: 100 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '邮编', key: 'PIZ', width: 100 },
        { title: '微信', key: 'WEIXIN', width: 100 },
        { title: 'QQ', key: 'QQ', width: 100 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 100 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 100 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: '描述', key: 'DESCRIPTION', width: 200 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 102002,
                    title: '品牌信息',
                    url: "PPGL/BRAND/BrandEdit/" + row.ID
                });
            }
        }
    ];
    search.service = "XtglService";
    search.method = "GetBrandData";

    search.searchParam.CATEGORYCODE = "";
    search.screenParam.CATEGORY = [];
    search.uploadName = "品牌信息";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 102002,
        title: '品牌信息',
        url: "PPGL/BRAND/BrandEdit/"
    });
}

search.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(search.screenParam, "CATEData", data.treeOrg.Obj);
    });

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
        id: "upload",
        authority: ""
    }];
}


search.otherMethods = {
    orgChange: function (value, selectedData) {
        search.searchParam.CATEGORYCODE = selectedData[selectedData.length - 1].code;
    },
}