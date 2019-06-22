
Vue.component('un-edit-table', {
    props: ['options'],
    template:` <div style="width:100%;height:100%;"> `+
            ` <div style="width:100%;height:100%;float:left;"> `+
                 ` <i-table v-bind:columns="columnsDef" `+
                    ` v-bind:data="data" `+
                    ` v-bind:stripe="stripe" `+
                    ` v-bind:show-header="showHeader" `+
                    ` v-bind:border="border" `+
                    ` v-bind:width="width" `+
                    ` v-bind:height="height" `+
                    ` v-bind:max-height="maxHeight" `+
                    ` v-bind:loading="loading" `+
                    ` v-bind:highlight-row="highlightRow" `+
                    ` v-bind:size="size" `+
                    ` v-bind:draggable="draggable" `+
                    ` v-bind:tooltipTheme="tooltipTheme" `+
                    ` v-on:on-drag-drop="onDragDrop" `+
                    ` v-on:on-current-change="currentChange" `+
                    ` v-on:on-sort-change="sortChange" `+
                    ` v-on:on-row-click="rowClick" `+
                    ` v-on:on-row-dblclick="rowDblClick" `+
                    ` v-on:on-selection-change="onSelectionChange"> `+
                 ` </i-table> `+
            ` </div> `+
            ` <div style="width:0;height:100%;float:right;" v-if="columns.length>0"> `+
                ` <Poptip placement="left" trigger="hover" style="margin-left:-20px;margin-top:5px;"> `+
                    ` <Icon type="md-settings" /> `+
                    ` <div slot="content"> `+
                        ` <CheckboxGroup v-model="visibleCols" v-on:on-change="checkBoxChange"> `+
                            ` <ul style="text-align: left;"> `+
                                ` <li v-for="(i,k) in columns" key="k"><Checkbox v-bind:label="i.title">{{i.title}}</Checkbox></li> `+
                           ` </ul> `+
                        ` </CheckboxGroup> `+
                    ` </div> `+
                ` </Poptip> `+
            ` </div> `+
            ` </div> `,
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
            columnsDef: [],
            selectData: [],
            currentRow: null            
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
                this.colsList = nv;
                this.columnsDef = this.initCols(nv);
                this.columns = nv.filter(item=> {
                    return item.type != "selection";
                });;
                this.visibleCols = $.map(this.columns, item=> {
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
                item.minWidth = item.minWidth || 120;
                return item;
            });
            return data;
        },
        //列显示隐藏控制checkBox chang事件
        checkBoxChange(val) {
            this.visibleCols = val;
            this.columnsDef = this.colsList.filter(item=> {
                return this.visibleCols.indexOf(item.title) > -1 || item.type == "selection";
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
        currentChange(currentRow, oldCurrentRow) {
            this.currentRow = currentRow;
        },
        //排序时有效，当点击排序时触发
        sortChange(column, key, order) { },
        //单击某一行时触发
        rowClick(row, index) { },
        //在多选模式下有效，只要选中项发生变化时就会触发
        onSelectionChange(data) {
            this.selectData = data;
        },
        getSelectData() {
            return this.selectData;
        }
    }
});

Vue.component('yx-input', {
    //props: [''],
    template: ``,
    data() {
        return {
          
        }
    },
    mounted() {
     
    },
    watch: {
      
    },
    computed: {
        data() {
            return this.options.data || [];
        }
    },
    methods: {
    
    }
});