search.beforeVue = function () {
    search.searchParam.MAPID = "";
    var col = [

        { title: "图纸编号", key: 'MAPID', width: 105, sortable: true },
        { title: '楼层', key: 'FLOORNAME', width: 200 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true }
    ];
    
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "DpglService";
    search.method = "GetFloorMap";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10200101,
        title: '楼层分析图纸定义',
        url: "BJGL/FLOORMAP/FloorMapEdit/"
    });
};
search.modHref = function (row, index) {
    _.OpenPage({
        id: 10200101,
        title: '编辑楼层分析图纸',
        url: "BJGL/FLOORMAP/FloorMapEdit/" + row.MAPID
    });
};

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 10200102,
        title: '浏览图纸信息',
        url: "BJGL/FLOORMAP/FloorMapDetail/" + row.MAPID
    });
};



