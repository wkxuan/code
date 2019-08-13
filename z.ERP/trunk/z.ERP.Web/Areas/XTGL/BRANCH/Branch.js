defineNew.beforeVue = function () {
    defineNew.service = "XtglService";
    defineNew.method = "GetBranch";
    defineNew.screenParam.defineDetailSrc = null;
    defineNew.screenParam.showDefineDetail = false;
    defineNew.screenParam.title = "门店定义";

    defineNew.columnsDef = [
        { title: "门店代码", key: "ID", width: 100 },
        { title: "门店名称", key: "NAME", width: 160 },
        { title: "管理部门", key: "ORGNAME", width: 160 },
        { title: '建筑面积', key: 'AREA_BUILD' },
        { title: '使用面积', key: 'AREA_USABLE' },
        { title: '租用面积', key: 'AREA_RENTABLE' },
        { title: "停用标记", key: "STATUSMC" },
        { title: "打印标题", key: "PRINTNAME" },
        { title: "联系人", key: "CONTACT" },
        { title: "电话", key: "CONTACT_NUM" },
        { title: "银行", key: "BANK" },
        { title: "账户", key: "ACCOUNT" },
        { title: "地址", key: "ADDRESS" },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/BRANCH/BranchDetail/" + row.ID;
                defineNew.screenParam.showDefineDetail = true;
            }
        }];
}
defineNew.add = function () {
    defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/BRANCH/BranchDetail/";
    defineNew.screenParam.showDefineDetail = true;
};

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
        authority: ""
    }, {
        id: "del",
        enabled: function () {
            return false;
        },
        authority: ""
    }];
};