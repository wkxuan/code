editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //账单收款
    editDetail.dataParam.TYPE = 2;
    editDetail.screenParam.colDef = [
    {
        title: "账单号", key: 'FINAL_BILLID', width: 160,
        render: function (h, params) {
            return h('div',
                [
            h('Input', {
                props: {
                    value: params.row.FINAL_BILLID
                },
                style: { marginRight: '5px', width: '80px' },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_OBTAIN_ITEM[params.index].FINAL_BILLID = event.target.value;
                    }
                },
            }),
                ])
        },
    },
    { title: '租约号', key: 'CONTRACTID', width: 100 },
    {
        title: "类型", key: 'TYPE', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.TYPE
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_OBTAIN_ITEM[params.index].TYPE = event.target.value;
                    }
                },
            })
        },
    },
   { title: '收费项目', key: 'TERMMC', width: 100 },
    {
        title: "应收金额", key: 'MUST_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.MUST_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_OBTAIN_ITEM[params.index].MUST_MONEY = event.target.value;
                    }
                },
            })
        },
    },
   { title: '未付金额', key: 'UNPAID_MONEY', width: 100 },
    {
        title: "付款金额", key: 'RECEIVE_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.RECEIVE_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_OBTAIN_ITEM[params.index].RECEIVE_MONEY = event.target.value;
                    }
                },
            })
        },
    },
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
                            editDetail.dataParam.BILL_OBTAIN_ITEM.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];
    if (!editDetail.dataParam.BILL_OBTAIN_ITEM) {
        editDetail.dataParam.BILL_OBTAIN_ITEM = [{
            FINAL_BILLID: "",
            RECEIVE_MONEY: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_OBTAIN_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_OBTAIN_ITEM = temp;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billObtain);
        editDetail.dataParam.BILL_OBTAIN_ITEM = data.billObtainItem;
        callback && callback(data);
    });
}