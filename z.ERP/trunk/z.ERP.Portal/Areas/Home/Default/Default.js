//计划完成表的当前所选
var indexnum = 0;
var color = ['#F35331', '#2499F8', '#3DF098', '#33B734'];
var fontColor = '#FFF';

//定义进度条组件和属性
var y_gauge1 = null;
var y_gauge2 = null;
var y_gauge3 = null;
var y_gauge4 = null;
var m_gauge1 = null;
var option_Progress = null;

//订单情况螺旋图
var orderStatus = null;
var orderStatus1 = null;
var orderStatus2 = null;
var orderStatus_option = null;

//定义仪表盘组件和属性
var gauge1 = null;
var gauge2 = null;
var gauge3 = null;
var gauge4 = null;
var gauge5 = null;
var option_gauge = null;

//产品饼图组件和属性
var productPie = null;
var productPie_option = null;

//业务进展图组件和属性
var businessProgress = null;
var businessProgress_placeHoledStyle = null;
var businessProgress_dataStyle = null;
var businessProgress_option = null;

//生产质量堆积图组件和属性
var quality_chart = null;
var quality_option = null;

//词云组件和属性
var wordCloud = null;
var wordCloud_option = null;

//生产计划折线图组件和属性
var plan_chart = null;
var plan_option = null;

//环形图的风格定义
var dataStyle = {
    normal: {
        label: { show: false },
        labelLine: { show: false }
    }
};
var placeHolderStyle = {
    normal: {
        color: 'rgba(0,0,0,0.1)',
        label: { show: false },
        labelLine: { show: false }
    },
    emphasis: {
        color: 'rgba(0,0,0,0)'
    }
};

//最大订单号
var lastOrderNumber = 1;

