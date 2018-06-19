var index;
editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "JsglService";
    editDetail.method = "GetBillAdjust";
    editDetail.Key = 'BILLID';

    //初始化弹窗所要传递参数
    editDetail.screenParam.ParentFeeSubject = {};
    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.showPopFeeSubject = false;
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopBill = __BaseUrl + "/" + "Pop/Pop/PopBillList/";
    editDetail.screenParam.srcPopFeeSubject = __BaseUrl + "/" + "Pop/Pop/PopFeeSubjectList/";
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    editDetail.screenParam.popParam = {};



    editDetail.screenParam.colDef = [
                {
                    title: '选择',
                    key: 'action',
                    width: 70,
                    align: 'center',
                    render: function (h, params) {
                        return h('div', [
                            h('Button', {
                                props: {
                                    type: 'primary',
                                    size: 'small'
                                },
                                style: {
                                    marginRight: '5px'
                                },
                                on: {
                                    click: function (event) {
                                        editDetail.screenParam.SelContract(params.index);
                                    }
                                }
                            }, '...')
                        ]);
                    }
                },
                {
                    title: "租约号", key: 'CONTRACTID', width: 150,
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
    { title: "商户", key: 'MERCHANTNAME', width: 200 },
        {
            title: '选择',
            key: 'action',
            width: 70,
            align: 'center',
            render: function (h, params) {
                return h('div', [
                    h('Button', {
                        props: {
                            type: 'primary',
                            size: 'small'
                        },
                        style: {
                            marginRight: '5px'
                        },
                        on: {
                            click: function (event) {
                                editDetail.screenParam.SelFeeSubject(params.index);
                            }
                        }
                    }, '...')
                ]);
            }
        },
    { title: "收费项目", key: 'TERMNAME', width: 150 },
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
            MERCHANTNAME: "",
            TERMID: "",
            TERMNAME: "",
            MUST_MONEY: "",
        }]
    }


    /////////js中调用方法
    ///添加一行
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_ADJUST_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_ADJUST_ITEM = temp;
    }
    ///选择收费项目
    editDetail.screenParam.SelFeeSubject = function (inx) {
        index = inx;
        editDetail.screenParam.showPopFeeSubject = true;
    }
    ///选择合同
    editDetail.screenParam.SelContract = function (inx) {
        index = inx;
        editDetail.screenParam.showPopContract = true;
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

///html中绑定方法
editDetail.otherMethods = {
    SelBill: function () {
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
    }
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BILL_ADJUST_ITEM.push(data.sj[i]);
        }
    }
    else if (editDetail.screenParam.showPopFeeSubject) {
        editDetail.screenParam.showPopFeeSubject = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BILL_ADJUST_ITEM[index].TERMID = data.sj[i].TERMID;
            editDetail.dataParam.BILL_ADJUST_ITEM[index].TERMNAME = data.sj[i].NAME;
        }
    }
    else if (editDetail.screenParam.showPopContract) {
        editDetail.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BILL_ADJUST_ITEM[index].CONTRACTID = data.sj[i].CONTRACTID;
            editDetail.dataParam.BILL_ADJUST_ITEM[index].MERCHANTNAME = data.sj[i].MERCHANTNAME;
        }
    }
}

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.YEARMONTH = null;
    editDetail.dataParam.START_DATE = null;
    editDetail.dataParam.END_DATE = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_ADJUST_ITEM = [];
}

editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };

    if (editDetail.dataParam.BILL_ADJUST_ITEM.length == 0) {
        iview.Message.info("请录入费用信息!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.BILL_ADJUST_ITEM.length; i++) {
            if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].CONTRACTID) {
                iview.Message.info("请录入租约!");
                return false;
            };
            if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].TERMID) {
                iview.Message.info("请选择收费项目!");
                return false;
            };
            if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].MUST_MONEY) {
                iview.Message.info("请录入费用金额!");
                return false;
            };
        };
    };

    return true;
}