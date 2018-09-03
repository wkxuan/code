/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:17
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("WRITEDATA", "日结表")]
    public partial class WRITEDATAEntity : TableEntityBase
    {
        public WRITEDATAEntity()
        {
        }

        public WRITEDATAEntity(string rq)
        {
            RQ = rq;
        }

 
        [PrimaryKey]
        [Field("日期")]
        [DbType(DbType.DateTime)]
        public string RQ
        {
            get; set;
        }

        [Field("状态")]
        public string STATUS
        {
            get; set;
        }

        [Field("分店")]
        public string BRANCHID
        {
            get; set;
        }
    }
}