$(document).ready(function () {
    //环形进度条设置对象	
    option_Progress = {
        title: {
            text: '目前进度',
            subtext: '50%',
            x: 'center',
            y: 90,
            itemGap: 10,
            textStyle: {
                color: '#B7E1FF',
                fontWeight: 'normal',
                fontFamily: '微软雅黑',
                fontSize: 12
            },
            subtextStyle: {
                color: '#B7E1FF',
                fontWeight: 'bolder',
                fontSize: 12,
                fontFamily: '微软雅黑'
            }
        },
        series: [{
            type: 'pie',
            center: ['50%', '50%'],
            radius: [75, 90],
            x: '0%',
            tooltip: { show: false },
            data: [{
                name: '达成率',
                value: 79,
                itemStyle: { color: 'rgba(0,153,255,0.8)' },
                hoverAnimation: false,
                label: {
                    show: false,
                    position: 'center',
                    textStyle: {
                        fontFamily: '微软雅黑',
                        fontWeight: 'bolder',
                        color: '#B7E1FF',
                        fontSize: 12
                    }
                },
                labelLine: {
                    show: false
                }
            },
			{
			    name: '79%',
			    value: 21,
			    itemStyle: { color: 'rgba(0,153,255,0.1)' },
			    hoverAnimation: false,
			    label: {
			        show: false,
			        position: 'center',
			        padding: 20,
			        textStyle: {
			            fontFamily: '微软雅黑',
			            fontSize: 12,
			            color: '#B7E1FF'
			        }
			    },
			    labelLine: {
			        show: false
			    }
			}]
        },
		{
		    type: 'pie',
		    center: ['50%', '50%'],
		    radius: [95, 100],
		    x: '0%',
		    hoverAnimation: false,
		    data: [{
		        value: 100,
		        itemStyle: { color: 'rgba(0,153,255,0.3)' },
		        label: { show: false },
		        labelLine: { show: false }
		    }]
		},
		{
		    type: 'pie',
		    center: ['50%', '50%'],
		    radius: [69, 70],
		    x: '0%',
		    hoverAnimation: false,
		    data: [{
		        value: 100,
		        itemStyle: { color: 'rgba(0,153,255,0.3)' },
		        label: { show: false },
		        labelLine: { show: false }
		    }]
		}]
    };

    //年仪表盘
    // y_gauge1 = echarts.init(document.getElementById('y_gauge1'));
    // y_gauge2 = echarts.init(document.getElementById('y_gauge2'));
    // y_gauge3 = echarts.init(document.getElementById('y_gauge3'));
    // y_gauge4 = echarts.init(document.getElementById('y_gauge4'));

    //订单完成情况螺旋图
    var yearPlanData = [];
    var yearOrderData = [];
    var differenceData = [];
    var visibityData = [];
    var xAxisData = [];

    for (var i = 0; i < 12; i++) {
        yearPlanData.push(Math.round(Math.random() * 900) + 100);
        yearOrderData.push(Math.round(Math.random() * yearPlanData[i]));
        // differenceData.push(yearPlanData[i]-yearOrderData[i]);
        // visibityData.push(yearOrderData[i]);
        xAxisData.push((i + 1).toString() + "月");
    }














    //生产质量堆积图
    quality_chart = echarts.init(document.getElementById('quality'));
    quality_option = {
        title: {
            show: false,
            text: 'AUDIT',
            left: 'center',
            textStyle: {
                color: '#F00',
                fontSize: 12
            }
        },
        xAxis: {
            data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
            axisLabel: {
                textStyle: {
                    color: '#B7E1FF',
                    fontSize: 12
                }
            },
            axisLine: {
                lineStyle: {
                    color: '#09F'
                }
            },
            axisTick: {
                lineStyle: {
                    color: '#09F'
                }
            }
        },
        yAxis: {
            inverse: false,
            splitArea: { show: false },
            axisLine: { show: false },
            axisTick: { show: false },
            axisLabel: {
                textStyle: {
                    color: '#B7E1FF',
                    fontSize: 12,
                    fontFamily: 'Arial',
                }
            },
            splitLine: {
                lineStyle: {
                    color: '#09F'
                }
            }
        },
        grid: {
            left: 100
        },
        tooltip: {
            trigger: 'item',
            textStyle: {
                color: '#B7E1FF',
                fontSize: 12
            }
        },
        legend: {
            show: false,
            top: 'bottom',
            textStyle: {
                color: '#F00',
                fontSize: 12,
                fontFamily: '微软雅黑'
            },
            data: ['AUDIT分数1', 'AUDIT分数']
        },
        series: [
			{
			    name: 'AUDIT分数1',
			    type: 'bar',
			    stack: 'one',
			    itemStyle:
				{
				    normal: { color: color[1] }
				},
			    barWidth: 30,
			    data: [2200, 2900, 3680, 2200, 2900, 3680, 2200, 2900, 3680, 2200, 2900, 3680]
			},
			{
			    name: 'AUDIT分数',
			    type: 'bar',
			    stack: 'one',
			    itemStyle: {
			        normal: {
			            color: '#F90',
			            label: {
			                show: true,
			                position: 'insideTop',
			                textStyle: {
			                    color: '#000',
			                    fontSize: 12
			                }
			            }
			        }
			    },
			    barWidth: 50,
			    data: [1800, 1100, 320, 1800, 1100, 320, 1800, 1100, 320, 1800, 1100, 320]
			}
        ]
    };
    quality_chart.setOption(quality_option);

    //生产计划折线图
    var plan_data1 = [];
    var plan_data2 = [];
    var plan_xAxis = [];
    for (var i = 1; i <= 12; i++) {
        plan_xAxis.push(i + "月");
        //plan_data1.push(Math.round(Math.random() * 100));
        //plan_data2.push(Math.round(Math.random() * 100));

        plan_data1.push(Math.round(Math.random() * 100));
        plan_data2.push(Math.round(Math.random() * 100));
    }
    plan_chart = echarts.init(document.getElementById('plan'));
    plan_option = {
        xAxis: {
            data: plan_xAxis,
            axisLabel: {
                textStyle: {
                    color: '#B7E1FF',
                    fontSize: 12
                }
            },
            axisLine: {
                lineStyle: {
                    color: '#09F'
                }
            },
            axisTick: {
                lineStyle: {
                    color: '#09F'
                }
            }
        },
        yAxis: {
            inverse: false,
            splitArea: { show: false },
            axisLine: { show: false },
            axisTick: { show: false },
            axisLabel: {
                textStyle: {
                    color: '#B7E1FF',
                    fontSize: 12,
                    fontFamily: 'Arial',
                }
            },
            splitLine: {
                lineStyle: {
                    color: '#09F'
                }
            }
        },
        tooltip: {
            trigger: 'axis',
            textStyle: {
                color: '#FFF',
                fontSize: 12
            }
        },
        grid: {
            left: 100
        },
        legend: {
            show: false,
            top: 'bottom',
            textStyle: {
                color: '#F00',
                fontSize: 12
            },
            data: ['计划完成数', '实际完成数']
        },
        series: [
			{
			    name: '计划完成数',
			    type: 'bar',
			    itemStyle:
				{
				    normal: { color: color[1] },
				    emphasis: { color: color[2] }
				},
			    barWidth: 30,
			    data: plan_data1
			},
			{
			    name: '实际完成数',
			    type: 'line',
			    itemStyle: {
			        normal: {
			            color: '#F90',
			            label: {
			                show: true,
			                position: 'top',
			                textStyle: {
			                    color: '#CCC',
			                    fontSize: 12
			                }
			            },
			            lineStyle: {
			                color: '#F90',
			                width: 4
			            }
			        },
			        emphasis: {
			            color: '#FF0'
			        }
			    },
			    symbolSize: 24,
			    data: plan_data2
			}
        ]
    };
    plan_chart.setOption(plan_option);

    //轮番显示tips
    function clock() {
        showToolTip_highlight(plan_chart);
    }
    setInterval(clock, 5000);




    resresh();

    //开始定时刷新
    setInterval(resresh, 5 * 1000);
});

