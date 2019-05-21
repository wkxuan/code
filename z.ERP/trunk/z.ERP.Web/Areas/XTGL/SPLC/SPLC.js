var splc = new Vue({
    el: "#List_Main",
    data: {
        spjdDrawer: false,
        spjdjgDrawer: false,
        colSplcjd: [
            { type: 'selection', width: 60, align: 'center' },
            { title: '节点名称', key: 'JDNAME', width: 200 },
            { title: '类型', key: 'JDTYPE', width: 80 },
            { title: '角色组', key: 'ROLENAME', width: 100 },
            { title: '顺序', key: 'JDINX', width: 80, sortable: true }
        ],
        SPLCJD: [],

        colSplcjg: [
            { title: '结果ID', key: 'JGID', width: 100 },
            { title: '条件描述', key: 'TJMC', width: 200 },
            { title: '结果类型', key: 'JGTYPE', width: 100 },
            { title: '结果描述', key: 'JGMC', width: 200 }
        ],
        SPLCJG: [],
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
        showPopRole: false,
        srcPopRole: __BaseUrl + "/" + "Pop/Pop/PopRoleList/"
    },
    methods: {
        add: function () { },
        mod: function () { },
        save: function () { },
        quit: function () { },
        del: function () { },
        exec: function () { },
        over: function () { },
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
            let jdDataOne = {
                JDNAME: this.JDNAME,
                JDTYPE: this.JDTYPE,
                ROLENAME: this.ROLENAME,
                JDINX: this.JDINX,
                ROLEID: this.ROLEID
            };
            this.SPLCJD.push(jdDataOne);
            this.spjdDrawer = false;
        },
        cancelSpjd: function () {
            this.spjdDrawer = false;
        },
        SpjdJg: function () {
            if (this.SPLCJD.length == 0) {
                iview.Message.info("请先定义审批流程节点!");
                return;
            };
            //并且选中一条流程节点数据
            var selectSolcjd = this.$refs.selectSplcjd.getSelection();
            if (selectSolcjd.length != 1) {
                iview.Message.info("请选中一条流程节点!");
                return;
            };
            this.spjdjgDrawer = true;
        },
        sureSpjdjg: function () {
            this.spjdjgDrawer = false;
        },
        cancelSpjdjg: function () {
            this.spjdjgDrawer = false;
        },
        SelRole: function () {
            this.showPopRole = true;
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