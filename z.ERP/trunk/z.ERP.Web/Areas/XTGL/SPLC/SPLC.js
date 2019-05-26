//应该提炼成一个模板,待后续处理
var splc = new Vue({
    el: "#List_Main",
    data: {
        spjdDrawer: false,
        spjdjgDrawer: false,
        colSplcjd: [
            { type: 'index', width: 60, align: 'center' },
            { title: '节点名称', key: 'JDNAME', width: 200 },
            { title: '类型', key: 'JDTYPENAME', width: 80 },
            { title: '角色组', key: 'ROLENAME', width: 100 },
            { title: '顺序', key: 'JDINX', width: 80, sortable: true }
        ],
        SPLCJD: [],

        colSplcjg: [
           // { title: '结果ID', key: 'JGID', width: 100 },
            { title: '条件描述', key: 'TJMC', width: 200 },
            { title: '结果类型', key: 'JGTYPE', width: 100 },
            { title: '类型名称', key: 'JGTYPENAME', width: 100 },
            { title: '节点名称', key: 'JDNAMEXS', width: 200 },
            { title: '结果描述', key: 'JGMC', width: 200 }
        ],
        SPLCJG: [],
        SPLCJGALL: [],
        disabled: false,
        BILLID: "",
        MENUID: "",
        JDNAME: "",
        JDTYPE: "",
        ROLEID: "",
        ROLENAME: "",
        JDINX: "",
        REPORTER_NAME: "",
        REPORTER_TIME: "",
        VERIFY_NAME: "",
        VERIFY_TIME: "",
        TERMINATE_NAME: "",
        TERMINATE_TIME: "",
        JDTYPENAME: "",
        TJMC: "",
        JGTYPE: "",
        JGMC: "",
        JGTYPENAME: "",
        JDID: "-1",
        JDNAMEXS: "",
        JGID: "",
        JGNANE: "",
        JDTYPESelect: "-1",
        STATUSMC: "",
        showPopRole: false,
        CanAdd: false,
        CanModify: true,
        CanSave: true,
        CanQuit: true,
        CanDel: true,
        CanExec: true,
        CanOver: true,
        CanSpjdDef: true,
        CanSpjdJg: true,
        CanBillid: false,
        srcPopRole: __BaseUrl + "/" + "Pop/Pop/PopRoleList/"
    },
    mounted: function () {
        this.ButtonEnable(false, true, true, true);
    },
    methods: {
        billEnter: function () {
            if (this.BILLID != "") {
                this.srch(this.BILLID);
            }
        },
        add: function () {
            this.ButtonEnable(true, true, true, true);
            this.CanSpjdDef = false;
            this.CanSpjdJg = false;
            this.CanBillid = true;
        },
        mod: function () {
            this.ButtonEnable(true, true, true, true);
            this.CanSpjdDef = false;
            this.CanSpjdJg = false;
            this.CanBillid = true;
        },
        isValidSave: function () {
            if (this.MENUID == "") {
                iview.Message.info("请选择流程设定菜单号!");
                return false;
            };
            if (this.SPLCJD.length == 0) {
                iview.Message.info("请维护流程节点!");
                return false;
            };
            if (this.SPLCJGALL.length == 0) {
                iview.Message.info("请维护流程节点步骤!");
                return false;
            };
            return true;
        },
        save: function () {
            var that = this;
            if (!that.isValidSave()) {
                return;
            };
            //先将保存置灰避免重复提交
            that.ButtonEnable(false, false, false, true);
            _.Ajax('Save', {
                SPLCDEFD: { MENUID: this.MENUID },
                SPLCJD: this.SPLCJD,
                SPLCJG: this.SPLCJGALL
            }, function (data) {
                that.BILLID = data;
                that.srch(that.BILLID);
                iview.Message.info("保存成功!");
            });
        },
        srch: function (billid) {
            var that = this;
            _.Ajax('Srch', {
                Data: { BILLID: billid }
            }, function (data) {
                that.BILLID = data.spd.BILLID;
                that.MENUID = data.spd.MENUID;
                that.STATUSMC = data.spd.STATUSMC;
                that.REPORTER_NAME = data.spd.REPORTER_NAME;
                that.REPORTER_TIME = data.spd.REPORTER_TIME;
                that.VERIFY_NAME = data.spd.VERIFY_NAME;
                that.VERIFY_TIME = data.spd.VERIFY_TIME;
                that.TERMINATE_NAME = data.spd.TERMINATE_NAME;
                that.TERMINATE_TIME = data.spd.TERMINATE_TIME;
                that.SPLCJD = data.spjd;
                that.SPLCJGALL = data.spjg;
                let splcjg = [];
                for (var i = 0; i <= data.spjg.length - 1; i++) {
                    if (data.spjg[i].JDTYPE == 1) {
                        splcjg.push(data.spjg[i]);
                    };
                };
                that.SPLCJG = splcjg;

                //单号输入完之后当查到数据,根据数据信息控制按钮的可操作
                let discanadd = false; //那种情况添加都是可以点的
                let discanmodify = false;//初始化为修改可以点
                if (that.VERIFY_NAME) {
                    discanmodify = true; //当审核了修改不可以点
                };
                let discanexec = true;
                if (!that.VERIFY_NAME) {
                    discanexec = false;
                };
                let discanover = true;
                if ((that.VERIFY_NAME) && (!that.TERMINATE_NAME)) {
                    discanover = false;
                };
                that.ButtonEnable(discanadd, discanmodify, discanexec, discanover);
            });
        },
        quit: function () {
            if (this.BILLID != "")
                this.ButtonEnable(false, false, false, true);
            else
                this.ButtonEnable(false, true, true, true);
        },
        del: function () {
            if (this.BILLID == "") {
                iview.Message.info("请确认要删除的单号!");
                return;
            };
            var that = this;
            that.ButtonEnable(false, true, true, true);
            _.Ajax('Delete', {
                Data: { BILLID: that.BILLID }
            }, function (data) {
                //删除后界面清空
                that.BILLID = "";
                that.MENUID = "";
                that.STATUSMC = "";
                that.REPORTER_NAME = "";
                that.REPORTER_TIME = "";
                that.VERIFY_NAME = "";
                that.VERIFY_TIME = "";
                that.TERMINATE_NAME = "";
                that.TERMINATE_TIME = "";
                that.SPLCJD = [];
                that.SPLCJGALL = [];
                that.SPLCJG = [];
                iview.Message.info("删除成功!");
            });
        },
        exec: function () {
            var that = this;
            _.Ajax('exec', {
                Data: { BILLID: that.BILLID }
            }, function (data) {
                that.BILLID = data;
                that.srch(that.BILLID);
                iview.Message.info("审核成功!");
            });
        },
        over: function () {
            var that = this;
            _.Ajax('over', {
                Data: { BILLID: that.BILLID }
            }, function (data) {
                that.BILLID = data;
                that.srch(that.BILLID);
                iview.Message.info("终止成功!");
            });
        },
        SpjdDef: function () {
            this.spjdDrawer = true;
        },
        sureSpjd: function () {

            //判断节点数据合法性
            //开始节点只能有一个,结束节点只能有一个
            if (!this.JDNAME) {
                iview.Message.info("请维护节点名称!");
                return false;
            };
            if (!this.JDTYPE) {
                iview.Message.info("请维护节点类型!");
                return false;
            };
            if (!this.ROLENAME) {
                iview.Message.info("请维护节点角色!");
                return false;
            };
            if (!this.JDINX) {
                iview.Message.info("请维护节点顺序!");
                return false;
            };
            if (this.SPLCJD.length > 0) {
                for (var i = 0; i < this.SPLCJD.length; i++) {
                    if (this.JDTYPE == 1) {
                        if (this.SPLCJD[i].JDTYPE == 1) {
                            iview.Message.info("一个审批流程只能有一个开始节点!");
                            return;
                        };
                    };
                    if (this.JDTYPE == 3) {
                        if (this.SPLCJD[i].JDTYPE == 3) {
                            iview.Message.info("一个审批流程只能有一个结束节点!");
                            return;
                        };
                    };
                    if (this.SPLCJD[i].JDINX == this.JDINX) {
                        iview.Message.info("当前的节点顺序已经存在!");
                        return;
                    };
                };
            };

            if (this.JDTYPE == 1) {
                this.JDTYPENAME = "开始";
            } else if (this.JDTYPE == 2) {
                this.JDTYPENAME = "表决";
            } else if (this.JDTYPE == 3) {
                this.JDTYPENAME = "结束";
            };
            let jdDataOne = {
                JDNAME: this.JDNAME,
                JDTYPE: this.JDTYPE,
                ROLENAME: this.ROLENAME,
                JDINX: this.JDINX,
                ROLEID: this.ROLEID,
                JDTYPENAME: this.JDTYPENAME,
                JDID: this.JDINX
            };
            this.SPLCJD.push(jdDataOne);
            this.spjdDrawer = false;
        },
        cancelSpjd: function () {
            this.spjdDrawer = false;
        },

        selectSpjd: function (currentRow, oldCurrentRow) {
            this.JDID = currentRow.JDID;

            this.JDTYPESelect = currentRow.JDTYPE;

            let splcjgLocal = [];
            for (var i = 0; i <= this.SPLCJGALL.length - 1; i++) {
                if (this.SPLCJGALL[i].JDID == this.JDID) {
                    splcjgLocal.push(this.SPLCJGALL[i]);
                }
            };
            this.SPLCJG = [];
            this.SPLCJG = splcjgLocal;
        },

        SpjdJg: function () {
            if (this.SPLCJD.length == 0) {
                iview.Message.info("请先定义审批流程节点!");
                return;
            };
            //并且选中一条流程节点数据
            if (parseInt(this.JDID) == -1) {
                iview.Message.info("请先单击选择流程节点!");
                return;
            };
            if (this.JDTYPESelect == 3) {
                iview.Message.info("节点类型为结束节点不需要定义节点步骤!");
                return;
            };
            this.spjdjgDrawer = true;
        },
        sureSpjdjg: function () {
            if (!this.TJMC) {
                iview.Message.info("请条件描述!");
                return false;
            };
            if (!this.JGTYPE) {
                iview.Message.info("请维护结果类型!");
                return false;
            };
            if (!this.JGMC) {
                iview.Message.info("请维护结果描述!");
                return false;
            };

            if ((this.JDTYPESelect == 1) && (this.JGTYPE != 1)) {
                iview.Message.info("节点类型为开始必须流程结果为通过!");
                return false;
            }
            if (this.JGTYPE == 1) {
                this.JGTYPENAME = "通过";
            } else if (this.JGTYPE == 2) {
                this.JGTYPENAME = "不通过";
            };
            for (var i = 0; i <= this.SPLCJD.length - 1; i++) {
                if (this.JGID == this.SPLCJD[i].JDID) {
                    this.JDNAMEXS = this.SPLCJD[i].JDNAME;
                };
            };
            let jgDataOne = {
                JDID: this.JDID,
                JDNAMEXS: this.JDNAMEXS,
                JGID: this.JGID,
                TJMC: this.TJMC,
                JGTYPE: this.JGTYPE,
                JGTYPENAME: this.JGTYPENAME,
                JGMC: this.JGMC
            };


            this.SPLCJGALL.push(jgDataOne);
            //把当前==this.JDID的数据重新给节点结果
            let splcjgLocal = [];
            for (var i = 0; i <= this.SPLCJGALL.length - 1; i++) {
                if (this.SPLCJGALL[i].JDID == this.JDID) {
                    splcjgLocal.push(this.SPLCJGALL[i]);
                }
            };
            this.SPLCJG = [];
            this.SPLCJG = splcjgLocal;
            this.spjdjgDrawer = false;
        },
        cancelSpjdjg: function () {
            this.spjdjgDrawer = false;
        },
        SelRole: function () {
            this.showPopRole = true;
        },
        ButtonEnable: function (b1, b2, b3, b4) {
            this.CanAdd = b1;
            this.CanModify = b2;
            this.CanSave = (!b1);
            this.CanQuit = (!b1);
            this.CanDel = b2;
            this.CanExec = b3;
            this.CanOver = b4;
        },
        SpjdDel: function () {
        },
        SpjdJgDel: function () {
        }
    }
});

splc.popCallBack = function (data) {
    if (splc.showPopRole) {
        splc.showPopRole = false;
        for (var i = 0; i < data.sj.length; i++) {
            splc.ROLEID = data.sj[i].ROLEID;
            splc.ROLENAME = data.sj[i].ROLENAME;
        };
    };
};