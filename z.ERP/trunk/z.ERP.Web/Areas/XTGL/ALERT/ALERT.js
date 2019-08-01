define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '代码', key: 'ID', width: 80 },
        { title: '名称', key: 'MC'},
        { title: '显示顺序', key: 'XSSX', width: 100 },
    ];
    define.dataParam.ALERT_FIELD = define.dataParam.ALERT_FIELD || [];
    define.screenParam.colAlertField = [
    {
        title: "字段名",
        key: 'FIELDMC', width: 200, cellType: "input"
    },
    {
        title: '显示名',
        key: 'CHINAMC', width: 200, cellType: "input"
    },
    {
        title: '宽度',
        key: 'WIDTH', width: 100, cellType: "input", cellDataType: "number"
    },
    {
        title: '排列顺序',
        key : 'PLSX', width: 100, cellType: "input", cellDataType: "number"
    }];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetAlertElement";
    define.methodList = "GetAlert";
    define.Key = 'ID';
};

define.otherMethods = {
    addCol: function () {
        var plsx;
        if (!define.dataParam.ALERT_FIELD) {
            plsx = 1
        } else {
            plsx = define.dataParam.ALERT_FIELD.length+1;
        };
        var temp = define.dataParam.ALERT_FIELD || [];
        temp.push({ PLSX: plsx});
        Vue.set(define.dataParam, "ALERT_FIELD", temp);
    },
    delCol: function () {
        var selectton = this.$refs.selectAlert.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < define.dataParam.ALERT_FIELD.length; j++) {
                    if (define.dataParam.ALERT_FIELD[j].FIELDMC == selectton[i].FIELDMC) {
                        define.dataParam.ALERT_FIELD.splice(j, 1);
                    }
                }
            }
        }
    }
};

define.IsValidSave = function () {
    if (!define.dataParam.MC) {
        iview.Message.info("预警名称不能为空!");
        return false;
    }

    if (!define.dataParam.SQLSTR) {
        iview.Message.info("预警名称SQL不能为空!");
        return false;
    }

    if ((!define.dataParam.ALERT_FIELD) || (define.dataParam.ALERT_FIELD.length == 0)) {
        iview.Message.info("请设置预警信息显示!");
        return false;
    }
    if (define.dataParam.ALERT_FIELD.length > 0) {
        for (var i = 0; i < define.dataParam.ALERT_FIELD.length; i++) {
            if (!define.dataParam.ALERT_FIELD[i].FIELDMC || !define.dataParam.ALERT_FIELD[i].CHINAMC || !define.dataParam.ALERT_FIELD[i].WIDTH) {
                iview.Message.info("请设置完整预警信息显示!");
                return false;
            }
        }       
    }
    return true;
};

define.showone = function (data, callback) {
    _.Ajax('SearchAlert', {
        Data: { ID: data }
    }, function (data) {
        define.dataParam = {};
        $.extend(define.dataParam, data.defalert);
        define.dataParam.ALERT_FIELD = data.item;
        callback && callback();
    });
}
