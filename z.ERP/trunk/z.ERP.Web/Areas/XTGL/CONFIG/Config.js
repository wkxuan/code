defineNew.beforeVue = function () {
    defineNew.service = "XtglService";
    defineNew.method = "GetConfig";
    defineNew.screenParam.defineDetailSrc = null;
    defineNew.screenParam.showDefineDetail = false;
    defineNew.screenParam.title = "系统参数定义";

    defineNew.columnsDef = [
        { title: "描述", key: "DESCRIPTION" },
        { title: "当前值", key: "CUR_VAL", width: 100 },
        { title: "缺省值", key: "DEF_VAL", width: 100 },
        { title: "最大值", key: "MAX_VAL", width: 100 },
        { title: "最小值", key: "MIN_VAL", width: 100 },
        { title: "编号", key: "ID", width: 100 },
        {
           title: '操作', key: 'operate', onClick: function (index, row, data) {
               defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/CONFIG/ConfigDetail/" + row.ID;
               defineNew.screenParam.showDefineDetail = true;
           }
        }];
}

defineNew.popCallBack = function (data) {
    if (defineNew.screenParam.showDefineDetail) {
        defineNew.screenParam.showDefineDetail = false;
        defineNew.searchList();
    }
};
defineNew.mountedInit = function () {
    defineNew.btnConfig = [{
        id: "select",
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