var convertData = function (data) {
    var res = [];
    for (var i = 0; i < data.length; i++) {
        var dataItem = data[i];
        var fromCoord = geoCoordMap[dataItem[0].name];
        var toCoord = geoCoordMap[dataItem[1].name];
        if (fromCoord && toCoord) {
            res.push({
                fromName: dataItem[0].name,
                toName: dataItem[1].name,
                coords: [fromCoord, toCoord]
            });
        }
    }
    return res;
};

function showToolTip_highlight(mychart) {
    var echartObj = mychart;

    // 高亮当前图形
    var highlight = setInterval(function () {
        echartObj.dispatchAction({
            type: 'highlight',
            seriesIndex: 0,
            dataIndex: indexnum
        });

        echartObj.dispatchAction({
            type: 'showTip',
            seriesIndex: 0,
            dataIndex: indexnum
        });
        clearInterval(highlight);
        indexnum = indexnum + 1;
        if (indexnum >= 7) indexnum = 0;
    }, 1000);
}

//定时刷新数据
function resresh() {
    var myDate = new Date();

    $('#refresh').html("<img src=\"images/wait.gif\" align=\"absmiddle\"><span>数据刷新中...</span>");
    $('#currentDate').html(myDate.getFullYear() + "/" + insertZero(myDate.getMonth() + 1) + "/" + insertZero(myDate.getDate()));

    var maxg = Math.round(Math.random() * 500) + 400;
    var n1 = Math.round(Math.random() * (maxg - 100)) + 100;
    var n2 = Math.round(Math.random() * (n1 - 50)) + 50;
    var n3 = (n2 / maxg * 100).toFixed(2);

    //显示最后更新时间
    $('#refresh').html("<span id=\"refreshTime\">最后刷新时间：" + myDate.toLocaleDateString() + " " + myDate.toLocaleTimeString() + "</span>");
}

//生成订单号
function getOrderNumber(n) {
    var no = "000000" + n.toString();
    return no.substring(no.length - 6);
}

//前面补0
function insertZero(n) {
    var no = "000000" + n.toString();
    return no.substring(no.length - 2);
}

//打开模态窗口
function openDialog(DlgName) {
    $('#' + DlgName).dialog('open');
}