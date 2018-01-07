editDetail.beforeVue = function () {

    editDetail.service = "WyglService";
    editDetail.method = "GetEnergyreGister"
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
                        }
                    }
                })
            }
        },
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
}