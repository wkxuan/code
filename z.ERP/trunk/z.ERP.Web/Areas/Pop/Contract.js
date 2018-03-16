Vue.component('Contract', {
    template: '<div>' +
                    '<i-input v-model="hehe1" style="width: 150px"></i-input>' +
                    '<i-input v-model="hehe2" style="width: 150px"></i-input>' +
                    '<i-table border highlight-row  ref="currentRowTable" :columns="perColumn" :data="perData" ></i-table>' +
                    '<i-button id="asd" a="b" v-on:click="cx">查询</i-button>' +
                    '<i-button v-on:click="qk">清空</i-button>' +
                    '<i-button v-on:click="qr">确认</i-button>' +
	           '</div>',
    data: function () {
        return {
            hehe1: '',
            hehe2: '',
            perColumn: [
              {
                  type: 'selection',
                  width: 60,
                  align: 'center'
              },

        {
            title: '年月',
            key: 'YEARMONTH',
            width: 150,
        },
        {
            title: '开始日期',
            key: 'START_DATE',
            width: 150,

        },
        {
            title: '结束日期',
            key: 'END_DATE',
            width: 150,
        }
            ],
            perData: [

            ],
        }
    },

    created: function () {
        this.perData = [];
    },
    methods: {
        qr: function () {
            var data = {};
            data.hehe1 = this.hehe1;
            data.PopMerchant = false;
            data.sj = this.$refs.currentRowTable.getSelection();
            this.perData = [];
            this.$refs.currentRowTable.makeObjData();
            this.$emit('setdialog', data)
        },
        cx: function () {
            this.perData = [];
            this.perData = [
          { YEARMONTH: '201201', START_DATE: '2012-01-01', END_DATE: '2012-01-31' },
          { YEARMONTH: '201202', START_DATE: '2012-02-01', END_DATE: '2012-02-28' },
          { YEARMONTH: '201203', START_DATE: '2012-03-01', END_DATE: '2012-03-31' },
          ];
            console.log(this.parenttochild);
        },
        qk: function () {
            this.perData = [];
        },
    },
    props: {
        parenttochild: {
            type: Object,
            required: true
        }
    }
})