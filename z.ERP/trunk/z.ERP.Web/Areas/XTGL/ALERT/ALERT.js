define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '代码', key: 'ID', width: 150 },
        { title: '名称', key: 'MC', width: 150 },
    ];
    define.dataParam.ALERT_FIELD = define.dataParam.ALERT_FIELD || [];
    define.screenParam.colAlertField = [
    { type: 'selection', width: 60, align: 'center' },
    {
        title: "字段名",
        key: 'FIELDMC', width: 200,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.FIELDMC
                },
                on: {
                    'on-blur': function (event) {
                        define.dataParam.ALERT_FIELD[params.index].FIELDMC = event.target.value;
                    }
                },
            })
        },
    },
    {
        title: '显示名',
        key: 'CHINAMC', width: 200,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.CHINAMC
                },
                on: {
                    'on-blur': function (event) {
                        define.dataParam.ALERT_FIELD[params.index].CHINAMC = event.target.value;
                    }
                },
            })
        },
    },
    {
        title: '宽度',
        key: 'WIDTH', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.WIDTH
                },
                on: {
                    'on-blur': function (event) {
                        define.dataParam.ALERT_FIELD[params.index].WIDTH = event.target.value;
                    }
                },
            })
        },
    }];

    define.screenParam.dataDef = [];



    define.service = "XtglService";
    define.method = "GetAlertElement";
    define.methodList = "GetAlert";
    define.Key = 'ID';
};

define.otherMethods = {
    addCol: function () {
        var temp = define.dataParam.ALERT_FIELD || [];
        temp.push({});
        Vue.set(define.dataParam, "ALERT_FIELD", temp);
    },
    delCol: function () {
        var selectton = this.$refs.selectAlert.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的品牌!");
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

    return true;
};

define.showone = function (data, callback) {
    _.Ajax('SearchAlert', {
        Data: { ID: data }
    }, function (data) {
        $.extend(define.dataParam, data.defalert);
        define.dataParam.ALERT_FIELD = data.item;
        callback && callback();
    });
}
