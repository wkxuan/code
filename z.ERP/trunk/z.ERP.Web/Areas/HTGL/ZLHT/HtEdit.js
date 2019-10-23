var ORG;
editDetail.beforeVue = function () {
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.screenParam.yearLoading = false;
    editDetail.screenParam.monthLoading = false;
    //初始化返款日信息
    //转换为string 是为了保持从后台返回后一致，来显示
    let tempList = [];
    for (let i = 1; i <= 31; i++) {
        tempList.push({
            value: i.toString(),
            label: i.toString(),
        });
    };
    editDetail.screenParam.fkrList = tempList;

    editDetail.screenParam.popParam = {};
    editDetail.screenParam.show = false;
    editDetail.screenParam.srcPop = "";
    editDetail.screenParam.title = "";

    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        let FeeRule = $.map(data.FeeRule.Obj.rows, item => {
            return {
                label: item.NAME,
                value: item.ID
            }
        });
        let LateFeeRule = $.map(data.LateFeeRule.Obj.rows, item => {
            return {
                label: item.NAME,
                value: item.ID
            }
        });
        editDetail.screenParam.orgList = $.map(data.Org, function (item) {
            return {
                label: item.ORGNAME,
                value: Number(item.ORGID)
            };
        });
        ORG = $.map(data.Org, function (item) {
            return {
                label: item.ORGNAME,
                value: Number(item.ORGID),
                branchid: item.BRANCHID,
            };
        });
        editDetail.screenParam.operateruleList = $.map(data.Operrule, function (item) {
            return {
                label: item.Value,
                value: Number(item.Key)
            };
        });
        //收费项目
        editDetail.screenParam.colDefCOST = [
            { title: '序号', key: 'INX', width: 50 },
            {
                title: "费用项目", key: 'TERMID', width: 90, cellType: "input",
                onEnter: function (index, row, data) {
                    let tbData = data;
                    _.Ajax('GetFeeSubject', {
                        Data: { TRIMID: row.TERMID }
                    }, function (data) {
                        if (data.dt) {
                            row.NAME = data.dt.NAME;
                            row.TYPE = data.dt.TYPE;
                        } else {
                            row.TERMID = null;
                            row.NAME = null;
                            iview.Message.info('当前费用项目不存在!');
                        }
                    });
                }
            },
            { title: "费用项目名称", key: 'NAME', width: 120 },
            {
                title: '开始日期', key: 'STARTDATE', width: 140, cellType: "date", enableCellEdit: true,
                onChange: function (index, row, data) {
                    if (row.TYPE == 1) {
                        row.ENDDATE = row.STARTDATE;
                        row.SFFS = 4;
                    }
                }
            },
            {
                title: '结束日期', key: 'ENDDATE', width: 140, cellType: "date", enableCellEdit: true,
                onChange: function (index, row, data) {
                    if (row.TYPE == 1) {
                        row.STARTDATE = row.ENDDATE;
                        row.SFFS = 4;
                    }
                }
            },
            {
                title: '收费方式', key: 'SFFS', cellType: "select", width: 130, enableCellEdit: true,
                selectList: [{ label: "日金额", value: 1 },
                        { label: "月金额", value: 2 },
                        { label: "按销售金额比例", value: 3 },
                        { label: "月固定金额", value: 4 }]
            },
            {
                title: "单价", key: 'PRICE', cellType: "input", cellDataType: "number",
                onChange: function (index, row, data) {
                    if (row.PRICE && editDetail.dataParam.AREAR) {
                        row.COST = (Number(row.PRICE) * Number(editDetail.dataParam.AREAR)).toFixed(2);
                    }
                }
            },
            {
                title: "金额", key: 'COST', cellType: "input", cellDataType: "number",
                onChange: function (index, row, data) {
                    if (row.COST && editDetail.dataParam.AREAR) {
                        row.PRICE = (Number(row.COST) / Number(editDetail.dataParam.AREAR)).toFixed(2);
                    }
                }
            },
            { title: "比例(%)", key: 'KL', cellType: "input", cellDataType: "number", width: 90 },
            {
                title: '收费规则', key: 'FEERULEID', cellType: "select", width: 120, enableCellEdit: true,
                selectList: FeeRule
            },
            {
                title: '滞纳规则', key: 'ZNGZID', cellType: "select", width: 120, enableCellEdit: true,
                selectList: LateFeeRule
            },
            {
                title: '生成日期是否和租金保持一致', key: 'IF_RENT_FEERULE', width: 150, cellType: "select", enableCellEdit: true,
                selectList: [{ label: "是", value: 1 }, { label: "否", value: 2 }]
            }
        ];
    });

    //品牌表格
    editDetail.screenParam.colDefPP = [
        { title: "品牌代码", key: 'BRANDID' },
        { title: '品牌名称', key: 'NAME' }
    ];
    //商铺表格
    editDetail.screenParam.colDefSHOP = [
        {
            title: "商铺代码", key: 'CODE', cellType: "input",
            onEnter: function (index, row, data) {
                if (!row.CODE) {
                    return;
                }
                let tbData = data;
                _.Ajax('GetShop', {
                    Data: { CODE: row.CODE, BRANCHID: editDetail.dataParam.BRANCHID }
                }, function (data) {
                    if (data.dt) {
                        if (tbData.filter(item=> { return data.dt.SHOPID == item.SHOPID }).length) {
                            iview.Message.info('当前商铺代码已存在!');
                            return;
                        }
                        row.SHOPID = data.dt.SHOPID;
                        row.CATEGORYID = data.dt.CATEGORYID;
                        row.CATEGORYCODE = data.dt.CATEGORYCODE;
                        row.CATEGORYNAME = data.dt.CATEGORYNAME;
                        row.AREA = data.dt.AREA_BUILD;
                        row.AREA_RENTABLE = data.dt.AREA_RENTABLE;
                    } else {
                        for (let item in row) {
                            row[item] = null;
                        }
                        iview.Message.info('当前单元代码不存在或者不属于当前门店!');
                    }
                    editDetail.otherMethods.calculateArea();
                });
            }
        },
        { title: '业态代码', key: 'CATEGORYCODE' },
        { title: '业态名称', key: 'CATEGORYNAME' },
        { title: '建筑面积', key: 'AREA' },
        { title: '租用面积', key: 'AREA_RENTABLE' }
    ];
    //扣率组
    editDetail.screenParam.colGroup = [
        { title: '扣点序号', key: 'GROUPNO' },
        { title: "基础扣点", key: 'JSKL', cellType: "input", cellDataType: "number" },
        { title: "描述", key: 'DESCRIPTION', cellType: "input" }
    ];
    //扣率信息
    editDetail.screenParam.colDefJskl = [
        {
            title: '时间段', key: 'INX', sortable: true, cellType: "input", cellDataType: "number",
            onChange: function (index, row, data) {
                let rent = editDetail.dataParam.CONTRACT_RENT;
                let dataf = rent.filter(item=> {
                    return item.INX == row.INX;
                });
                if (dataf.length) {
                    row.STARTDATE = dataf[0].STARTDATE;
                    row.ENDDATE = dataf[0].ENDDATE;
                    row.SALES_START = 0;
                    row.SALES_END = 999999999;
                } else {
                    row.STARTDATE = null;
                    row.ENDDATE = null;
                }
            }
        },
        {
            title: '扣点序号', key: 'GROUPNO', sortable: true, cellType: "input", cellDataType: "number",
            onChange: function (index, row, data) {
                let contract = editDetail.dataParam.CONTRACT_GROUP;
                let dataf = contract.filter(item=> {
                    return item.GROUPNO == row.GROUPNO;
                });
                if (dataf.length) {
                    row.JSKL = dataf[0].JSKL;
                } else {
                    row.JSKL = null;
                }
            }
        },
        { title: '开始日期', key: 'STARTDATE', sortable: true, cellType: "date" },
        { title: '结束日期', key: 'ENDDATE', cellType: "date" },
        { title: "起始金额", key: 'SALES_START', cellType: "input", cellDataType: "number" },
        { title: "结束金额(包含)", key: 'SALES_END', cellType: "input", cellDataType: "number" },
        { title: "扣点", key: 'JSKL', cellType: "input", cellDataType: "number" },
    ];
    //租金表格
    editDetail.screenParam.colDefRENT = [
       { title: '时间段', key: 'INX', width: 80 },
       { title: '开始日期', key: 'STARTDATE', cellType: "date" },
       {
           title: '结束日期', key: 'ENDDATE', cellType: "date", enableCellEdit: true,
           onChange: function (index, row, data) {
               if (data.length > index + 1) {
                   data[index + 1].STARTDATE = new Date((addDate(row.ENDDATE))).Format('yyyy-MM-dd');
               }
               row.SUMRENTS = 0;
               editDetail.dataParam.CONTRACT_RENTITEM = [];
               editDetail.otherMethods.upDataJskl();
           }
       },
       {
           title: '金额类型', key: 'DJLX', cellType: "select", enableCellEdit: true,
           selectList: [{ label: "日金额", value: 1 }, { label: "月金额", value: 2 }],
           onChange: function (index, row, data) {
               editDetail.dataParam.CONTRACT_RENTITEM = [];
           }
       },
        {
            title: "单价", key: 'PRICE', cellType: "input", cellDataType: "number",
            onChange: function (index, row, data) {
                row.RENTS = (Number(row.PRICE) * Number(editDetail.dataParam.AREAR)).toFixed(2);
                row.SUMRENTS = 0;
                editDetail.dataParam.CONTRACT_RENTITEM = [];
            }
        },
        {
            title: "租金", key: 'RENTS', cellType: "input", cellDataType: "number",
            onChange: function (index, row, data) {
                row.PRICE = (Number(row.RENTS) / Number(editDetail.dataParam.AREAR)).toFixed(2);
                row.SUMRENTS = 0;
                editDetail.dataParam.CONTRACT_RENTITEM = [];
            }
        },
        { title: "总租金", key: 'SUMRENTS' }
    ];
    //月度分解表格
    editDetail.screenParam.colDefRENTITEM = [
        { title: '时间段', key: 'INX', width: 80 },
        {
            title: '开始日期', key: 'STARTDATE', cellType: "date"
        },
        {
            title: '结束日期', key: 'ENDDATE', cellType: "date"
        },
        { title: '年月', key: 'YEARMONTH', width: 100 },
        {
            title: '租金', key: 'RENTS', cellType: "input", cellDataType: "number",
            onChange: function (index, row, data) {
                if (Number(row.JMJE) > Number(row.RENTS)) {
                    row.JMJE = row.RENTS;
                    iview.Message.info(`时间段${row.INX}的租金不能小于减免金额!`);
                }
                let rent = editDetail.dataParam.CONTRACT_RENT;
                let sum = 0;
                for (let i = 0; i < data.length; i++) {
                    if (data[i].INX == row.INX) {
                        sum += Number(data[i].RENTS);
                    }
                }
                for (let i = 0; i < rent.length; i++) {
                    if (rent[i].INX == row.INX) {
                        rent[i].SUMRENTS = sum;
                        break;
                    }
                }
            }
        },
        {
            title: '减免金额', key: 'JMJE', cellType: "input", cellDataType: "number",
            onChange: function (index, row, data) {
                if (Number(row.JMJE) > Number(row.RENTS)) {
                    row.JMJE = null;
                    iview.Message.info(`时间段${row.INX}的减免金额不能大于租金!`);
                }
            }
        },
        { title: '生成日期', key: 'CREATEDATE', cellType: "date", enableCellEdit: true },
        {
            title: '月清算标记', key: 'QSBJ', cellType: "select", enableCellEdit: true,
            selectList: [{ label: "是", value: 1 }, { label: "否", value: 2 }]
        },
        {
            title: '区间清算标记', key: 'QJQSBJ', cellType: "select", enableCellEdit: true,
            selectList: [{ label: "是", value: 1 }, { label: "否", value: 2 }]
        },
    ];
    //收款方式手续费
    editDetail.screenParam.colDefPAY = [
        { title: "收款方式", key: 'PAYID' },
        { title: "收款方式名称", key: 'NAME' },
        {
            title: "费用项目", key: 'TERMID', cellType: "input",
            onEnter: function (index, row, data) {
                _.Ajax('GetFeeSubject', {
                    Data: { TRIMID: row.TERMID }
                }, function (data) {
                    if (data.dt) {
                        row.TERMNAME = data.dt.NAME;
                    } else {
                        row.TERMID = null;
                        row.TERMNAME = null;
                        iview.Message.info('当前费用项目不存在!');
                    }
                });
            }
        },
        { title: "费用项目名称", key: 'TERMNAME' },
        { title: '开始日期', key: 'STARTDATE', cellType: "date", enableCellEdit: true },
        { title: '结束日期', key: 'ENDDATE', cellType: "date", enableCellEdit: true },
        { title: "比例(%)", key: 'KL', cellType: "input", cellDataType: "number" },
    ];

    editDetail.screenParam.splcjd = [];
    editDetail.screenParam.splcjg = [];
    editDetail.screenParam.curLcjd = -1;
    editDetail.screenParam.JDID = "";
    editDetail.screenParam.BZ = "";
    editDetail.screenParam.JGTYPE = -1;
};

