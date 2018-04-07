editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
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
                        editDetail.dataParam.BILL_NOTICE_ITEM[params.index].FINAL_BILLID = event.target.value;
                    }
                },
            }),
                ])
        },
    },
    { title: '收费项目', key: 'TERMMC', width: 100 },
    { title: "应收金额", key: 'MUST_MONEY', width: 100 },
    { title: "未付金额", key: 'UNPAID_MONEY', width: 100 },
    {
        title: "通知金额", key: 'NOTICE_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.NOTICE_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_NOTICE_ITEM[params.index].NOTICE_MONEY = event.target.value;
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
                            editDetail.dataParam.BILL_NOTICE_ITEM.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];
    if (!editDetail.dataParam.BILL_NOTICE_ITEM) {
        editDetail.dataParam.BILL_NOTICE_ITEM = [{
            FINAL_BILLID: "",
            NOTICE_MONEY: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_NOTICE_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_NOTICE_ITEM = temp;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Notice', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billNotice);
        editDetail.dataParam.BILL_NOTICE_ITEM = data.billNoticeItem;
        callback && callback(data);
    });
}