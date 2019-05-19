var aaa = '<i-input style="width: 150px"></i-input>'

var period = new Vue({
    el: "#List_Main",
    data: {
        showDrawer: false,
        stylesDrawer: {
            height: 'calc(100% - 55px)',
            overflow: 'auto',
            paddingBottom: '53px',
            position: 'static'
        },
        disabled: false,
        DrawerHtml: '<i-input style="width: 150px"></i-input>'
    },
    methods: {
        add: function () { },
        mod: function () { },
        save: function () { },
        quit: function () { },
        del: function () { },
        exec: function () { },
        over: function () { },
        createDrawer: function () {
            this.showDrawer = true;
        },
        sureDrawer: function () {
            this.showDrawer = false;
        },
        cancelDrawer: function () {
            this.showDrawer = false;
        },
    }
})