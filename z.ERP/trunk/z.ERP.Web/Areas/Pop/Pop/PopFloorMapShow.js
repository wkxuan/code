popShow.beforeVue = function () {
    //var col = [
    //    { title: '人员代码', key: 'USERCODE', width: 100 },
    //    { title: '人员名称', key: 'USERNAME', width: 200 },
    //    { title: '人员类型', key: 'USER_TYPEMC', width: 100 },
    //    { title: '所属机构', key: 'ORGNAME', width: 200 }
    //];
    //popShow.screenParam.colDef = col.concat(popShow.colMul);
    popShow.screenParam.SHOPCODE = "";
    popShow.screenParam.USERCODE = "";
    popShow.screenParam.USERNAME = "";
    popShow.service = "UserService";
    popShow.method = "GetUser";
}


//获取父页面参数
popShow.popInitParam = function (data) {
    if (data) {
        popShow.screenParam.USERCODE = data.SHOPCODE;
    }
}

popShow.showOne = function (data, callback) {
    _.Ajax('SearchFloorMapData', {
        Data: { MAPID: data }
    }, function (data) {
        //$.extend(editDetail.dataParam, data.floormap);
        //editDetail.dataParam.BILLID = data.floormap.MAPID;
        //editDetail.dataParam.FLOORID = data.floormap.MAPID;
        popShow.screenParam.SHOPCODE = data.floorshopdata[0].SHOPCODE;
        popShow.screenParam.USERCODE = data.floorshopdata[0].SHOPCODE;
        popShow.screenParam.USERNAME = data.floorshopdata[0].SHOPCODE;
        //editDetail.screenParam.mappath = '/BackMap/10.jpg';
        //editDetail.screenParam.widths = editDetail.dataParam.WIDTHS;
        //editDetail.screenParam.lengths = editDetail.dataParam.LENGTHS;
        //for (var i = 0; i < data.floorshop.length; i++) {
        //    editDetail.screenParam.SHOPDATA.push({
        //        name: data.floorshop[i].SHOPCODE,
        //        html: editDetail.GetHtml,
        //        x: data.floorshop[i].P_X,
        //        y: data.floorshop[i].P_Y
        //    });
        //};
        //editDetail.screenParam.map = $("#div_map").zMapPoint(editDetail.screenParam.options);
        callback && callback(data);
    });
}