﻿//table组件
Vue.component('yx-table', {
    props: {
        columns: {
            type: Array,
            default: () =>[],
            required: true
        },
        data: {
            type: Array,
            default: [],
            required: true
        },
        disabled: {
            type: Boolean,
            default: false
        },
        selection: {
            type: Boolean,
            default: false
        },
        stripe: {
            type: Boolean,
            default: true
        },
        showheader: {
            type: Boolean,
            default: true
        },
        border: {
            type: Boolean,
            default: true
        },
        width: {
            type: Number,
        },
        height: {
            type: Number,
        },
        size: {
            type: String,
            default: "small"
        },
        showindex: {
            type: Boolean,
            default: false
        },
        loading: {
            type: Boolean,
            default: false
        },
        highlightrow: {
            type: Boolean,
            default: false
        },
        showsummary: {
            type: Boolean,
            default: false
        },
        sumtext: {
            type: String,
            default: "合计"
        },
        summarymethod: {
            type: Function
        }
    },
    template: ` <div style="width:100%;height:100%;position: relative;"> ` +
                 ` <i-table ref="tableRef" v-bind:columns="curColumns" ` +
                    ` v-bind:data="curData" ` +
                    ` v-bind:stripe="stripe" ` +
                    ` v-bind:show-header="showheader" ` +
                    ` v-bind:border="border" ` +
                    ` v-bind:width="width" ` +
                    ` v-bind:height="height" ` +
                    ` v-bind:loading="loading" ` +
                    ` v-bind:highlight-row="highlightrow" ` +
                    ` v-bind:size="size" ` +
                    ` v-bind:tooltipTheme="curTooltipTheme" ` +
                    ` v-bind:show-summary="showsummary" ` +
                    ` v-bind:sum-text="sumtext" ` +
                    ` v-bind:summary-method="summarymethod" ` +
                    ` v-on:on-current-change="currentChange" ` +
                    ` v-on:on-sort-change="sortChange" ` +
                    ` v-on:on-row-click="rowClick" ` +
                    ` v-on:on-row-dblclick="rowDblClick" ` +
                    ` :row-class-name="rowClassName" ` +
                    ` v-on:on-selection-change="onSelectionChange"> ` +
                 ` </i-table> ` +
                 ` <div style="position: absolute;top:2px;right:-5px;" v-if="visibleList.length>0"> ` +
                    ` <Poptip placement="left" trigger="hover" transfer style="margin-left:-20px;margin-top:5px;"> ` +
                        ` <Icon type="md-settings" /> ` +
                        ` <div slot="content"> ` +
                            ` <CheckboxGroup v-model="visibleCols" v-on:on-change="checkBoxChange"> ` +
                                ` <ul style="text-align: left;"> ` +
                                    ` <li v-for="(i,k) in visibleList" key="k"><Checkbox v-bind:label="i.title">{{i.title}}</Checkbox></li> ` +
                               ` </ul> ` +
                            ` </CheckboxGroup> ` +
                        ` </div> ` +
                    ` </Poptip> ` +
                ` </div> ` +
            ` </div> `,
    data() {
        return {
            curTooltipTheme: "light",
            visibleCols: [],
            visibleList: [],
            colsList: [],
            curColumns: [],
            selectionData: [],
            currentRow: null,
            operateCol: []
        };
    },
    mounted() { },
    watch: {
        columns: {
            handler: function (nv, ov) {
                let _self = this;
                if (!nv || !nv.length)
                    return;

                _self.newCols(nv);
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
        curData() {
            return this.data || [];
        }
    },
    methods: {
        newCols(nv) {
            let _self = this;
            _self.visibleList = nv.filter(function (item) {
                if (item.type && item.type == "selection") {
                    return false;
                }
                if (item.key == "operate" && item.authority) {
                    return false;
                }
                return true;
            });

            let operateCol = nv.filter(function (item) {
                if (item.key == "operate" && item.authority) {
                    return true;
                }
                return false;
            });

            if (operateCol.length) {
                let param = $.map(operateCol, function (item) {
                    return {
                        id: item.key,
                        authority: item.authority,
                        enable: false
                    };
                });

                _.Ajax('CheckMenu', {
                    MenuAuthority: param
                }, function (data) {
                    let len = data.filter(function (item) {
                        return item.enable;
                    });
                    if (len.length) {
                        _self.colsList = _self.visibleList.concat(operateCol);
                    } else {
                        _self.colsList = _self.visibleList;
                    }
                    _self.curColumns = _self.initCols();
                    _self.visibleCols = $.map(_self.visibleList, function (item) {
                        return item.title;
                    });
                });
            } else {
                _self.colsList = _self.visibleList;
                _self.curColumns = _self.initCols();
                _self.visibleCols = $.map(_self.colsList, function (item) {
                    return item.title;
                });
            }
        },
        rowClassName (row, index) {
            if (index % 2 == 0) {
                return 'demo-table-info-row';
            } else {
                return 'demo-table-error-row';
            }
        },
        //初始化table列设置
        initCols() {
            let _self = this;
            let data = $.map(_self.colsList, item => {
                item.ellipsis = true;
                item.tooltip = true;
                item.resizable = true;
                if (!item.cellDisabled) {
                    item.cellDisabled = function (row) {
                        return false;
                    }
                }
                if (!item.width) {
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
                    item.width = 120;
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
                data.push({ type: 'selection', width: 80, align: 'center', fixed: 'left' });
            }
            if (this.showindex) {
                data.unshift({ key: 'N', title: '序号', width: 80, align: 'center', fixed: 'left' });
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
                                        type: params.column.cellDataType,
                                        disabled: params.column.cellDisabled(params.row)
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
                                };
                                return h(
                                 "Select",
                                 {
                                     props: {
                                         value: params.row[params.column.key] + "",
                                         transfer: true,
                                         disabled: params.column.cellDisabled(params.row)
                                     },
                                     on: _self.initOn(params)
                                 },
                                 $.map(_list, function (item) {
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
                                        format: formatStr,
                                        disabled: params.column.cellDisabled(params.row)
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
                                        format: formatStr,
                                        disabled: params.column.cellDisabled(params.row)
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
                                        format: formatStr,
                                        disabled: params.column.cellDisabled(params.row)
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
                                        format: formatStr,
                                        disabled: params.column.cellDisabled(params.row)
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "time":
                            data[i].render = function (h, params) {
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return _self.dataZhTime(params.row[params.column.key]);
                                }
                                return h("yx-time-picker", {
                                    props: {
                                        value: params.row[params.column.key],
                                        disabled: params.column.cellDisabled(params.row)
                                    },
                                    on: _self.initOn(params)
                                });
                            };
                            break;
                        case "pop":
                            data[i].render = function (h, params) {
                                if (_self.disabled || !data[i].enableCellEdit) {
                                    return params.row[params.column.key];
                                }
                                return h("Input", {
                                    props: {
                                        value: params.row[params.column.key],
                                        transfer: true,
                                        readonly: true,
                                        icon: "md-search",
                                        disabled: params.column.cellDisabled(params.row)
                                    },
                                    on: _self.initOn(params)
                                });
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
        dataZhTime (val) {
            let m = Number(val) / 60;
            let s = Number(val) % 60;
            let strM = m == 0 ? "00" : parseInt(m);
            let strS = s == 0 ? "00" : parseInt(s);
            let strM1, strS1;
            if (strM != "00" && strM < 10) {
                strM1 = "0" + strM;
            } else {
                strM1 = strM;
            }
            if (strS != "00" && strS < 10) {
                strS1 = "0" + strS;
            } else {
                strS1 = strS;
            }
            return strM1 + ":" + strS1;
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
        //双击某一行时触发
        rowDblClick(row, index) {
            this.$emit("rowdblclick", row, index);
        },
        //开启 highlight-row 后有效，当表格的当前行发生变化的时候会触发
        currentChange(currentRow, oldCurrentRow) {
            this.currentRow = currentRow;
            this.$emit("currentchange", currentRow, oldCurrentRow);
        },
        //排序时有效，当点击排序时触发
        sortChange(column, key, order) { },
        //单击某一行时触发
        rowClick(row, index) {
            this.$emit("rowclick", row, index);
        },
        //在多选模式下有效，只要选中项发生变化时就会触发
        onSelectionChange(data) {
            this.$emit("onselectionchange", data);
        },
        //获取已选中的数据
        getSelection() {
            return this.$refs.tableRef.getSelection();
        },
        //全选数据
        selectAll(isAll) {
            return this.$refs.tableRef.selectAll(isAll);
        },
        //获取当前高亮行
        getCurrentRow() {
            return this.currentRow;
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
    props: {
        list: {
            type: Array,
            default: [],
        },
        disabled: {
            type: Boolean,
            default: false,
        },
        domdata: {
            type: Object
        }
    },
    template: ` <div> ` +
              `   <span v-for="(i,k) in curList" v-bind:key="k">` +
              `      <i-button type="text" v-bind:icon="i.icon" v-on:click="i.fun" shape="circle">{{i.name}}</i-button>` +
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
                _.Ajax('CheckMenu', {
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
                                fun: function (event) {
                                    event.stopPropagation();
                                    _list[i].fun();
                                },
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
//echart柱状图组件
Vue.component('yx-echart-bar', {
    props: ['data', 'enumlist', 'value', 'sumlist'],
    template: ` <div> ` +
              `  <Card v-bind:padding=0>` +
              `    <div slot="title">` +
              `       <radio-group v-model="curValue" v-on:on-change="echartRadioChange">` +
              `          <radio v-for="(i,k) in curEnumlist" v-bind:key="k" v-bind:label="i.value">` +
              `             <span>{{i.label}}</span>` +
              `          </radio>` +
              `       </radio-group>` +
              `    </div>` +
              `    <div v-bind:id="id" style="width:100%;height:300%;"></div>` +
              `  </Card>` +
              ` </div>`,
    data() {
        return {
            curValue: null,
            legendData: [],
            echartObj: null,
            id: Guid()
        }
    },
    mounted() {
        this.curValue = this.value;
        this.echartObj = echarts.init(document.getElementById(this.id));
        this.initLegend();
    },
    computed: {
        curEnumlist() {
            return this.enumlist || [];
        }
    },
    watch: {
        data: {
            handler: function (nv, ov) {
                this.initEchart();
            }
        },
        value: {
            handler: function (nv, ov) {
                if (nv == ov) {
                    return;
                }
                this.curValue = nv;
                this.initEchart();
            },
            deep: true
        }
    },
    methods: {
        initLegend() {
            this.legendData = [];
            for (let i = 0; i < this.sumlist.length; i++) {
                this.legendData.push(this.sumlist[i].label);
            }
        },
        echartRadioChange(value) {
            this.initEchart();
        },
        initEchart () {
            let _self = this;
            let xData = [];
            let data = _self.data;
            let key = _self.curValue;
            //查找data中不同项初始x轴数据
            for (let i = 0; i < data.length; i++) {
                let len = xData.filter(function (item) {
                    if (!data[i][key] || item == data[i][key]) {
                        return true;
                    } else {
                        return false;
                    }
                })
                if (!len.length && data[i][key] != "合计") {
                    xData.push(data[i][key]);
                }
            }
            //构造series数据
            let seriesData = [];
            for (let i = 0; i < _self.sumlist.length; i++) {
                let dataList = [];
                for (let j = 0; j < xData.length; j++) {
                    let list = data.filter(function (item) {
                        if (item[key] == xData[j]) {
                            return true;
                        } else {
                            return false;
                        }
                    });
                    let sum = 0;
                    for (let k = 0; k < list.length; k++) {
                        sum += list[k][_self.sumlist[i].value];
                    }
                    dataList.push(sum.toFixed(2));
                }
                seriesData.push({
                    name: _self.sumlist[i].label,
                    type: 'bar',
                    data: dataList
                });
            }
            //初始化echart图形
            _self.echartObj.setOption({
                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    data: _self.legendData
                },
                dataZoom: [{
                    show: true,
                    realtime: true,
                }],
                toolbox: {
                    show: true,
                    //orient: 'vertical',
                    right: 'auto',
                    top: 'auto',
                    width: 500,
                    feature: {
                        mark: { show: true },
                        magicType: {
                            show: true, type: ['line', 'bar', 'stack']
                        },
                        restore: {
                            show: true
                        },
                        saveAsImage: {
                            show: true
                        }
                    }
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: xData,
                    boundaryGap: ['30%', '30%']
                },
                yAxis: {
                    type: 'value'
                },
                series: seriesData
            });
        }
    }
});
//弹窗组件
Vue.component('yx-modal', {
    props: {
        src: {
            type: String,
            default: "",
            required: true
        },
        title: {
            type: String,
            default: "弹窗"
        },
        visible: {
            type: Boolean,
            default: false,
            required: true
        },
        width: {
            type: Number,
            default: 900
        },
        height: {
            type: Number,
            default: 500
        }
    },
    template: `<Modal v-model="curVisible" :width="width" v-on:on-visible-change="visibleChange" draggable footer-hide transfer>` +
                 `<div slot="header" name="header">` +
                    `<span>{{title}}</span>` +
                 `</div>` +
                 `<iframe :id="id" :src="src" frameborder="false" scrolling="no" style="border:none;"` +
                          `width="100%" :height="height+'px'"></iframe>` +
            `</Modal>`,
    data() {
        return {
            id: Guid(),
            curVisible: false
        }
    },
    mounted() { },
    watch: {
        visible: {
            handler: function (nv, ov) {
                this.curVisible = nv;
            }
        }
    },
    computed: {},
    methods: {
        visibleChange() {
            let iframe = document.getElementById(this.id);
            let iwindow = iframe.contentWindow;
            if (!this.curVisible) {
                this.$emit('update:visible', this.curVisible);
            } else {
                iwindow.location.reload();
            }
        }
    }
});
//tree组件
Vue.component('yx-tree', {
    props: {
        data: {
            type: Array,
            default: [],
            required: true
        },
        disabled: {
            type: Boolean,
            default: false
        },
        showcheckbox: {
            type: Boolean,
            default: false
        },
        disablecheckbox: {
            type: Boolean,
            default: false
        },
        multiple: {
            type: Boolean,
            default: false
        },
        checkstrictly: {
            type: Boolean,
            default: false
        }
    },
    template: `<Tree ref="treeRef" :data="curData" :multiple="multiple" :show-checkbox="showcheckbox" ` +
              ` :check-strictly="checkstrictly" ` +
              ` v-on:on-select-change="onSelectChange" ` +
              ` v-on:on-check-change="onCheckChange" ` +
              ` v-on:on-toggle-expand="onToggleExpand"></Tree> `,
    data() {
        return {
            curData: [],
        }
    },
    mounted() { },
    computed: {},
    watch: {
        data: {
            handler: function (nv, ov) {
                this.curData = nv;
                this.initData();
            },
            immediate: true,
            deep: true
        },
        disabled: {
            handler: function (nv, ov) {
                this.initData();
            }
        },
        disablecheckbox: {
            handler: function (nv, ov) {
                this.initData();
            }
        }
    },
    methods: {
        //初始化节点
        initData() {
            let data = this.data;
            if (!data)
                return;
            for (let i = 0; i < data.length; i++) {
                data[i].disabled = this.disabled;
                data[i].disableCheckbox = this.disablecheckbox;
                if (data[i].children && data[i].children.length) {
                    this.setDisabled(data[i].children);
                }
            }
            this.curData = data;
        },
        //设置子孙节点disabled
        setDisabled(data) {
            for (let i = 0; i < data.length; i++) {
                data[i].disabled = this.disabled;
                data[i].disableCheckbox = this.disablecheckbox;
                if (data[i].children && data[i].children.length) {
                    this.setDisabled(data[i].children);
                }
            }
        },
        //点击树节点时触发
        onSelectChange(selectArr, node) {
            this.$emit('onselectchange', selectArr, node);
        },
        //点击复选框时触发
        onCheckChange(checkArr, node) {
            this.$emit('oncheckchange', checkArr, node);
        },
        //展开和收起子列表时触发
        onToggleExpand(node) {
            this.$emit('ontoggleexpand', node);
        },
        //获取被勾选的节点
        getCheckedNodes() {
            return this.$refs.treeRef.getCheckedNodes();
        },
        //获取被勾选的节点（除父节点外）
        getOnlyChlCheckedNodes() {
            let _data = [];
            let _func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].checked) {
                        if (data[i].children.length) {
                            _func(data[i].children);
                        } else {
                            _data.push(data[i]);
                        }
                    } else if (data[i].indeterminate) {
                        _func(data[i].children);
                    }
                };
            }
            _func(this.data);
            return _data;
        },
        //获取被勾选的节点（如果父节点也被勾选，则子节点不返回）
        getFilterCheckedNodes() {
            let _data = [];
            let _func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].checked) {
                        if (data[i].children.length) {
                            _data.push(data[i]);
                            continue;
                        } else {
                            _data.push(data[i]);
                        }
                    } else {
                        if (data[i].children.length) {
                            _func(data[i].children);
                        }
                    }
                };
            }
            _func(this.data);
            return _data;
        },
        //获取半选的节点集合
        getOnlyIndeterminateNodes() {
            let _data = [];
            let _func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].indeterminate) {
                        _data.push(data[i]);
                    }
                    if (data[i].children.length) {
                        _func(data[i].children);
                    }
                };
            }
            _func(this.data);
            return _data;
        },
        //获取被选中的节点
        getSelectedNodes() {
            return this.$refs.treeRef.getSelectedNodes();
        },
        //获取被勾选中及半勾选节点
        getCheckedAndIndeterminateNodes() {
            return this.$refs.treeRef.getCheckedAndIndeterminateNodes();
        }
    }
});
//日期选择器组件增加start、end控制
Vue.component('yx-date-picker', {
    props: {
        value: {},
        disabled: {
            type: Boolean,
            default: false,
        },
        //可选值为 date、daterange、datetime、datetimerange、year、month
        type: {
            type: String,
            default: "date",
        },
        size: {},
        format: {
            type: String,
            default: "",
        },
        clearable: {
            type: Boolean,
            default: true,
        },
        editable: {
            type: Boolean,
            default: true,
        },
        showweeknumbers: {
            type: Boolean,
            default: true,
        },
        multiple: {
            type: Boolean,
            default: false,
        },
        confirm: {
            type: Boolean,
            default: false,
        },
        start: {
            type: String,
            default: "",
        },
        end: {
            type: String,
            default: "",
        }
    },
    template: `<date-picker :value="curValue" :type="type" :size="size" :format="curFormat" transfer :disabled="disabled"` +
              ` :multiple="multiple" :show-week-numbers="showweeknumbers"` +
              ` :clearable="clearable" :editable="editable" :confirm="confirm" :options="options" v-on:on-change="onChange">` +
              `</date-picker>`,
    data() {
        return {
            curValue: null,
            curFormat: "",
            options: {}
        }
    },
    mounted() {
        this.initFormat();
        this.initOptions();
    },
    watch: {
        value: {
            handler: function (nv, ov) {
                this.curValue = nv + "";
            },
            immediate: true,
            deep: true
        },
        start: {
            handler: function (nv, ov) {
                if (nv)
                    this.initOptions();
            },
            deep: true
        },
        end: {
            handler: function (nv, ov) {
                if (nv)
                    this.initOptions();
            },
            deep: true
        }
    },
    methods: {
        onChange(val, type) {
            this.$emit('update:value', val);
        },
        initOptions() {
            let _self = this;
            _self.options = {
                disabledDate (date) {
                    if (_self.start && !_self.end) {
                        return date < new Date(_self.start);
                    }
                    if (!_self.start && _self.end) {
                        return date > new Date(_self.end);
                    }
                    if (_self.start && _self.end) {
                        if (date < new Date(_self.start) || date > new Date(_self.end)) {
                            return true;
                        } else {
                            return false;
                        }
                    }
                    return false;
                }
            };
        },
        initFormat() {
            if (this.format) {
                this.curFormat = this.format;
            } else {
                switch (this.type) {
                    case "date":
                    case "daterange":
                        this.curFormat = "yyyy-MM-dd";
                        break;
                    case "date":
                    case "datetimerange":
                        this.curFormat = "yyyy-MM-dd HH:mm:ss";
                        break;
                    case "year":
                        this.curFormat = "yyyy";
                        break;
                    case "month":
                        this.curFormat = "yyyyMM";
                        break;
                    default:
                        this.curFormat = "yyyy-MM-dd";
                        break;
                }
            }
        }
    }
});
//时间选择器组件增加数据转化
Vue.component('yx-time-picker', {
    props: {
        value: {},
        disabled: {
            type: Boolean,
            default: false,
        },
        //可选值为 time、timerange
        type: {
            type: String,
            default: "time",
        },
        size: {},
        format: {
            type: String,
            default: "HH:mm",
        },
        clearable: {
            type: Boolean,
            default: true,
        },
        editable: {
            type: Boolean,
            default: true,
        },
        confirm: {
            type: Boolean,
            default: false,
        },
        readonly: {
            type: Boolean,
            default: false,
        }
    },
    template: `<time-picker :format="format" :value="curValue" :size="size" :disabled="disabled" :clearable="clearable"` +
              ` :editable="editable" :confirm="confirm" :readonly="readonly" v-on:on-change="onChange" transfer></time-picker>`,
    data() {
        return {
            curValue: "",
            curFormat: ""
        }
    },
    watch: {
        value: {
            handler: function (nv, ov) {
                this.curValue = this.dataZhTime(nv);
            },
            immediate: true,
            deep: true
        }
    },
    methods: {
        onChange(val, type) {
            this.$emit('update:value', this.dataZhNum(val));
            this.$emit('change', val, type);
        },
        dataZhNum (val) {
            if (!val) {
                return "";
            }
            let strArr = val.split(":");
            let num = Number(strArr[0]) * 60 + Number(strArr[1]);
            return num;
        },
        dataZhTime (val) {
            let m = Number(val) / 60;
            let s = Number(val) % 60;
            let strM = m == 0 ? "00" : parseInt(m);
            let strS = s == 0 ? "00" : parseInt(s);
            let strM1, strS1;
            if (strM != "00" && strM < 10) {
                strM1 = "0" + strM;
            } else {
                strM1 = strM;
            }
            if (strS != "00" && strS < 10) {
                strS1 = "0" + strS;
            } else {
                strS1 = strS;
            }
            return strM1 + ":" + strS1;
        }
    }
});
//周checkGroup组件
Vue.component('yx-week-check-group', {
    props: {
        value: {},
        disabled: {
            type: Boolean,
            default: false,
        }
    },
    template: ` <checkbox-group :value="curValue" v-on:on-change="onChange">` +
                `<checkbox label="1" :disabled="disabled">星期一</checkbox>` +
                `<checkbox label="2" :disabled="disabled">星期二</checkbox>` +
                `<checkbox label="3" :disabled="disabled">星期三</checkbox>` +
                `<checkbox label="4" :disabled="disabled">星期四</checkbox>` +
                `<checkbox label="5" :disabled="disabled">星期五</checkbox>` +
                `<checkbox label="6" :disabled="disabled">星期六</checkbox>` +
                `<checkbox label="7" :disabled="disabled">星期日</checkbox>` +
            `</checkbox-group>`,
    data() {
        return {
            curValue: []
        }
    },
    watch: {
        value: {
            handler: function (nv, ov) {
                if (nv && typeof nv == "string") {
                    this.curValue = nv.split(",");
                } else {
                    this.curValue = [];
                }
            },
            immediate: true,
            deep: true
        }
    },
    methods: {
        onChange(val, type) {
            this.$emit('update:value', val.join(","));
        }
    }
});
//上传文件导入组件
Vue.component('yx-upload', {
    props: {
        open: {
            type: Boolean,
            default: false,
        },
        name: {
            type: String,
            default: "",
        }
    },
    template: `<Modal :title="Title" v-model="curopen" :mask-closable="false" v-on:on-visible-change="onvisiblechange"> ` +
                `<Upload type="drag" :action="curAction" accept="xlsx,xls" :format ="['xlsx','xls']" ` +
                    `:on-error="onError":on-success="onSuccess" :on-format-error="onFormatError" :before-upload="beforeUpload" :on-progress="onProgress"> ` +
                    `<div style="padding: 20px 0">` +
                       ` <icon type="ios-cloud-upload" size="52" style="color: #3399ff"></icon> ` +
                       ` <p>单击或拖动文件以上传</p> ` +
                    `</div> ` +
                `</Upload> ` +
                `<div slot="footer"><i-button type="text" icon="md-download" v-on:click="download">下载模板</i-button></div> ` +
            `</Modal>`,
    data() {
        return {
            curopen: false,
            Title: "导入",
            curAction: _.AjaxUrl + "Import"
        }
    },
    mounted() {
    },
    watch: {
        open: {
            handler: function (nv, ov) {
                this.curopen = nv;
            },
            immediate: true,
            deep: true
        }
    },
    methods: {
        onSuccess(response, file, fileList) {
            this.$emit('update:open', false);
            _.Ajax('ImportExcel', {
                fileUrl: response
            }, function (data) {
                if (data.SuccFlag) {
                    iview.Message.info("导入成功！");
                } else {
                    let option = {
                        render: h=> {
                            return h('div', {
                                style: {
                                    'table-layout': 'fixed',
                                    'word-break': 'break-all',
                                    'overflow': 'hidden',
                                    'margin': '0 20px'
                                }
                            }, data.Message);
                        },
                        title: "提示",
                        width: 350,
                        okText: "确定",
                        cancelText: "取消"
                    }
                    iview.Modal.warning(option);
                }
            });
        },
        onFormatError(file, fileList) {
            iview.Message.error("导入的文件格式不对！");
        },
        onError(error, file, fileList) {
            iview.Message.error("文件上传失败！");
        },
        beforeUpload(file) {
        },
        onProgress(event, file, fileList) {
        },
        download() {
            if (this.name) {
                var fileName = this.name + ".xls";
                window.location.href = __BaseUrl + "/File/Import/" + fileName;
            }
        },
        onvisiblechange(val) {
            this.$emit('update:open', val);
        }
    }
});
//下拉框组件支持service，method异步数据请求
Vue.component('yx-select', {
    props: {
        value: {
            required: true
        },
        disabled: {
            type: Boolean,
            default: false,
        },
        data: {},
        multiple: {
            type: Boolean,
            default: false
        },
        clearable: {
            type: Boolean,
            default: true
        },
        nullterm: {
            type: Boolean,
            default: false
        },
        service: {
            type: String,
            default: "DataService"
        },
        method: {
            type: String,
            default: ""
        },
        placeholder: {
            type: String,
            default: "请选择"
        },
        params: {
            type: Object,
        }
    },
    template: `<i-select v-model="curValue" :disabled="disabled" transfer` +
                  ` :clearable="clearable" :placeholder="placeholder" :multiple="multiple" ` +
                  ` v-on:on-change="onChange" v-on:on-clear="onClear" v-on:on-open-change="onOpenChange">` +
                  ` <i-option v-for="(i,k) in curData" v-bind:value="i.Key" v-bind:key="k">{{ i.Value }}</i-option>` +
              `</i-select>`,
    data() {
        return {
            curValue: "",
            curData: [],
            itemData: [{ Value: this.placeholder, Key: "", IsSelected: false, Obj: {} }]
        };
    },
    mounted() {
        if ((!this.data || !this.data.length) && this.service) {
            this.serviceGetData();
        }
    },
    watch: {
        value: {
            handler: function (nv, ov) {
                if (this.multiple) {
                    this.curValue = nv;
                } else {
                    this.curValue = nv + "";
                }
            },
            immediate: true
        },
        data: {
            handler: function (nv, ov) {
                let list = nv;
                if (list && list.length) {
                    if (this.nullterm) {
                        list = this.itemData.concat(list);
                    }
                    list = $.map(list, function (item) {
                        item.Value = item.Value + "";
                        item.Key = item.Key + "";
                        return item;
                    });
                    //if (list.length == 1 && !this.multiple) {
                    //    this.$emit('update:value', list[0].Key);
                    //}
                    this.curData = list;
                }
            },
            immediate: true
        }
    },
    methods: {
        //选中的Option变化时触
        onChange(curItem) {
            this.$emit('update:value', curItem);
            let obj = this.getCurItemObj();
            this.$emit('change', curItem, obj);
        },
        //下拉框展开或收起时触发
        onOpenChange(isOpen) {
            this.$emit('openchange', isOpen);
        },
        //点击清空按钮时触发
        onClear() {
            this.$emit('clear');
        },
        //service服务获取数据
        serviceGetData() {
            let list = [];
            let _self = this;
            _.GetCommonData({
                Service: _self.service,
                Method: _self.method,
                Data: _self.params,
                Success: function (data) {
                    if (data || data.length) {
                        list = data;
                        if (_self.nullterm) {
                            list = _self.itemData.concat(list);;
                        }
                    }
                    //if (list.length == 1 && !_self.multiple) {
                    //    _self.$emit('update:value', list[0].Key);
                    //}
                    _self.curData = $.map(list, function (item) {
                        item.Value = item.Value + "";
                        item.Key = item.Key + "";
                        return item;
                    });
                }
            })

        },
        //获取当前选中项
        getCurItemObj() {
            let obj = {};
            let _self = this;
            if (_self.curValue) {
                let list = _self.curData.filter(function (i) {
                    if (i.Key == _self.curValue) {
                        return i;
                    }
                });
                if (list.length) {
                    obj = list[0];
                }
            }
            return obj;
        }
    }
})
//级联选择组件支持service，method异步数据请求
Vue.component('yx-cascader', {
    props: {
        value: {
            required: true
        },
        data: {},
        disabled: {
            type: Boolean,
            default: false,
        },
        clearable: {
            type: Boolean,
            default: true
        },
        service: {
            type: String,
            default: "DataService"
        },
        method: {
            type: String,
            default: ""
        },
        placeholder: {
            type: String,
            default: "请选择"
        },
        notlevellast: {
            type: Boolean,
            default: false
        }
    },
    template: `<Cascader :data="curData" :value="curValue" :disabled="disabled" :placeholder="placeholder" :change-on-select="notlevellast" clearable transfer` +
              ` v-on:on-change="onChange" v-on:on-visible-change="onVisibleChange"></Cascader>`,
    data() {
        return {
            curValue: null,
            curData: []
        };
    },
    mounted() {
        if ((!this.data || !this.data.length) && this.service) {
            this.serviceGetData();
        }
    },
    watch: {
        value: {
            handler: function (nv, ov) {
                this.curValue = nv;
            },
            immediate: true
        },
        data: {
            handler: function (nv, ov) {
                let list = nv;
                if (list && list.length) {
                    this.curData = list;
                }
            },
            immediate: true
        }
    },
    methods: {
        //选中的Option变化时触
        onChange(value, selectedData) {
            this.$emit('update:value', value);
            this.$emit('change', value, selectedData);
        },
        //下拉框展开或收起时触发
        onVisibleChange(isOpen) {
            this.$emit('visiblechange', isOpen);
        },
        //service服务获取数据
        serviceGetData() {
            let list = [];
            let _self = this;
            _.GetCommonData({
                Service: _self.service,
                Method: _self.method,
                Data: {},
                Success: function (data) {
                    if (data && data.Item1) {
                        _self.curData = data.Item1;
                    }
                }
            })

        }
    }
});