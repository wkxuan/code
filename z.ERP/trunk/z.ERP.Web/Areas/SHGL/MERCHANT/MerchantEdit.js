editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       { DESCRIPTION:"已完成", OPERATION:"张三  2019-01-01" },
       { DESCRIPTION:"待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = false;
}