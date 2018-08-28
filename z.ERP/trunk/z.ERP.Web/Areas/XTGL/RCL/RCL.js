var vue = new Vue({
    el: '#List_Main',
    data: {
        RQ: new Date().Format('yyyy-MM-dd'),
        disabled: false,
    },
    methods: {
        exec: function () {
            var _self = this;
            _.Ajax('Exec', {
                RQ: _self.RQ
            }, function (data) {
                _self.$Message.info("处理成功");
            });
        }


    }
})