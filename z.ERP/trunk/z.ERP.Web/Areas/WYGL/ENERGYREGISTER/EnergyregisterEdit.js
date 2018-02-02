editDetail.beforeVue = function () {

    editDetail.service = "WyglService";
    editDetail.method = "GetEnergyreGisterElement"
    editDetail.Key = 'BILLID';

    editDetail.screenParam.colDef = [
        {
            title: "设备编码", key: "FILEID", width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.FILEID
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].FILEID = event.target.value;

                            _.Ajax('GetRegister', {
                                Data: { FILEID: event.target.value }
                            }, function (data) {
                                //editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].VALUE_LAST = data.
                                var ss = data;
                                editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].INX = params.index;
                                editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].FILENAME = data.dt[0].FILENAME;
                                editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].SHOPID = data.dt[0].SHOPID;
                                editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].SHOPDM = data.dt[0].SHOPDM;
                                editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].VALUE_LAST = data.dt[0].VALUE_LAST;
                                editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].PRICE = data.dt[0].PRICE;
                                editDetail.$set(editDetail.dataParam, editDetail.dataParam);
                            });                            
                        }
                    }
                })
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
        {
            title: '操作',
            key: 'action',
            width: 80,
            align: 'center',
            render: function (h, params) {
                return h('div',
                    [
                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled: false },

                        style: { marginRight: '50px' },
                        on: {
                            click: function (event) {
                                editDetail.dataParam.ENERGY_REGISTER_ITEM.splice(params.index, 1);
                            }
                        },
                    }, '删除')
                    ]);
            }
        }

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
