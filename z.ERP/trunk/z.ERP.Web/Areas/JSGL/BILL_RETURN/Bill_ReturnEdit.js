editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "JsglService";
    editDetail.method = "GetBillReturn";
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
                    'on-enter': function (event) {
                        _self = this;
                        editDetail.dataParam.BILL_RETURN_ITEM[params.index].FINAL_BILLID = event.target.value;
                        _.Ajax('GetBill', {
                            Data: { FINAL_BILLID: event.target.value }
                        }, function (data) {
                            Vue.set(editDetail.dataParam.BILL_RETURN_ITEM[params.index], 'MUST_MONEY', data.dt.MUST_MONEY),
                            Vue.set(editDetail.dataParam.BILL_RETURN_ITEM[params.index], 'RECEIVE_MONEY', data.dt.RECEIVE_MONEY)
                        });
                    }
                },
            }),
            //h('Button', {
            //    props: { type: 'primary', size: 'small', disabled: false },

            //    style: { marginRight: '5px', width: '30px' },
            //    on: {
            //        click: editDetail.screenParam.openPop
            //    },
            //}, '...'),
                ])
        },
    },
    { title: '应收金额', key: 'MUST_MONEY', width: 100 },
    { title: '已收金额', key: 'RECEIVE_MONEY', width: 100 },
    {
        title: "返还金额", key: 'RETURN_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.RETURN_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_RETURN_ITEM[params.index].RETURN_MONEY = event.target.value;
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
                              editDetail.dataParam.BILL_RETURN_ITEM.splice(params.index, 1);
                          }
                      },
                  }, '删除')
                  ]);
          }
      }
    ];
    if (!editDetail.dataParam.BILL_RETURN_ITEM) {
        editDetail.dataParam.BILL_RETURN_ITEM = [{
            FINAL_BILLID: "",
            RETURN_MONEY: "",
            RECEIVE_MONEY: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_RETURN_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_RETURN_ITEM = temp;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchbillReturn', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billReturn);
        editDetail.dataParam.BILL_RETURN_ITEM = data.billReturnItem;
        callback && callback(data);
    });
}