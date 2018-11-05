var oldSqlIndex = 0;
var curSqlIndex = 0;
var oldRecordIndex = 0;
var curRecordIndex = 0;
var curRecordRow;
var RecordPzkm;
var RecordZy;
editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = false;
    editDetail.service = "CwglService";
    editDetail.method = "GetVoucher";
    editDetail.Key = 'VOUCHERID';

    //初始化弹窗所要传递参数


    ///账单类型初始化默认给1
    editDetail.dataParam.TYPE = 1;
    editDetail.dataParam.MAKESQL = "";
    editDetail.screenParam.colDef_Sql = [
    {
        title: "SQL序号", key: 'SQLINX', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.SQLINX
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.VOUCHER_MAKESQL[params.index].SQLINX = event.target.value;
                    }
                },
            })
        },
    },
        {
            title: "SQL类型", key: 'EXESQLTYPE', width: 120,
            render: function (h, params) {
                return h('Select', {
                    props: {
                        value: params.row.EXESQLTYPE
                    },
                    on: {
                        'on-change': function (event) {
                            editDetail.dataParam.VOUCHER_RECORD[params.index].EXESQLTYPE = event;
                            //var len = 0;
                            //for (var i = 0; i < editDetail.dataParam.VOUCHER_RECORD.length; i++) {
                            //    var lenold = len;
                            //    len += editDetail.dataParam.VOUCHER_RECORD[i].VOUCHER_RECORD.length;
                            //    var colIndex = params.index + 1;

                            //    if ((colIndex > lenold) && (colIndex <= len)) {
                            //        editDetail.dataParam.VOUCHER_RECORD[i].VOUCHER_RECORD[params.index - lenold].TYPE = event;
                            //        break;
                            //    }
                            //};
                        }
                    }
                },
                [
                    h('Option', { props: { value: 1} }, 'S'),
                    h('Option', { props: { value: 2} }, 'U')
                ])
            }
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
                              editDetail.dataParam.VOUCHER_MAKESQL.splice(params.index, 1);
                          }
                      },
                  }, '删除')
                  ]);
          }
      }
    ];
    editDetail.screenParam.colDef_Record = [
{
    title: "分录序号", key: 'RECORDID', width: 100,
    render: function (h, params) {
        return h('Input', {
            props: {
                value: params.row.RECORDID
            },
            on: {
                'on-blur': function (event) {
                    editDetail.dataParam.VOUCHER_RECORD[params.index].RECORDID = event.target.value;
                }
            },
        })
    },
},
    {
        title: "分录名称", key: 'RECORDNAME', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.RECORDNAME
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD[params.index].RECORDNAME = event.target.value;
                    }
                },
            })
        },
    },
        {
            title: "方向", key: 'TYPE', width: 120,
            render: function (h, params) {
                return h('Select', {
                    props: {
                        value: params.row.TYPE
                    },
                    on: {
                        'on-change': function (event) {
                            editDetail.dataParam.VOUCHER_RECORD[params.index].TYPE = event;
                        }
                    }
                },
                [
                    h('Option', { props: { value: 1 } }, '记'),
                    h('Option', { props: { value: 2 } }, '贷')
                ])
            }
        },
    {
        title: "取值字段", key: 'SQLCOLTORECORD', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.SQLCOLTORECORD
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD[params.index].SQLCOLTORECORD = event.target.value;
                    }
                },
            })
        },
    },
        {
            title: "供应商", key: 'SQLCOLTOMERCHANT', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.SQLCOLTOMERCHANT
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.VOUCHER_RECORD[params.index].SQLCOLTOMERCHANT = event.target.value;
                        }
                    },
                })
            },
        },
            {
                title: "部门", key: 'SQLCOLTOORG', width: 100,
                render: function (h, params) {
                    return h('Input', {
                        props: {
                            value: params.row.SQLCOLTOORG
                        },
                        on: {
                            'on-blur': function (event) {
                                editDetail.dataParam.VOUCHER_RECORD[params.index].SQLCOLTOORG = event.target.value;
                            }
                        },
                    })
                },
            },
                {
                    title: "人员", key: 'SQLCOLTOUSER', width: 100,
                    render: function (h, params) {
                        return h('Input', {
                            props: {
                                value: params.row.SQLCOLTOUSER
                            },
                            on: {
                                'on-blur': function (event) {
                                    editDetail.dataParam.VOUCHER_RECORD[params.index].SQLCOLTOUSER = event.target.value;
                                }
                            },
                        })
                    },
                },
            {
                title: "SQL序号", key: 'SQLINX', width: 100,
                render: function (h, params) {
                    return h('Input', {
                        props: {
                            value: params.row.SQLINX
                        },
                        on: {
                            'on-blur': function (event) {
                                editDetail.dataParam.VOUCHER_RECORD[params.index].SQLINX = event.target.value;
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
                          editDetail.dataParam.VOUCHER_RECORD.splice(params.index, 1);
                      }
                  },
              }, '删除')
              ]);
      }
  }
    ];
    editDetail.screenParam.colDef_Pzkm = [
{
    title: "科目级次", key: 'INX', width: 100,
    render: function (h, params) {
        return h('Input', {
            props: {
                value: params.row.INX
            },
            on: {
                'on-blur': function (event) {
                    if (curRecordRow.RECORDID == 0)
                    {
                        iview.Message.info("请输入分录序号!");
                        return;
                    }
                    else {
                        editDetail.dataParam.VOUCHER_RECORD_PZKM[params.index].INX = event.target.value;
                        editDetail.dataParam.VOUCHER_RECORD_PZKM[params.index].RECORDID = curRecordRow.RECORDID;
                    }
                    
                }
            },
        })
    },
},
    {
        title: "科目代码", key: 'DESCRIPTION', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.DESCRIPTION
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD_PZKM[params.index].DESCRIPTION = event.target.value;
                    }
                },
            })
        },
    },
    {
        title: "使用分录SQL", key: 'SQLBJ', width: 120,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.SQLBJ
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD_PZKM[params.index].SQLBJ = event.target.value;
                    }
                },
            })
        },
        render: function (h, params) {
            return h('Select', {
                props: {
                    value: params.row.SQLBJ
                },
                on: {
                    'on-change': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD_PZKM[params.index].SQLBJ = event;
                    }
                }
            },
            [
                h('Option', { props: { value: 1} }, '是'),
                h('Option', { props: { value: 2 } }, '否')
            ])
        }
    },
        {
            title: "取值字段", key: 'SQLCOLTORECORD', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.SQLCOLTORECORD
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.VOUCHER_RECORD_PZKM[params.index].SQLCOLTORECORD = event.target.value;
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
                          editDetail.dataParam.VOUCHER_RECORD_PZKM.splice(params.index, 1);
                      }
                  },
              }, '删除')
              ]);
      }
  }
    ];
    editDetail.screenParam.colDef_Zy = [
{
    title: "摘要级次", key: 'INX', width: 100,
    render: function (h, params) {
        return h('Input', {
            props: {
                value: params.row.INX
            },
            on: {
                'on-blur': function (event) {
                    
                    if (curRecordRow.RECORDID == 0) {
                        iview.Message.info("请输入分录序号!");
                        return;
                    }
                    else {
                        editDetail.dataParam.VOUCHER_RECORD_ZY[params.index].INX = event.target.value;
                        editDetail.dataParam.VOUCHER_RECORD_ZY[params.index].RECORDID = curRecordRow.RECORDID;
                    }
                }
            },
        })
    },
},
    {
        title: "摘要内容", key: 'DESCRIPTION', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.DESCRIPTION
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD_ZY[params.index].DESCRIPTION = event.target.value;
                    }
                },
            })
        },
    },
    {
        title: "使用分录SQL", key: 'SQLCOLTORECORD', width: 120,
        render: function (h, params) {
            return h('Select', {
                props: {
                    value: params.row.SQLBJ
                },
                on: {
                    'on-change': function (event) {
                        editDetail.dataParam.VOUCHER_RECORD_ZY[params.index].SQLBJ = event;
                    }
                }
            },
            [
                h('Option', { props: { value: 1 } }, '是'),
                h('Option', { props: { value: 2 } }, '否')
            ])
        }
    },
        {
            title: "取值字段", key: 'TYPE', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.SQLCOLTORECORD
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.VOUCHER_RECORD_ZY[params.index].SQLCOLTORECORD = event.target.value;
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
                          editDetail.dataParam.VOUCHER_RECORD_ZY.splice(params.index, 1);
                      }
                  },
              }, '删除')
              ]);
      }
  }
    ];
    if (!editDetail.dataParam.VOUCHER_MAKESQL) {
        editDetail.dataParam.VOUCHER_MAKESQL = [{
            SQLINX: "",
            EXESQLTYPE: "1",
            MAKESQL: ""
        }]
    }
    if (!editDetail.dataParam.VOUCHER_RECORD) {
        editDetail.dataParam.VOUCHER_RECORD = [{
            RECORDID: "",
            RECORDNAME: "",
            TYPE: "1",
            SQLCOLTORECORD: "",
            SQLINX:""
        }]
    }
    if (!editDetail.dataParam.VOUCHER_RECORD_PZKM) {
        editDetail.dataParam.VOUCHER_RECORD_PZKM = [{
            RECORDID: "",
            INX: "",
            SQLCOLTORECORD: "",
            DESCRIPTION: "",
            SQLBJ: "2"
        }]
    }
    if (!editDetail.dataParam.VOUCHER_RECORD_ZY) {
        editDetail.dataParam.VOUCHER_RECORD_ZY = [{
            RECORDID: "",
            INX: "",
            SQLCOLTORECORD: "",
            DESCRIPTION: "",
            SQLBJ: "2"
        }]
    }

    /////////js中调用方法
    ///添加一行
    editDetail.screenParam.addColSql = function () {
        var temp = editDetail.dataParam.VOUCHER_MAKESQL || [];
        temp.push({
            SQLINX: "",
            EXESQLTYPE: "1",
            MAKESQL: ""
        });
        editDetail.dataParam.VOUCHER_MAKESQL = temp;
    }
    editDetail.screenParam.addColRecord = function () {
        var temp = editDetail.dataParam.VOUCHER_RECORD || [];
        temp.push({
            RECORDID: "",
            RECORDNAME: "",
            TYPE: "1",
            SQLCOLTORECORD: "",
            SQLINX: ""
        });
        editDetail.dataParam.VOUCHER_RECORD = temp;
    }
    editDetail.screenParam.addColRecordPzkm = function () {
        if (curRecordRow.RECORDID == 0) {
            iview.Message.info("请输入分录序号!");
            return;
        }
        else {
            var temp = editDetail.dataParam.VOUCHER_RECORD_PZKM || [];
            temp.push({
                RECORDID: "",
                INX: "",
                DESCRIPTION: "",
                SQLCOLTORECORD: "",
                SQLBJ: "2"
            });
            editDetail.dataParam.VOUCHER_RECORD_PZKM = temp;
        }
    }
    editDetail.screenParam.addColRecordZy = function () {
        if (curRecordRow.RECORDID == 0) {
            iview.Message.info("请输入分录序号!");
            return;
        }
        else {
            var temp = editDetail.dataParam.VOUCHER_RECORD_ZY || [];
            temp.push({
                RECORDID: "",
                INX: "",
                DESCRIPTION: "",
                SQLCOLTORECORD: "",
                SQLBJ: "2"
            });
            editDetail.dataParam.VOUCHER_RECORD_ZY = temp;
        }
    }
    editDetail.screenParam.makeSql = function () {
        var temp = editDetail.dataParam.VOUCHER_RECORD_ZY || [];
        if (editDetail.dataParam.VOUCHER_MAKESQL[curSqlIndex].SQLINX=="" )
        {
            iview.Message.info("请输入SQL序号!");
            return;
        }
        else
        {
            editDetail.dataParam.VOUCHER_MAKESQL[curSqlIndex].MAKESQL = editDetail.dataParam.MAKESQL;
        }
    }
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchVoucher', {
        Data: { VOUCHERID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.voucher);
        editDetail.dataParam.BILLID = data.voucher.VOUCHERID;
        editDetail.dataParam.VOUCHER_MAKESQL = data.voucherSql;
        editDetail.dataParam.VOUCHER_RECORD = data.voucherRecord;
        editDetail.dataParam.VOUCHER_RECORD_PZKM = data.voucherRecordPzkm;
        editDetail.dataParam.VOUCHER_RECORD_ZY = data.voucherRecordZy;
        if (editDetail.dataParam.VOUCHER_MAKESQL.length > 0)
        {
            editDetail.dataParam.MAKESQL = editDetail.dataParam.VOUCHER_MAKESQL[0].MAKESQL;
        }
        if (editDetail.dataParam.VOUCHER_MAKESQL.length > 0)
        {
            editDetail.otherMethods.recordChange(editDetail.dataParam.VOUCHER_RECORD[0], undefined);
        }
        callback && callback(data);
    });
}

