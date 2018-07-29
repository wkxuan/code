search.beforeVue = function () {
    var col = [
        { title: "扣率组编号", key: 'GROUPNO', width: 100 },
        { title: '基础扣率', key: 'JSKL', width: 100 },
        { title: '描述', key: 'DESCRIPTION', width: 200 },        
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "DataService";
    search.method = "GetJsklGroup";
}


//获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.CONTRACTID = data.CONTRACTID;
    }
}