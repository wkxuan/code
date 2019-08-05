search.beforeVue = function () {
    search.searchParam.MAPID = "";
    search.screenParam.colDef = [
        { title: "图纸编号", key: 'MAPID', sortable: true },
        { title: '门店', key: 'BRANCHNAME' },
        { title: '区域', key: 'REGIONNAME'},
        { title: '楼层', key: 'FLOORNAME'},
        { title: '状态', key: 'STATUSMC' },
        { title: '编辑人', key: 'REPORTER_NAME' },
        { title: '编辑时间', key: 'REPORTER_TIME', sortable: true },
        { title: '审核人', key: 'VERIFY_NAME'},
        { title: '审核时间', key: 'VERIFY_TIME', sortable: true },       
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 11100101,
                    title: '分析图纸定义',
                    url: "BJGL/FLOORMAP/FloorMapEdit/" + row.MAPID
                });
            }
        }
    ];

    search.service = "DpglService";
    search.method = "GetFloorMap";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 11100101,
        title: '楼层分析图纸定义',
        url: "BJGL/FLOORMAP/FloorMapEdit/"
    });
};



