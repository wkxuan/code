editDetail.beforeVue = function () {
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.otherTitle = "结算信息";
    editDetail.screenParam.yearLoading = false;
    editDetail.screenParam.monthLoading = false;
    let tempList = [];
    for (let i = 1; i <= 31; i++) {
        tempList.push({
            value: i.toString(),
            label: i.toString(),
        });
    };
    editDetail.screenParam.fkrList = tempList;
    editDetail.screenParam.orgData = [];
    editDetail.screenParam.orgList = [];
    editDetail.screenParam.targetValue = "";

    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        editDetail.screenParam.orgList = $.map(data.Org, function (item) {
            return {
                label: item.ORGNAME,
                value: Number(item.ORGID)
            };
        });
        editDetail.screenParam.orgData = $.map(data.Org, function (item) {
            return {
                label: item.ORGNAME,
                value: Number(item.ORGID),
                branchid: item.BRANCHID,
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
                selectList: $.map(data.FeeRule.Obj.rows, function (item) {
                    return {
                        label: item.NAME,
                        value: item.ID
                    }
                })
            },
            {
                title: '滞纳规则', key: 'ZNGZID', cellType: "select", width: 120, enableCellEdit: true,
                selectList: $.map(data.LateFeeRule.Obj.rows, item => {
                    return {
                        label: item.NAME,
                        value: item.ID
                    }
                })
            },
            {
                title: '生成日期是否和租金保持一致', key: 'IF_RENT_FEERULE', width: 150, cellType: "select", enableCellEdit: true,
                selectList: [{ label: "是", value: 1 }, { label: "否", value: 2 }]
            }
        ];
        //租金表格
        editDetail.screenParam.colDefRENT = [
           { title: '时间段', key: 'INX', width: 80 },
           { title: '开始日期', key: 'STARTDATE', cellType: "date" },
           {
               title: '结束日期', key: 'ENDDATE', cellType: "date", enableCellEdit: true,
               onChange: function (index, row, data) {
                   if (data.length > index + 1) {
                       data[index + 1].STARTDATE = new Date(row.ENDDATE).getNextDate(0, 1).Format('yyyy-MM-dd');
                   }
                   row.SUMRENTS = 0;
                   editDetail.otherMethods.clearRentItem();
               }
           },
           {
               title: '结算方式', key: 'DJLX', cellType: "select", enableCellEdit: true,
               selectList: $.map(data.jsfs, item => {
                   return {
                       label: item.Value,
                       value: Number(item.Key)
                   }
               }),
               onChange: function (index, row, data) {
                   if (row.DJLX == "1") {
                       row.RENTS = 0;
                       row.RENTS_JSKL = 0;
                   } else if (row.DJLX == "2") {
                       row.RENTS_JSKL = 0;
                   }
                   editDetail.otherMethods.clearRentItem();
               }
           },
            {
                title: "保底金额", key: 'RENTS', cellType: "input", cellDataType: "number", cellDisabled: function (row) {
                    if (!row.DJLX || row.DJLX == "1") {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            {
                title: "保底扣率(%)", key: 'RENTS_JSKL', cellType: "input", cellDataType: "number", cellDisabled: function (row) {
                    if (!row.DJLX || row.DJLX == "1" || row.DJLX == "2") {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            { title: "总保底", key: 'SUMRENTS' }
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
};

editDetail.branchChange = function () {
    editDetail.dataParam.CONTRACT_SHOP = [];
    editDetail.dataParam.ORGID = null;
    editDetail.screenParam.orgList = editDetail.screenParam.orgData.filter(function (item) { return item.branchid == editDetail.dataParam.BRANCHID });
    editDetail.otherMethods.calculateArea();
};

editDetail.popCallBack = function (data) {
    if (editDetail.popConfig.open) {
        editDetail.popConfig.open = false;
        switch (editDetail.popConfig.title) {
            case "选择人员":
                for (let i = 0; i < data.sj.length; i++) {
                    editDetail.dataParam.SIGNER = data.sj[i].USERID;
                    editDetail.dataParam.SIGNER_NAME = data.sj[i].USERNAME;
                };
                break;
            case "选择商户":
                for (let i = 0; i < data.sj.length; i++) {
                    editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
                    editDetail.dataParam.MERNAME = data.sj[i].NAME;
                };
                break;
            case "选择品牌":
                let brand = editDetail.dataParam.CONTRACT_BRAND;
                for (let i = 0; i < data.sj.length; i++) {
                    if (brand.filter(function (item) { return (data.sj[i].BRANDID == item.BRANDID) }).length == 0) {
                        brand.push(data.sj[i]);
                    }
                };
                break;
            case "选择商铺":
                let shop = editDetail.dataParam.CONTRACT_SHOP;
                for (let i = 0; i < data.sj.length; i++) {
                    if (shop.filter(item=> { return (data.sj[i].SHOPID == item.SHOPID) }).length == 0) {
                        shop.push(data.sj[i]);
                    }
                };
                editDetail.otherMethods.calculateArea();
                break;
            case "选择费用项目":
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
                break;
            case "选择收款方式的费用项目":
                let selection = editDetail.vueObj.$refs.refPay.getSelection();
                let paylist = editDetail.dataParam.CONTRACT_PAY;
                for (let i = 0; i < selection.length; i++) {
                    for (let j = 0; j < paylist.length; j++) {
                        if (selection[i].PAYID == paylist[j].PAYID) {
                            paylist[j].TERMID = data.sj[0].TERMID;
                            paylist[j].TERMNAME = data.sj[0].NAME;
                        }
                    }
                }
                break;
            case "选择收款方式":
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
                break;
            case "选择商户付款信息":
                for (let i = 0; i < data.sj.length; i++) {
                    editDetail.dataParam.PAYMENTID = data.sj[i].PAYMENTID;
                    editDetail.screenParam.CARDNO = data.sj[i].CARDNO;
                    editDetail.screenParam.BANKNAME = data.sj[i].BANKNAME;
                    editDetail.screenParam.HOLDERNAME = data.sj[i].HOLDERNAME;
                    editDetail.screenParam.IDCARD = data.sj[i].IDCARD;
                };
                break;
        }
    }
};

editDetail.otherMethods = {
    //点击合同员
    srchSigner: function () {
        editDetail.screenParam.popParam = { USER_TYPE: "7" };
        editDetail.popConfig.title = "选择人员";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        editDetail.popConfig.open = true;
    },
    //点击商户弹窗
    srchMerchant: function () {
        editDetail.screenParam.popParam = { };
        editDetail.popConfig.title = "选择商户";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        editDetail.popConfig.open = true;
    },
    //点击品牌弹窗
    srchBrand: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择商户!");
            return;
        }
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
        editDetail.popConfig.title = "选择品牌";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopBrandList/";
        editDetail.popConfig.open = true;
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
    srchPayMent: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择商户!");
            return;
        }
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID, MERCHANTNAME: editDetail.dataParam.MERNAME };
        editDetail.popConfig.title = "选择商户付款信息";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantPaymentList/";
        editDetail.popConfig.open = true;
    },
    //清空付款信息
    clearPayMent: function () {
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
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, RENT_STATUS: 1, STATUS: 2 };
        editDetail.popConfig.title = "选择商铺";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopShopList/";
        editDetail.popConfig.open = true;
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
        editDetail.screenParam.popParam = { CUSTOM: 1 };
        editDetail.popConfig.title = "选择费用项目";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";
        editDetail.popConfig.open = true;
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
        editDetail.screenParam.popParam = {};
        editDetail.popConfig.title = "选择收款方式";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopPayList/";
        editDetail.popConfig.open = true;
    },
    srchPayCost: function () {
        let selection = this.$refs.refPay.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请先添加收款方式并且选中收款方式!");
            return;
        };
        editDetail.screenParam.popParam = { CUSTOM: 1 };
        editDetail.popConfig.title = "选择收款方式的费用项目";
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";
        editDetail.popConfig.open = true;
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
    addColRent: function () {
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
            loc.INX = (temp.length + 1) + "";
            loc.STARTDATE = new Date(temp[temp.length - 1].ENDDATE).addDay(1).Format('yyyy-MM-dd');
        } else {
            loc.INX = 1 + "";
            loc.STARTDATE = new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd');
        }
        loc.RENTS = 0;
        temp.push(loc);

        editDetail.screenParam.targetInfo.push({ INX: loc.INX, targetData: [] });
        editDetail.screenParam.targetValue = loc.INX;
    },
    //删除一行保底
    delColRent: function () {
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
            let targetData = editDetail.screenParam.targetInfo;
            for (let i = 0; i < selection.length; i++) {
                for (let j = 0; j < targetData.length; j++) {
                    if (targetData[j].INX == selection[i].INX) {
                        targetData.splice(j, 1);
                    }
                }
            }
            if (targetData.length) {
                editDetail.screenParam.targetValue = targetData[targetData.length - 1].INX;
            }
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

        editDetail.screenParam.monthLoading = true;
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
            temp[i].DESCRIPTION = `扣率组${i + 1}`;
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
    //计算合同建筑面积、租用面积
    calculateArea: function () {
        let shop = editDetail.dataParam.CONTRACT_SHOP;
        let areaBuild = 0, arear = 0;
        for (var i = 0; i < shop.length; i++) {
            if (shop[i].SHOPID) {
                areaBuild += Number(shop[i].AREA);
                arear += Number(shop[i].AREA_RENTABLE);
            }
        }
        editDetail.dataParam.AREA_BUILD = areaBuild;
        editDetail.dataParam.AREAR = arear;
        editDetail.otherMethods.clearRentItem();
    },
    //租约开始日期chenge
    contStartChange: function ($event) {
        if (new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd') == new Date($event).Format('yyyy-MM-dd')) {
            return;
        }
        editDetail.dataParam.CONT_START = $event;
        editDetail.dataParam.CONTRACT_RENT = [];
        editDetail.screenParam.targetInfo = [];
    },
    //租约结束日期chenge
    contEndChange: function ($event) {
        if (new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd') == new Date($event).Format('yyyy-MM-dd')) {
            return;
        }
        editDetail.dataParam.CONT_END = $event;
        editDetail.dataParam.CONTRACT_RENT = [];
        editDetail.screenParam.targetInfo = [];
    },
    //org加载
    orgOpenChange: function (event) {
        if (event) {
            if (!editDetail.dataParam.BRANCHID) {
                iview.Message.info("请先选择门店!");
                return;
            }
        }
    },
    //添加目标
    delTarget: function (inx) {
        let data = editDetail.screenParam.targetInfo;
        for (var i = 0; i < data.length; i++) {
            if (data[i].INX == inx) {
                data[i].targetData.splice(data[i].targetData.length - 1, 1);
                break;
            }
        }
    },
    //删除目标
    addTarget: function (inx) {
        let target = editDetail.screenParam.targetInfo;
        for (var i = 0; i < target.length; i++) {
            if (target[i].INX == inx) {
                let data = target[i].targetData;
                if (data.length && (!data[data.length - 1].SALES_END || (Number(data[data.length - 1].SALES_END) <= Number(data[data.length - 1].SALES_START)))) {
                    iview.Message.info(`目标${data[data.length - 1].TARGETID}的结束金额必须大于其起始金额`);
                    return;
                }
                if (data.length && !data[data.length - 1].JSKL) {
                    iview.Message.info(`请先确定目标${data[data.length - 1].TARGETID}的扣点`);
                    return;
                }
                let rent = editDetail.dataParam.CONTRACT_RENT.filter(function (item) {
                    if (target[i].INX == item.INX) {
                        return item;
                    }
                });
                data.push({
                    TARGETID: data.length + 1,
                    SALES_START: data.length ? Number(data[data.length - 1].SALES_END) : rent[0].RENTS,
                    SALES_END: null,
                    JSKL: null
                });
                break;
            }
        }
    },
    //初始化目标信息数据
    initTargetData: function () {
        editDetail.screenParam.targetInfo = [];
        let rent = editDetail.dataParam.CONTRACT_RENT;
        let jskl = editDetail.dataParam.CONTJSKL;
        let allRentItem = editDetail.dataParam.CONTRACT_RENTITEM;

        for (var i = 0; i < rent.length; i++) {
            var list = [];
            list = $.map(jskl, function (item, k) {
                if (rent[i].INX == item.INX) {
                    return {
                        TARGETID: item.TARGETID,
                        SALES_START: item.SALES_START,
                        SALES_END: item.SALES_END,
                        JSKL: item.JSKL
                    }
                }
            });
            let rentitem = allRentItem.filter(function (item) {
                if (rent[i].INX == item.INX) {
                    return item;
                }
            });
            editDetail.screenParam.targetInfo.push({
                INX: rent[i].INX,
                targetData: list,
                rentItemData: rentitem
            });
        }
    },
    //目标起始金额chenge
    salesStartChange: function (inx, groupno) {
        let info = editDetail.screenParam.targetInfo;
        for (var i = 0; i < info.length; i++) {
            if (info[i].INX == inx) {
                for (let j = 0; j < info[i].targetData.length; j++) {
                    if (info[i].targetData[j].TARGETID == groupno && j != 0) {
                        info[i].targetData[j - 1].SALES_END = info[i].targetData[j].SALES_START;
                    }
                }
            }
        }
    },
    //目标结束金额chenge
    salesEndChange: function (inx, groupno) {
        let info = editDetail.screenParam.targetInfo;
        for (var i = 0; i < info.length; i++) {
            if (info[i].INX == inx) {
                for (let j = 0; j < info[i].targetData.length; j++) {
                    if (info[i].targetData[j].TARGETID == groupno && j < info[i].targetData.length - 1) {
                        info[i].targetData[j + 1].SALES_START = info[i].targetData[j].SALES_END;
                    }
                }
            }
        }
    },
    //清空分解信息
    clearRentItem: function () {
        let info = editDetail.screenParam.targetInfo;
        for (var i = 0; i < info.length; i++) {
            info[i].rentItemData = [];
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
    editDetail.dataParam.STYLE = 2;
    editDetail.dataParam.STYLEMC = "联营合同";
    editDetail.dataParam.CONT_START = null;
    editDetail.dataParam.CONT_END = null;
    editDetail.dataParam.SIGNER_NAME = null;
    editDetail.dataParam.CONTRACT_OLD = null;
    editDetail.dataParam.ORGID = null;
    editDetail.dataParam.OPERATERULE = 0;
    editDetail.dataParam.AREA_BUILD = null;
    editDetail.dataParam.AREAR = null;
    editDetail.dataParam.FIT_BEGIN = null;
    editDetail.dataParam.FIT_END = null;
    editDetail.dataParam.JXSL = 0;
    editDetail.dataParam.XXSL = 0;
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
    editDetail.dataParam.TQFKR = null;
    editDetail.screenParam.TQFKR = [];
    editDetail.dataParam.PAYMENTID = null;
    editDetail.screenParam.CARDNO = null;
    editDetail.screenParam.BANKNAME = null;
    editDetail.screenParam.HOLDERNAME = null;
    editDetail.screenParam.IDCARD = null;

    editDetail.screenParam.targetInfo = [];
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
    editDetail.otherMethods.initTargetData();
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

    if (!data.ORGID) {
        iview.Message.info("请确定招商部门!");
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
 
    //扣率信息数据校验
    let targetInfo = editDetail.screenParam.targetInfo;
    let jsklData = [];
    for (let i = 0; i < targetInfo.length; i++) {
        if (targetInfo[i].targetData.length) {
            let item = targetInfo[i].targetData;
            let rent = contract_rent.filter(function (k) {
                return k.INX == targetInfo[i].INX;
            });
            for (let j = 0; j < item.length; j++) {
                if (item[j].SALES_START == "" || item[j].SALES_START == null) {
                    iview.Message.info(`时间段${targetInfo[i].INX}的目标${item[j].TARGETID}的起始金额不能为空!`);
                    return false;
                };
                if (rent[0].DJLX == 2 || rent[0].DJLX == 3 || rent[0].DJLX == 4) {
                    if (Number(item[j].SALES_START) < Number(rent[0].RENTS)) {
                        iview.Message.info(`时间段${targetInfo[i].INX}的目标${item[j].TARGETID}的起始金额不能小于保底金额${rent[0].RENTS}!`);
                        return false;
                    }
                };
                if (!item[j].SALES_END) {
                    iview.Message.info(`时间段${targetInfo[i].INX}的目标${item[j].TARGETID}的结束金额不能为空!`);
                    return false;
                };
                if (Number(item[j].SALES_END) <= Number(item[j].SALES_START)) {
                    iview.Message.info(`时间段${targetInfo[i].INX}的目标${item[j].TARGETID}的结束金额不能小于等于起始金额!`);
                    return false;
                };
                if (!item[j].JSKL) {
                    iview.Message.info(`时间段${targetInfo[i].INX}的目标${item[j].TARGETID}的目标扣率不能为空!`);
                    return false;
                };
                
                jsklData.push({
                    INX: targetInfo[i].INX,
                    GROUPNO: 0,
                    TARGETID: item[j].TARGETID,
                    STARTDATE: rent[0].STARTDATE,
                    ENDDATE: rent[0].ENDDATE,
                    SALES_START: item[j].SALES_START,
                    SALES_END: item[j].SALES_END,
                    JSKL: item[j].JSKL,
                });
            }
        };

        for (let j = 0; j < contract_rent.length ; j++) {
            if (contract_rent[j].INX == targetInfo[i].INX) {
                contract_rent[j].CONTRACT_RENTITEM = targetInfo[i].rentItemData;
            }
        };
    }
    data.CONTJSKL = jsklData;
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
    debugger
    return false;
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
        editDetail.otherMethods.initTargetData();
    });
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10600101"
    }, {
        id: "edit",
        authority: "10600101"
    }, {
        id: "del",
        authority: "10600101"
    }, {
        id: "save",
        authority: "10600101"
    }, {
        id: "abandon",
        authority: "10600101"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10600102",
        fun: function () {
            _.Ajax('ExecData', {
                Data: editDetail.dataParam,
            }, function (data) {
                iview.Message.info("审核成功");
                editDetail.refreshDataParam(data);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data && data.BILLID && data.STATUS == 1) {
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
        authority: "10600103",
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
        authority: "10600104",
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
        authority: "10600105",
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