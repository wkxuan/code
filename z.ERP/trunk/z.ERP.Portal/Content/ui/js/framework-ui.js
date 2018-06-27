$(function () {
    document.body.className = localStorage.getItem('config-skin');
    $("[data-toggle='tooltip']").tooltip();
})
$.reload = function () {
    location.reload();
    return false;
}
$.loading = function (bool, text) {
    var $loadingpage = top.$("#loadingPage");
    var $loadingtext = $loadingpage.find('.loading-content');
    if (bool) {
        $loadingpage.show();
    } else {
        if ($loadingtext.attr('istableloading') == undefined) {
            $loadingpage.hide();
        }
    }
    if (!!text) {
        $loadingtext.html(text);
    } else {
        $loadingtext.html("数据加载中，请稍后…");
    }
    $loadingtext.css("left", (top.$('body').width() - $loadingtext.width()) / 2 - 50);
    $loadingtext.css("top", (top.$('body').height() - $loadingtext.height()) / 2);
}
$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
$.currentWindow = function () {
    var iframeId = top.$(".NFine_iframe:visible").attr("id");
    return top.frames[iframeId];
}
$.browser = function () {
    var userAgent = navigator.userAgent;
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    };
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    }
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    }
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    };
}
$.download = function (url, data, method) {
    if (url && data) {
        data = typeof data == 'string' ? data : jQuery.param(data);
        var inputs = '';
        $.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
    };
};
$.modalOpen = function (options) {
    var w = $(document).width() * 0.7
    var h = w * 0.56;
    var defaults = {
        id: null,
        title: '系统窗口',
        width: w + 'px',
        height: h + 'px',
        url: '',
        shade: 0.3,
        btn: ['确认', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        callBack: null
    };
    var options = $.extend(defaults, options);
    var _width = top.$(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() + 'px';
    var _height = top.$(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() + 'px';
    top.layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: options.url,
        btn: options.btn,
        btnclass: options.btnclass,
        yes: function () {
            options.callBack(options.id)
        }, cancel: function () {
            return true;
        }
    });
}
$.modalConfirm = function (content, callBack) {
    top.layer.confirm(content, {
        icon: "fa-exclamation-circle",
        title: "系统提示",
        btn: ['确认', '取消'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
    }, function (index) {
        top.layer.close(index);
        callBack(true);
    }, function () {
        callBack(false)
    });
}
$.modalAlert = function (content, type, callBack) {
    var icon = "";
    if (type == 'success') {
        icon = "fa-check-circle";
    }
    if (type == 'error') {
        icon = "fa-times-circle";
    }
    if (type == 'warning') {
        icon = "fa-exclamation-circle";
    }
    top.layer.alert(content, {
        icon: icon,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
    }, function (index) {
        top.layer.close(index);
        if (callBack) {
            callBack();
        }
    });
}
$.modalMsg = function (content, type) {
    if (type != undefined) {
        var icon = "";
        if (type == 'success') {
            icon = "fa-check-circle";
        }
        if (type == 'error') {
            icon = "fa-times-circle";
        }
        if (type == 'warning') {
            icon = "fa-exclamation-circle";
        }
        top.layer.msg(content, { icon: icon, time: 4000, shift: 5 });
        top.$(".layui-layer-msg").find('i.' + icon).parents('.layui-layer-msg').addClass('layui-layer-msg-' + type);
    } else {
        top.layer.msg(content);
    }
}
$.modalClose = function () {
    var index = top.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-bindSelect-btn').find("#IsdialogClose");
    var IsClose = $IsdialogClose.is(":checked");
    if ($IsdialogClose.length == 0) {
        IsClose = true;
    }
    if (IsClose) {
        top.layer.close(index);
    } else {
        location.reload();
    }
}
$.submitForm = function (options) {
    var defaults = {
        url: "",
        param: [],
        loading: "正在提交数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    $.loading(true, options.loading);
    window.setTimeout(function () {
        if ($('[name=__RequestVerificationToken]').length > 0) {
            options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.state == "success") {
                    options.success(data);
                    $.modalMsg(data.message, data.state);
                    if (options.close == true) {
                        $.modalClose();
                    }
                } else {
                    $.modalAlert(data.message, data.state);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.loading(false);
                $.modalMsg(errorThrown, "error");
            },
            beforeSend: function () {
                $.loading(true, options.loading);
            },
            complete: function () {
                $.loading(false);
            }
        });
    }, 100);
}
$.deleteForm = function (options) {
    var defaults = {
        prompt: "注：您确定要删除该项数据吗？",
        url: "",
        param: [],
        loading: "正在删除数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            $.loading(true, options.loading);
            window.setTimeout(function () {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        if (data.state == "success") {
                            options.success(data);
                            $.modalMsg(data.message, data.state);
                        } else {
                            $.modalAlert(data.message, data.state);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.loading(false);
                        $.modalMsg(errorThrown, "error");
                    },
                    beforeSend: function () {
                        $.loading(true, options.loading);
                    },
                    complete: function () {
                        $.loading(false);
                    }
                });
            }, 100);
        }
    });
}
$.optionForm = function (options) {
    var defaults = {
        prompt: null,
        url: "",
        param: [],
        loading: '正在执行...',
        success: null,
        close: true,
    };
    var hander = function () {
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.state == "success") {
                    options.success(data);
                    $.modalMsg(data.message, data.state);
                } else {
                    $.modalAlert(data.message, data.state);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.loading(false);
                $.modalMsg(errorThrown, "error");
            },
            beforeSend: function () {
                $.loading(true, options.loading);
            },
            complete: function () {
                $.loading(false);
            }
        });
    }
    var options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    if (options.prompt) {
        $.modalConfirm(options.prompt, function (r) {
            if (r) {
                $.loading(true, options.loading);
                window.setTimeout(hander, 100);
            }
        });
    }
    else {
        $.loading(true, options.loading);
        hander();
    }
}
$.jsonWhere = function (data, action) {
    if (action == null) return;
    var reval = new Array();
    $(data).each(function (i, v) {
        if (action(v)) {
            reval.push(v);
        }
    })
    return reval;
}
$.fn.jqGridRowValue = function () {
    var $grid = $(this);
    var selectedRowIds = $grid.jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds != "") {
        var json = [];
        var len = selectedRowIds.length;
        for (var i = 0; i < len; i++) {
            var rowData = $grid.jqGrid('getRowData', selectedRowIds[i]);
            json.push(rowData);
        }
        return json;
    } else {
        return $grid.jqGrid('getRowData', $grid.jqGrid('getGridParam', 'selrow'));
    }
}
$.fn.formValid = function () {
    var $this = $(this);
    return $this.valid({
        errorPlacement: function (error, element) {
            element.parents('.formValue').addClass('has-error');
            if ($this.data("tabform")) {
                element.parent().find("span.error").remove();
                element.parent().append('<span class="error">' + error + '<span>');
            }
            else {
                element.parents('.has-error').find('i.error').remove();
                element.parents('.has-error').append('<i class="form-control-feedback fa fa-exclamation-circle error" data-placement="left" data-toggle="tooltip" title="' + error + '"></i>');
                $("[data-toggle='tooltip']").tooltip();
                if (element.parents('.input-group').hasClass('input-group')) {
                    element.parents('.has-error').find('i.error').css('right', '33px')
                }
            }
        },
        success: function (element) {
            if ($this.data("tabform")) {
                element.parent().find('span.error').remove();
            }
            else {
                element.parents('.has-error').find('i.error').remove();
                element.parent().removeClass('has-error');
            }
        }
    });
}
$.fn.formSerialize = function (formdate) {
    var element = $(this);
    if (!!formdate) {
        for (var key in formdate) {
            var $id = element.find('#' + key);
            var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
            var type = $id.attr('type');
            if ($id.hasClass("select2-hidden-accessible")) {
                type = "select";
            }
            switch (type) {
                case "checkbox":
                    if (value == "true") {
                        $id.attr("checked", 'checked');
                    } else {
                        $id.removeAttr("checked");
                    }
                    break;
                case "select":
                    $id.val(value).trigger("change");
                    break;
                default:
                    $id.val(value);
                    break;
            }
        };
        return false;
    }
    var postdata = {};
    element.find('input,select,textarea').each(function (r) {
        var $this = $(this);
        var id = $this.attr('id');
        var type = $this.attr('type');
        switch (type) {
            case "checkbox":
                postdata[id] = $this.is(":checked");
                break;
            default:
                var value = $this.val() == "" ? "&nbsp;" : $this.val();
                if (!$.request("keyValue")) {
                    value = value.replace(/&nbsp;/g, '');
                }
                postdata[id] = value;
                break;
        }
    });
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return postdata;
};
$.fn.fillInfo = function (data) {
    var element = $(this);
    if (!!data) {
        for (var key in data) {
            var info = element.find('#' + key);
            if (info.size() > 0) {
                var value = $.trim(data[key]).replace(/&nbsp;/g, '');
                if (info.attr('setvalue') != null && info.attr('setvalue') != '') {
                    var func = eval(info.attr('setvalue'));
                    func(info, value, data);
                }
                else {
                    info.text(value);
                }
            }
        }
    }
}
$.fn.bindSelect = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        search: false,
        url: "",
        data: null,
        param: [],
        change: null
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data, function (i, item) {
                    var op = $('<option value="' + item[options.id] + '">' + item[options.text] + '</option >');
                    $element.append(op);
                });
                $element.select2({
                    minimumResultsForSearch: options.search == true ? 0 : -1
                });
                $element.on("change", function (e) {
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                    $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                });
            }
        });
    }
    else if (options.data) {
        $.each(options.data, function (i, item) {
            var op = $('<option value="' + item[options.id] + '">' + item[options.text] + '</option >');
            $element.append(op);
        });
        $element.select2({
            minimumResultsForSearch: options.search == true ? 0 : -1
        });
        $element.on("change", function (e) {
            if (options.change != null) {
                options.change(data[$(this).find("option:selected").index()]);
            }
            $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
        });
    }
    else {
        $element.select2({
            minimumResultsForSearch: -1
        });
    }
}
$.fn.authorizeButton = function () {
    var moduleId = top.$(".NFine_iframe:visible").attr("id").substr(6);
    var dataJson = top.clients.authorizeButton[moduleId];
    var $element = $(this);
    $element.find('a[authorize=yes]').attr('authorize', 'no');
    if (dataJson != undefined) {
        $.each(dataJson, function (i) {
            $element.find("#" + dataJson[i].F_EnCode).attr('authorize', 'yes');
        });
    }
    $element.find("[authorize=no]").parents('li').prev('.split').remove();
    $element.find("[authorize=no]").parents('li').remove();
    $element.find('[authorize=no]').remove();
}
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
$.fn.dataGrid = function (options) {
    var defaults = {
        datatype: "json",
        autowidth: true,
        shrinkToFit: true,
        gridview: true,
        forceFit: true,
        rowNum: 20,
        rowList: [10, 20, 30, 50, 100],
        jsonReader: {
            root: "data.Datas",
            page: "data.Pagination.Index",
            total: "data.Pagination.TotalPage",
            records: "data.Pagination.RecordCount",
            repeatitems: true,
            cell: "cell",
            id: "id",
            userdata: "userdata",
            subgrid: {
                root: "childs",
                repeatitems: true,
                cell: "cell"
            }
        },
        prmNames: {
            page: "pageIndex",
            rows: "pageSize",
            sort: "SortFields[0].FiledName",
            order: "SortFields[0].Mode",
            search: null,
            nd: "nd",
            id: "id",
            oper: "oper",
            editoper: "edit",
            addoper: "add",
            deloper: "del",
            subgridid: "id",
            npage: null,
            totalrows: "totalrows"
        },
        serializeGridData: function (postData) {
            postData = $.extend(postData, $(this.p.searchFormId).serializeObject());
            return postData;
        }
    };
    if (options.colModel) {
        var width = this.parent().width();
        var t = 0;
        $.each(options.colModel, function (index, item) {
            if (item.width && item.width < 1) {
                item.width = width * item.width;
                t += item.width;
            }
        })
    }
    var options = $.extend(defaults, options);
    var $element = $(this);
    //if (options.onSelectRow == null) {
    //    options["onSelectRow"] = function (rowid) {
    //        var length = $(this).jqGrid("getGridParam", "selrow").length;
    //        var $operate = $(".operate");
    //        if (length > 0) {
    //            $operate.animate({ "left": 0 }, 200);
    //        } else {
    //            $operate.animate({ "left": '-100.1%' }, 200);
    //        }
    //        $operate.find('.close').click(function () {
    //            $operate.animate({ "left": '-100.1%' }, 200);
    //        })
    //    };
    //}
    $element.jqGrid(options);
};
$.exportSubmit = function (opt) {
    $("#_frame").remove();
    $("#_form").remove();
    var postData = $('#' + opt.searchFormId).serializeObject();
    var colModels = $('#' + opt.gridId).jqGrid('getGridParam', 'colModel');

    var iframe = $('<iframe id="_frame" name="rfFrame" src="about:blank" style="display:none"></iframe >');
    $(document.body).append(iframe);

    var form = $('<form id="_form" method="post"></form>');
    form.attr('target', 'rfFrame');
    form.attr('action', opt.url);

    for (var x in postData) {
        var input = $('<input type="hidden" />');
        input.attr('name', x);
        input.val(postData[x]);
        form.append(input);
    }

    $.each(colModels, function (i, item) {
        if (!item.noexport) {
            var input = $('<input type="hidden" />');
            input.attr('name', 'ExportColSettings[' + i + '].Id');
            input.val(item.name);
            form.append(input);

            var input1 = $('<input type="hidden" />');
            input1.attr('name', 'ExportColSettings[' + i + '].Name');
            input1.val(item.label);
            form.append(input1);
        }
    });
    var input = $('<input type="submit" style="display:none" />');
    form.append(input);
    var input = $('<input type="hidden" />')
    input.attr('name', 'ExportFileType');
    input.val(opt.fileType || 'Excel');
    form.append(input);

    var input = $('<input type="hidden" />')
    input.attr('name', 'ExportFileName');
    input.val(opt.fileName);
    form.append(input);

    var input = $('<input type="hidden" />')
    input.attr('name', 'ExportFileTitle');
    input.val(opt.contentTitle);
    form.append(input);

    $(document.body).append(form);
    form.submit();
};
$.fn.initZtreeSelect = function (opt) {
    var $this = $(this);
    if ($('div[data-url="' + opt.treeDataUrl + '"]').size() == 0) {
        $('body').append('<div class="selectTreeContent" style="display:none" data-url="' + opt.treeDataUrl + '"><div class="ztree"></div></div>');
        $.get(opt.treeDataUrl, function (res) {
            if (res.state == "success") {
                var setting = {
                    data: {
                        simpleData: {
                            enable: true,
                            idKey: "id",
                            pIdKey: "pId",
                            rootPId: 0
                        }
                    },
                    callback: {
                        onClick: function (event, treeId, treeNode) {
                            $this.empty();
                            $this.append('<option value="' + treeNode.id + '">' + treeNode.name + '</option>');
                            $('div[data-url="' + opt.treeDataUrl + '"]').fadeOut("fast");
                        }
                    }
                };
                var treeObj = $.fn.zTree.init($('div[data-url="' + opt.treeDataUrl + '"]').find('.ztree'), setting, res.data);
                var nodes = treeObj.getNodes();

                for (var i = 0; i < nodes.length; i++) { //设置节点展开
                    treeObj.expandNode(nodes[i], true, false, true);
                }
            }
        })
    }
    $this.click(function () {
        var obj = $(this).parent();
        $('div[data-url="' + opt.treeDataUrl + '"]').css({
            left: obj.offset().left + "px",
            top: (obj.offset().top + obj.outerHeight() - 2) + "px",
            width: obj.width() + "px"
        }).slideDown("fast");
        $(document).on("mousedown", "body", function (e) {
            var obj = e.srcElement || e.target
            if ($(obj).parents('div[data-url="' + opt.treeDataUrl + '"]').size() == 0) {
                $('div[data-url="' + opt.treeDataUrl + '"]').fadeOut("fast");
            }
        });
    })

}