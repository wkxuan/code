
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
    props: ['value', 'type', 'size', 'disabled', 'readonly', 'validate', 'maxlength', 'pacement'],
    template: ` <Poptip class="yxInputClass" style="width:100%;" :placement="curPacement" v-model="visible" :disabled="tipDisabled">` +
              `   <i-input :id="id" v-model="curValue" :type="curType" :size="curSize" :disabled="curDisabled"` +
              `            :readonly="curReadonly" :maxlength="curMaxlength" clearable />` +
              `   <div slot="content">{{msgText}}</div>` +
              ` </Poptip  >`,
    data() {
        return {
            id: Guid(),
            curValue: this.value,
            msgText: "",
            curType: "text",
            curSize: "default",
            curReadonly: false,
            curMaxlength: null,
            visible: false,
            curPacement: "right",
            tipDisabled: true
        }
    },
    computed:{
        curDisabled() {
            return this.disabled;
        }
    },
    mounted() {
        curType = this.type;
        curSize = this.size;
        curReadonly = this.readonly;
        curMaxlength = this.maxlength;
        curPacement = this.pacement;
    },
    watch: {
        curValue: {
             handler: function (nv, ov) {
                 let _self = this;
                 _self.tipDisabled = false;
                 _self.$emit('input', nv);
                 _self.validateFun();
            },
            immediate: false,
            deep: true
        }
    },
    methods: {
        validateFun() {
            let _self = this;
            let list = _self.validate || [];
            for (let i = 0, len = list.length; i < len; i++) {
                _self.visible = false;
                if (typeof list[i].validate == "string") {
                    switch (list[i].validate) {
                        case "required":
                            if (_self.curValue == undefined || _self.curValue == null || _self.curValue == "") {
                                _self.visible = true;
                                _self.msgText = "不能为空！";
                            }
                            break;
                    }
                } else if (typeof list[i].validate == "function") {
                    if (!list[i].validate(_self.curValue)) {
                        _self.visible = true;
                        _self.msgText = list[i].msg;
                    }
                }
                if (_self.visible) {
                    break;
                }
            }
        }
    }
});

Vue.component('yx-editor', {
    template: `<div >`+
              `<div id="editor" style="text-align:left" ></div>` +
              `</div>`,
    model: {
        prop: "content",
        event: "change"
    },
    props: ["content", "disabled"],
    data() {
        return {
            editorObj: null,
        }
    },
    computed: {
        curDisabled() {
            return this.disabled;
        }
    },
    mounted() {
        /*实例化*/
        var E = window.wangEditor;
        this.editorObj = new E('#editor');
        /*设置BASE64图片*/
        this.editorObj.customConfig.uploadImgShowBase64 = true
        //黏贴样式不过滤
        this.editorObj.customConfig.pasteFilterStyle = false
        /*绑定回馈事件*/
        this.editorObj.customConfig.onchange = html => {
            this.$emit("change", html);
        };
        this.editorObj.customConfig.uploadImgMaxSize= 1 * 1024 * 1024,
        this.editorObj.customConfig.zIndex = 100;
        /*创建编辑器*/
        this.editorObj.create();
        this.editorObj.$textElem.attr('contenteditable', !this.disabled);
        /*初始内容*/
        //this.editorObj.txt.html("");
    },
    watch: {
        content: {
            handler: function (nv, ov) {
                this.editorObj.txt.html(nv);
            },
            immediate: false,
            deep: true
        },
        disabled: {
            handler: function (nv, ov) {
                if (nv) {
                    this.editorObj.$textElem.attr('contenteditable', false)
                } else {
                    this.editorObj.$textElem.attr('contenteditable', true)
                }
            },
            immediate: false,
            deep: true
        }
    },
});