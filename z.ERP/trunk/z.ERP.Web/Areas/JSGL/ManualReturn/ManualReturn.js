﻿var MR = new Vue({
    el: "#ManualReturn",
    data: {
        BRANCHID: "",
        ENDTIME: "",
        disabled: false,
        loading: false
    },
    methods: {
        Exec: function () {
            if (!MR.BRANCHID) {
                iview.Message.info("请选择门店!");
                return false
            }
            if (!MR.ENDTIME) {
                iview.Message.info("请选择截至时间!");
                return false
            }
            MR.loading = true;
            setTimeout(function () {
                MR.loading = false;
            }, 2000);
            _.Ajax('ExecReturn', {
                branchid: MR.BRANCHID, endtime: MR.ENDTIME
            }, function (data) {
                if (data.Result) {                   
                    iview.Message.success("执行成功!");
                } else {
                    iview.Message.error("执行失败!");
                }
            });
        }
    },   
});
