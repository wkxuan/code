define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '编号', key: 'ID', width: 60 },
    { title: '通知标题', key: 'TITLE', width: 300 },
    {
        title: '通知状态', key: 'STATUS',
        
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
    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetNOTICE";
    define.methodList = "GetNOTICE";
    define.Key = 'ID';
}

define.otherMethods = {
    editorChange(val) {
        define.dataParam.CONTENT = val;
    }
}

define.newRecord = function () {
    define.dataParam.CONTENT = "";
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
    return true;
}