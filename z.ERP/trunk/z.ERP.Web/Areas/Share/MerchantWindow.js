window['search'].otherMethods = {
    ok: function () {
        window['search'].searchParam.BILLID = window['search'].windowParam.MERCHANTID;
        window['search'].searchParam.YEARMONTH_END = window['search'].windowParam.NAME;
    },
    showdata: function () {
        _.Ajax('GetRegister', {
            Data: { FILEID: 11}
        }, function (data) {
            //editDetail.dataParam.ENERGY_REGISTER_ITEM[params.index].VALUE_LAST = data.
            var ss = data;
            window['search'].searchParam.BILLID = data.dt[0].FILENAME;
        });
    }
};