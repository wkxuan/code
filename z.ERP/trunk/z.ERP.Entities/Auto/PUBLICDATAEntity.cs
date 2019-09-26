using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PUBLICDATA", "公共设施数据")]
    public partial class PUBLICDATAEntity: TableEntityBase
    {
        public PUBLICDATAEntity() { }
        public PUBLICDATAEntity(string id) {
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
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
        [Field("图片地址")]
        public string IMAGEURL
        {
            get; set;
        }
        [Field("图片地址")]
        public string COLOR
        {
            get; set;
        }
    }
}
