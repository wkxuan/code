$.fn.extend({
    zMapPoint: function (options) {
        if (!options)
            return $(this).data('zMapPoint');
        var defaults = {
            interaction: 'hover', //'click' 点击弹出
            width: 100,
            height: 100,
            canEdit: false,
            Url: '',
            data: [],
            showmodel: false
        }
        var res = {};
        var options = $.extend(defaults, options);
        
        var self = this;
        $(self).html('');
        $(self).width(options.width);
        $(self).height(options.height);
        var maparea = $('<div class="ip_slide"></div>');
        $(self).append(maparea);
        maparea.append('<img id="point_map" class="ip_tooltipImg" src="' + options.Url + '" width="' + options.width + '" height="' + options.height + '">');
        function addPoint(d) {
            var newdom = $('<div code="'+d.name+'"class="ip_tooltip ip_img32" data-button="moreblue" data-tooltipbg="bgblack" data-round="roundBgW" data-animationtype="btt-slide" style="color:white"></div>');
            //btt-slide: 上
            //ltr-slide: 右
            //rtl-slide: 左
            //ttb-slide: 下
            newdom.css('top', options.height * d.y);
            newdom.css('left', options.width * d.x);
            newdom.html(d.html ? d.html(d) : d.name);
            newdom.data('data', d);
            maparea.append(newdom);
            Render(newdom);
        }
        options.data.forEach(function (d, i) {
            addPoint(d);
        });
        function Render(value) {
            htmlContent = $(value).html();
            button = $(value).data('button');
            round = $(value).data('round');
            tooltipBg = $(value).data('tooltipbg');
            $(value).html('');
            if (round != undefined && round != "") {
                $('<div class="' + round + '"></div><div class="' + round + 'In"></div><div class="' + round + 'Inner"></div>')
                    .appendTo(value);
            }
            $('<div class="button ' + button + '"></div>').appendTo(value);
            descrContainer = $('<div class="descrContainer"><div class="ip_descr ' + tooltipBg + '"><div class="xs">' + htmlContent + '</div></div></div>')
                .appendTo(value);
            var type = $(value).data('animationtype');
            switch (type) {
                case "ttb-slide":
                    insertDescr($(value), 'ttb-before', 'pass-ttb');
                    if (options.interaction == "click") clickHandler($(value), 'ttb-slide');
                    else mouseOverHandler($(value), 'ttb-slide');
                    break;
                case "btt-slide":
                    insertDescr($(value), 'btt-before', 'pass-btt');
                    if (options.interaction == "click") clickHandler($(value), 'btt-slide');
                    else mouseOverHandler($(value), 'btt-slide');
                    break;
                case "rtl-slide":
                    insertDescr($(value), 'rtl-before', 'pass-rtl');
                    if (options.interaction == "click") clickHandler($(value), 'rtl-slide');
                    else mouseOverHandler($(value), 'rtl-slide');
                    break;
                default:
                    insertDescr($(value), 'ltr-before', 'pass-ltr');
                    if (options.interaction == "click") clickHandler($(value), 'ltr-slide');
                    else mouseOverHandler($(value), 'ltr-slide');
                    break;
            };
            if (options.canEdit) {
                //拖动
                {
                    var _drag = {};
                    _drag.top = 0; //拖动过的位置距离上边
                    _drag.left = 0; //拖动过的位置距离左边
                    _drag.maxLeft; //距离左边最大的距离
                    _drag.maxTop; //距离上边最大的距离
                    _drag.dragging = false; //是否拖动标志
                    //拖动函数
                    function bindDrag(el) {
                        var winWidth = $(self).width(),
                        winHeight = $(self).height(),
                        objWidth = $(el).outerWidth(),
                        objHeight = $(el).outerHeight();
                        _drag.maxLeft = winWidth - objWidth,
                        _drag.maxTop = winHeight - objHeight;
                        var els = el.style,
                        x = 0,
                        y = 0;
                        var objTop = $(el).offset().top,
                        objLeft = $(el).offset().left;
                        $(el).mousedown(function (e) {
                            _drag.dragging = true;
                            _drag.isDragged = true;
                            x = e.clientX - el.offsetLeft;
                            y = e.clientY - el.offsetTop;
                            el.setCapture && el.setCapture();
                            $(document).bind('mousemove', mouseMove).bind('mouseup', mouseUp);
                            return false;
                        });
                        function mouseMove(e) {
                            e = e || window.event;
                            if (_drag.dragging) {
                                _drag.top = e.clientY - y;
                                _drag.left = e.clientX - x;
                                _drag.top = _drag.top > _drag.maxTop ? _drag.maxTop : _drag.top;
                                _drag.left = _drag.left > _drag.maxLeft ? _drag.maxLeft : _drag.left;
                                _drag.top = _drag.top < 0 ? 0 : _drag.top;
                                _drag.left = _drag.left < 0 ? 0 : _drag.left;
                                els.top = _drag.top + 'px';
                                els.left = _drag.left + 'px';
                                return false;
                            }
                        }
                        function mouseUp(e) {
                            _drag.dragging = false;
                            el.releaseCapture && el.releaseCapture();
                            e.cancelBubble = true;
                            $(document).unbind('mousemove', mouseMove).unbind('mouseup', mouseUp);
                            var data = $(el).data('data');
                            data.x = _drag.left / options.width;
                            data.y = _drag.top / options.height;
                            $(el).data('data', data);
                        }
                        $(window).resize(function () {
                            var winWidth = $(window).width(),
                            winHeight = $(window).height(),
                            el = $(el),
                            elWidth = el.outerWidth(),
                            elHeight = el.outerHeight(),
                            elLeft = parseFloat(el.css('left')),
                            elTop = parseFloat(el.css('top'));
                            _drag.maxLeft = winWidth - elWidth;
                            _drag.maxTop = winHeight - elHeight;
                            _drag.top = _drag.maxTop < elTop ? _drag.maxTop : elTop;
                            _drag.left = _drag.maxLeft < elLeft ? _drag.maxLeft : elLeft;
                            el.css({
                                top: _drag.top,
                                left: _drag.left
                            })
                        })
                    }
                    bindDrag($(value)[0]);
                }
                //右键菜单
                {
                    $(value).contextMenu({
                        width: 110,// width
                        itemHeight: 30,// 菜单项height
                        bgColor: "#333",// 背景颜色
                        color: "#fff",// 字体颜色
                        fontSize: 12,// 字体大小
                        hoverBgColor: "#99CC66",// hover背景颜色
                        menu: [{
                            text: "更换店铺号",
                            callback: function () {
                                var data = $(value).data('data');
                                var newname = prompt("请输入新店铺号", data.name);
                                data.name = newname;
                                $(value).html(data.html ? data.html(data) : data.name);
                                $(value).data('data', data);
                                Render($(value));
                            }
                        }, {
                            text: "删除",
                            callback: function () {
                                $(value).remove();
                            }
                        }]
                    });
                }
            }
            else
                //弹窗
            {
               //拖动函数
                function bindmouseUp(el) {
                    $(el).mousedown(function (e) {
                        $(el).bind('mouseup', mouseUp);
                        return false;
                    });
                    function mouseUp(e) {
                        window.parent.mapShow.screenParam.selectCode = e.delegateTarget.code;
                        window.parent.mapShow.screenParam.showPopShop = true;
                    }
                }
                bindmouseUp($(value)[0]);
            }
        }
        $(self).data('mapdata', options);
        function insertDescr(selector, descrClass, divClass) {
            var descr = selector.find(".ip_descr").addClass(descrClass);
            $('<div class="' + divClass + '"></div>').insertBefore(descr);
        };
        function clickHandler(selector, animationType) {
            var clickToggle = true;
            selector.find(".button").on('click', function () {
                if (clickToggle) {
                    showTooltip(selector, animationType);
                    clickToggle = false;
                } else {
                    hideTooltip(selector, animationType);
                    clickToggle = true;
                }
            });
        };
        function mouseOverHandler(selector, animationType) {
            selector.on('mouseover', function (eventObject) {
                showTooltip(selector, animationType);
            });
            selector.on('mouseout', function (eventObject) {
                hideTooltip(selector, animationType);
            });
        };
        function showTooltip(selector, animationType) {
            selector.css('z-index', '9999');
            selector.addClass('show');
            selector.find(".xs").css('display', 'block');
            selector.find(".ip_descr").addClass(animationType);
        };
        function hideTooltip(selector, animationType) {
            selector.css('z-index', '1');
            selector.removeClass('show');
            selector.find(".xs").css('display', 'none');
            selector.find(".ip_descr").removeClass(animationType);
        };
        res.GetData = function () {
            var data = new Array();
            $(self).find('.ip_tooltip').each(function (index, value) {
                data.push($(value).data('data'));
            });
            return data;
        };
        res.Add = function (d) {
            if (options.canEdit)
                addPoint(d)
            else
                alert("不允许编辑");
        }
        $(self).data('zMapPoint', res)
        return res;
    }
});