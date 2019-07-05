//table组件
Vue.component('yx-table', {
    props: ['columns', 'data', 'disabled', 'selection', 'stripe', 'showHeader', 'border', 'width', 'height', 'loading', 'highlightRow', 'size'],
    template: ` <div style="width:100%;height:100%;position: relative;"> ` +
                 ` <i-table ref="tableRef" v-bind:columns="curColumns" ` +
                    ` v-bind:data="curData" ` +
                    ` v-bind:stripe="curStripe" ` +
                    ` v-bind:show-header="curShowHeader" ` +
                    ` v-bind:border="curBorder" ` +
                    ` v-bind:width="curWidth" ` +
                    ` v-bind:height="curHeight" ` +
                    ` v-bind:loading="curLoading" ` +
                    ` v-bind:highlight-row="curHighlightRow" ` +
                    ` v-bind:size="curSize" ` +
                    ` v-bind:draggable="curDraggable" ` +
                    ` v-bind:tooltipTheme="curTooltipTheme" ` +
                    ` v-on:on-drag-drop="onDragDrop" ` +
                    ` v-on:on-current-change="currentChange" ` +
                    ` v-on:on-sort-change="sortChange" ` +
                    ` v-on:on-row-click="rowClick" ` +
                    ` v-on:on-row-dblclick="rowDblClick" ` +
                    ` v-on:on-selection-change="onSelectionChange"> ` +
                 ` </i-table> ` +
                 ` <div style="position: absolute;top:2px;right:-5px;" v-if="list.length>0"> ` +
                    ` <Poptip placement="left" trigger="hover" style="margin-left:-20px;margin-top:5px;"> ` +
                        ` <Icon type="md-settings" /> ` +
                        ` <div slot="content"> ` +
                            ` <CheckboxGroup v-model="visibleCols" v-on:on-change="checkBoxChange"> ` +
                                ` <ul style="text-align: left;"> ` +
                                    ` <li v-for="(i,k) in list" key="k"><Checkbox v-bind:label="i.title">{{i.title}}</Checkbox></li> ` +
                               ` </ul> ` +
                            ` </CheckboxGroup> ` +
                        ` </div> ` +
                    ` </Poptip> ` +
                ` </div> ` +
            ` </div> `,
    data() {
        return {
            curShowHeader: true,
            curWidth: null,
            curHeight: null,
            curMaxHeight: null,
            curHighlightRow: true,
            curSize: "small",
            curDraggable: true,
            curTooltipTheme: "dark",
            visibleCols: [],
            list: [],
            colsList: [],
            curColumns: [],
            selectionData: [],
            currentRow: null
        };
    },
    mounted() {
        this.curShowHeader = this.showHeader;
        this.curWidth = this.width;
        this.curHeight = this.height;
    },
    watch: {
        columns: {
            handler: function (nv, ov) {
                if (!nv || !nv.length)
                    return;
                this.colsList = nv.filter(item => {
                    return item.type != "selection";
                });
                this.list = this.colsList;
                this.curColumns = this.initCols();
                this.visibleCols = $.map(this.colsList, item => {
                    return item.title;
                });
            },
            immediate: true,
            deep: true
        },
        disabled: {
            handler: function (nv, ov) {
                if (nv != ov) {
                    this.curColumns = this.initCols();
                }
            },
            immediate: true,
            deep: true
        }
    },
    computed: {
        curStripe() {
            return this.stripe || true;
        },
        curBorder() {
            return this.border || true;
        },
        curData() {
            return this.data || [];
        },
        curLoading() {
            return this.loading;
        }
    },
    methods: {
        //初始化table列设置
        initCols() {
            let _self = this;
            let data = $.map(_self.colsList, item => {
                item.ellipsis = true;
                item.tooltip = true;
                if (item.width) {
                    item.minWidth = item.width || 120;
                    item.width = null;
                } else {
                    item.minWidth = item.minWidth || 120;
                }
                item.renderHeader = function (h, params) {
                    return h('span', {
                        style: {
                            overflow: 'hidden',
                            'text-overflow': 'ellipsis',
                            'white-space': 'nowrap'
                        },
                        attrs: {
                            title: params.column.title
                        }
                    },
                    params.column.title);
                };
                if (item.key == "operate") {
                    item.title = item.title || "操作";
                    item.align = item.align || "center";
                    item.fixed = item.fixed || "right";
                    item.render = function (h, params) {
                        return h('div',
                              [h('Button', {
                                  props: { type: 'primary', size: 'small' },
                                  style: { marginRight: '1px' },
                                  on: {
                                      click: function (event) {
                                          params.column.onClick(params.index, _self.curData[params.index], _self.curData);
                                      }
                                  },
                              }, '详情')]
                          );
                    }
                }
                return item;
            });
            if (this.selection) {
                data.push({ type: 'selection', width: 50, align: 'center', fixed: 'left' });
            }
            return this.initRender(data);
        },
        //初始化render函数
        initRender(data) {
            let newData = [];
            let _self = this;
            for (let i = 0, len = data.length; i < len; i++) {
                let formatStr;
                if (!data[i].render) {
                    switch (data[i].cellType) {
                        case "input":
                            data[i].render = function (h, params) {
                                if (_self.disabled) {
                                    return params.row[params.column.key];
                                }
                                return h("Input", {
                                    props: {
                                        value: params.row[params.column.key],
                                        type: params.column.cellDataType
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "select":
                            data[i].render = function (h, params) {
                                let _list = params.column.selectList;
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    let _val = _list.filter(item=> {
                                        if (item.value == params.row[params.column.key]) {
                                            return true;
                                        }
                                    });
                                    if (_val.length) {
                                        return _val[0].label;
                                    }
                                    return null;
                                }
                                return h(
                                  "Select",
                                  {
                                      props: {
                                          value: params.row[params.column.key],
                                          transfer: true
                                      },
                                      on: _self.initOn(params)
                                  },
                                  $.map(_list, item => {
                                      return h(
                                        "Option",
                                        {
                                            props: { value: item.value + "" }
                                        },
                                        item.label
                                      );
                                  })
                                );
                            };
                            break;
                        case "date":
                            data[i].render = function (h, params) {
                                formatStr = params.column.format || "yyyy-MM-dd";
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return params.row[params.column.key] ? new Date(params.row[params.column.key]).Format(formatStr) : null;
                                }
                                return h("DatePicker", {
                                    props: {
                                        value: params.row[params.column.key],
                                        transfer: true,
                                        format: formatStr
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "datetime":
                            data[i].render = function (h, params) {
                                formatStr = params.column.format || "yyyy-MM-dd HH:mm:ss";
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return params.row[params.column.key];
                                }
                                return h("DatePicker", {
                                    props: {
                                        value: params.row[params.column.key],
                                        transfer: true,
                                        format: formatStr
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "year":
                            data[i].render = function (h, params) {
                                formatStr = params.column.format || "yyyy";
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return params.row[params.column.key] ? new Date(params.row[params.column.key]).Format(formatStr) : null;
                                }
                                return h("DatePicker", {
                                    props: {
                                        value: params.row[params.column.key],
                                        transfer: true,
                                        format: formatStr
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "yearMonth":
                            data[i].render = function (h, params) {
                                formatStr = params.column.format || "yyyyMM";
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return params.row[params.column.key] ? new Date(params.row[params.column.key]).Format(formatStr) : null;
                                }
                                return h("DatePicker", {
                                    props: {
                                        value: params.row[params.column.key],
                                        transfer: true,
                                        format: formatStr
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "time ":
                            data[i].render = function (h, params) {
                                formatStr = params.column.format || "HH:mm:ss";
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return params.row[params.column.key] ? new Date(params.row[params.column.key]).Format(formatStr) : null;
                                }
                                return h("TimePicker ", {
                                    props: {
                                        value: params.row[params.column.key],
                                        transfer: true,
                                        format: formatStr
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        default:
                            data[i].render = function (h, params) {
                                return params.row[params.column.key];
                            };
                            break;

                    }
                }
                newData.push(data[i]);
            }
            return newData;
        },
        //初始化render的on事件
        initOn(params) {
            let _self = this;
            let _row = _self.curData[params.index];
            let onFun = {
                "on-change": function (event) {
                    _row[params.column.key] = params.column.cellType == "input" ? event.target.value : event;
                    if (params.column.onChange) {
                        params.column.onChange(params.index, _row, _self.curData);
                    }
                },
                "on-blur": function (event) {
                    if (params.column.onBlur) {
                        params.column.onBlur(params.index, _row, _self.curData);
                    }
                },
                "on-focus": function (event) {
                    if (params.column.onFocus) {
                        params.column.onFocus(params.index, _row, _self.curData);
                    }
                },
                "on-click": function (event) {
                    if (params.column.onClick) {
                        params.column.onClick(params.index, _row, _self.curData);
                    }
                },
                "on-enter": function (event) {
                    if (params.column.onEnter) {
                        params.column.onEnter(params.index, _row, _self.curData);
                    }
                },
            };
            return onFun;
        },
        //列显示隐藏控制checkBox chang事件
        checkBoxChange(val) {
            this.visibleCols = val;
            let data = this.colsList.filter(item => {
                return (
                  this.visibleCols.indexOf(item.title) > -1 || item.type == "selection"
                );
            });
            if (this.selection) {
                data.push({ type: 'selection', width: 50, align: 'center', fixed: 'left' });
            }
            this.curColumns = data;
        },
        //拖拽排序松开时触发，返回置换的两行数据索引
        onDragDrop: function (index1, index2) {
            let dropObj = this.curData.splice(index1, 1);
            this.curData.splice(index2, 0, dropObj[0]);
        },
        //双击某一行时触发
        rowDblClick(row, index) {
            this.$emit("rowDblClick", row, index);
        },
        //开启 highlight-row 后有效，当表格的当前行发生变化的时候会触发
        currentChange(currentRow, oldCurrentRow) {
            this.$emit("currentChange", currentRow, oldCurrentRow);
        },
        //排序时有效，当点击排序时触发
        sortChange(column, key, order) { },
        //单击某一行时触发
        rowClick(row, index) {
            this.$emit("rowClick", row, index);
        },
        //在多选模式下有效，只要选中项发生变化时就会触发
        onSelectionChange(data) {
            this.$emit("onSelectionChange", data);
        },
        //获取已选中的数据
        getSelection() {
            return this.$refs.tableRef.getSelection();
        },
        //全选数据
        selectAll(isAll) {
            return this.$refs.tableRef.selectAll(isAll);
        }
    }
});
//input组件
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
    computed: {
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
//文本编辑器组件
Vue.component('yx-editor', {
    template: `<div id="editor" style="text-align:left" ></div>`,
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
//菜单按钮权限控制组件
Vue.component('yx-tool-bar', {
    props: ['list', 'disabled', 'domdata'],
    template: ` <div> ` +
              `   <span v-for="(i,k) in curList" v-bind:key="k">` +
              `      <i-button type="text" v-bind:icon="i.icon" v-on:click="i.fun" >{{i.name}}</i-button>` +
              `   </span>` +
              ` </div>`,
    data() {
        return {
            btnList: [],
            toolList: []
        }
    },
    computed: {
        curList() {
            return this.toolList || [];
        }
    },
    watch: {
        domdata: {
            handler: function (nv, ov) {
                this.checkBtn();
            },
            deep: true
        },
        disabled: {
            handler: function (nv, ov) {
                this.checkBtn();
            },
            deep: true
        },
        list: {
            handler: function (nv, ov) {
                this.initBtn();
            },
            deep: true
        }
    },
    methods: {
        initBtn() {
            let _self = this;
            let param = $.map(_self.list, item => {
                return {
                    id: item.id,
                    authority: item.authority,
                    enable: false
                };
            });
            if (param.length) {
                _.Ajax('checkMenu', {
                    MenuAuthority: param
                }, function (data) {
                    _self.btnList = data.filter(item=> {
                        return item.enable;
                    });
                    _self.checkBtn();
                });
            }
        },
        checkBtn() {
            let _self = this;
            let _data = _self.btnList;
            let _list = _self.list
            let _backdata = [];
            for (let i = 0, ilen = _list.length; i < ilen; i++) {
                for (let j = 0, jlen = _data.length; j < jlen; j++) {
                    if (_list[i].id == _data[j].id) {
                        let flag = _list[i].enabled(_self.disabled, _self.domdata);
                        if (flag) {
                            _backdata.push({
                                icon: _list[i].icon,
                                fun: _list[i].fun,
                                name: _list[i].name,
                            })
                        }
                    }
                }
            }
            _self.toolList = _backdata;
        },
    }
});