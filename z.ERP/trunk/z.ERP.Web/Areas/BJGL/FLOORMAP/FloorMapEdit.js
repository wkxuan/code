editDetail.beforeVue = function () {

    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "DpglService";
    editDetail.method = "GetFloorMap";
    editDetail.Key = 'MAPID';
    editDetail.dataParam.STATUS = "1";
    editDetail.screenParam.branchData = [];
    editDetail.screenParam.regionData = [];
    editDetail.screenParam.floorData = [];
    editDetail.screenParam.BRANCHID = 0;
    editDetail.screenParam.mappath = '/BackMap/10.jpg';
    editDetail.screenParam.widths = 1200;
    editDetail.screenParam.lengths = 600;
    editDetail.screenParam.file = null;
    editDetail.screenParam.loadingStatus = true;
    editDetail.dataParam.BRANCHID = 0;
    editDetail.dataParam.REGIONID = 0;
    editDetail.dataParam.FLOORID = 0;
    editDetail.dataParam.FLOORSHOP = [];
    editDetail.screenParam.SHOPDATA = [];
    editDetail.screenParam.colDef = [
    { title: '店铺代码', key: 'SHOPCODE', width: 100 }
    ];
    editDetail.GetHtml = function (data) {
        return "店铺号:<br>" + data.name
    };
    editDetail.screenParam.options = {
        Url: editDetail.screenParam.mappath,  //底图
        width: editDetail.screenParam.widths,   //这个尺寸是显示的尺寸,多大都行,不影响底下的坐标
        height: editDetail.screenParam.lengths,
        canEdit: true,   //是否可以编辑,如果不可以编辑,就不能拖动,删除,新增,不能编辑状态是用来展示用的
        //假设一些数据,这些是要从后台取到的
        //data: [    //这里的数据,不能删,但是可以任意的加,不要占用这4个属性就行了,方便显示的时候通过GetHtml渲染数据
        //    {
        //        name: "aaaaaaa",
        //        x: 0.16,   //这俩是坐标,是个相对坐标,所以图片尺寸变了也没关系,存库就存这个,把这俩拼成一个字段存起来也行
        //        y: 0.62,
        //        html: editDetail.GetHtml
        //    }
        //]
        data: editDetail.screenParam.SHOPDATA
    };
    editDetail.screenParam.map = $("#div_map").zMapPoint(editDetail.screenParam.options);
    //加载底图按钮的验证
    editDetail.screenParam.LoadMap=function(){
    if (editDetail.dataParam.BRANCHID == 0) {
        iview.Message.info("请选择门店!");
        return ;
    };
        if (editDetail.dataParam.REGIONID == 0) {
            iview.Message.info("请选择区域!");
            return ;
        };
        if (editDetail.dataParam.FLOORID == 0) {
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
    editDetail.screenParam.addPoint = function () {
        var newname = prompt("请输入店铺号", "");
        editDetail.screenParam.SHOPDATA.push({
            name: newname,
            html: editDetail.GetHtml,
            x: 0.01,
            y: 0.03
        });
        //editDetail.dataParam.map.Add({
        //    name: newname,
        //    html: editDetail.GetHtml,
        //    x: 0.01,
        //    y: 0.03
        //});
        editDetail.dataParam.FLOORSHOP.push({
            SHOPCODE: newname
        });
        editDetail.screenParam.map = $("#div_map").zMapPoint(editDetail.screenParam.options);

    }

    ///添加一行
    editDetail.screenParam.savePoint = function () {
        var a = editDetail.screenParam.map.GetData();
    }


    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            editDetail.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                editDetail.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            editDetail.dataParam.BRANCHID = data.dt[0].ID;
        }
        else {

        }
    });
    _.Ajax('GetRegion', {
        Data: { BRANCHID: editDetail.dataParam.BRANCHID }
    }, function (data) {
        if (data.dt) {
            editDetail.screenParam.regionData = [];
            editDetail.dataParam.REGIONID = 0;

            for (var i = 0; i < data.dt.length; i++) {
                editDetail.screenParam.regionData.push({ value: data.dt[i].REGIONID, label: data.dt[i].NAME })
            }
            if (data.dt.length > 0)
            {
                editDetail.dataParam.REGIONID = data.dt[0].REGIONID;
            }
            
        }
        else {

        }
    });
    _.Ajax('GetFloor', {
        Data: { REGIONID: editDetail.dataParam.REGIONID }
    }, function (data) {
        if (data.dt) {
            editDetail.screenParam.floorData = [];
            editDetail.dataParam.FLOORID = 0;
            for (var i = 0; i < data.dt.length; i++) {
                editDetail.screenParam.floorData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            if (data.dt.length>0)
            {
                editDetail.dataParam.FLOORID = data.dt[0].ID;
            }
            
            //editDetail.showlist();
        }
        else {

        }
    });
}

