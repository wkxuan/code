defineNew.beforeVue = function () {
    defineNew.service = "DpglService";
    defineNew.method = "SearchShop";
    defineNew.screenParam.defineDetailSrc = null;
    defineNew.screenParam.showDefineDetail = false;
    defineNew.screenParam.title = "资产单元信息";
    defineNew.screenParam.branchData = [];
    defineNew.screenParam.regionData = [];
    defineNew.screenParam.floorData = [];

    defineNew.columnsDef = [
        { title: "代码", key: 'CODE' },
        { title: '名称', key: 'NAME' },
        { title: '门店', key: 'BRANCHNAME' },
        { title: '区域', key: 'REGIONNAME' },
        { title: '楼层', key: 'FLOORNAME' },
        { title: '单元类型', key: 'TYPEMC' },
        { title: '管理机构', key: 'ORGNAME' },
        { title: '业态', key: 'CATEGORYNAME' },
        { title: '租用面积', key: 'AREA_RENTABLE' },
        { title: '使用面积', key: 'AREA_USABLE' },
        { title: '建筑面积', key: 'AREA_BUILD' },
        { title: '单元状态', key: 'STATUSMC' },
        { title: '租用状态', key: 'RENT_STATUSMC' },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/SHOP/ShopDetail/" + row.SHOPID;
                defineNew.screenParam.showDefineDetail = true;
            }
        }];
};

defineNew.otherMethods = {
    branchChange: function () {
        defineNew.otherMethods.initRegion();
        defineNew.searchParam.REGIONID = null;
        defineNew.searchParam.FLOORID = null;
    },
    regionChange: function () {
        defineNew.otherMethods.initFloor();
        defineNew.searchParam.FLOORID = null;
    },
    floorChange: function () { },
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
    },
    initFloor: function () {
        _.Ajax('GetFloor', {
            Data: { REGIONID: defineNew.searchParam.REGIONID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                defineNew.screenParam.floorData = [];
                for (var i = 0; i < dt.length; i++) {
                    defineNew.screenParam.floorData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
};

defineNew.mountedInit = function () {
    defineNew.otherMethods.initBranch();
};

defineNew.add = function () {
    defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/SHOP/ShopDetail/";
    defineNew.screenParam.showDefineDetail = true;
};

defineNew.popCallBack = function (data) {
    if (defineNew.screenParam.showDefineDetail) {
        defineNew.screenParam.showDefineDetail = false;
        defineNew.searchList();
    }
};