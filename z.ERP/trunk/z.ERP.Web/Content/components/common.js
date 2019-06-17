
Vue.component('un-edit-table', {
    props: ['options'],
    template: "#unEditTable",
    data() {
        return {
            stripe: true,
            border: true,
            showHeader:true,
            width: null,
            height: null,
            maxHeight:null,
            loading: false,
            highlightRow: true,
            size: "small",
            draggable: true,
            tooltipTheme: "dark",
            visibleCols: [],
            columns: [],
            columnsDef:[]
        }
    },
    mounted() {
        this.stripe = this.options.stripe || true;
        this.border = this.options.border || true;
        this.showHeader = this.options.showHeader || true;
        this.width = this.options.width || null;
        this.height = this.options.height || null;
        this.maxHeight = this.options.maxHeight || "500";
        this.loading = this.options.loading || false;
        this.highlightRow = this.options.highlightRow || true;
        this.size = this.options.size || "small";
        this.draggable = this.options.draggable || true;
        this.tooltipTheme = this.options.tooltipTheme || "dark";
    },
    watch: {
        "options.columns": {
            handler: function (nv, ov) {
                this.columnsDef = this.initCols(nv);
                this.columns = nv;
                this.visibleCols = $.map(nv, item=> {
                    return item.title;
                });
            },
            immediate:true,
            deep: true
        }
    },
    computed: {
        data() {
            return this.options.data || [];
        }
    },
    methods: {
        //初始化table列设置
        initCols(cols) {
            let data = $.map(cols, item=> {
                item.ellipsis = item.ellipsis || true;
                item.tooltip = item.tooltip || true;
                return item;
            });
            return data;
        },
        //列显示隐藏控制checkBox chang事件
        checkBoxChange(val) {
            this.visibleCols = val;
            this.columnsDef = this.columns.filter(item=> {
                return this.visibleCols.indexOf(item.title) > -1;
            });
        },
        //拖拽排序松开时触发，返回置换的两行数据索引
        onDragDrop: function (index1, index2) {
            let dropObj = this.options.data.splice(index1, 1);
            this.options.data.splice(index2, 0, dropObj[0]);
        },
        //双击某一行时触发
        rowDblClick(row, index) { },
        //开启 highlight-row 后有效，当表格的当前行发生变化的时候会触发
        currentChange(currentRow, oldCurrentRow) { },
        //排序时有效，当点击排序时触发
        sortChange(column, key, order) { },
        //单击某一行时触发
        rowClick(row, index) { }
    }
});

Vue.component('edit-table', {
    props: ['options'],
    template: "#editTable",
    data() {
        return {
            stripe: true,
            border: true,
            showHeader: true,
            width: null,
            height: null,
            maxHeight: null,
            loading: false,
            highlightRow: true,
            size: "small",
            draggable: true,
            tooltipTheme: "dark",
            visibleCols: [],
            columns: [],
            columnsDef: []
        }
    },
    mounted() {
        this.stripe = this.options.stripe || true;
        this.border = this.options.border || true;
        this.showHeader = this.options.showHeader || true;
        this.width = this.options.width || null;
        this.height = this.options.height || null;
        this.maxHeight = this.options.maxHeight || null;
        this.loading = this.options.loading || false;
        this.highlightRow = this.options.highlightRow || true;
        this.size = this.options.size || "small";
        this.draggable = this.options.draggable || true;
        this.tooltipTheme = this.options.tooltipTheme || "dark";
    },
    watch: {
        "options.columns": {
            handler: function (nv, ov) {
                this.columnsDef = this.initCols(nv);
                this.columns = nv;
                this.visibleCols = $.map(nv, item=> {
                    return item.title;
                });
            },
            immediate: true,
            deep: true
        }
    },
    computed: {
        data() {
            return this.options.data || [];
        }
    },
    methods: {
        //初始化table列设置
        initCols(cols) {
            let data = $.map(cols, item=> {
                item.ellipsis = item.ellipsis || true;
                item.tooltip = item.tooltip || true;
                item.slot = item.key;
                return item;  
            });
            return data;
        },
        //列显示隐藏控制checkBox chang事件
        checkBoxChange(val) {
            this.visibleCols = val;
            this.columnsDef = this.columns.filter(item=> {
                return this.visibleCols.indexOf(item.title) > -1;
            });
        },
        //拖拽排序松开时触发，返回置换的两行数据索引
        onDragDrop: function (index1, index2) {
            let dropObj = this.options.data.splice(index1, 1);
            this.options.data.splice(index2, 0, dropObj[0]);
        },
        //双击某一行时触发
        rowDblClick(row, index) { },
        //开启 highlight-row 后有效，当表格的当前行发生变化的时候会触发
        currentChange(currentRow, oldCurrentRow) { },
        //排序时有效，当点击排序时触发
        sortChange(column, key, order) { },
        //单击某一行时触发
        rowClick(row, index) { }
    }
});
