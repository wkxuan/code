defineNew.beforeVue = function () {
    defineNew.service = "CxglService";
    defineNew.method = "SearchPromotion";
    defineNew.screenParam.defineDetailSrc = "";
    defineNew.screenParam.title = "促销活动主题信息";
    defineNew.screenParam.showDefineDetail = false;

    defineNew.columnsDef = [
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
                defineNew.screenParam.defineDetailSrc = __BaseUrl + "/CXGL/PROMOTION/PromotionDetail/" + row.ID;
                defineNew.screenParam.showDefineDetail = true;
            }
        }];
    defineNew.searchParam.START_DATE_START = "";
    defineNew.searchParam.START_DATE_END = "";
    defineNew.searchParam.END_DATE_START = "";
    defineNew.searchParam.END_DATE_END = "";
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
    defineNew.screenParam.defineDetailSrc = __BaseUrl + "/CXGL/PROMOTION/PromotionDetail/";
    defineNew.screenParam.showDefineDetail = true;
};

defineNew.popCallBack = function (data) {
    if (defineNew.screenParam.showDefineDetail) {
        defineNew.screenParam.showDefineDetail = false;
        defineNew.searchList();
    }
};