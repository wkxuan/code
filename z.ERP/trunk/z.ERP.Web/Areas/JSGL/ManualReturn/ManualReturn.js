var MR = new Vue({
    el: "#ManualReturn",
    data: {
        BRANCHID: "",
        ENDTIME: "",
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
            _.Ajax('ExecReturn', {
                branchid: MR.BRANCHID, endtime: MR.ENDTIME
            }, function (data) {
                debugger
            });
        }
    },   
});
