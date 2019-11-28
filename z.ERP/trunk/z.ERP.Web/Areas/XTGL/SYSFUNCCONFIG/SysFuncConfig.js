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
                TYPE: "1",
                MENUID: "",
                ICON: ""
            },
            searchParam: {
                NAME: "",
                TYPE: "2"
            },
            modalValue: false,
            curNode: {},
            tabsValue: ''
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
        //tree节点render
        renderContent(h, dom) {
            let _self = this;
            return h('div', {
                class: 'nodeClass',
                style: {
                    width: 'calc(100% - 15px)',
                    float: 'right',
                    background: _self.modalValue && (dom.data.value == _self.curNode.value) ? '#d5e8fc' : ''
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
                   _self.titleRenderFunc(h, dom.data, dom.node, dom.root)
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
                                     _self.addNode(dom.data, dom.node, dom.root);
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
                                     _self.editNode(dom.data, dom.node, dom.root)
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
                                     _self.removeNode(dom.data, dom.node, dom.root)
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
                                     _self.save(dom.data, dom.node, dom.root)
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
                                     _self.abandon(dom.data, dom.node, dom.root)
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
                                 display: (_self.enableEditTitle || dom.data.parentId == "0") || _self.upAndDownShow(dom.data, dom.node, dom.root, "up") ? "none" : ""
                             },
                             on: {
                                 click: function () {
                                     _self.roundUpNode(dom.data, dom.node, dom.root)
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
                                 display: (_self.enableEditTitle || dom.data.parentId == "0") || _self.upAndDownShow(dom.data, dom.node, dom.root, "down") ? "none" : ""
                             },
                             on: {
                                 click: function () {
                                     _self.roundDownNode(dom.data, dom.node, dom.root)
                                 }
                             }
                         })
                ])
            ]);
        },
        //节点title render
        titleRenderFunc(h, data, node, root) {
            let _self = this;
            if (data.value == _self.enableEditNodeId && _self.enableEditTitle) {
                return h('i-input', {
                    props: {
                        value: data.title,
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
                            data.title = event.target.value;
                        }
                    }
                });
            } else {
                return h('span', [
                        h('Icon', {
                            props: {
                                type: data.data.TYPE == '1' ? 'ios-folder-outline' : 'ios-document-outline'
                            },
                            style: {
                                marginRight: '8px'
                            }
                        }),
                      h('span', data.title)
                ]);
            }
        },
        //添加节点
        addNode (data, node, root) {
            if (data.data.TYPE == 2) {
                iview.Message.error("此节点下不能再增加节点！");
                return;
            }
            this.modalValue = true;
            this.curNode = node.node;
        },
        //删除
        removeNode (data, node, root) {
            let _self = this;
            _.MessageBox("确认删除当前内容？", function () {
                _.Ajax('Delete', {
                    Data: data.data
                }, function (res) {
                    _self.deleteNode(data);
                    iview.Message.info("删除成功");
                });
            });
        },
        //编辑节点title
        editNode (data, node, root) {
            this.enableEditTitle = true;
            this.enableEditNodeId = data.value;
        },
        //编辑节点title时保存
        save (data, node, root) {
            let _self = this;
            let param = data.data;
            param.MODULENAME = data.title;
            _.Ajax('Edit', {
                Data: param
            }, function (res) {
                iview.Message.info("编辑成功");
                _self.enableEditTitle = false;
                _self.enableEditNodeId = null;
            });
        },
        //编辑节点title时取消编辑
        abandon (data, node, root) {
            let _self = this;
            _self.enableEditTitle = false;
            _self.enableEditNodeId = null;
            if (_self.editValue) {
                data.title = _self.editValue;
            }
        },
        //向上移动
        roundUpNode (data, node, root) {
            this.nodeUpDown(data, node, root, "up");
        },
        //向下移动
        roundDownNode (data, node, root) {
            this.nodeUpDown(data, node, root, "down");
        },
        //节点上下移动
        nodeUpDown(data, node, root, type) {
            let _self = this;
            let param = [];
            let parNode = _self.getCurNodeParNode(data, node, root);
            let chl = parNode.node.children;
            let bnode = {};
            for (let i = 0; i < chl.length; i++) {
                if (chl[i].value == data.value) {
                    let adata = data.data;
                    let bdata = {};
                    let index = adata.INX;
                    if (type == "up") {
                        bdata = chl[i - 1].data;
                        bnode = chl[i - 1];
                    } else {
                        bdata = chl[i + 1].data;
                        bnode = chl[i + 1];
                    }
                    adata.INX = bdata.INX;
                    bdata.INX = index;
                    param = [adata, bdata];
                    break;
                }
            }
            _.Ajax('RoundUpAndDown', {
                Data: param
            }, function (res) {
                _self.deleteNode(data);
                if (type == "up") {
                    _self.insertBefore(bnode, node.node);
                } else {
                    _self.insertAfter(bnode, node.node);
                }
                iview.Message.info("移动成功");
            });
        },
        //获取当前节点的父级节点
        getCurNodeParNode(data, node, root) {
            if (node.parent == undefined) {
                return null;
            };
            for (let i = 0; i < root.length; i++) {
                if (root[i].nodeKey == node.parent) {
                    return root[i];
                }
            }
        },
        //获取当前节点在父级节点children的序号
        getCurNodeInParNodeInx(data, node, root) {
            let parNode = this.getCurNodeParNode(data, node, root);
            if (!parNode) {
                return null;
            }
            for (let i = 0; i < parNode.children.length; i++) {
                if (parNode.children[i] == data.nodeKey) {
                    return (i + 1);
                }
            }
            return null;
        },
        //判断向上移动、向下移动按钮是否显示
        upAndDownShow(data, node, root, type) {
            let _self = this;
            let inx = _self.getCurNodeInParNodeInx(data, node, root);
            let treeData = _self.treeData;
            if (!inx) {
                if (treeData.length < 2) {
                    return true;
                } else {
                    let index;
                    for (let i = 0; i < treeData.length; i++) {
                        if (treeData[i].nodeKey == data.nodeKey) {
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
                        if (index == treeData.length) {
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
                    let parChl = _self.getCurNodeParNode(data, node, root);
                    if (parChl.children && inx == parChl.children.length) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        },
        //删除节点
        deleteNode(ndata) {
            let _self = this;
            let func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].value == ndata.value) {
                        data.splice(i, 1);
                        return;
                    }
                    if (data[i].children && data[i].children.length) {
                        func(data[i].children);
                    }
                }
            }
            func(_self.treeData);
        },
        //为 Tree 的一个节点的前面增加一个节点
        insertBefore(node, item) {
            let _self = this;
            let func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].nodeKey == node.nodeKey) {
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
        insertAfter(node, item) {
            let _self = this;
            let func = function (data) {
                for (let i = 0; i < data.length; i++) {
                    if (data[i].nodeKey == node.nodeKey) {
                        data.splice(i + 1, 0, item);
                        return;
                    }
                    if (data[i].children && data[i].children.length) {
                        func(data[i].children);
                    }
                }
            }
            func(_self.treeData);
        },
        //为 Tree 的一个节点添加子节点
        insertChildren(pnode, data) {
            pnode.expand = true;
            pnode.children = pnode.children.concat(data);
        },
        //查询菜单
        search() {
            let _self = this;
            _self.tbLoading = true;
            _self.tableData = [];
            _.Search({
                Service: "XtglService",
                Method: "GetMenu",
                Data: _self.searchParam,
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
        //添加新节点
        addNew() {
            let _self = this;
            let param = [];
            let pnode = _self.curNode;
            if (_self.tabsValue == 'select') {
                let selection = _self.$refs.selectData.getSelection();
                if (!selection.length) {
                    iview.Message.error("请选择要添加的菜单!");
                    return;
                }
                let loc = {};
                for (let i = 0; i < selection.length; i++) {
                    for (let j = 0; j < pnode.children.length; j++) {
                        if (selection[i].ID == pnode.children[j].data.MENUID) {
                            iview.Message.error(`此节点下已存在菜单"${selection[i].NAME}"!`);
                            return;
                        }
                    }
                    loc = {
                        MODULENAME: selection[i].NAME,
                        MENUID: selection[i].ID,
                        TYPE: 2,
                        PMODULEID: pnode.value,
                    };
                    param.push(loc);
                }
            } else {
                if (!_self.dataParam.NAME) {
                    iview.Message.error("名称不能为空!");
                    return;
                }
                for (let j = 0; j < pnode.children.length; j++) {
                    if (_self.dataParam.NAME == pnode.children[j].data.MODULENAME) {
                        iview.Message.error(`此节点下已存在"${_self.dataParam.NAME}"!`);
                        return;
                    }
                }
                let loc = {
                    MODULENAME: _self.dataParam.NAME,
                    TYPE: 1,
                    PMODULEID: pnode.value,
                }
                param.push(loc);
            }
            _.Ajax('Add', {
                Data: param
            }, function (data) {
                _self.insertChildren(pnode, data.res);
                iview.Message.info("添加成功");
                _self.modalValue = false;
            });
        }
    }
});