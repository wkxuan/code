search.beforeVue = function () {
    search.searchParam.MAPID = "";
    var col = [

        { title: "图纸编号", key: 'MAPID', width: 110, sortable: true },
        { title: '分店', key: 'BRANCHNAME', width: 150 },
        { title: '区域', key: 'REGIONNAME', width: 100 },
        { title: '楼层', key: 'FLOORNAME', width: 100 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: '图纸类型', key: 'TZBJMC', width: 105 },
        { title: '原图纸编号', key: 'MAPID_OLD', width: 120 },
    ];
    
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "DpglService";
    search.method = "GetFloorMapAdjust";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 11100201,
        title: '楼层分析图纸定义',
        url: "BJGL/FLOORMAPADJUST/FloorMapAdjustEdit/"
    });
};
search.modHref = function (row, index) {
    _.OpenPage({
        id: 11100201,
        title: '编辑楼层分析图纸',
        url: "BJGL/FLOORMAPADJUST/FloorMapAdjustEdit/" + row.MAPID
    });
};

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 11100202,
        title: '浏览图纸信息',
        url: "BJGL/FLOORMAPADJUST/FloorMapAdjustDetail/" + row.MAPID
    });
};



