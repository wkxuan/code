editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "JsglService";
    editDetail.method = "GetBillAdjust";
    editDetail.Key = 'BILLID';

    //初始化弹窗所要传递参数
    editDetail.screenParam.ParentFeeSubject = {};
    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.srcPopBill = __BaseUrl + "/" + "Pop/Pop/PopBillList/";
    editDetail.screenParam.popParam = { };


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
            TERMID: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_ADJUST_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_ADJUST_ITEM = temp;
    }
}
editDetail.otherMethods = {

    SelBill: function () {
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
        //var site = localStorage.getItem("relt");
    },
    srchCost: function () {
        Vue.set(editDetail.screenParam, "PopFeeSubject", true);
    },

    FeeSubjectBack: function (val) {
        Vue.set(editDetail.screenParam, "PopFeeSubject", false);
        var maxIndex = 1;
        for (var i = 0; i < val.sj.length; i++) {
            if (editDetail.dataParam.CONTRACT_COST) {
                for (var j = 0; j < editDetail.dataParam.CONTRACT_COST.length; j++) {
                    maxIndex = editDetail.dataParam.CONTRACT_COST[0].INX;
                    if (editDetail.dataParam.CONTRACT_COST[j].INX > maxIndex) {
                        maxIndex = editDetail.dataParam.CONTRACT_COST[j].INX
                    };
                    maxIndex++;
                };
            };
            editDetail.dataParam.CONTRACT_COST.push({
                INX: maxIndex,
                TERMID: val.sj[i].TERMID,
                NAME: val.sj[i].NAME
            });
        };
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

editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopBill = false;
    for (var i = 0; i < data.sj.length; i++) {
        editDetail.dataParam.BILL_ADJUST_ITEM.push(data.sj[i]);
    };
    //alert('Clicked ok');
};