///html中绑定方法
editDetail.otherMethods = {
    SelBill: function () {
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
    },
    sqlfilter: function (curRow, oldRow) {
        //显示SQL

    },
    makesqlrowclick: function (curRow, index) {
        //后执行
        editDetail.dataParam.MAKESQL = curRow.MAKESQL;
        curSqlIndex = index;
    },
    recordChange: function (curRow, oldRow)
    {
        curRecordRow = curRow;
        //科目
        if (RecordPzkm == undefined) {
            RecordPzkm = editDetail.dataParam.VOUCHER_RECORD_PZKM
        }
        else {
            for (inx in editDetail.dataParam.VOUCHER_RECORD_PZKM) {
                if ((RecordPzkm.length == 0) || (RecordPzkm.length > 0 && RecordPzkm.filter(function (item) {
              return item.RECORDID == editDetail.dataParam.VOUCHER_RECORD_PZKM[inx].RECORDID
                    && item.INX == editDetail.dataParam.VOUCHER_RECORD_PZKM[inx].INX;
                }).length === 0)) {
                    RecordPzkm.push(editDetail.dataParam.VOUCHER_RECORD_PZKM[inx]);
                }
            }
        }
        var filterdata = RecordPzkm.filter(function (row) {
            return parseInt(row.RECORDID) == curRow.RECORDID;
        });
        Vue.set(editDetail.dataParam, "VOUCHER_RECORD_PZKM", filterdata);
        if (RecordPzkm != undefined) {
            RecordPzkm = RecordPzkm.filter(function (row) {
                return parseInt(row.RECORDID) !== curRow.RECORDID;
            })
        }
        //摘要
        if (RecordZy == undefined) {
            RecordZy = editDetail.dataParam.VOUCHER_RECORD_ZY
        }
        else {
            for (inx in editDetail.dataParam.VOUCHER_RECORD_ZY) {
                if ((RecordZy.length == 0) || (RecordZy.length > 0 && RecordZy.filter(function (item) {
              return item.RECORDID == editDetail.dataParam.VOUCHER_RECORD_ZY[inx].RECORDID
                    && item.INX == editDetail.dataParam.VOUCHER_RECORD_ZY[inx].INX;
                }).length === 0)) {
                    RecordZy.push(editDetail.dataParam.VOUCHER_RECORD_ZY[inx]);
                }
            }
        }
        filterdata = RecordZy.filter(function (row) {
            return parseInt(row.RECORDID) == curRow.RECORDID;
        });
        Vue.set(editDetail.dataParam, "VOUCHER_RECORD_ZY", filterdata);
        if (RecordZy != undefined) {
            RecordZy = RecordZy.filter(function (row) {
                return parseInt(row.RECORDID) !== curRow.RECORDID;
            })
        }
    }
}


