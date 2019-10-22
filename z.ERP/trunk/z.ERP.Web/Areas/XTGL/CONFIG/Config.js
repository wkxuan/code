search.beforeVue = function () {
    search.service = "XtglService";
    search.method = "GetConfig";
    search.indexShow = true;
    search.selectionShow = false;

    search.screenParam.colDef = [
        { title: "描述", key: "DESCRIPTION" },
        { title: "当前值", key: "CUR_VAL", width: 100 },
        { title: "缺省值", key: "DEF_VAL", width: 100 },
        { title: "最大值", key: "MAX_VAL", width: 100 },
        { title: "最小值", key: "MIN_VAL", width: 100 },
        { title: "编号", key: "ID", width: 100 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
               search.popConfig.title = "系统参数定义";
               search.popConfig.height = 350;
               search.popConfig.src = __BaseUrl + "/XTGL/CONFIG/ConfigDetail/" + row.ID;
               search.popConfig.open = true;
           }
        }];
}
search.newCondition = function () {
    search.searchParam.DESCRIPTION = "";
    search.searchParam.ID = "";
};
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        enabled: function () {
            return false;
        },
        authority: ""
    }, {
        id: "del",
        enabled: function () {
            return false;
        },
        authority: ""
    }];
};