mapShow.beforeVue = function () {

    mapShow.theme3 = "light";
    mapShow.screenParam.showPopShop = false;
    mapShow.screenParam.shopCode = "";
    mapShow.screenParam.srcPopShop = "";//__BaseUrl + "/" + "Pop/Pop/PopFloorMapShow/";
    mapShow.screenParam.popParam = {};

    mapShow.screenParam.branchData = [];
    mapShow.screenParam.regionData = [];
    mapShow.screenParam.floorData = [];
    mapShow.screenParam.mapid = 0;
    mapShow.screenParam.mappath = '/BackMap/10.jpg';
    mapShow.screenParam.widths = 1200;
    mapShow.screenParam.lengths = 600;
    mapShow.screenParam.file = null;
    mapShow.screenParam.loadingStatus = true;
    mapShow.screenParam.YEARMONTH = '';
    mapShow.screenParam.BRANCHID = 0;
    mapShow.screenParam.REGIONID = 0;
    mapShow.screenParam.FLOORID = 0;
    mapShow.screenParam.FXWD = '';

    mapShow.screenParam.FLOORCATEGERY = [];
    mapShow.screenParam.shopRentStatus = [];
    mapShow.screenParam.shopData = [];
    mapShow.screenParam.shopDataInfo = [];
    mapShow.screenParam.colDef = [
                {
                    title: ' ',
                    key: 'action',
                    width: 50,
                    align: 'center',
                    render: function (h, params) {
                        return h('div',
                            [
                            h('Button', {
                                props: { type: 'primary', size: 'small', disabled: false ,shape:"circle"},
                                style: { marginRight: '50px', background: mapShow.screenParam.FLOORCATEGERY[params.index].COLOR },
                                //on: {
                                //    click: function (event) {
                                //        editDetail.dataParam.ASSETCHANGEITEM.splice(params.index, 1);
                                //    }
                                //},
                            }, mapShow.screenParam.FLOORCATEGERY[params.index].CATEGORYNAME.substr(0,1))
                            ]);
                    }
                },
    { title: ' ', key: 'CATEGORYNAME'}

    ];
    mapShow.screenParam.colDef1 = [
                {
                    title: ' ',
                    key: 'action',
                    width: 50,
                    align: 'center',
                    render: function (h, params) {
                        return h('div',
                            [
                            h('Button', {
                                props: { type: 'primary', size: 'small', disabled: false, shape: "circle" },
                                style: { marginRight: '50px', background: mapShow.screenParam.shopRentStatus[params.index].COLOR },
                            }, mapShow.screenParam.shopRentStatus[params.index].STATUSNAME.substr(0, 1))
                            ]);
                    }
                },
    { title: ' ', key: 'STATUSNAME' }

    ];
    mapShow.GetHtml = function (data) {
        return "店铺号:<br>" + data.name
    };
    mapShow.ShowModel = function () {
        mapShow.screenParam.showPopShop = true;
        mapShow.screenParam.popParam = { BRANCHID: mapShow.screenParam.BRANCHID, STATUS: "2" };
    };

    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            mapShow.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                mapShow.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            mapShow.screenParam.BRANCHID = data.dt[0].ID;
        }
        else {

        }
    });
    //_.Ajax('GetRegion', {
    //    Data: { BRANCHID: mapShow.screenParam.BRANCHID }
    //}, function (data) {
    //    if (data.dt) {
    //        mapShow.screenParam.regionData = [];
    //        mapShow.screenParam.REGIONID = 0;

    //        for (var i = 0; i < data.dt.length; i++) {
    //            mapShow.screenParam.regionData.push({ value: data.dt[i].REGIONID, label: data.dt[i].NAME })
    //        }
    //        if (data.dt.length > 0)
    //        {
    //            mapShow.screenParam.REGIONID = data.dt[0].REGIONID;
    //        }
            
    //    }
    //    else {

    //    }
    //});
    //_.Ajax('GetFloor', {
    //    Data: { REGIONID: mapShow.screenParam.REGIONID }
    //}, function (data) {
    //    if (data.dt) {
    //        mapShow.screenParam.floorData = [];
    //        mapShow.screenParam.FLOORID = 0;
    //        for (var i = 0; i < data.dt.length; i++) {
    //            mapShow.screenParam.floorData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
    //        }
    //        if (data.dt.length>0)
    //        {
    //            mapShow.screenParam.FLOORID = data.dt[0].ID;
    //        }
            
    //        //mapShow.showlist();
    //    }
    //    else {

    //    }
    //});
}

