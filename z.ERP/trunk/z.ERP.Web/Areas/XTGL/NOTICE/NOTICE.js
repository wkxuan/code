define.beforeVue = function () {
    _.Ajax('GetBRANCH', {
        1: 1
    }, function (data) {
        let List = $.map(data, item => {
            return {
                label: item.Value,
                value: item.Key
            }
        });
        define.screenParam.BRANCHID = List;
    });
    define.screenParam.colDef = [
    { title: '编号', key: 'ID', width: 80 },
    { title: '通知标题', key: 'TITLE',tooltip :true},
    {
        title: '通知状态', key: 'STATUS',tooltip :true,
        
        render: (h, params) => {
        const row = params.row;
        const color = row.STATUS === 2 ? 'success' : 'warning';
        const text = row.STATUS === 2 ? '发布状态' : '草稿状态';

        return h('Tag', {
            props: {
                type: 'dot',
                color: color
            }
        }, text);
    }}
    ];
    define.dataParam.NOTICE_BRANCH = [];
    define.service = "XtglService";
    define.method = "GetNOTICE";
    define.methodList = "GetNOTICE";
    define.Key = 'ID';

}

define.otherMethods = {
    editorChange(val) {
        define.dataParam.CONTENT = val;
    },
    CheckBoxChange(data){
        let localData = [];
        for (var i = 0; i < data.length; i++) {
            localData.push({ BRANCHID: data[i]});
        };
        Vue.set(define.dataParam, 'NOTICE_BRANCH', localData);
    }
}

define.showOne = function (data, callback) {
    _.Ajax('SearchNOTICE', {
        Data: { ID: data }
    }, function (data) {
        define.dataParam = data.notice;
        Vue.set(define.dataParam, data.notice);       
        let s = $.map(data.branch, item=> {
            return item.ID+"";
        });
        define.dataParam.BRANCHID = s;
        let localData = [];
        if (data.branch.length > 0) {
            for (var i = 0; i < data.branch.length; i++) {
                localData.push({ BRANCHID: data.branch[i].ID });
            };
        }
        Vue.set(define.dataParam, 'NOTICE_BRANCH', localData);
        callback && callback();
    });
}

define.newRecord = function () {
    define.dataParam.CONTENT = "";
    define.dataParam.NOTICE_BRANCH = [];
}

define.IsValidSave = function () {
    if (!define.dataParam.STATUS) {
        iview.Message.info("通知状态不能为空!");
        return false;
    }
    if (!define.dataParam.TITLE) {
        iview.Message.info("通知标题不能为空!");
        return false;
    }
    if (!define.dataParam.CONTENT) {
        iview.Message.info("通知内容不能为空!");
        return false;
    }
    if (define.dataParam.NOTICE_BRANCH.length==0) {
        iview.Message.info("通知门店不能为空!");
        return false;
    }
    return true;
}