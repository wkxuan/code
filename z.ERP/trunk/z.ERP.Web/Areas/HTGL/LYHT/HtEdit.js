editDetail.beforeVue = function () {

    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.Key = 'CONTRACTID';
    editDetail.dataParam.STATUS = "1";
}

editDetail.showOne = function (data, callback) {
}

editDetail.clearKey = function () {

}