editDetail.branchChange = function () {
    editDetail.dataParam.CONTRACT_SHOP = [];
    editDetail.dataParam.ORGID = null;
    editDetail.screenParam.orgList = ORG.filter(item=> { return item.branchid == editDetail.dataParam.BRANCHID });
    editDetail.otherMethods.calculateArea();
};
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPop) {
        editDetail.screenParam.showPop = false;
        if (editDetail.screenParam.title == "选择人员") {
            for (let i = 0; i < data.sj.length; i++) {
                editDetail.dataParam.SIGNER = data.sj[i].USERID;
                editDetail.dataParam.SIGNER_NAME = data.sj[i].USERNAME;
            };
        }
        if (editDetail.screenParam.title == "选择商户") {
            for (let i = 0; i < data.sj.length; i++) {
                editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
                editDetail.dataParam.MERNAME = data.sj[i].NAME;
            };
        }
        if (editDetail.screenParam.title == "选择品牌") {
            let brand = editDetail.dataParam.CONTRACT_BRAND;
            for (let i = 0; i < data.sj.length; i++) {
                if (brand.filter(item=> { return (data.sj[i].BRANDID == item.BRANDID) }).length == 0) {
                    brand.push(data.sj[i]);
                }
            };
        }
        if (editDetail.screenParam.title == "选择商铺") {
            let shop = editDetail.dataParam.CONTRACT_SHOP;
            for (let i = 0; i < data.sj.length; i++) {
                if (shop.filter(item=> { return (data.sj[i].SHOPID == item.SHOPID) }).length == 0) {
                    shop.push(data.sj[i]);
                }
            };
            editDetail.otherMethods.calculateArea();
        }
        if (editDetail.screenParam.title == "选择费用项目") {
            let cost = editDetail.dataParam.CONTRACT_COST;
            for (let i = 0; i < data.sj.length; i++) {
                let loc = {};
                editDetail.screenParam.colDefCOST.forEach(function (item) {
                    switch (item.key) {
                        case "TERMID":
                            loc[item.key] = data.sj[i].TERMID;
                            break;
                        case "NAME":
                            loc[item.key] = data.sj[i].NAME;
                            break;
                        default:
                            loc[item.key] = null;
                            break;
                    }
                });
                loc["TYPE"] = data.sj[i].TYPE;
                cost.push(loc);
            };
            for (let i = 0; i < cost.length; i++) {
                cost[i].INX = i + 1;
            }
        }
        if (editDetail.screenParam.title == "选择收款方式的费用项目") {
            let selection = editDetail.vueObj.$refs.refPay.getSelection();
            let pay = editDetail.dataParam.CONTRACT_PAY
            for (let i = 0; i < selection.length; i++) {
                for (let j = 0; j < pay.length; j++) {
                    if (selection[i].PAYID == pay[j].PAYID) {
                        pay[j].TERMID = data.sj[0].TERMID;
                        pay[j].TERMNAME = data.sj[0].NAME;
                    }
                }
            }
        }
        if (editDetail.screenParam.title == "选择收款方式") {
            let pay = editDetail.dataParam.CONTRACT_PAY;
            for (let i = 0; i < data.sj.length; i++) {
                let len = pay.filter(item=> { return data.sj[i].PAYID == item.PAYID });
                if (!len.length) {
                    let loc = {};
                    editDetail.screenParam.colDefPAY.forEach(item=> {
                        switch (item.key) {
                            case "PAYID":
                                loc[item.key] = data.sj[i].PAYID;
                                break;
                            case "NAME":
                                loc[item.key] = data.sj[i].NAME;
                                break;
                            default:
                                loc[item.key] = null;
                                break;
                        }
                    });
                    pay.push(loc);
                }
            };
        }
        if (editDetail.screenParam.title == "选择商户付款信息") {
            for (let i = 0; i < data.sj.length; i++) {
                for (let i = 0; i < data.sj.length; i++) {
                    editDetail.dataParam.PAYMENTID = data.sj[i].PAYMENTID;
                    editDetail.screenParam.CARDNO = data.sj[i].CARDNO;
                    editDetail.screenParam.BANKNAME = data.sj[i].BANKNAME;
                    editDetail.screenParam.HOLDERNAME = data.sj[i].HOLDERNAME;
                    editDetail.screenParam.IDCARD = data.sj[i].IDCARD;
                };
            };
        }
    }
};