editDetail.clearKey = function () {
    editDetail.dataParam.VOUCHERID = null;
    editDetail.dataParam.VOUCHERNAME = null;
    editDetail.dataParam.VOUCHERTYPE = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.VOUCHER_MAKESQL = [];
    editDetail.dataParam.VOUCHER_RECORD = [];
    editDetail.dataParam.VOUCHER_RECORD_PZKM = [];
    editDetail.dataParam.VOUCHER_RECORD_ZY = [];
}

editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.VOUCHERNAME) {
        iview.Message.info("请输入凭证模板名称!");
        return false;
    };
    if (editDetail.dataParam.VOUCHER_MAKESQL.length > 0) {
        for (var i = 0; i < editDetail.dataParam.VOUCHER_MAKESQL.length; i++) {
            if (!editDetail.dataParam.VOUCHER_MAKESQL[i].SQLINX) {
                iview.Message.info("请选输入SQL序号!");
                return false;
            };
            if (!editDetail.dataParam.VOUCHER_MAKESQL[i].EXESQLTYPE) {
                iview.Message.info("请选选择SQL类型!");
                return false;
            };
        };
    };
    if (editDetail.dataParam.VOUCHER_RECORD.length > 0) {
        for (var i = 0; i < editDetail.dataParam.VOUCHER_RECORD.length; i++) {
            if (!editDetail.dataParam.VOUCHER_RECORD[i].RECORDID) {
                iview.Message.info("请选输入分录序号!");
                return false;
            };
            if (!editDetail.dataParam.VOUCHER_RECORD[i].RECORDNAME) {
                iview.Message.info("请输入分录名称!");
                return false;
            };
            if (!editDetail.dataParam.VOUCHER_RECORD[i].TYPE) {
                iview.Message.info("请选择分录方向!");
                return false;
            };
            if (!editDetail.dataParam.VOUCHER_RECORD[i].SQLINX) {
                iview.Message.info("请输入分录[" + VOUCHER_RECORD[i] .RECORDID+ "]取数SQL序号!");
                return false;
            };
        };
    };
    //处理科目
    RecordPzkm = RecordPzkm.filter(function (row) {
        return parseInt(row.RECORDID) !== curRecordRow.RECORDID;
    })
    for (inx in editDetail.dataParam.VOUCHER_RECORD_PZKM) {
        RecordPzkm.push(editDetail.dataParam.VOUCHER_RECORD_PZKM[inx]);
    }
    editDetail.dataParam.VOUCHER_RECORD_PZKM = RecordPzkm.filter(function (row) {
        return row.INX != "" && row.RECORDID != "";
    });

    //处理摘要
    RecordZy = RecordZy.filter(function (row) {
        return parseInt(row.RECORDID) != curRecordRow.RECORDID;
    })
    for (inx in editDetail.dataParam.VOUCHER_RECORD_ZY) {
        RecordZy.push(editDetail.dataParam.VOUCHER_RECORD_ZY[inx]);
    }
    editDetail.dataParam.VOUCHER_RECORD_ZY = RecordZy.filter(function (row) {
        return row.INX != "" && row.RECORDID != "";
    });

    if (editDetail.dataParam.VOUCHER_RECORD_PZKM.length > 0) {
        for (var i = 0; i < editDetail.dataParam.VOUCHER_RECORD_PZKM.length; i++) {
            if (parseInt(editDetail.dataParam.VOUCHER_RECORD_PZKM[i].SQLBJ) > 0 && !editDetail.dataParam.VOUCHER_RECORD_PZKM[i].SQLCOLTORECORD) {
                iview.Message.info("请选输入科目取值字段!");
                return false;
            };
        };
    };
    if (editDetail.dataParam.VOUCHER_RECORD_ZY.length > 0) {
        for (var i = 0; i < editDetail.dataParam.VOUCHER_RECORD_ZY.length; i++) {
            if (parseInt(editDetail.dataParam.VOUCHER_RECORD_ZY[i].SQLBJ) > 0 && !editDetail.dataParam.VOUCHER_RECORD_ZY[i].SQLCOLTORECORD) {
                iview.Message.info("请选输入摘要取值字段!");
                return false;
            };
        };
    };
    return true;
    //if (editDetail.dataParam.BILL_ADJUST_ITEM.length == 0) {
    //    iview.Message.info("请录入费用信息!");
    //    return false;
    //} else {
    //    for (var i = 0; i < editDetail.dataParam.BILL_ADJUST_ITEM.length; i++) {
    //        if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].CONTRACTID) {
    //            iview.Message.info("请录入租约!");
    //            return false;
    //        };
    //        if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].TERMID) {
    //            iview.Message.info("请选择收费项目!");
    //            return false;
    //        };
    //        if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].MUST_MONEY) {
    //            iview.Message.info("请录入费用金额!");
    //            return false;
    //        };
    //    };
    //};

    return true;
}