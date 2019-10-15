search.beforeVue = function () {
    search.service = "CxglService";
    search.method = "Present";
    search.indexShow = true;
    search.selectionShow = false;
    search.popConfig = {
        title: "赠品定义",
        src: "",
        width: 350,
        height: 250,
        open: false
    };
    search.screenParam.colDef = [
        { title: '门店编号', key: 'BRANCHID' },
        { title: '门店名称', key: 'BRANCHNAME' },
        { title: '赠品编号', key: 'ID' },
        { title: '赠品名称', key: 'NAME' },
        { title: '价格', key: 'PRICE' },
        { title: '状态', key: 'STATUSMC' },
        {
            title: '操作', key: 'operate', authority: "", onClick: function (index, row, data) {
                search.popConfig.src = __BaseUrl + "/CXGL/PRESENT/PresentDetail/" + row.ID;
                search.popConfig.open = true;
            }
        }];
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
search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/CXGL/PRESENT/PresentDetail/";
    search.popConfig.open = true;
};

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};