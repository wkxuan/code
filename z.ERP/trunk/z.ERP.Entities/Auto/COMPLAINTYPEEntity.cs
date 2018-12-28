using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("COMPLAINTYPE", "投诉类型")]
    public partial class COMPLAINTYPEEntity : TableEntityBase
    {
        public COMPLAINTYPEEntity()
        {
        }

        public COMPLAINTYPEEntity(string id)
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