mapShow.otherMethods = {
    rowClassName:function (row, index) {
        if (index === 1) {
            return 'background-color: #2db7f5;color: #fff;';
        } else if (index === 3) {
            return 'demo-table-info-row';
        }
        return '';
    },
    branchChange: function (value) {
        mapShow.screenParam.MAPID = "";

        _.Ajax('GetRegion', {
            Data: { BRANCHID: value }
        }, function (Data) {
            if (Data.dt) {
                mapShow.screenParam.regionData = [];
                mapShow.screenParam.REGIONID = 0;
                for (var i = 0; i < Data.dt.length; i++) {
                    mapShow.screenParam.regionData.push({ value: Data.dt[i].REGIONID, label: Data.dt[i].NAME })
                }
                if (Data.dt.length > 0)
                {
                    mapShow.screenParam.REGIONID = Data.dt[0].RGIONID;
                    //mapShow.mapShowParam.REGIONID = Data.dt[0].RGIONID;
                }
            }
            else {

            }
        });
        //_.Ajax('GetFloor', {
        //    Data: { REGIONID: value }
        //}, function (Data) {
        //    if (Data.dt) {
        //        mapShow.screenParam.floorData = [];
        //        mapShow.screenParam.FLOORID = 0;
        //        for (var i = 0; i < Data.dt.length; i++) {
        //            mapShow.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
        //        }
        //        if (Data.dt.length > 0)
        //        {
        //            mapShow.screenParam.FLOORID = Data.dt[0].ID;
        //        }
        //        //mapShow.showlist();
        //    }
        //    else {

        //    }
        //});
    },
    regionChange: function (value) {
        //if (mapShow.screenParam.REGIONID == 0) {
        //    mapShow.mapShowParam.REGIONID = "";
        //}
        //else {
        //    mapShow.mapShowParam.REGIONID = mapShow.screenParam.REGIONID;
        //}
        //if (mapShow.myve.disabled)
        {
            //mapShow.screenParam.SHOPID = "";
             _.Ajax('GetFloor', {
                Data: { REGIONID: value }
            }, function (Data) {
                if (Data.dt) {
                    mapShow.screenParam.floorData = [];
                    mapShow.screenParam.FLOORID = 0;
                    for (var i = 0; i < Data.dt.length; i++) {
                        mapShow.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
                    }
                    if (Data.dt.length > 0) {
                        mapShow.screenParam.FLOORID = Data.dt[0].ID;
                    }
                    //mapShow.showlist();
                }
                else {

                }
            });
        }
    },
    floorChange: function (value) {
        //if (mapShow.myve.disabled) {
        //    //mapShow.screenParam.SHOPID = "";
        //    //mapShow.showlist();
        //}
    },
SearchFloorMap: function (data) {
    if (mapShow.searchParam.YEARMONTH == '') {
        iview.Message.info("请选择年月!");
        return false;
    };
        if (mapShow.screenParam.BRANCHID == 0) {
            iview.Message.info("请选择门店!");
            return false;
        };
        if (mapShow.screenParam.REGIONID == 0) {
            iview.Message.info("请选择区域!");
            return false;
        };
        if (mapShow.screenParam.FLOORID == 0) {
            iview.Message.info("请选择楼层!");
            return false;
        };

    _.Ajax('SearchFloorShowMapData', {
        Data: { FLOORID: mapShow.searchParam.FLOORID, YEARMONTH: mapShow.searchParam.YEARMONTH }
    }, function (data) {
        mapShow.screenParam.mapid = data.floormap.MAPID;
        mapShow.screenParam.mappath = '../../../BackMap/' + data.floormap.FILENAME;
        mapShow.screenParam.widths = data.floormap.WIDTHS;
        mapShow.screenParam.lengths = data.floormap.LENGTHS;
        mapShow.screenParam.FLOORCATEGERY = data.floorcategory;
        mapShow.screenParam.shopRentStatus = data.floorshoprent_status;
        mapShow.screenParam.shopDataInfo = data.floorshopdata;        
        mapShow.screenParam.FXWD = 'YTFB';

        for (var i = 0; i < data.floorshopdata.length; i++) {
            mapShow.screenParam.shopData.push({
                name: data.floorshopdata[i].SHOPCODE,
                html: mapShow.GetHtml,
                x: data.floorshopdata[i].P_X,
                y: data.floorshopdata[i].P_Y,
                catrgoryname: data.floorshopdata[i].CATEGORYNAME,
                color: data.floorshopdata[i].COLOR
            });
        };
        mapShow.screenParam.options = {
            Url: mapShow.screenParam.mappath,  //底图
            width: mapShow.screenParam.widths,   //这个尺寸是显示的尺寸,多大都行,不影响底下的坐标
            height: mapShow.screenParam.lengths,
            canEdit: false,   
            data: mapShow.screenParam.shopData
        };
        mapShow.screenParam.map = $("#div_map").zMapPoint(mapShow.screenParam.options);
    });
},
PopVisibleChange: function (data) {
    if (mapShow.screenParam.showPopShop == true)
    {        
        for (var i = 0; i < mapShow.screenParam.shopDataInfo.length; i++) {
            if (mapShow.screenParam.shopDataInfo[i].SHOPCODE == mapShow.screenParam.shopCode)
            {
                mapShow.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopFloorMapShow/";
                mapShow.screenParam.popParam = {
                    SHOPCODE: mapShow.screenParam.shopDataInfo[i].SHOPCODE,
                    STATUSMC: mapShow.screenParam.shopDataInfo[i].STATUSMC,
                    RENTAREA:mapShow.screenParam.shopDataInfo[i].RENTAREA,
                    CATEGORYNAME:mapShow.screenParam.shopDataInfo[i].CATEGORYNAME,
                    BRANDNAME:mapShow.screenParam.shopDataInfo[i].BRANDNAME ,
                    MERCHANTNAME:mapShow.screenParam.shopDataInfo[i].MERCHANTNAME,
                    CONTRACTID:mapShow.screenParam.shopDataInfo[i].CONTRACTID ,
                    CONT_S_E:mapShow.screenParam.shopDataInfo[i].CONT_S_E ,
                    OPERATERULENAME: mapShow.screenParam.shopDataInfo[i].OPERATERULENAME,
                    QZRQ:mapShow.screenParam.shopDataInfo[i].QZRQ ,
                    RENT:mapShow.screenParam.shopDataInfo[i].RENT ,
                    RENEFFECT: mapShow.screenParam.shopDataInfo[i].RENEFFECT ,
                    AMOUNT:mapShow.screenParam.shopDataInfo[i].AMOUNT ,
                    AMOUNTEFFECT:mapShow.screenParam.shopDataInfo[i].AMOUNTEFFECT,
                    DISCRIPTION: mapShow.screenParam.shopDataInfo[i].DISCRIPTION 
                }
                }
            };


    }
    else
    {
        mapShow.screenParam.srcPopShop = "";
    }
}
}
;







