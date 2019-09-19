search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '品牌代码', key: 'ID', width: 150, },
        { title: '品牌名称', key: 'NAME' },
    ];
    search.service = "XtglService";
    search.method = "GetBrandData";
}
search.initSearchParam = function () {
    search.searchParam.ID = "";
    search.searchParam.NAME = "";
}