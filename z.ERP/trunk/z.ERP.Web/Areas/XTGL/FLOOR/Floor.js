defineNew.beforeVue = function () {
    defineNew.service = "DpglService";
    defineNew.method = "SearchFloor";
    defineNew.screenParam.defineDetailSrc = null;
    defineNew.screenParam.showDefineDetail = false;
    defineNew.screenParam.title = "楼层信息";
    defineNew.screenParam.branchData = [];
    defineNew.screenParam.regionData = [];

    defineNew.columnsDef = [
       { title: "楼层代码", key: 'CODE' },
       { title: '楼层名称', key: 'NAME' },
       { title: '门店', key: 'BRANCHNAME' },
       { title: '区域', key: 'REGIONNAME' },
       { title: '管理机构', key: 'ORGNAME' },
       { title: '建筑面积', key: 'AREA_BUILD' },
       { title: '租用面积', key: 'AREA_RENTABLE' },
       { title: '使用面积', key: 'AREA_USABLE' },      
       { title: '使用标记', key: 'STATUSMC' },
       {
           title: '操作', key: 'operate', onClick: function (index, row, data) {
               defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/FLOOR/FloorDetail/" + row.ID;
               defineNew.screenParam.showDefineDetail = true;
           }
       }];
}

defineNew.mountedInit = function () {
    defineNew.otherMethods.initBranch();

    defineNew.btnConfig = [{
        id: "select",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        authority: ""
    }, {
        id: "del",
        authority: ""
    }];
};

defineNew.add = function () {
    defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/FLOOR/FloorDetail/";
    defineNew.screenParam.showDefineDetail = true;
};

defineNew.popCallBack = function (data) {
    if (defineNew.screenParam.showDefineDetail) {
        defineNew.screenParam.showDefineDetail = false;
        defineNew.searchList();
    }
};

defineNew.otherMethods = {
    branchChange: function () {
        defineNew.otherMethods.initRegion();
        defineNew.searchParam.REGIONID = undefined;
    },
    initBranch: function () {
        _.Ajax('GetBranch', {
            Data: { ID: "" }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                defineNew.screenParam.branchData = [];
                for (var i = 0; i < dt.length; i++) {
                    defineNew.screenParam.branchData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
    initRegion: function () {
        _.Ajax('GetRegion', {
            Data: { BRANCHID: defineNew.searchParam.BRANCHID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                defineNew.screenParam.regionData = [];
                for (var i = 0; i < dt.length; i++) {
                    defineNew.screenParam.regionData.push({ value: dt[i].REGIONID, label: dt[i].NAME })
                }
            }
        });
    }
}