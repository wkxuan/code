
using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("ALERT_FIELD", "预警信息显示")]
    public partial class ALERT_FIELDEntity : TableEntityBase
    {
        public ALERT_FIELDEntity()
        {
        }

        public ALERT_FIELDEntity(string id, string fieldmc)
        {
            ID = id;
            FIELDMC = fieldmc;
        }

        /// <summary>
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("代码")]
        public string ID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("字段名")]
        public string FIELDMC
        {
            get; set;
        }

        [Field("中文名")]
        public string CHINAMC
        {
            get; set;
        }


        [Field("预警显示的宽度")]
        public string WIDTH
        {
            get; set;
        }
        [Field("排列顺序")]
        public string PLSX
        {
            get; set;
        }
    }
}
