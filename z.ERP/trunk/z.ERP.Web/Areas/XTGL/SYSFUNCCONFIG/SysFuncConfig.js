var sysFuncConfig = new Vue({
    el: "#List_Main",
    data() {
        return {
            treeData: [],
            enableEditTitle: false,
            enableEditNodeId: null,
            editValue: "",
            columns: [
            {
                title: 'ID', key: 'ID', width: 120
            },
            {
                title: '名称', key: 'NAME'
            }],
            tableData: [],
            tbLoading: false,
            dataParam: {
                NAME: "",
                URL: "",
                STATUS: "1",
                TYPE: "1",
                SYSTYPE: "1",
                ISALL: "1"
            },
            modalValue: false,
            curNode: {}
        }
    },
    mounted() {
        this.initData();
    },
    methods: {
        initData() {
            let _self = this;
            _.Ajax('GetMenuModule', {
                Data: {}
            }, function (data) {
                _self.treeData = data.module;
            });
        },
        renderContent(h, dom) {
            let _self = this;
            return h('div', {
                class: 'nodeClass',
                style: {
                    width: 'calc(100% - 15px)',
                    float: 'right'
                },
                on: {
                    mouseover() {
                        $('#btn_div' + dom.data.value).show();
                    },
                    mouseleave() {
                        $('#btn_div' + dom.data.value).hide();
                    }
                }
            },
            [
                h('div', {
                    style: {
                        float: 'left',
                        width: '50%',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        whiteSpace: 'nowrap'
                    },
                    attrs: {
                        title: dom.data.title
                    },
                }, [
                   _self.titleRenderFunc(h, dom)
                ]),
                h('div', {
                    style: {
                        float: 'right',
                        display: 'none',
                        width: '50%',
                        textAlign: 'right'
                    },
                    attrs: {
                        id: 'btn_div' + dom.data.value
                    }
                }, [
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-add'
                             }),
                             style: {
                                 display: _self.enableEditTitle ? "none" : ""
                             },
                             attrs: {
                                 title: '添加'
                             },
                             on: {
                                 click: function () {
                                     _self.add(dom)
                                 }
                             }
                         }),
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-create'
                             }),
                             attrs: {
                                 title: '编辑'
                             },
                             style: {
                                 display: _self.enableEditTitle || (dom.data.parentId == "0") ? "none" : ""
                             },
                             on: {
                                 click: function () {
                                     _self.edit(dom)
                                 }
                             }
                         }),
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-trash'
                             }),
                             attrs: {
                                 title: '删除'
                             },
                             style: {
                                 display: _self.enableEditTitle || (dom.data.parentId == "0") || (dom.data.children && dom.data.children.length) ? "none" : ""
                             },
                             on: {
                                 click: function () {
                                     _self.remove(dom)
                                 }
                             }
                         }),
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-checkmark-circle'
                             }),
                             attrs: {
                                 title: '保存'
                             },
                             style: {
                                 display: _self.enableEditTitle && dom.data.value == _self.enableEditNodeId ? "" : "none"
                             },
                             on: {
                                 click: function () {
                                     _self.save(dom)
                                 }
                             }
                         }),
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-refresh'
                             }),
                             attrs: {
                                 title: '取消'
                             },
                             style: {
                                 display: _self.enableEditTitle && dom.data.value == _self.enableEditNodeId ? "" : "none"
                             },
                             on: {
                                 click: function () {
                                     _self.abandon(dom)
                                 }
                             }
                         }),
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-arrow-round-up'
                             }),
                             attrs: {
                                 title: '向上移动'
                             },
                             style: {
                                 display: (_self.enableEditTitle || dom.data.parentId == "0") || _self.upAndDownShow(dom, "up") ? "none" : ""
                             },
                             on: {
                                 click: function () {
                                     _self.roundUp(dom)
                                 }
                             }
                         }),
                         h('Button', {
                             props: Object.assign({}, {
                                 type: 'text',
                                 size: 'small',
                             }, {
                                 icon: 'md-arrow-round-down'
                             }),
                             attrs: {
                                 title: '向下移动'
                             },
                             style: {
                                 display: (_self.enableEditTitle || dom.data.parentId == "0") || _self.upAndDownShow(dom, "down") ? "none" : ""
                             },
                             on: {
                                 click: function () {
                                     _self.roundDown(dom)
                                 }
                             }
                         })
                ])
            ]);
        },
        titleRenderFunc(h, dom) {
            let _self = this;
            if (dom.data.value == _self.enableEditNodeId && _self.enableEditTitle) {
                return h('i-input', {
                    props: {
                        value: dom.data.title,
                        type: "text"
                    },
                    style: {
                        width: "100%",
                    },
                    on: {
                        "on-focus": function (event) {
                            _self.editValue = event.target.value;
                        },
                        "on-change": function (event) {
                            dom.data.title = event.target.value;
                        }
                    }
                });
            } else {
                return h('span', [
                        h('Icon', {
                            props: {
                                type: (dom.data.children && dom.data.children.length) ? 'ios-folder-outline' : 'ios-document-outline'
                            },
                            style: {
                                marginRight: '8px'
                            }
                        }),
                      h('span', dom.data.title)
                ]);
            }
        },
        add (dom) {
            if (dom.data.data.type == 2) {
                iview.Message.error("此节点下不能再增加节点！");
                return;
            }
            this.modalValue = true;
            this.curNode = dom.node;
        },
        remove (dom) {
            let _self = this;
            _.MessageBox("确认删除当前内容？", function () {
                _.Ajax('Delete', {
                    Data: dom.data.data
                }, function (data) {
                    _self.deleteNode(dom);
                    iview.Message.info("删除成功");
                });
            });
        },
        edit (dom) {
            this.enableEditTitle = true;
            this.enableEditNodeId = dom.data.value;
        },
        save (dom) {
            let _self = this;
            let param = dom.data.data;
            param.MODULENAME = dom.data.title;
            _.Ajax('Edit', {
                Data: param
            }, function (data) {
                iview.Message.info("编辑成功");
                _self.enableEditTitle = false;
                _self.enableEditNodeId = null;
            });
        },
        abandon (dom) {
            let _self = this;
            _self.enableEditTitle = false;
            _self.enableEditNodeId = null;
            if (_self.editValue) {
                dom.data.title = _self.editValue;
            }
        },
        roundUp (dom) {
            this.nodeUpDown(dom, "up");
        },
        roundDown (dom) {
            this.nodeUpDown(dom, "down");
        },
        //节点上下移动
        nodeUpDown(dom, type) {
            let _self = this;
            let param = [];
            let parNode = _self.getCurNodeParNode(dom);
            let chl = parNode.node.children;
            for (let i = 0; i < chl.length; i++) {
                if (chl[i].value == dom.data.value) {
                    let a = dom.data.data;
                    let index = a.INX;
                    let b;
                    if (type == "up") {
                        b = chl[i - 1].data;
                    } else {
                        b = chl[i + 1].data;
                    }
                    a.INX = b.INX;
                    b.INX = index;
                    param = [a, b];
                    break;
                }
            }
            _.Ajax('RoundUpAndDown', {
                Data: param
            }, function (data) {
                iview.Message.info("成功");
            });
        },
        //获取当前节点的父级节点
        getCurNodeParNode(dom) {
            if (dom.node.parent == undefined) {
                return null;
            };
            for (let i = 0; i < dom.root.length; i++) {
                if (dom.root[i].nodeKey == dom.node.parent) {
                    return dom.root[i];
                }
            }
        },
        //获取当前节点在父级节点children的序号
        getCurNodeInParNodeInx(dom) {
            let parNode = this.getCurNodeParNode(dom);
            if (!parNode) {
                return null;
            }
            for (let i = 0; i < parNode.children.length; i++) {
                if (parNode.children[i] == dom.data.nodeKey) {
                    return (i + 1);
                }
            }
            return null;
        },
        //判断向上移动、向下移动按钮是否显示
        upAndDownShow(dom, type) {
            let _self = this;
            let inx = _self.getCurNodeInParNodeInx(dom);
            let data = _self.treeData;
            if (!inx) {
                if (data.length < 2) {
                    return true;
                } else {
                    let index;
                    for (let i = 0; i < data.length; i++) {
                        if (data[i].nodeKey == dom.data.nodeKey) {
                            index = i + 1;
                        }
                    }
                    if (type == "up") {
                        if (index == 1) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        if (index == data.length) {
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
            } else {
                if (type == "up") {
                    if (inx == 1) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    let parChl = _self.getCurNodeParNode(dom);
                    if (parChl.children && inx == parChl.children.length) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        },
        //删除节点
        deleteNode(dom) {
            let _self = this;
            let data = _self.treeData;
            let func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].value == dom.data.value) {
                        data.splice(i, 1);
                        return;
                    }
                    if (data[i].children && data[i].children.length) {
                        func(data[i].children);
                    }
                }
            }
            func(data);
        },
        //为 Tree 的一个节点的前面增加一个节点
        insertBefore(item, nodeKey) {
            let _self = this;
            let func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].nodeKey == nodeKey) {
                        data.splice(i, 0, item);
                        return;
                    }
                    if (data[i].children && data[i].children.length) {
                        func(data[i].children);
                    }
                }
            }
            func(_self.treeData);
        },
        //为 Tree 的一个节点的后面增加一个节点
        insertAfter(item, nodeKey) {
            let _self = this;
            let func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].nodeKey == nodeKey) {
                        data.splice(i, 0, item);
                        return;
                    }
                    if (data[i].children && data[i].children.length) {
                        func(data[i].children);
                    }
                }
            }
            func(_self.treeData);
        },
        search() {
            let _self = this;
            _self.tbLoading = true;
            _self.tableData = [];
            _.Search({
                Service: "XtglService",
                Method: "GetMenu",
                Data: _self.dataParam,
                PageInfo: {},
                Success: function (data) {
                    _self.tbLoading = false;
                    if (data.rows.length > 0) {
                        _self.tableData = data.rows;
                    }
                    else {
                        iview.Message.info("没有满足当前查询条件的结果!");
                    }
                },
                Error: function () {
                    _self.tbLoading = false;
                }
            });
        },
        addNew() {
            let _self = this;
            if (!_self.dataParam.TYPE) {
                iview.Message.error("类型不能为空!");
                return;
            }
            if (!_self.dataParam.NAME) {
                iview.Message.error("名称不能为空!");
                return;
            }
            let data = [];
            if (_self.dataParam.TYPE == 2) {
                if (!_self.dataParam.URL) {
                    iview.Message.error("URL不能为空!");
                    return;
                }
                let selection = this.$refs.selectData.getSelection();
                data = [{
                    MODULENAME: _self.dataParam.NAME,
                    TYPE: _self.dataParam.TYPE,
                    PMODULEID: _self.curNode.node.value,
                    URL: _self.dataParam.URL
                }];
            } else {
                data = [{
                    MODULENAME: _self.dataParam.NAME,
                    TYPE: _self.dataParam.TYPE,
                    PMODULEID: _self.curNode.node.value,
                }];
            }
            _.Ajax('Add', {
                Data: data
            }, function (data) {
                debugger
                iview.Message.info("添加成功");
                _self.modalValue = false;
            });
        }
    }
});