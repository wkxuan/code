search.beforeVue = function () {
    search.screenParam.colDef = [
        {
            title: '编码',
            key: 'ID', width: 80
        }, {
            title: "方案名称",
            key: 'NAME', tooltip: true
        },
        {
            title: "满减方式",
            key: 'FRTYPEMC', width: 120
        },
        {
            title: "状态",
            key: 'STATUSMC', width: 100
        }];
    search.service = "CxglService";
    search.method = "GetFRPLAN";
}
search.initSearchParam = function () {
    search.searchParam.TRIMID = "";
    search.searchParam.NAME = "";
    search.searchParam.FRTYPE = "";
    search.searchParam.STATUS = "";
}


