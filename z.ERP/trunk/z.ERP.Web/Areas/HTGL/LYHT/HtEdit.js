﻿editDetail.beforeVue = function () {
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

    //初始化弹窗所要传递参数
    editDetail.screenParam.ParentMerchant = {};
    editDetail.screenParam.ParentBrand = {};
    editDetail.screenParam.ParentShop = {};
    editDetail.screenParam.ParentFeeSubject = {};
    editDetail.screenParam.ParentPay = {};

    editDetail.screenParam.popParam = {};

    editDetail.screenParam.showSysuser = false;
    editDetail.screenParam.srcPopSigner = __BaseUrl + "/Pop/Pop/PopSysuserList/";

    editDetail.screenParam.showPopMerchant = false;
    editDetail.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";

    editDetail.screenParam.showPopBrand = false;
    editDetail.screenParam.srcPopBrand = __BaseUrl + "/Pop/Pop/PopBrandList/";

    editDetail.screenParam.showPopShop = false;
    editDetail.screenParam.srcPopShop = __BaseUrl + "/Pop/Pop/PopShopList/";

    editDetail.screenParam.showPopFeeSubject = false;
    editDetail.screenParam.srcPopFeeSubject = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";

    editDetail.screenParam.showPopPay = false;
    editDetail.screenParam.srcPopPay = __BaseUrl + "/Pop/Pop/PopPayList/";

    editDetail.screenParam.showPopPayment = false;
    editDetail.screenParam.srcPopPayment = __BaseUrl + "/Pop/Pop/PopMerchantPaymentList/";

    //品牌表格
    editDetail.screenParam.colDefPP = [
        {
            title: "品牌代码", key: 'BRANDID', cellType: "input", cellDataType: "number",
            onEnter: function (index, row, data) {
                if (!row.BRANDID) {
                    for (let item in row) {
                        row[item] = null;
                    }
                    return;
                }
                let tbData = data;
                _.Ajax('GetBrand', {
                    Data: { ID: row.BRANDID }
                }, function (data) {
                    if (data.dt) {
                        if (tbData.filter(item=> { return data.dt.NAME == item.NAME }).length) {
                            iview.Message.info('当前品牌代码已存在!');
                            return;
                        }
                        row.NAME = data.dt.NAME;
                    } else {
                        for (let item in row) {
                            row[item] = null;
                        }
                        iview.Message.info('当前品牌不存在!');
                    }
                });
            }
        },
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
        { title: "扣点", key: 'JSKL', cellType: "input", cellDataType: "number" },
    ];
    //租金表格
    editDetail.screenParam.colDefRENT = [
       { title: '时间段', key: 'INX' },
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
        { title: '时间段', key: 'INX' },
        {
            title: '开始日期', key: 'STARTDATE', cellType: "date"
        },
        {
            title: '结束日期', key: 'ENDDATE', cellType: "date"
        },
        { title: '年月', key: 'YEARMONTH' },
        { title: '租金', key: 'RENTS' },
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
    //收费项目
    editDetail.screenParam.colDefCOST = [
        { title: '序号', key: 'INX' },
        {
            title: "费用项目", key: 'TERMID', cellType: "input",
            onEnter: function (index, row, data) {
                let tbData = data;
                _.Ajax('GetFeeSubject', {
                    Data: { TRIMID: row.TERMID }
                }, function (data) {
                    if (data.dt) {
                        if (tbData.filter(item=> { return data.dt.NAME == item.NAME }).length) {
                            iview.Message.info('当前费用项目已存在!');
                            return;
                        }
                        row.NAME = data.dt.NAME;
                    } else {
                        row.TERMID = null;
                        row.NAME = null;
                        iview.Message.info('当前费用项目不存在!');
                    }
                });
            }
        },
        { title: "费用项目名称", key: 'NAME' },
        { title: '开始日期', key: 'STARTDATE', width: 150, cellType: "date", enableCellEdit: true },
        { title: '结束日期', key: 'ENDDATE', width: 150, cellType: "date", enableCellEdit: true },
        {
            title: '收费方式', key: 'SFFS', cellType: "select", width: 150, enableCellEdit: true,
            selectList: [{ label: "按日计算固定金额", value: 1 },
                         { label: "月固定金额", value: 2 }]
        },
        { title: "金额", key: 'COST', cellType: "input", cellDataType: "number" },
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

    editDetail.screenParam.splcs = [];
    editDetail.screenParam.splcjg = [];
    editDetail.screenParam.current = -1;
    editDetail.screenParam.JDID = "";
    editDetail.screenParam.BZ = "";
    editDetail.screenParam.JGTYPE = -1;
};

editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showSysuser) {
        editDetail.screenParam.showSysuser = false;
        for (let i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.SIGNER = data.sj[i].USERID;
            editDetail.dataParam.SIGNER_NAME = data.sj[i].USERNAME;
        };
    }
    if (editDetail.screenParam.showPopMerchant) {
        editDetail.screenParam.showPopMerchant = false;
        for (let i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERNAME = data.sj[i].NAME;
        };
    }
    if (editDetail.screenParam.showPopBrand) {
        editDetail.screenParam.showPopBrand = false;
        let brand = editDetail.dataParam.CONTRACT_BRAND;
        for (let i = 0; i < data.sj.length; i++) {
            if (brand.filter(item=> { return (data.sj[i].BRANDID == item.BRANDID) }).length == 0) {
                brand.push(data.sj[i]);
            }
        };
    }
    if (editDetail.screenParam.showPopShop) {
        editDetail.screenParam.showPopShop = false;
        let shop = editDetail.dataParam.CONTRACT_SHOP;
        for (let i = 0; i < data.sj.length; i++) {
            if (shop.filter(item=> { return (data.sj[i].SHOPID == item.SHOPID) }).length == 0) {
                shop.push(data.sj[i]);
            }
        };
        editDetail.otherMethods.calculateArea();
    }
    if (editDetail.screenParam.showPopFeeSubject) {
        editDetail.screenParam.showPopFeeSubject = false;
        if (editDetail.screenParam.payCost) {
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
        } else {
            let cost = editDetail.dataParam.CONTRACT_COST;
            for (let i = 0; i < data.sj.length; i++) {
                if (cost.filter(item=> { return (data.sj[i].TERMID == item.TERMID) }).length == 0) {
                    let loc = {};
                    editDetail.screenParam.colDefCOST.forEach(item=> {
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
                    cost.push(loc);
                }
            };
            for (let i = 0; i < cost.length; i++) {
                cost[i].INX = i + 1;
            }
        }

    }
    if (editDetail.screenParam.showPopPay) {
        editDetail.screenParam.showPopPay = false;
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
    if (editDetail.screenParam.showPopPayment) {
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
};

editDetail.otherMethods = {
    //点击合同员
    srchSigner: function () {
        Vue.set(editDetail.screenParam, "showSysuser", true);
        editDetail.screenParam.popParam = { USER_TYPE: "7" };
    },
    //点击商户弹窗
    srchMerchant: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        Vue.set(editDetail.screenParam, "showPopMerchant", true);
    },
    //点击品牌弹窗
    srchBrand: function () {
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
        Vue.set(editDetail.screenParam, "showPopBrand", true);
    },
    addRowBrand: function () {
        let temp = editDetail.dataParam.CONTRACT_BRAND || [];
        let loc = {};
        editDetail.screenParam.colDefPP.forEach(item=> {
            loc[item.key] = null;
        });
        temp.push(loc);
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
    //点击商铺弹窗
    srchShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        editDetail.screenParam.ParentShop = { BRANCHID: editDetail.dataParam.BRANCHID };
        Vue.set(editDetail.screenParam, "showPopShop", true);
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
        editDetail.screenParam.payCost = false;
        Vue.set(editDetail.screenParam, "showPopFeeSubject", true);
    },
    //添加租约收费项目信息
    addRowFeeSubject: function () {
        let temp = editDetail.dataParam.CONTRACT_COST || [];
        let loc = {};
        editDetail.screenParam.colDefCOST.forEach(item=> {
            loc[item.key] = null;
        });
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
        Vue.set(editDetail.screenParam, "showPopPay", true);
    },
    srchPayCost: function () {
        let selection = this.$refs.refPay.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请先添加收款方式并且选中收款方式!");
            return;
        };
        editDetail.screenParam.payCost = true;
        Vue.set(editDetail.screenParam, "showPopFeeSubject", true);
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
                        break;
                    }
                }
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
        editDetail.dataParam.CONTRACT_RENTITEM = [];
        let rent = editDetail.dataParam.CONTRACT_RENT;
        rent = [];
        debugger
        let yearsValue = getYears(new Date(editDetail.dataParam.CONT_START),
            new Date(editDetail.dataParam.CONT_END));
        let nestYear = null;
        let rentData = null;
        let beginHtq = editDetail.dataParam.CONT_START;
        let beginMzqHtq = editDetail.dataParam.CONT_START;

        let inx = 0;
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

            let yearMzq = getNextYears(editDetail.dataParam.FREE_BEGIN);

            if (yearMzq <= (new Date(editDetail.dataParam.FREE_END).Format('yyyy-MM-dd'))) {
                beginHtq = beginMzqHtq;
            }
        };
        let copyHtQsr = (beginMzqHtq);
        //循环年数
        for (let i = 0; i <= yearsValue; i++) {
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
        editDetail.screenParam.monthLoading = true;

        _.Ajax('lyYdFj', {
            Data: editDetail.dataParam.CONTRACT_RENT,
            ContractData: {
                CONT_START: editDetail.dataParam.CONT_START,
                CONT_END: editDetail.dataParam.CONT_END,
            }
        }, function (data) {
            let contractRent = editDetail.dataParam.CONTRACT_RENT;
            for (let i = 0; i < contractRent.length; i++) {
                let sumRents = 0;
                for (let j = 0; j < data.length; j++) {
                    if (data[j].INX == contractRent[i].INX) {
                        sumRents += parseFloat(data[j].RENTS);
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
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].GROUPNO == selection[i].GROUPNO) {
                        temp.splice(j, 1);
                        break;
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
                    SALES_END: 999999999,
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
    //清空付款信息
    clearpayment: function () {
    editDetail.dataParam.PAYMENTID = null;
    editDetail.screenParam.CARDNO = null;
    editDetail.screenParam.BANKNAME = null;
    editDetail.screenParam.HOLDERNAME = null;
    editDetail.screenParam.IDCARD = null;
    },
    //商户付款信息
    srchMPAYMENT: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择商户!");
            return;
        }
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID, MERCHANTNAME: editDetail.dataParam.MERNAME };
        Vue.set(editDetail.screenParam, "showPopPayment", true);
    },
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
    editDetail.dataParam.JXSL = 0.13;
    editDetail.dataParam.XXSL = 0.13;
    editDetail.dataParam.FREE_BEGIN = null;
    editDetail.dataParam.FREE_END = null;
    editDetail.dataParam.JHRQ = null;
    editDetail.dataParam.DESCRIPTION = null;
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
    if (editDetail.dataParam.CONTRACT_OLD) {
        if (!editDetail.dataParam.JHRQ) {
            iview.Message.info("请维护变更日期!");
            return false;
        };
        if (new Date(editDetail.dataParam.JHRQ).Format('yyyy-MM-dd') < new Date().Format('yyyy-MM-dd')) {
            iview.Message.info("变更日期不能小于当前日期!");
            return false;
        };

        editDetail.dataParam.CONTRACT_UPDATE = [];
        editDetail.dataParam.CONTRACT_UPDATE.push({
            CONTRACTID_OLD: editDetail.dataParam.CONTRACT_OLD,
            JHRQ: editDetail.dataParam.JHRQ
        });
    }
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请确认门店!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!editDetail.dataParam.CONT_START) {
        iview.Message.info("请维护开始日期!");
        return false;
    };

    if (!editDetail.dataParam.CONT_END) {
        iview.Message.info("请维护结束日期!");
        return false;
    };

    if (!editDetail.dataParam.FIT_BEGIN) {
        iview.Message.info("请维护装修开始日期!");
        return false;
    }

    if (!editDetail.dataParam.FIT_END) {
        iview.Message.info("请维护装修结束日期!");
        return false;
    }
    if (!editDetail.dataParam.FREE_BEGIN) {
        iview.Message.info("请维护免租开始日期!");
        return false;
    }
    if (!editDetail.dataParam.FREE_END) {
        iview.Message.info("请维护免租结束日期!");
        return false;
    }

    if (((new Date(editDetail.dataParam.FIT_BEGIN).Format('yyyy-MM-dd') < new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd')))
             ||
             ((new Date(editDetail.dataParam.FIT_BEGIN).Format('yyyy-MM-dd') > new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')))) {
        iview.Message.info("装修开始日期需在租约有效期内!");
        return false;
    };

    if (((new Date(editDetail.dataParam.FIT_END).Format('yyyy-MM-dd') < new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd')))
        ||
        ((new Date(editDetail.dataParam.FIT_END).Format('yyyy-MM-dd') > new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')))) {
        iview.Message.info("装修结束日期需在租约有效期内!");
        return false;
    };

    if (((new Date(editDetail.dataParam.FREE_BEGIN).Format('yyyy-MM-dd') < new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd')))
            ||
            ((new Date(editDetail.dataParam.FREE_BEGIN).Format('yyyy-MM-dd') > new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')))) {
        iview.Message.info("免租开始日期需在租约有效期内!");
        return false;
    };

    if (((new Date(editDetail.dataParam.FREE_END).Format('yyyy-MM-dd') < new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd')))
            ||
            ((new Date(editDetail.dataParam.FREE_END).Format('yyyy-MM-dd') > new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')))) {
        iview.Message.info("免租结束日期需在租约有效期内!");
        return false;
    };

    if (!editDetail.dataParam.ORGID) {
        iview.Message.info("请确定招商部门!");
        return false;
    };
    if (!editDetail.dataParam.OPERATERULE) {
        iview.Message.info("请确定合作方式!");
        return false;
    };
    //品牌数据校验
    let brand = editDetail.dataParam.CONTRACT_BRAND;
    if (brand.length == 0) {
        iview.Message.info("品牌不能为空!");
        return false;
    }
    for (let i = 0; i < brand.length; i++) {
        if (!brand[i].BRANDID) {
            iview.Message.info(`请确定品牌信息中第${i + 1}行的品牌!`);
            return false;
        };
    };
    //资产数据校验
    let shop = editDetail.dataParam.CONTRACT_SHOP;
    if (shop.length == 0) {
        iview.Message.info("商铺信息不能为空!");
        return false;
    }
    for (let i = 0; i < shop.length; i++) {
        if (!shop[i].SHOPID) {
            iview.Message.info(`请确定商铺信息中第${i+1}行的商铺!`);
            return false;
        };
    };
    //租金规则数据校验
    let contract_rent = editDetail.dataParam.CONTRACT_RENT;
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
            < new Date(editDetail.dataParam.CONT_START).Format('yyyy-MM-dd')) {
            iview.Message.info(`时间段${contract_rent[i].INX}的开始日期不能小于租约开始日期!`);
            return false;
        };

        if (new Date(contract_rent[i].ENDDATE).Format('yyyy-MM-dd')
            > new Date(editDetail.dataParam.CONT_END).Format('yyyy-MM-dd')) {
            iview.Message.info(`时间段${contract_rent[i].INX}的结束日期不能大于租约结束日期!`);
            return false;
        };
    };
    //租金分解数据校验
    let rentitem = editDetail.dataParam.CONTRACT_RENTITEM;
    if (rentitem.length == 0) {
        iview.Message.info("请生成租金分解信息!");
        return false;
    };
    for (let j = 0; j < rentitem.length; j++) {
        if (!rentitem[j].CREATEDATE) {
            iview.Message.info("请生成月度分解生成日期不能为空!");
            return false;
        };
        if (!rentitem[j].JMJE) {
            rentitem[j].JMJE = 0;
        };
        if (Number(rentitem[j].JMJE) > Number(rentitem[j].RENTS)) {
            iview.Message.info(`时间段${contract_rent[i].INX}的租金月度分解中减免金额不能大于租金金额!`);
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
    let group = editDetail.dataParam.CONTRACT_GROUP;
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
    let contjskl = editDetail.dataParam.CONTJSKL;
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
            };
        };
    };
    //费用项目数据校验
    let contract_cost = editDetail.dataParam.CONTRACT_COST;
    if (contract_cost.length) {
        for (let i = 0; i < contract_cost.length; i++) {
            if (!contract_cost[i].SFFS) {
                iview.Message.info("请选择收费项目中的收费方式!");
                return false;
            };
            if (!contract_cost[i].STARTDATE) {
                iview.Message.info(`请确定收费项目中序号${contract_cost[i].INX}的起始日期!`);
                return false;
            };
            if (!contract_cost[i].ENDDATE) {
                iview.Message.info(`请确定收费项目中序号${contract_cost[i].INX}的结束日期!`);
                return false;
            };
            if (new Date(contract_cost[i].STARTDATE).Format('yyyy-MM-dd') > new Date(contract_cost[i].ENDDATE).Format('yyyy-MM-dd')) {
                iview.Message.info(`收费项目中序号${contract_cost[i].INX}的起始日期不能大于结束日期!`);
                return false;
            };
            if (!contract_cost[i].COST || Number(contract_cost[i].COST) < 0) {
                iview.Message.info(`请确定收费项目中序号${contract_cost[i].INX}的金额,且大于0!`);
                return false;
            };
        };
    };
    //收款方式数据校验
    let contract_pay = editDetail.dataParam.CONTRACT_PAY;
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
            if (!contract_pay[i].STARTDATE) {
                iview.Message.info(`请确定收款方式${contract_pay[i].NAME}的起始日期!`);
                return false;
            };
            if (!contract_pay[i].ENDDATE) {
                iview.Message.info(`请确定收款方式${contract_pay[i].NAME}的结束日期!`);
                return false;
            };
            if (new Date(contract_pay[i].STARTDATE).Format('yyyy-MM-dd') > new Date(contract_pay[i].ENDDATE).Format('yyyy-MM-dd')) {
                iview.Message.info(`收款方式${contract_pay[i].NAME}的起始日期不能大于结束日期!`);
                return false;
            };
            if ((!contract_pay[i].KL) || (Number(contract_pay[i].KL) <= 0) || (Number(contract_pay[i].KL) > 100)) {
                iview.Message.info(`收款方式${contract_pay[i].NAME}的比例应大于0且小于等于100!`);
                return false;
            }
        };
    };

    if (editDetail.screenParam.TQFKR.length > 0) {
        editDetail.dataParam.TQFKR = editDetail.screenParam.TQFKR.join(',');
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
        //editDetail.otherMethods.srchSplc();
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
                setTimeout(function () {
                    window.location.reload();
                }, 100);
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
            editDetail.backData = DeepClone(editDetail.dataParam);
            editDetail.vueObj.disabled = true;
            editDetail.dataParam.CONTRACT_OLD = editDetail.dataParam.BILLID;
            editDetail.dataParam.BILLID = null;
            editDetail.dataParam.CONTRACTID = null;
        },
        enabled: function (disabled, data) {
            if (!disabled && data && data.BILLID && data.STATUS == 2 && data.HTLX == 1) {
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
