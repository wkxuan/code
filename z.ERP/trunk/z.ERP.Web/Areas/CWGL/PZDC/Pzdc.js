let selectid = "";
var pzdc = new Vue({
    el: "#List_Main",
    data: {        
        colDef: [{ title: '凭证编号', key: 'VOUCHERID' },
                 { title: '凭证名称', key: 'VOUCHERNAME' },   
                 ],
        dataDef: [],
        disabled: true,
        BRANCHID: 1,
        CWNY: 201810,        
        RQ_START: "",
        RQ_END: "",
        PZRQ: ""
    },
    mounted: function () {
        var myDate = new Date();
        //this.PZRQ = Date;
        //this.CWNY = 201810;
        this.PZRQ = myDate.Format('yyyy-MM-dd');
        this.CWNY = myDate.getFullYear()*100 + myDate.getMonth();
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
            if (!selectid)
            {
                iview.Message.info("请先选择凭证模板!");
                return false;
            }
            if (!this.BRANCHID)
            {
                iview.Message.info("分店不能为空!");
                return false;
            }
            if (!this.CWNY) {
                iview.Message.info("年月不能为空!");
                return false;
            }
            if (!this.PZRQ) {
                iview.Message.info("凭证日期不能为空!");
                return false;
            }
            _.Ajax('ExportPz', {
                data: {
                    VOUCHERID: selectid, BRANCHID: this.BRANCHID, CWNY: this.CWNY, DATE1: this.RQ_START,
                    DATE2: this.RQ_END,
                    PZRQ: this.PZRQ
                },
            }, function (data) {
                if (data == "未导出数据")
                {
                    iview.Message.info(data);
                } else
                {
                    window.open(__BaseUrl + data);
                }                                    
            });
            //_.Search({
            //    Service: 'CwglService',
            //    Method: 'ExportPz',
            //    Data: { VOUCHERID: selectid, BRANCHID: this.BRANCHID, CWNY: this.CWNY, pDATE2: this.RQ_END, pDATE1: this.RQ_START },
            //    PageInfo: 1,
            //    Success: function (data) {
            //        pzdc.dataDef = data.rows;
            //    }
            //})

        },
        select: function (currentRow, oldCurrentRow) {
            selectid = currentRow.VOUCHERID            
        },
    }

});
