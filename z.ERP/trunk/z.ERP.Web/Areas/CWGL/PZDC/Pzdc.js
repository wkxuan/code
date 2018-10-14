var pzdc = new Vue({
    el: "#List_Main",
    data: {        
        colDef: [{ title: '凭证编号', key: 'VOUCHERID' },
                 { title: '凭证名称', key: 'VOUCHERNAME' },   
                 ],
        dataDef: [],
        disabled: true,
        BRANCHID: 1,
        NY_START: 201810,
        NY_END: 201810,
        RQ_START: null,
        RQ_END: null,
    },
    mounted: function () {
        _.Search({
            Service: 'CwglService',
            Method: 'GetVoucher',
            Success: function (data) {
                pzdc.dataDef = data.rows;
            }
        })
    },
    methods: {
        exportPz: function (event) {
            if (!this.BRANCHID)
            {
                iview.Message.info("分店不能为空!");
                return false;
            }
            if (!this.NY_START) {
                iview.Message.info("开始年月不能为空!");
                return false;
            }
            if (!this.NY_END) {
                iview.Message.info("结束年月不能为空!");
                return false;
            }
            _.Ajax('ExportPz', {
                Data: { VOUCHERID: 1, BRANCHID: 1, NY_START:1}
            }, function (data) {
                if (data)
                {
                    alert('导出成功！');
                }
            });
        },
    }

});
