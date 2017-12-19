define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '年月', key: 'YEARMONTH', width: 150 },
        { title: '开始日期', key: 'DATE_START', width: 150 },
        { title: '结束日期', key: 'DATE_END', width: 150 },
    ]
    define.screenParam.dataDef = [];
}

define.search = function () {
    define.screenParam.dataDef = [];
}