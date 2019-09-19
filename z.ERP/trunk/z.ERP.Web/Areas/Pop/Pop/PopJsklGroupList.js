search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "扣率组编号", key: 'GROUPNO', width: 100 },
        { title: '基础扣率', key: 'JSKL', width: 100 },
        { title: '描述', key: 'DESCRIPTION', width: 200 },        
    ];
    search.service = "DataService";
    search.method = "GetJsklGroup";
}
search.initSearchParam = function () {
    search.searchParam.GROUPNO = "";
    search.searchParam.DESCRIPTION = "";
}