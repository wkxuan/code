defineNew.beforeVue = function () {
    defineNew.service = "CxglService";
    defineNew.method = "Present";
    defineNew.screenParam.defineDetailSrc = null;
    defineNew.screenParam.showDefineDetail = false;
    defineNew.screenParam.title = "赠品定义";
    defineNew.screenParam.branchData = [];
    defineNew.key = 'ID';
    defineNew.screenParam.STATUSMC = "未使用";

    defineNew.columnsDef = [
                { title: '门店编号', key: 'BRANCHID' },
            { title: '门店名称', key: 'BRANCHNAME' },
            { title: '赠品编号', key: 'ID' },
            { title: '赠品名称', key: 'NAME' },
            { title: '价格', key: 'PRICE' },
            { title: '状态', key: 'STATUSMC' },

        {
            title: '操作', key: 'operate', authority: "104004", onClick: function (index, row, data) {
                defineNew.screenParam.defineDetailSrc = __BaseUrl + "/CXGL/PRESENT/PresentDetail/" + row.ID;
                defineNew.screenParam.showDefineDetail = true;
            }
        }];
};
defineNew.mountedInit = function () {
  

    defineNew.btnConfig = [{
        id: "select",
        authority: "104004"
    }, {
        id: "clear",
        authority: "104004"
    }, {
        id: "add",
        authority: "104004"
    }, {
        id: "del",
        authority: "104004"
    }];
};
defineNew.add = function () {
    defineNew.screenParam.defineDetailSrc = __BaseUrl + "/CXGL/PRESENT/PresentDetail/";
    defineNew.screenParam.showDefineDetail = true;
};

defineNew.popCallBack = function (data) {
    if (defineNew.screenParam.showDefineDetail) {
        defineNew.screenParam.showDefineDetail = false;
        defineNew.searchList();
    }
};