editDetail.beforeVue = function () {

    editDetail.service = "WyglService";
    editDetail.method = "GetEnergyreGisterElement"
    editDetail.Key = 'BILLID';

    editDetail.screenParam.colDef = [
        {
            title: "设备编码", key: "FILEID", width: 100, cellType: "input",
            onBlur: function (index, row, data) {
                editDetail.dataParam.ENERGY_REGISTER_ITEM[index].FILEID = row.FILEID;

                _.Ajax('GetRegister', {
                    Data: { FILEID: row.FILEID }
                }, function (data) {
                    if (data.dt.length>0) {
                    //editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].VALUE_LAST = data.
                    Vue.set(editDetail.dataParam.ENERGY_REGISTER_ITEM[index], 'INX', index);
                    Vue.set(editDetail.dataParam.ENERGY_REGISTER_ITEM[index], 'FILENAME', data.dt[0].FILENAME);
                    Vue.set(editDetail.dataParam.ENERGY_REGISTER_ITEM[index], 'SHOPID', data.dt[0].SHOPID);
                    Vue.set(editDetail.dataParam.ENERGY_REGISTER_ITEM[index], 'SHOPDM', data.dt[0].SHOPDM);
                    Vue.set(editDetail.dataParam.ENERGY_REGISTER_ITEM[index], 'VALUE_LAST', data.dt[0].VALUE_LAST);
                    Vue.set(editDetail.dataParam.ENERGY_REGISTER_ITEM[index], 'PRICE', data.dt[0].PRICE);
                }
                });                            
            }
        },
        { title: "设备名称", key: "FILENAME", width: 100, },
        { title: "序号", key: "INX", width: 100, },
        { title: "商铺ID", key: "SHOPID", width: 100, },
        { title: "商铺代码", key: "SHOPDM", width: 100, },
        { title: "租约号", key: "CONTRACTID", width: 100, },
        { title: "上次读数", key: "VALUE_LAST", width: 100, },
        { title: "当前读数", key: "VALUE_CURRENT", width: 100, },
        { title: "使用量", key: "VALUE_USE", width: 100, },
        { title: "单价", key: "PRICE", width: 100, },
        { title: "金额", key: "AMOUNT", width: 100, },
        { title: "开始日期", key: "START_DATE", width: 100, },
        { title: "结束日期", key: "END_DATE", width: 100, },
    ]

    if (!editDetail.dataParam.ENERGY_REGISTER_ITEM) {
        editDetail.dataParam.ENERGY_REGISTER_ITEM = [{
            FILEID: "1",
            INX: "1",
            SHOPID: "1",
            CONTRACTID: "1",
            VALUE_LAST:"0",
            VALUE_CURRENT: "100",
            VALUE_USE: "100",
            PRICE:"1.2",
            AMOUNT:"120",
            START_DATE:"2018.1.1",
            END_DATE:"2018.1.7",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.ENERGY_REGISTER_ITEM || [];
        temp.push({});
        editDetail.dataParam.ENERGY_REGISTER_ITEM = temp;
    }
    editDetail.IsValidSave = function () {
        var d = new Date(editDetail.dataParam.CHECK_DATE);
        editDetail.dataParam.CHECK_DATE = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();

        return true;
    }
 
}

editDetail.otherMethods = {
    delCol:function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.ENERGY_REGISTER_ITEM.length; j++) {
                    if (editDetail.dataParam.ENERGY_REGISTER_ITEM[j].FILEID == selectton[i].FILEID) {
                        editDetail.dataParam.ENERGY_REGISTER_ITEM.splice(j, 1);
                    }
                }
            }
        }
    } 
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchElement', {
        Data: { BILLID: data }
    }, function (data) {   
        editDetail.dataParam.YEARMONTH = data.main[0].YEARMONTH;
        editDetail.dataParam.CHECK_DATE = data.main[0].CHECK_DATE;        
        editDetail.dataParam.BILLID = data.main[0].BILLID;
        editDetail.dataParam.DESCRIPTION = data.main[0].DESCRIPTION;
        
        editDetail.dataParam.ENERGY_REGISTER_ITEM = data.item[0];
        callback && callback(data);
    });
}

//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10300201"
    }, {
        id: "edit",
        authority: "10300201"
    }, {
        id: "del",
        authority: "10300201"
    }, {
        id: "save",
        authority: "10300201"
    }, {
        id: "abandon",
        authority: "10300201"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10300202",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { BILLID: editDetail.dataParam.BILLID },
            }, function (data) {
                iview.Message.info("审核成功");
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};
