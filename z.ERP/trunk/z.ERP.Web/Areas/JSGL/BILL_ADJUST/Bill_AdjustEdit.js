editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "JsglService";
    editDetail.method = "GetBillAdjust";
    editDetail.Key = 'BILLID';

    editDetail.screenParam.colDef = [
                {
                    title: "租约号", key: 'CONTRACTID', width: 100,
                    render: function (h, params) {
                        return h('Input', {
                            props: {
                                value: params.row.CONTRACTID
                            },
                            on: {
                                'on-blur': function (event) {
                                    editDetail.dataParam.BILL_ADJUST_ITEM[params.index].CONTRACTID = event.target.value;
                                }
                            },
                        })
                    },
                },
        {
            title: "收费项目", key: 'TERMID', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.TERMID
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.BILL_ADJUST_ITEM[params.index].TERMID = event.target.value;
                        }
                    },
                })
            },
        },
    {
        title: "调整金额", key: 'MUST_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.MUST_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_ADJUST_ITEM[params.index].MUST_MONEY = event.target.value;
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
                              editDetail.dataParam.BILL_ADJUST_ITEM.splice(params.index, 1);
                          }
                      },
                  }, '删除')
                  ]);
          }
      }
    ];
    if (!editDetail.dataParam.BILL_ADJUST_ITEM) {
        editDetail.dataParam.BILL_ADJUST_ITEM = [{
            CONTRACTID: "",
            TERIMID: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_ADJUST_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_ADJUST_ITEM = temp;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Adjust', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billAdjust);
        editDetail.dataParam.BILL_ADJUST_ITEM = data.billAdjustItem;
        callback && callback(data);
    });
}