editDetail.otherMethods = {
    branchChange: function (value) {
        editDetail.dataParam.MAPID = "";

        _.Ajax('GetRegion', {
            Data: { BRANCHID: value }
        }, function (Data) {
            if (Data.dt) {
                editDetail.screenParam.regionData = [];
                editDetail.dataParam.REGIONID = 0;
                for (var i = 0; i < Data.dt.length; i++) {
                    editDetail.screenParam.regionData.push({ value: Data.dt[i].REGIONID, label: Data.dt[i].NAME })
                }
                if (Data.dt.length > 0)
                {
                    editDetail.dataParam.REGIONID = Data.dt[0].RGIONID;
                    //editDetail.searchParam.REGIONID = Data.dt[0].RGIONID;
                }
            }
            else {

            }
        });
        _.Ajax('GetFloor', {
            Data: { REGIONID: value }
        }, function (Data) {
            if (Data.dt) {
                editDetail.screenParam.floorData = [];
                editDetail.dataParam.FLOORID = 0;
                for (var i = 0; i < Data.dt.length; i++) {
                    editDetail.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
                }
                if (Data.dt.length > 0)
                {
                    editDetail.dataParam.FLOORID = Data.dt[0].ID;
                }
                //editDetail.showlist();
            }
            else {

            }
        });
    },
    regionChange: function (value) {
        if (editDetail.dataParam.REGIONID == 0) {
            editDetail.searchParam.REGIONID = "";
        }
        else {
            editDetail.searchParam.REGIONID = editDetail.dataParam.REGIONID;
        }
        if (editDetail.myve.disabled) {
            //editDetail.dataParam.SHOPID = "";
             _.Ajax('GetFloor', {
                Data: { REGIONID: value }
            }, function (Data) {
                if (Data.dt) {
                    editDetail.screenParam.floorData = [];
                    editDetail.dataParam.FLOORID = 0;
                    for (var i = 0; i < Data.dt.length; i++) {
                        editDetail.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
                    }
                    if (Data.dt.length > 0) {
                        editDetail.dataParam.FLOORID = Data.dt[0].ID;
                    }
                    //editDetail.showlist();
                }
                else {

                }
            });
        }
    },
    floorChange: function (value) {
        //if (editDetail.myve.disabled) {
        //    //editDetail.dataParam.SHOPID = "";
        //    //editDetail.showlist();
        //}
    },
    beforeUpload :function (file) {
        editDetail.screenParam.file = file;
        editDetail.screenParam.filename = file.name;
        editDetail.dataParam.WIDTHS = 1200;
        editDetail.dataParam.LENGTHS = 600;
        _.Ajax('UploadMap', {
            Data: { path: '../../../BackMap' }
        }, function (data) {
            editDetail.screenParam.uploadpath = data.uploadpath;
        });
        editDetail.screenParam.loadingStatus = false;
        return false;
    },
    upload :function() {
        editDetail.screenParam.loadingStatus = true;
        setTimeout(function(){
            //editDetail.screenParam.file = null;
            editDetail.screenParam.mappath = 'BackMap/10.jpg'
            editDetail.screenParam.loadingStatus = false;
            iview.Message.success('上传成功');
        }, 1500);
        editDetail.screenParam.map = $("#div_map").zMapPoint(editDetail.screenParam.options);
    }
}
editDetail.IsValidSave = function () {
    if (editDetail.dataParam.BRANCHID == 0) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (editDetail.dataParam.REGIONID == 0) {
        iview.Message.info("请选择区域!");
        return false;
    };
    if (editDetail.dataParam.FLOORID == 0) {
        iview.Message.info("请选择楼层!");
        return false;
    };
    editDetail.dataParam.FLOORSHOP = [];
    var data = editDetail.screenParam.map.GetData();

    for (var i = 0; i < data.length; i++) {
        //if ((editDetail.dataParam.FLOORSHOP.length === 0)
        //      || (editDetail.dataParam.FLOORSHOP.length > 0
        //      && editDetail.dataParam.FLOORSHOP.filter(function (item) {
        //      return parseInt(item.SHOPCODE) === data[i].name;
        //}).length === 0)) {
            var shop = {};
            shop.SHOPCODE = data[i].name;
            shop.P_X = data[i].x;
            shop.P_Y = data[i].y;
            editDetail.dataParam.FLOORSHOP.push(shop);
        //}
    };

    return true;
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchFloorMap', {
        Data: { MAPID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.floormap);
        editDetail.dataParam.BILLID = data.floormap.MAPID;
        editDetail.dataParam.FLOORID = data.floormap.MAPID;
        editDetail.dataParam.FLOORSHOP = data.floorshop;
        editDetail.screenParam.mappath = '/BackMap/10.jpg';
        editDetail.screenParam.widths = editDetail.dataParam.WIDTHS;
        editDetail.screenParam.lengths = editDetail.dataParam.LENGTHS;
        for (var i = 0; i < data.floorshop.length; i++) {
            editDetail.screenParam.SHOPDATA.push({
                name: data.floorshop[i].SHOPCODE,
                html: editDetail.GetHtml,
                x: data.floorshop[i].P_X,
                y: data.floorshop[i].P_Y
            });
        };
        editDetail.screenParam.map = $("#div_map").zMapPoint(editDetail.screenParam.options);
        callback && callback(data);
    });
}
;