editDetail.otherMethods = {
    //审核
    exec: function () {
        let _self = this;
        for (let i = 0; i <= editDetail.screenParam.splcjg.length - 1; i++) {
            if (editDetail.screenParam.JDID == editDetail.screenParam.splcjg[i].JGID) {
                editDetail.screenParam.JGTYPE = editDetail.screenParam.splcjg[i].JGTYPE;
                break;
            }
        }
        if (editDetail.screenParam.curLcjd != -1) {
            if (!editDetail.screenParam.JDID) {
                iview.Message.info("请确认结果选择!");
                return;
            }
            if (!editDetail.screenParam.BZ) {
                iview.Message.info("请确认描述信息!");
                return;
            }

            _.Ajax('ExecSplc', {
                Data: {
                    BILLID: editDetail.dataParam.CONTRACTID,
                    MENUID: "10600200",
                    JGJDID: editDetail.screenParam.JDID,
                    BZ: editDetail.screenParam.BZ,
                    CURJDID: editDetail.screenParam.curLcjd
                },
            }, function (data) {
                iview.Message.info("审批成功");
                window.location.reload();
            });
        }
        else {
            _.Ajax('ExecData', {
                Data: editDetail.dataParam,
            }, function (data) {
                iview.Message.info("审核成功");
                editDetail.refreshDataParam(data);
            });
        }
    },
    //查询审批流程
    srchSplc: function () {
        //找审批流程节点
        _.Ajax('Srchsplc', {
            Data: {
                MENUID: "10600200",
                JLBH: editDetail.dataParam.CONTRACTID,
            }
        }, function (data) {
            editDetail.screenParam.splcjd = data.splcjd;
            editDetail.screenParam.splcjg = data.splcjg;
            if (data.splcjg.length) {
                editDetail.screenParam.curLcjd = data.curJdid - 1;
            }
        });
    },
    //点击合同员
    srchSigner: function () {
        editDetail.screenParam.title = "选择人员";
        editDetail.screenParam.popParam = { USER_TYPE: "7" };
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    //点击商户弹窗
    srchMerchant: function () {
        editDetail.screenParam.title = "选择商户";
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    //点击品牌弹窗
    srchBrand: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择商户!");
            return;
        }
        editDetail.screenParam.title = "选择品牌";
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopBrandList/";
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    delBrand: function () {
        let selection = this.$refs.refBrand.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的品牌!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.CONTRACT_BRAND;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].BRANDID == selection[i].BRANDID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
    },
    //商户付款信息
    srchMPAYMENT: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择商户!");
            return;
        }
        editDetail.screenParam.title = "选择商户付款信息";
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID, MERCHANTNAME: editDetail.dataParam.MERNAME };
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopMerchantPaymentList/";
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    //清空付款信息
    clearpayment: function () {
        editDetail.dataParam.PAYMENTID = null;
        editDetail.screenParam.CARDNO = null;
        editDetail.screenParam.BANKNAME = null;
        editDetail.screenParam.HOLDERNAME = null;
        editDetail.screenParam.IDCARD = null;
    },
    //点击商铺弹窗
    srchShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        //查询空置的资产
        editDetail.screenParam.title = "选择商铺";
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, RENT_STATUS: 1, STATUS: 2 };
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopShopList/";
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    addRowShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        let temp = editDetail.dataParam.CONTRACT_SHOP || [];
        let loc = {};
        editDetail.screenParam.colDefSHOP.forEach(item=> {
            loc[item.key] = null;
        });
        temp.push(loc);
        editDetail.dataParam.CONTRACT_SHOP = temp;
    },
    delShop: function () {
        let selection = this.$refs.refShop.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的商铺!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.CONTRACT_SHOP;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].SHOPID == selection[i].SHOPID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
            this.calculateArea();
        }
    },
    //点击费用项目弹窗
    srchFeeSubject: function () {
        editDetail.screenParam.title = "选择费用项目";
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";
        editDetail.screenParam.popParam = { CUSTOM: 1 }
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    //添加租约收费项目信息
    addRowFeeSubject: function () {
        let temp = editDetail.dataParam.CONTRACT_COST || [];
        let loc = {};
        editDetail.screenParam.colDefCOST.forEach(item=> {
            loc[item.key] = null;
        });
        loc["TYPE"] = null;
        temp.push(loc);
        for (let i = 0; i < temp.length; i++) {
            temp[i].INX = i + 1;
        }
    },
    //删除租约收费项目信息
    delFeeSubject: function () {
        let selection = this.$refs.refCost.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            let temp = editDetail.dataParam.CONTRACT_COST;
            for (let i = 0; i < selection.length; i++) {
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].INX == selection[i].INX) {
                        temp.splice(j, 1);
                    }
                }
            }
            for (let i = 0; i < temp.length; i++) {
                temp[i].INX = i + 1;
            }
        }
    },
    //点击收款方式弹窗
    srchPay: function () {
        editDetail.screenParam.title = "选择收款方式";
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopPayList/";
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    srchPayCost: function () {
        let selection = this.$refs.refPay.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请先添加收款方式并且选中收款方式!");
            return;
        };
        editDetail.screenParam.title = "选择收款方式的费用项目";
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";
        editDetail.screenParam.popParam = { CUSTOM: 1 };
        Vue.set(editDetail.screenParam, "showPop", true);
    },
    //删除收款方式手续费
    delPay: function () {
        let selection = this.$refs.refPay.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.CONTRACT_PAY
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].PAYID == selection[i].PAYID) {
                        temp.splice(j, 1);
                    }
                }
            }
        }
    },
    //添加一行保底信息
    addColDefRENT: function () {
        if (editDetail.dataParam.CONTRACT_SHOP.length == 0) {
            iview.Message.info("请确定商铺!");
            return;
        };
        if (!editDetail.dataParam.CONT_START || !editDetail.dataParam.CONT_END) {
            iview.Message.info("请先维护租约开始结束日期!");
            return;
        }
        if (editDetail.dataParam.CONT_START > editDetail.dataParam.CONT_END) {
            iview.Message.info("租约结束日期不能小于开始日期！");
            return;
        }
        let loc = {};
        let temp = editDetail.dataParam.CONTRACT_RENT || [];
        editDetail.screenParam.colDefRENT.forEach(item=> {
            loc[item.key] = null;
        });
        loc.CONTRACT_RENTITEM = [];

        if (temp.length) {
            for (let i = 0; i < temp.length; i++) {
                if (!temp[i].ENDDATE) {
                    iview.Message.info(`请先维护时间段${temp[i].INX}的结束日期!`);
                    return;
                }
            }
            loc.INX = temp.length + 1;
            loc.STARTDATE = new Date((addDate(temp[temp.length - 1].ENDDATE))).Format('yyyy-MM-dd');
        } else {
            loc.INX = 1;
            loc.STARTDATE = new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd');
        }
        loc.RENTS = 0;
        temp.push(loc);
    },
    //删除一行保底
    delColDefRENT: function () {
        let selection = this.$refs.refRent.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            let temp = editDetail.dataParam.CONTRACT_RENT;
            let inxArr = $.map(selection, item=> {
                return item.INX;
            });
            let flag = false;
            for (let i = 0; i < inxArr.length; i++) {
                if (inxArr[i + 1] - inxArr[i] > 1) {
                    flag = true;
                }
            }
            if (flag || inxArr[inxArr.length - 1] < temp.length) {
                iview.Message.info("只能按照时间段序号从大到小删除!");
                return;
            }
            for (let i = 0; i < selection.length; i++) {
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].INX == selection[i].INX) {
                        temp.splice(j, 1);
                    }
                }
            }
            editDetail.dataParam.CONTRACT_RENTITEM = [];

            let contjskl = [];
            for (let i = 0; i < selection.length; i++) {
                let jskl = editDetail.dataParam.CONTJSKL.filter(item=> {
                    if (item.INX != selection[i].INX) {
                        return true;
                    }
                });
                contjskl = contjskl.concat(jskl);
            }
            editDetail.dataParam.CONTJSKL = contjskl;
        }
    },
    //按年度分解
    operPeriod: function () {
        if (editDetail.dataParam.CONTRACT_SHOP.length == 0) {
            iview.Message.info("请确定商铺!");
            return false;
        };
        if (!editDetail.dataParam.CONT_START || !editDetail.dataParam.CONT_END) {
            iview.Message.info("请先维护租约开始结束日期!");
            return;
        }
        if (editDetail.dataParam.CONT_START > editDetail.dataParam.CONT_END) {
            iview.Message.info("租约结束日期不能小于开始日期！");
            return;
        }
        editDetail.screenParam.yearLoading = true;
        editDetail.dataParam.CONTRACT_RENT = [];
        editDetail.dataParam.CONTRACT_RENTITEM = [];

        var yearsValue = getYears(new Date(editDetail.dataParam.CONT_START),
            new Date(editDetail.dataParam.CONT_END));
        var nestYear = null;
        var rentData = null;
        var beginHtq = editDetail.dataParam.CONT_START;
        var beginMzqHtq = editDetail.dataParam.CONT_START;

        var inx = 0;
        if (editDetail.dataParam.FREE_END) {
            rentData = {
                INX: inx + 1,
                STARTDATE: beginHtq,
                ENDDATE: editDetail.dataParam.FREE_END,
                DJLX: '2',  //默认月金额
                PRICE: 0,
                RENTS: 0,
                RENTS_JSKL: 0,
                SUMRENTS: 0
            }
            editDetail.dataParam.CONTRACT_RENT.push(rentData);
            inx = 1;

            beginMzqHtq = addDate(editDetail.dataParam.FREE_END, 1);

            var yearMzq = getNextYears(editDetail.dataParam.FREE_BEGIN);

            if (yearMzq <= (new Date(editDetail.dataParam.FREE_END).Format('yyyy-MM-dd'))) {
                beginHtq = beginMzqHtq;
            }
        };

        var copyHtQsr = (beginMzqHtq);


        //循环年数
        for (var i = 0; i <= yearsValue; i++) {
            if (i != 0) {
                beginHtq = copyHtQsr;
            }
            nestYear = getNextYears(beginHtq);
            if (nestYear < (new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd'))) {
                rentData = {
                    INX: i + 1 + inx,
                    STARTDATE: copyHtQsr,
                    ENDDATE: nestYear,
                    DJLX: '2',  //默认月金额
                    PRICE: 0,
                    RENTS: 0,
                    RENTS_JSKL: 0,
                    SUMRENTS: 0
                }
                editDetail.dataParam.CONTRACT_RENT.push(rentData);
                copyHtQsr = addDate(nestYear);
            } else {
                rentData = {
                    INX: i + 1 + inx,
                    STARTDATE: copyHtQsr,
                    ENDDATE: (new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')),
                    DJLX: '2',  //默认月金额
                    PRICE: 0,
                    RENTS: 0,
                    RENTS_JSKL: 0,
                    SUMRENTS: 0
                }
                editDetail.dataParam.CONTRACT_RENT.push(rentData);
                break;
            }
        }
        editDetail.screenParam.yearLoading = false;
    },
    //分解月度数据
    decompose: function () {
        let temp = editDetail.dataParam.CONTRACT_RENT;
        if (temp.length == 0) {
            iview.Message.info("请先维护时间段信息!");
            return;
        };
        for (let i = 0; i < temp.length; i++) {
            if (temp[i].ENDDATE == undefined) {
                iview.Message.info(`间段${temp[i].INX}的结束日期不能为空！`);
                return;
            }
            if (new Date(temp[i].STARTDATE).Format('yyyy-MM-dd') > new Date(temp[i].ENDDATE).Format('yyyy-MM-dd')) {
                iview.Message.info(`间段${temp[i].INX}的结束日期不能小于开始日期！`);
                return;
            }
            if (new Date(temp[i].ENDDATE).Format('yyyy-MM-dd') > new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')) {
                iview.Message.info(`间段${temp[i].INX}的结束日期不能大于租约结束日期！`);
                return;
            }
            if (!temp[i].DJLX) {
                iview.Message.info(`间段${temp[i].INX}的金额类型不能为空！`);
                return;
            }
            if (temp[i].PRICE == null || temp[i].RENTS == null) {
                iview.Message.info(`间段${temp[i].INX}的单价和租金不能为空！`);
                return;
            }
        };
        if (!editDetail.dataParam.FEERULE_RENT) {
            iview.Message.info("请先选择租金收费规则!");
            return;
        };
        if (!editDetail.dataParam.OPERATERULE) {
            iview.Message.info("请先选择合作方式!");
            return;
        };

        editDetail.screenParam.monthLoading = true;
        _.Ajax('zlYdFj', {
            Data: temp,
            ContractData: {
                CONT_START: editDetail.dataParam.CONT_START,
                CONT_END: editDetail.dataParam.CONT_END,
                FEERULE_RENT: editDetail.dataParam.FEERULE_RENT,
                STANDARD: editDetail.dataParam.STANDARD,
                OPERATERULE: editDetail.dataParam.OPERATERULE,
                AREAR: editDetail.dataParam.AREAR
            }
        }, function (data) {
            let contractRent = editDetail.dataParam.CONTRACT_RENT;
            for (let i = 0; i < contractRent.length; i++) {
                let sumRents = 0;
                for (let j = 0; j < data.length; j++) {
                    if (data[j].INX == contractRent[i].INX) {
                        sumRents += parseFloat(data[j].RENTS);
                        data[j].QSBJ = 1;
                        data[j].QJQSBJ = 2;
                    };
                };
                contractRent[i].SUMRENTS = sumRents.toFixed(2);
            };
            editDetail.dataParam.CONTRACT_RENTITEM = data;
            editDetail.screenParam.monthLoading = false;
        }, () => {
            editDetail.screenParam.monthLoading = false;
        });
    },
    //添加扣率组
    addColGroup: function () {
        let temp = editDetail.dataParam.CONTRACT_GROUP || [];
        let loc = {};
        editDetail.screenParam.colGroup.forEach(item=> {
            loc[item.key] = null;
        });
        temp.push(loc);
        for (let i = 0; i < temp.length; i++) {
            temp[i].GROUPNO = i + 1;
        }
    },
    //删除扣率组
    delColGroup: function () {
        let selection = this.$refs.refGroup.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的扣点组!");
        } else {
            let temp = editDetail.dataParam.CONTRACT_GROUP;
            for (let i = 0; i < selection.length; i++) {
                let contjskl = editDetail.dataParam.CONTJSKL;
                for (let j = 0; j < contjskl.length; j++) {
                    if (selection[i].GROUPNO == contjskl[j].GROUPNO) {
                        contjskl.splice(j, 1)
                    }
                }
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].GROUPNO == selection[i].GROUPNO) {
                        temp.splice(j, 1);
                    }
                }
            }
            for (let i = 0; i < temp.length; i++) {
                temp[i].GROUPNO = i + 1;
            }
        }
    },
    //自动生成扣率信息
    autoMakeGroup: function () {
        let contractJskl = editDetail.dataParam.CONTJSKL = [];
        let contractRent = editDetail.dataParam.CONTRACT_RENT;
        let contractGroup = editDetail.dataParam.CONTRACT_GROUP;

        if (contractRent.length == 0) {
            iview.Message.info("请先维护时间段信息!");
            return;
        }
        if (contractGroup.length == 0) {
            iview.Message.info("请先维护扣点组信息!");
            return;
        }
        for (let i = 0; i < contractRent.length; i++) {
            if (!contractRent[i].STARTDATE) {
                iview.Message.info(`请先维护时间段${contractRent[i].INX}的开始日期!`);
                return;
            }
            if (!contractRent[i].ENDDATE) {
                iview.Message.info(`请先维护时间段${contractRent[i].INX}的结束日期!`);
                return;
            }
        }
        //先循环时间段信息,再循环扣点组信息
        for (let i = 0; i < contractRent.length; i++) {
            for (j = 0; j < contractGroup.length; j++) {
                contractJskl.push({
                    GROUPNO: contractGroup[j].GROUPNO,
                    INX: contractRent[i].INX,
                    STARTDATE: contractRent[i].STARTDATE,
                    ENDDATE: contractRent[i].ENDDATE,
                    JSKL: contractGroup[j].JSKL,
                    SALES_START: 0,
                    SALES_END: 999999999
                });
            }
        }
    },
    //添加一行扣率信息
    addCONTJSKL: function () {
        let contractRent = editDetail.dataParam.CONTRACT_RENT;
        let contractGroup = editDetail.dataParam.CONTRACT_GROUP;

        if (contractRent.length == 0) {
            iview.Message.info("请先维护时间段信息!");
            return;
        }
        if (contractGroup.length == 0) {
            iview.Message.info("请先维护扣点组信息!");
            return;
        }
        for (let i = 0; i < contractRent.length; i++) {
            if (!contractRent[i].STARTDATE) {
                iview.Message.info(`请先维护时间段${contractRent[i].INX}的开始日期!`);
                return;
            }
            if (!contractRent[i].ENDDATE) {
                iview.Message.info(`请先维护时间段${contractRent[i].INX}的结束日期!`);
                return;
            }
        }
        let temp = editDetail.dataParam.CONTJSKL || [];
        let loc = {};
        editDetail.screenParam.colDefJskl.forEach(item=> {
            loc[item.key] = null;
        });
        temp.push(loc);
    },
    //删除扣率信息
    delCONTJSKL: function () {
        let selection = this.$refs.refJskl.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的扣率信息!");
        } else {
            let temp = editDetail.dataParam.CONTJSKL;
            for (let i = 0; i < selection.length; i++) {
                for (let j = 0; j < temp.length; j++) {
                    if (selection[i].INX == temp[j].INX && selection[i].GROUPNO == temp[j].GROUPNO) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
    },
    //计算合同建筑面积、租用面积
    calculateArea: function () {
        let shop = editDetail.dataParam.CONTRACT_SHOP;
        let areaBuild = 0, arear = 0;
        for (var i = 0; i < shop.length; i++) {
            if (shop[i].SHOPID) {
                areaBuild += shop[i].AREA;
                arear += shop[i].AREA_RENTABLE;
            }
        }
        editDetail.dataParam.AREA_BUILD = areaBuild;
        editDetail.dataParam.AREAR = arear;

        let rent = editDetail.dataParam.CONTRACT_RENT;
        for (var i = 0; i < rent.length; i++) {
            rent[i].RENTS = (Number(rent[i].PRICE) * Number(editDetail.dataParam.AREAR)).toFixed(2);
            rent[i].SUMRENTS = 0;
        }
        editDetail.dataParam.CONTRACT_RENTITEM = [];
    },
    //更新扣率信息时间
    upDataJskl: function () {
        let contjskl = editDetail.dataParam.CONTJSKL;
        let contractRent = editDetail.dataParam.CONTRACT_RENT;
        for (let i = 0; i < contjskl.length; i++) {
            for (let j = 0; j < contractRent.length; j++) {
                if (contjskl[i].INX = contractRent[i].INX) {
                    contjskl[i].STARTDATE = contractRent[i].STARTDATE;
                    contjskl[i].ENDDATE = contractRent[i].ENDDATE;
                }
            }
        }
    },
    contStartChange: function ($event) {
        if (new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd') == new Date($event).Format('yyyy-MM-dd')) {
            return;
        }
        editDetail.dataParam.CONT_START = $event;
        editDetail.dataParam.CONTRACT_RENT = [];
        editDetail.dataParam.CONTRACT_RENTITEM = [];
        editDetail.dataParam.CONTJSKL = [];
    },
    contEndChange: function ($event) {
        if (new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd') == new Date($event).Format('yyyy-MM-dd')) {
            return;
        }
        editDetail.dataParam.CONT_END = $event;
        editDetail.dataParam.CONTRACT_RENT = [];
        editDetail.dataParam.CONTRACT_RENTITEM = [];
        editDetail.dataParam.CONTJSKL = [];
    },
    STANDARDChange: function ($event) {
        editDetail.dataParam.CONTRACT_RENTITEM = [];
    },
    FEERULE_RENTChange: function ($event) {
        editDetail.dataParam.CONTRACT_RENTITEM = [];
    },
    //org加载
    ORGOPEN: function (event) {
        debugger
        if (event) {
            if (!editDetail.dataParam.BRANCHID) {
                iview.Message.info("请先选择门店!");
                return;
            }
        }
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.MERNAME = null;
    editDetail.dataParam.CONTRACTID_PAPER = null;
    editDetail.dataParam.STANDARD = 1;
    editDetail.dataParam.STATUS = 1;
    editDetail.dataParam.STATUSMC = null;
    editDetail.dataParam.STYLE = 1;
    editDetail.dataParam.STYLEMC = null;
    editDetail.dataParam.CONT_START = null;
    editDetail.dataParam.CONT_END = null;
    editDetail.dataParam.SIGNER_NAME = null;
    editDetail.dataParam.CONTRACT_OLD = null;
    editDetail.dataParam.ORGID = null;
    editDetail.dataParam.OPERATERULE = null;
    editDetail.dataParam.AREA_BUILD = null;
    editDetail.dataParam.AREAR = null;
    editDetail.dataParam.FIT_BEGIN = null;
    editDetail.dataParam.FIT_END = null;
    editDetail.dataParam.JXSL = 0;
    editDetail.dataParam.XXSL = 0;
    editDetail.dataParam.FREE_BEGIN = null;
    editDetail.dataParam.FREE_END = null;
    editDetail.dataParam.ZNID_RENT = null;
    editDetail.dataParam.FEERULE_RENT = null;
    editDetail.dataParam.TAB_FLAG = 1;
    editDetail.dataParam.QS_START = 2;
    editDetail.dataParam.JHRQ = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.REPORTER = null;
    editDetail.dataParam.REPORTER_NAME = null;
    editDetail.dataParam.REPORTER_TIME = null;
    editDetail.dataParam.CONTRACT_BRAND = [];
    editDetail.dataParam.CONTRACT_SHOP = [];
    editDetail.dataParam.CONTRACT_RENT = [];
    editDetail.dataParam.CONTRACT_RENTITEM = [];
    editDetail.dataParam.CONTRACT_GROUP = [];
    editDetail.dataParam.CONTJSKL = [];
    editDetail.dataParam.CONTRACT_COST = [];
    editDetail.dataParam.CONTRACT_PAY = [];
    editDetail.dataParam.STANDARD = 1;
    editDetail.dataParam.TQFKR = null;
    editDetail.screenParam.TQFKR = [];
    editDetail.dataParam.PAYMENTID = null;
    editDetail.screenParam.CARDNO = null;
    editDetail.screenParam.BANKNAME = null;
    editDetail.screenParam.HOLDERNAME = null;
    editDetail.screenParam.IDCARD = null;
};

editDetail.afterEdit = function () {
    if (editDetail.dataParam.HTLX == 2) {
        editDetail.vueObj.branchDisabled = true;
    }
};

editDetail.afterAbandon = function () {
    if (editDetail.dataParam.TQFKR) {
        editDetail.screenParam.TQFKR = editDetail.dataParam.TQFKR.split(',');
    }
    if (!editDetail.dataParam.PAYMENTID) {
        editDetail.screenParam.CARDNO = null;
        editDetail.screenParam.BANKNAME = null;
        editDetail.screenParam.HOLDERNAME = null;
        editDetail.screenParam.IDCARD = null;
    }
};

editDetail.IsValidSave = function () {
    let data = editDetail.dataParam;
    if (data.CONTRACT_OLD) {
        if (!data.JHRQ) {
            iview.Message.info("请维护变更启动日期!");
            return false;
        };

        if (new Date(data.CONT_END).Format('yyyy-MM-dd') < new Date(data.JHRQ).Format('yyyy-MM-dd')) {
            iview.Message.info("变更启动日期不能大于结束日期!");
            return false;
        };

        data.CONTRACT_UPDATE = [];
        data.CONTRACT_UPDATE.push({
            CONTRACTID_OLD: data.CONTRACT_OLD,
            JHRQ: data.JHRQ
        });
    }
    if (!data.BRANCHID) {
        iview.Message.info("请确认门店!");
        return false;
    };
    if (!data.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!data.CONT_START) {
        iview.Message.info("请维护开始日期!");
        return false;
    };

    if (!data.CONT_END) {
        iview.Message.info("请维护结束日期!");
        return false;
    };

    if (data.FIT_BEGIN) {
        if (((new Date(data.FIT_BEGIN).Format('yyyy-MM-dd') < new Date(data.CONT_START).Format('yyyy-MM-dd')))
             ||
             ((new Date(data.FIT_BEGIN).Format('yyyy-MM-dd') > new Date(data.CONT_END).Format('yyyy-MM-dd')))) {
            iview.Message.info("装修开始日期需在租约有效期内!");
            return false;
        };

        if (!data.FIT_END) {
            iview.Message.info("请维护装修结束日期!");
            return false;
        } else if (new Date(data.FIT_BEGIN).Format('yyyy-MM-dd') > new Date(data.FIT_END).Format('yyyy-MM-dd')) {
            iview.Message.info("装修开始日期不能小于装修结束日期!");
            return false;
        }
    }

    if (data.FIT_END) {
        if (((new Date(data.FIT_END).Format('yyyy-MM-dd') < new Date(data.CONT_START).Format('yyyy-MM-dd')))
        ||
        ((new Date(data.FIT_END).Format('yyyy-MM-dd') > new Date(data.CONT_END).Format('yyyy-MM-dd')))) {
            iview.Message.info("装修结束日期需在租约有效期内!");
            return false;
        };

        if (!data.FIT_BEGIN) {
            iview.Message.info("请维护装修开始日期!");
            return false;
        } else if (new Date(data.FIT_BEGIN).Format('yyyy-MM-dd') > new Date(data.FIT_END).Format('yyyy-MM-dd')) {
            iview.Message.info("装修开始日期不能小于装修结束日期!");
            return false;
        }
    }

    if (data.FREE_BEGIN) {
        if (((new Date(data.FREE_BEGIN).Format('yyyy-MM-dd') < new Date(data.CONT_START).Format('yyyy-MM-dd')))
            ||
            ((new Date(data.FREE_BEGIN).Format('yyyy-MM-dd') > new Date(data.CONT_END).Format('yyyy-MM-dd')))) {
            iview.Message.info("免租开始日期需在租约有效期内!");
            return false;
        };

        if (!data.FREE_END) {
            iview.Message.info("请维护免租结束日期!");
            return false;
        } else if (new Date(data.FREE_BEGIN).Format('yyyy-MM-dd') > new Date(data.FREE_END).Format('yyyy-MM-dd')) {
            iview.Message.info("免租开始日期不能小于免租结束日期!");
            return false;
        }
    }

    if (data.FREE_END) {
        if (((new Date(data.FREE_END).Format('yyyy-MM-dd') < new Date(data.CONT_START).Format('yyyy-MM-dd')))
            ||
            ((new Date(data.FREE_END).Format('yyyy-MM-dd') > new Date(data.CONT_END).Format('yyyy-MM-dd')))) {
            iview.Message.info("免租结束日期需在租约有效期内!");
            return false;
        };

        if (!data.FREE_BEGIN) {
            iview.Message.info("请维护免租开始日期!");
            return false;
        } else if (new Date(data.FREE_BEGIN).Format('yyyy-MM-dd') > new Date(data.FREE_END).Format('yyyy-MM-dd')) {
            iview.Message.info("免租开始日期不能小于免租结束日期!");
            return false;
        }
    }

    if (!data.ORGID) {
        iview.Message.info("请确定招商部门!");
        return false;
    };
    if (!data.OPERATERULE) {
        iview.Message.info("请确定合作方式!");
        return false;
    };
    //品牌数据校验
    let brand = data.CONTRACT_BRAND;
    if (brand.length == 0) {
        iview.Message.info("品牌不能为空!");
        return false;
    }
    for (let i = 0; i < brand.length; i++) {
        if (!brand[i].BRANDID) {
            iview.Message.info("请确定品牌!");
            return false;
        };
    };
    //资产数据校验
    let shop = data.CONTRACT_SHOP;
    if (shop.length == 0) {
        iview.Message.info("资产信息不能为空!");
        return false;
    }
    for (let i = 0; i < shop.length; i++) {
        if (!shop[i].SHOPID) {
            iview.Message.info("请确定资产信息的商铺!");
            return false;
        };
    };
    //租金规则数据校验
    let contract_rent = data.CONTRACT_RENT;
    if (contract_rent.length == 0) {
        iview.Message.info("请确定时间段结算信息!");
        return false;
    }
    for (let i = 0; i < contract_rent.length ; i++) {
        if (new Date(contract_rent[i].STARTDATE).Format('yyyy-MM-dd')
           > new Date(contract_rent[i].ENDDATE).Format('yyyy-MM-dd')) {
            iview.Message.info(`时间段${contract_rent[i].INX}的开始日期不能大于结束日期!`);
            return false;
        };

        if (new Date(contract_rent[i].STARTDATE).Format('yyyy-MM-dd')
            < new Date(data.CONT_START).Format('yyyy-MM-dd')) {
            iview.Message.info(`时间段${contract_rent[i].INX}的开始日期不能小于租约开始日期!`);
            return false;
        };

        if (new Date(contract_rent[i].ENDDATE).Format('yyyy-MM-dd')
            > new Date(data.CONT_END).Format('yyyy-MM-dd')) {
            iview.Message.info(`时间段${contract_rent[i].INX}的结束日期不能大于租约结束日期!`);
            return false;
        };
    };
    //租金分解数据校验
    let rentitem = data.CONTRACT_RENTITEM;
    if (rentitem.length == 0) {
        iview.Message.info("请生成租金分解信息!");
        return false;
    };
    for (let i = 0; i < rentitem.length; i++) {
        if (!rentitem[i].CREATEDATE) {
            iview.Message.info("租金分解信息中月度分解生成日期不能为空!");
            return false;
        };
        if (!rentitem[i].RENTS) {
            iview.Message.info(`租金分解信息中时间段${rentitem[i].INX}的租金不能为空!`);
            return false;
        };
        if (!rentitem[i].JMJE) {
            rentitem[i].JMJE = 0;
        };
        if (Number(rentitem[i].JMJE) > Number(rentitem[i].RENTS)) {
            iview.Message.info(`租金分解信息中时间段${rentitem[i].INX}的租金月度分解中减免金额不能大于租金金额!`);
            return false;
        };
    };
    //分类租金分解的数据，构造save数据结构
    for (let i = 0; i < contract_rent.length ; i++) {
        let item = [];
        for (let j = 0; j < rentitem.length ; j++) {
            if (contract_rent[i].INX == rentitem[j].INX) {
                item.push(rentitem[j]);
            }
        }
        contract_rent[i].CONTRACT_RENTITEM = item;
    };
    //扣率组数据校验
    let group = data.CONTRACT_GROUP;
    if (group.length == 0) {
        iview.Message.info("请确定扣率组信息!");
        return false;
    };
    for (let i = 0; i < group.length; i++) {
        if (group[i].JSKL == null) {
            iview.Message.info(`扣点序号${group[i].GROUPNO}的基础扣点不能为空!`);
            return false;
        };
    };
    //扣率信息数据校验
    let contjskl = data.CONTJSKL;
    if (contjskl.length) {
        for (let i = 0; i < contjskl.length; i++) {
            if (!contjskl[i].INX) {
                iview.Message.info(`扣率信息中第${i + 1}行的时间段不能为空!`);
                return false;
            };
            if (!contjskl[i].GROUPNO) {
                iview.Message.info(`扣率信息中第${i + 1}行的扣点序号不能为空!`);
                return false;
            };
            for (let j = i + 1; j < contjskl.length; j++) {
                if ((contjskl[i].INX == contjskl[j].INX) && (contjskl[i].GROUPNO == contjskl[j].GROUPNO)) {
                    iview.Message.info(`扣率信息中第${i + 1}行和第${j + 1}行的时间段与扣点序号重复!`);
                    return false;
                }
                if (
                    (contjskl[i].INX == contjskl[j].INX) &&
                    (contjskl[i].GROUPNO == contjskl[j].GROUPNO) &&
                    ((parseFloat(contjskl[j].SALES_START) < parseFloat(contjskl[i].SALES_END)) ||
                      (parseFloat(contjskl[j].SALES_END) < parseFloat(contjskl[i].SALES_END)) ||
                    (parseFloat(contjskl[j].SALES_START) < parseFloat(contjskl[i].SALES_START)) ||
                      (parseFloat(contjskl[j].SALES_END) < parseFloat(contjskl[i].SALES_START))
                    )
                   ) {
                    iview.Message.info("扣率组销售额段之间有重叠!");
                    return false;

                }
            };
        };
    };
    //费用项目数据校验
    let contract_cost = data.CONTRACT_COST;
    if (contract_cost.length) {
        for (let i = 0; i < contract_cost.length; i++) {
            if (!contract_cost[i].SFFS) {
                iview.Message.info("请选择收费项目中的收费方式!");
                return false;
            };
            if (!contract_cost[i].FEERULEID) {
                iview.Message.info("请选择收费项目中的收费规则!");
                return false;
            };

            if (!contract_cost[i].STARTDATE) {
                iview.Message.info(`请确定收费项目中序号${contract_cost[i].INX}的起始日期!`);
                return false;
            };
            if ((!data.CONTRACT_OLD) || (contract_cost[i].TYPE != 1 && data.CONTRACT_OLD)) {
                if (new Date(contract_cost[i].STARTDATE).Format('yyyy-MM-dd') < new Date(data.CONT_START).Format('yyyy-MM-dd')) {
                    iview.Message.info(`收费项目中序号${contract_cost[i].INX}的开始日期必须在租约有效期内!`);
                    return false;
                };

                if (new Date(contract_cost[i].ENDDATE).Format('yyyy-MM-dd') > new Date(data.CONT_END).Format('yyyy-MM-dd')) {
                    iview.Message.info(`收费项目中序号${contract_cost[i].INX}的结束日期必须在租约有效期内!`);
                    return false;
                };
            }

            if (!contract_cost[i].ENDDATE) {
                iview.Message.info(`请确定收费项目中序号${contract_cost[i].INX}的结束日期!`);
                return false;
            };

            if (new Date(contract_cost[i].STARTDATE).Format('yyyy-MM-dd') > new Date(contract_cost[i].ENDDATE).Format('yyyy-MM-dd')) {
                iview.Message.info(`收费项目中序号${contract_cost[i].INX}的开始日期不能大于结束日期!`);
                return false;
            };

            if (!contract_cost[i].IF_RENT_FEERULE) {
                iview.Message.info("请确定收费项目中的生成日期是否和租金保持一致!");
                return false;
            };

            if (((contract_cost[i].SFFS == 1) || (contract_cost[i].SFFS == 2) || (contract_cost[i].SFFS == 4))
                && ((!contract_cost[i].COST) || (contract_cost[i].COST <= 0))) {
                iview.Message.info("请确定每月收费项目中的固定费用型收费方式对应的金额!");
                return false;
            }
            if ((contract_cost[i].SFFS == 3) && ((Number(contract_cost[i].KL) <= 0) || (Number(contract_cost[i].KL) > 100))) {
                iview.Message.info("请确定每月收费项目中正确的销售金额比例，且值大于0小于等于100!");
                return false;
            }

            for (let j = i + 1; j < contract_cost.length; j++) {
                if (contract_cost[i].TERMID == contract_cost[j].TERMID) {
                    let date_is = new Date(contract_cost[i].STARTDATE).Format('yyyy-MM-dd');
                    let date_ie = new Date(contract_cost[i].ENDDATE).Format('yyyy-MM-dd');
                    let date_js = new Date(contract_cost[j].STARTDATE).Format('yyyy-MM-dd');
                    let date_je = new Date(contract_cost[j].ENDDATE).Format('yyyy-MM-dd');
                    if ((date_is >= date_js && date_is <= date_je) ||
                        (date_ie >= date_js && date_ie <= date_je)) {
                        iview.Message.info(`费用项目为"${contract_cost[i].NAME}"的开始日期与结束日期之间的时间段不能交叉!`);
                        return false;
                    };
                }
            }
        };
    };
    //收款方式数据校验
    let contract_pay = data.CONTRACT_PAY;
    if (contract_pay.length) {
        for (let i = 0; i < contract_pay.length; i++) {
            if (!contract_pay[i].PAYID) {
                iview.Message.info(`请确定第${i + 1}行的收款方式!`);
                return false;
            };
            if (!contract_pay[i].TERMNAME) {
                iview.Message.info(`请确定第${i + 1}行的费用项目!`);
                return false;
            };

            if (new Date(contract_pay[i].STARTDATE).Format('yyyy-MM-dd') < new Date(data.CONT_START).Format('yyyy-MM-dd')) {
                iview.Message.info(`收款方式"${contract_pay[i].NAME}"的开始日期必须在租约有效期内!`);
                return false;
            };

            if (new Date(contract_pay[i].ENDDATE).Format('yyyy-MM-dd') > new Date(data.CONT_END).Format('yyyy-MM-dd')) {
                iview.Message.info(`收款方式"${contract_pay[i].NAME}"的结束日期必须在租约有效期内!`);
                return false;
            };

            if (!contract_pay[i].STARTDATE) {
                iview.Message.info(`请确定收款方式"${contract_pay[i].NAME}"的开始日期!`);
                return false;
            };
            if (!contract_pay[i].ENDDATE) {
                iview.Message.info(`请确定收款方式"${contract_pay[i].NAME}"的结束日期!`);
                return false;
            };

            if (new Date(contract_pay[i].STARTDATE).Format('yyyy-MM-dd') > new Date(contract_pay[i].ENDDATE).Format('yyyy-MM-dd')) {
                iview.Message.info(`收款方式"${contract_pay[i].NAME}"的开始日期不能大于结束日期!`);
                return false;
            };
            if ((!contract_pay[i].KL) || (Number(contract_pay[i].KL) <= 0) || (Number(contract_pay[i].KL) > 100)) {
                iview.Message.info(`收款方式"${contract_pay[i].NAME}"的比例应大于0且小于等于100!`);
                return false;
            }
        };
    };

    if (editDetail.screenParam.TQFKR.length != 0) {
        data.TQFKR = editDetail.screenParam.TQFKR.join(',');
    };
    return true;
};

editDetail.showOne = function (data) {
    _.Ajax('SearchContract', {
        Data: { CONTRACTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.contract);
        editDetail.dataParam.BILLID = data.contract.CONTRACTID;
        editDetail.dataParam.CONTRACT_BRAND = data.contractBrand;
        editDetail.dataParam.CONTRACT_SHOP = data.contractShop;
        editDetail.dataParam.CONTRACT_RENT = data.ContractParm.CONTRACT_RENT;
        editDetail.dataParam.CONTRACT_RENTITEM = data.ContractRentParm.CONTRACT_RENTITEM;
        editDetail.dataParam.CONTRACT_GROUP = data.ContractParm.CONTRACT_GROUP;
        editDetail.dataParam.CONTJSKL = data.ContractParm.CONTJSKL;
        editDetail.dataParam.CONTRACT_COST = data.contractCost;
        editDetail.dataParam.CONTRACT_PAY = data.contractPay;
        if (data.contractPayment) {   //付款方式
            editDetail.screenParam.CARDNO = data.contractPayment.CARDNO;
            editDetail.screenParam.BANKNAME = data.contractPayment.BANKNAME;
            editDetail.screenParam.HOLDERNAME = data.contractPayment.HOLDERNAME;
            editDetail.screenParam.IDCARD = data.contractPayment.IDCARD;
        }
        if (data.contract.TQFKR) {
            Vue.set(editDetail.screenParam, "TQFKR", data.contract.TQFKR.split(',') || []);
        };
        editDetail.otherMethods.srchSplc();
    });
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10600201"
    }, {
        id: "edit",
        authority: "10600201"
    }, {
        id: "del",
        authority: "10600201"
    }, {
        id: "save",
        authority: "10600201"
    }, {
        id: "abandon",
        authority: "10600201"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10600202",
        fun: function () {
            editDetail.otherMethods.exec();
        },
        enabled: function (disabled, data) {

            console.log(editDetail.screenParam);
            //没有审批流程或者有审批流程并且有当前审批流程节点权限才有审核按钮
            let splcjdLength = (editDetail.screenParam.splcjd.length > 0);
            let splcjgLength = (editDetail.screenParam.splcjg.length > 0);
            console.log(splcjdLength);
            console.log(splcjgLength);
            if ((!disabled && data.BILLID && (data.STATUS == 1) && !splcjdLength)
                ||
                (splcjdLength && splcjgLength)
                ) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }, {
        id: "change",
        name: "变更",
        icon: "md-star",
        authority: "10600203",
        fun: function () {
            _.Ajax('checkHtBgData', {
                Data: editDetail.dataParam
            }, function (data) {
                if (data) {
                    iview.Message.info(`原租约${editDetail.dataParam.CONTRACTID}已存在未启动的变更合同${data},不能再次变更!`);
                } else {
                    editDetail.backData = DeepClone(editDetail.dataParam);
                    editDetail.vueObj.branchDisabled = true;
                    editDetail.vueObj.disabled = true;
                    editDetail.dataParam.CONTRACT_OLD = editDetail.dataParam.BILLID;
                    editDetail.dataParam.BILLID = null;
                    editDetail.dataParam.CONTRACTID = null;

                    editDetail.dataParam.REPORTER = null;
                    editDetail.dataParam.REPORTER_NAME = null;
                    editDetail.dataParam.REPORTER_TIME = null;
                    editDetail.dataParam.VERIFY = null;
                    editDetail.dataParam.VERIFY_NAME = null;
                    editDetail.dataParam.VERIFY_TIME = null;

                    editDetail.dataParam.INITINATE = null;
                    editDetail.dataParam.INITINATE_NAME = null;
                    editDetail.dataParam.INITINATE_TIME = null;
                    editDetail.dataParam.TERMINATE = null;
                    editDetail.dataParam.TERMINATE_NAME = null;
                    editDetail.dataParam.TERMINATE_TIME = null;
                    editDetail.dataParam.STATUSMC = null;
                }
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.BILLID && (data.STATUS == 2 || data.STATUS == 3 || data.STATUS == 4) && data.HTLX == 1) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }, {
        id: "begin",
        name: "启动",
        icon: "md-arrow-dropright-circle",
        authority: "10600204",
        fun: function () {
            _.MessageBox("确认启动？", function () {
                _.Ajax('StartUp', {
                    Data: editDetail.dataParam
                }, function (data) {
                    iview.Message.info("启动成功！");
                    editDetail.refreshDataParam(data);
                });
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.BILLID && data.STATUS == 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }, {
        id: "Stop",
        name: "终止",
        icon: "md-close-circle",
        authority: "10600205",
        fun: function () {
            _.MessageBox("确认终止？", function () {
                _.Ajax('Stop', {
                    Data: editDetail.dataParam
                }, function (data) {
                    iview.Message.info("终止成功！");
                    editDetail.refreshDataParam(data);
                });
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.BILLID && (data.HTLX == 2 && data.STATUS == 2)) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};

function getYears(date1, date2) { //获取两个年份之差
    let years = date2.getFullYear() - date1.getFullYear();
    return years;
};

function getNextYears(date) { //获取当前日前的下一年上一天
    let tomYear = new Date(date);
    tomYear.setFullYear(tomYear.getFullYear() + 1); //下一年的今天
    tomYear.setDate(tomYear.getDate() - 1); //下一年的昨天
    tomYear = new Date(tomYear).Format('yyyy-MM-dd');
    return (tomYear);
};

function addDate(date, days) {
    if (days == undefined || days == '') {
        days = 1;
    }
    let lastDay = new Date(date); //日前复制防止原来日期发生变化
    lastDay.setDate(lastDay.getDate() + days); //日期加天数
    lastDay = new Date(lastDay).Format('yyyy-MM-dd');
    return lastDay;
};
