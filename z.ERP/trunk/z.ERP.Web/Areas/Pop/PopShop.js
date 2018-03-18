Vue.component('Shop', {
    template: '<div>' +
                    '<row>' +
                        '<i-col span="2" class="RowTitle">' +
                           '商铺编码'+
                        '</i-col>' +
                        '<i-col span="4">' +
                           '<i-input v-model="CODE"></i-input>' +
                        '</i-col>' +
                    '</row>'+
                    '<row>' +
                        '<i-table border highlight-row  ref="currentRowTable" :columns="Column" :data="Data" height=300></i-table>' +
                    '</row>' +
                    '<row>' +
                        '<i-col span="9">' +
                            '      ' +
                        '</i-col>'+
                        '<i-col span="2">' +
                            '<i-button v-on:click="cx">查询</i-button>' +
                        '</i-col>' +
                        '<i-col span="2">' +
                            '<i-button v-on:click="qk">清空</i-button>' +
                        '</i-col>' +
                        '<i-col span="2">' +
                            '<i-button v-on:click="qr">确认</i-button>' +
                        '</i-col>' +
                    '</row>'+
	           '</div>',
    data: function () {
        return {
            CODE: '',
            Column: [
                { type: 'selection', width: 60, align: 'center' },
                { title: '商铺代码', key: 'CODE', width: 150, }
            ],
            Data: [],
        }
    },
    created: function () {
        this.Data = [];
    },
    methods: {
        qr: function () {
            var data = {};
            data.sj = this.$refs.currentRowTable.getSelection();
            this.Data = [];
            this.$refs.currentRowTable.makeObjData();
            this.$emit('setdialog', data)
        },
        cx: function () {
            _self = this;
            this.Data = [];
            _.Search({
                Service: "XtglService",
                Method: "GetShop",
                Data: {
                    CODE: _self.CODE,
                    BRANCHID: _self.parenttochild.BRANCHID
                },
                Success: function (data) {
                    if (data.rows.length != 0) {
                        Vue.set(_self, "Data", data.rows);
                    } else {
                        iview.Message.info("没有满足条件的记录");
                    };
                }
            });
        },
        qk: function () {
            this.Data = [];
        },
    },
    props: {
        parenttochild: {
            type: Object,
            required: true
        }
    }
})