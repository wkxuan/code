var splc = new Vue({
    el: "#List_Main",
    data: {
        spjdDrawer: false,
        spjdjgDrawer: false,
        colSplcjd: [
            { title: '节点名称', key: 'JDNAME', width: 200 },
            { title: '类型', key: 'JDTYPE', width: 80 },
            { title: '角色组', key: 'ROLENAME', width: 100 },
            { title: '顺序', key: 'JDINX', width: 80 }
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