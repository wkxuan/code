editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "JsglService";
    editDetail.method = "GetBillReturn";
    editDetail.Key = 'BILLID';

    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopBill = __BaseUrl + "/" + "Pop/Pop/PopBillList/";
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    editDetail.screenParam.popParam = {};
    editDetail.dataParam.BILL_RETURN_ITEM = [];


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
    _.Ajax('SearchBill_Return', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billReturn);
        editDetail.dataParam.BILL_RETURN_ITEM = data.billReturnItem;
        callback && callback(data);
    });
}

///html中绑定方法
editDetail.otherMethods = {
    ///选择合同
    SelContract: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        };
        editDetail.screenParam.showPopContract = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
        editDetail.dataParam.BILL_RETURN_ITEM = [];
    },
    SelBill: function () {
        if (!editDetail.dataParam.CONTRACTID) {
            iview.Message.info("请选择租约!");
            return;
        };
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = {
            BRANCHID: editDetail.dataParam.BRANCHID,
            CONTRACTID: editDetail.dataParam.CONTRACTID,
            FTYPE: "1",    //保证金类型
            RRETURNFLAG: "1"  //返还标记 暂时判断 RECEIVE_MONEY <> 0 的记录
        };
    }
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopContract) {
        editDetail.screenParam.showPopContract = false;
        //接收选中的数据

        editDetail.dataParam.CONTRACTID = data.sj[0].CONTRACTID;
        editDetail.dataParam.MERCHANTID = data.sj[0].MERCHANTID;
        editDetail.dataParam.MERCHANTNAME = data.sj[0].MERCHANTNAME;
    };
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;

        //删除空行
        if (editDetail.dataParam.BILL_RETURN_ITEM.length > 0) {
            if (!editDetail.dataParam.BILL_RETURN_ITEM[0].FINAL_BILLID) {
                editDetail.dataParam.BILL_RETURN_ITEM.splice(0, 1);
            }
        }
        //接收选中的数据
        for (var i = 0; i < data.sj.length; i++) {
            if ((editDetail.dataParam.BILL_RETURN_ITEM.length === 0)
                || (editDetail.dataParam.BILL_RETURN_ITEM.length > 0
                && editDetail.dataParam.BILL_RETURN_ITEM.filter(function (item) {
                return parseInt(item.FINAL_BILLID) === data.sj[i].BILLID;
            }).length === 0))
                editDetail.dataParam.BILL_RETURN_ITEM.push({
                    FINAL_BILLID: data.sj[i].BILLID,
                    TERMMC: data.sj[i].TERMMC,
                    MUST_MONEY: data.sj[i].MUST_MONEY,
                    UNPAID_MONEY: data.sj[i].UNPAID_MONEY,
                    NOTICE_MONEY: data.sj[i].UNPAID_MONEY
                });
        }
    }
}

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_RETURN_ITEM = [];
}

editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };

    if (!editDetail.dataParam.CONTRACTID) {
        iview.Message.info("请选择租约!");
        return false;
    }
    if (!editDetail.dataParam.NIAYUE) {
        iview.Message.info("请输入债权发年月!");
        return false;
    }

    if (editDetail.dataParam.BILL_RETURN_ITEM.length == 0) {
        iview.Message.info("请选择账单!");
        return false;
    }
    else {
        for (var i = 0; i < editDetail.dataParam.BILL_RETURN_ITEM.length; i++) {
            if (!editDetail.dataParam.BILL_RETURN_ITEM[i].RETURN_MONEY) {
                iview.Message.info("请录入返还金额!");
                return false;
            }
        }
    }
    return true;
}