search.beforeVue = function () {
    search.service = "XtglService";
    search.method = "GetBranch";
    search.indexShow = true;
    search.selectionShow = false;
    search.popConfig = {
        title: "门店定义",
        src: "",
        width: 800,
        height: 350,
        open: false
    };
    search.screenParam.colDef = [
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
            title: '操作', key: 'operate', authority: "", onClick: function (index, row, data) {
                search.popConfig.src = __BaseUrl + "/XTGL/BRANCH/BranchDetail/" + row.ID;
                search.popConfig.open = true;
            }
        }];
}
search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/XTGL/BRANCH/BranchDetail/";
    search.popConfig.open = true;
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
        authority: ""
    }, {
        id: "del",
        enabled: function () {
            return false;
        },
        authority: ""
    }];
};