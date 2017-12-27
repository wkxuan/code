define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: 'POS终端号', key: 'STATIONBH', width: 150 }
    ]
    define.screenParam.dataDef = [];
    define.screenParam.data = [
        {
            key: 1,
            label: 'Content1 ',
            description: 'The desc of content'
        },
        {
            key: 2,
            label: 'Content2 ',
            description: 'The desc of content'
        },
        {
            key: 3,
            label: 'Content3 ',
            description: 'The desc of content'
        },
    ]

    define.service = "XtglService";
    define.method = "GetPos";
    define.methodList = "GetPos";
    define.Key = 'STATIONBH';
}
