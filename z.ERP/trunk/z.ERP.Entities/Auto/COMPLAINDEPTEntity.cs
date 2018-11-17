using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("COMPLAINDEPT", "投诉部门")]
    public partial class COMPLAINDEPTEntity : TableEntityBase
    {
        public COMPLAINDEPTEntity()
        {
        }

        public COMPLAINDEPTEntity(string id)
        {
            ID = id;
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
        /// <summary>
        /// 名称
        /// <summary>
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
    }
}
