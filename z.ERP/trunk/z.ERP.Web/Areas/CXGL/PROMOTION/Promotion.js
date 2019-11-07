search.beforeVue = function () {
    search.service = "CxglService";
    search.method = "SearchPromotion";
    search.indexShow = true;
    search.selectionShow = false;
    search.popConfig = {
        title: "促销活动主题信息",
        src: "",
        width: 800,
        height: 350,
        open: false
    };
    search.screenParam.colDef = [
        { title: "活动ID", key: 'ID' },      
        { title: '主题名称', key: 'NAME' },
        { title: '年度', key: 'YEAR' },
        { title: '内容', key: 'CONTENT',  width: 150 },
        { title: '开始日期', key: 'START_DATE', cellType: "date", width: 150 },
        { title: '结束日期', key: 'END_DATE', cellType: "date", width: 150 },
        { title: '录入人', key: 'REPORTER_NAME' },
        { title: '录入时间', key: 'REPORTER_TIME', width: 150 },
        { title: '审核人', key: 'VERIFY_NAME' },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150 },
        { title: '状态', key: 'STATUSMC' },
        {
            title: '操作', key: 'operate', authority: "104004", onClick: function (index, row, data) {
                search.popConfig.src = __BaseUrl + "/CXGL/PROMOTION/PromotionDetail/" + row.ID;
                search.popConfig.open = true;
            }
        }];

};
search.newCondition = function () {
    search.searchParam.START_DATE_START = "";
    search.searchParam.START_DATE_END = "";
    search.searchParam.END_DATE_START = "";
    search.searchParam.END_DATE_END = "";
    search.searchParam.NAME = "";
    search.searchParam.YEAR = "";
    search.searchParam.CONTENT = "";
    search.searchParam.STATUS = "";
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "11000100"
    }, {
        id: "clear",
        authority: "11000100"
    }, {
        id: "add",
        authority: "11000101"
    }, {
        id: "del",
        authority: "11000101"
    }];
};

search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/CXGL/PROMOTION/PromotionDetail/";
    search.popConfig.open = true;
};

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};