search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "活动ID", key: 'ID' },
        { title: '主题名称', key: 'NAME' },
        { title: '年度', key: 'YEAR' },
        { title: '内容', key: 'CONTENT', width: 150 },
        { title: '开始日期', key: 'START_DATE', cellType: "date", width: 150 },
        { title: '结束日期', key: 'END_DATE', cellType: "date", width: 150 },
        { title: '录入人', key: 'REPORTER_NAME' },
        { title: '录入时间', key: 'REPORTER_TIME', width: 150 },
        { title: '审核人', key: 'VERIFY_NAME' },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150 },
        { title: '状态', key: 'STATUSMC' },
    ];
    search.service = "CxglService";
    search.method = "SearchPromotion";

    search.searchParam.START_DATE_START = "";
    search.searchParam.START_DATE_END = "";
    search.searchParam.END_DATE_START = "";
    search.searchParam.END_DATE_END = "";
}