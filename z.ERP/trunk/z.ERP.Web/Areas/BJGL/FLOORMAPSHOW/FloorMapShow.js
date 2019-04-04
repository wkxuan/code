mapShow.beforeVue = function () {

    mapShow.theme3 = "light";
    mapShow.service = "DpglService";
    mapShow.method = "GetFloorMapData";
    mapShow.Key = 'MAPID';
    mapShow.screenParam.showPopShop = false;
    mapShow.screenParam.selectCode = "";
    mapShow.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopFloorMapShow/";
    mapShow.screenParam.popParam = {};

    mapShow.screenParam.branchData = [];
    mapShow.screenParam.regionData = [];
    mapShow.screenParam.floorData = [];
    mapShow.screenParam.mappath = '/BackMap/10.jpg';
    mapShow.screenParam.widths = 1200;
    mapShow.screenParam.lengths = 600;
    mapShow.screenParam.file = null;
    mapShow.screenParam.loadingStatus = true;
    mapShow.screenParam.BRANCHID = 0;
    mapShow.screenParam.REGIONID = 0;
    mapShow.screenParam.FLOORID = 0;
    mapShow.screenParam.FLOORCATEGERY = [];
    mapShow.screenParam.SHOPDATA = [];
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
                            }, mapShow.screenParam.FLOORCATEGERY[params.index].SHOPCODE.substr(0,1))
                            ]);
                    }
                },
    { title: ' ', key: 'SHOPCODE', width: 150 }

    ];
    mapShow.GetHtml = function (data) {
        return "店铺号:<br>" + data.name
    };
    mapShow.ShowModel = function () {
        mapShow.screenParam.showPopShop = true;
        mapShow.screenParam.popParam = { BRANCHID: mapShow.screenParam.BRANCHID, STATUS: "2" };
    };
    mapShow.screenParam.options = {
        Url: mapShow.screenParam.mappath,  //底图
        width: mapShow.screenParam.widths,   //这个尺寸是显示的尺寸,多大都行,不影响底下的坐标
        height: mapShow.screenParam.lengths,
        canEdit: false,   //是否可以编辑,如果不可以编辑,就不能拖动,删除,新增,不能编辑状态是用来展示用的
        //假设一些数据,这些是要从后台取到的
        //data: [    //这里的数据,不能删,但是可以任意的加,不要占用这4个属性就行了,方便显示的时候通过GetHtml渲染数据
        //    {
        //        name: "aaaaaaa",
        //        x: 0.16,   //这俩是坐标,是个相对坐标,所以图片尺寸变了也没关系,存库就存这个,把这俩拼成一个字段存起来也行
        //        y: 0.62,
        //        html: mapShow.GetHtml
        //    }
        //]
        data: mapShow.screenParam.SHOPDATA,
        showmodel: mapShow.screenParam.showPopShop
    };
    mapShow.screenParam.map = $("#div_map").zMapPoint(mapShow.screenParam.options);
    //加载底图按钮的验证
    mapShow.screenParam.LoadMap=function(){
    if (mapShow.screenParam.BRANCHID == 0) {
        iview.Message.info("请选择门店!");
        return ;
    };
        if (mapShow.screenParam.REGIONID == 0) {
            iview.Message.info("请选择区域!");
            return ;
        };
        if (mapShow.screenParam.FLOORID == 0) {
            iview.Message.info("请选择楼层!");
            return ;
        };

        //if (tzbj != "" && tzbj != null) {
        //    var datain = { op: 'ISCANADD', floorid: floorid };
        //    $.Baseutils.ajax(ajaxurl, datain, function (data) {
        //        if (data > 0 || data > "0") {
        //            $.Baseutils.alerterror("该楼层已有待启动布局图");
        //            return false;
        //        } else {
        //            var dt = $("#fileQueue").html();
        //            if ($.trim(dt) == "") {
        //                $.Baseutils.alerterror("请选择底图");
        //                return false;
        //            }
        //        }
        //    });
        //}
            var dt = $("#fileQueue").html();
            if ($.trim(dt) == "") {
                iview.Message.info("请选择上传文件");
                return false;
            }


        //保存一些信息 分店 楼层
        var msg = fdbh + "_" + floorid;
        msg += "$" + $("#txtqdrq").val();
        $('#uploadify').uploadifyUpload();
    }

    ///添加热点
    mapShow.screenParam.addPoint = function () {
        var newname = prompt("请输入店铺号", "");
        mapShow.screenParam.SHOPDATA.push({
            name: newname,
            html: mapShow.GetHtml,
            x: 0.01,
            y: 0.03
        });
        mapShow.screenParam.FLOORSHOP.push({
            SHOPCODE: newname
        });
        mapShow.screenParam.map = $("#div_map").zMapPoint(mapShow.screenParam.options);

    }

    ///添加一行
    mapShow.screenParam.savePoint = function () {
        var a = mapShow.screenParam.map.GetData();
    }


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
    _.Ajax('GetRegion', {
        Data: { BRANCHID: mapShow.screenParam.BRANCHID }
    }, function (data) {
        if (data.dt) {
            mapShow.screenParam.regionData = [];
            mapShow.screenParam.REGIONID = 0;

            for (var i = 0; i < data.dt.length; i++) {
                mapShow.screenParam.regionData.push({ value: data.dt[i].REGIONID, label: data.dt[i].NAME })
            }
            if (data.dt.length > 0)
            {
                mapShow.screenParam.REGIONID = data.dt[0].REGIONID;
            }
            
        }
        else {

        }
    });
    _.Ajax('GetFloor', {
        Data: { REGIONID: mapShow.screenParam.REGIONID }
    }, function (data) {
        if (data.dt) {
            mapShow.screenParam.floorData = [];
            mapShow.screenParam.FLOORID = 0;
            for (var i = 0; i < data.dt.length; i++) {
                mapShow.screenParam.floorData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            if (data.dt.length>0)
            {
                mapShow.screenParam.FLOORID = data.dt[0].ID;
            }
            
            //mapShow.showlist();
        }
        else {

        }
    });
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
        _.Ajax('GetFloor', {
            Data: { REGIONID: value }
        }, function (Data) {
            if (Data.dt) {
                mapShow.screenParam.floorData = [];
                mapShow.screenParam.FLOORID = 0;
                for (var i = 0; i < Data.dt.length; i++) {
                    mapShow.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
                }
                if (Data.dt.length > 0)
                {
                    mapShow.screenParam.FLOORID = Data.dt[0].ID;
                }
                //mapShow.showlist();
            }
            else {

            }
        });
    },
    regionChange: function (value) {
        //if (mapShow.screenParam.REGIONID == 0) {
        //    mapShow.mapShowParam.REGIONID = "";
        //}
        //else {
        //    mapShow.mapShowParam.REGIONID = mapShow.screenParam.REGIONID;
        //}
        if (mapShow.myve.disabled) {
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
regionChange: function (value) {
    //if (mapShow.screenParam.REGIONID == 0) {
    //    mapShow.mapShowParam.REGIONID = "";
    //}
    //else {
    //    mapShow.mapShowParam.REGIONID = mapShow.screenParam.REGIONID;
    //}
    if (mapShow.myve.disabled) {
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
SearchFloorMap : function (data) {
    _.Ajax('SearchFloorMap', {
        Data: { MAPID: '15' }
    }, function (data) {
        mapShow.screenParam.FLOORCATEGERY = data.floorshop;
        mapShow.screenParam.mappath = '/BackMap/10.jpg';
        mapShow.screenParam.widths = data.WIDTHS;
        mapShow.screenParam.lengths = data.LENGTHS;
        for (var i = 0; i < data.floorshop.length; i++) {
            mapShow.screenParam.SHOPDATA.push({
                name: data.floorshop[i].SHOPCODE,
                html: mapShow.GetHtml,
                x: data.floorshop[i].P_X,
                y: data.floorshop[i].P_Y
            });
        };
        mapShow.screenParam.map = $("#div_map").zMapPoint(mapShow.screenParam.options);
    });
},
PopVisibleChange: function (data) {
    if (mapShow.screenParam.showPopShop == true)
    {
        mapShow.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopFloorMapShow/";
        iview.Message.info("查数!");
        _.Ajax('SearchFloorMapData', {
            Data: { MAPID: '15' }
        }, function (data) {
            mapShow.screenParam.popParam = { SHOPCODE: 'ASAS' }
            //var _body = window.parent;
            //var _iframe1 = _body.document.getElementById('popshow');
            //_iframe1.contentWindow.location.reload(true);
            //mapShow.screenParam.SHOPCODE = mapShow.screenParam.selectCode;
            //popShow.screenParam.USERCODE = data.floorshopdata[0].SHOPCODE;
            //popShow.screenParam.USERNAME = data.floorshopdata[0].SHOPCODE;
        });
    }
    else
    {
        mapShow.screenParam.srcPopShop = "";
    }
},
//mapShow.IsValidSave = function () {
//    if (mapShow.screenParam.BRANCHID == 0) {
//        iview.Message.info("请选择门店!");
//        return false;
//    };
//    if (mapShow.screenParam.REGIONID == 0) {
//        iview.Message.info("请选择区域!");
//        return false;
//    };
//    if (mapShow.screenParam.FLOORID == 0) {
//        iview.Message.info("请选择楼层!");
//        return false;
//    };
//    mapShow.screenParam.FLOORSHOP = [];
//    var data = mapShow.screenParam.map.GetData();

//    for (var i = 0; i < data.length; i++) {
//        //if ((mapShow.screenParam.FLOORSHOP.length === 0)
//        //      || (mapShow.screenParam.FLOORSHOP.length > 0
//        //      && mapShow.screenParam.FLOORSHOP.filter(function (item) {
//        //      return parseInt(item.SHOPCODE) === data[i].name;
//        //}).length === 0)) {
//            var shop = {};
//            shop.SHOPCODE = data[i].name;
//            shop.P_X = data[i].x;
//            shop.P_Y = data[i].y;
//            mapShow.screenParam.FLOORSHOP.push(shop);
//        //}
//    };

//    return true;
//}

}
;







