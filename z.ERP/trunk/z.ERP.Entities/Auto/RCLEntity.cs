/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:17
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("RCL", "会员日结表")]
    public partial class RCLEntity : TableEntityBase
    {
        public RCLEntity()
        {
        }

        public RCLEntity(string rq)
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

        [Field("操作人")]
        public string OPERATOR_ID
        {
            get; set;
        }
        [Field("开始时间")]
        [DbType(DbType.DateTime)]
        public string PROC_KSSJ
        {
            get; set;
        }
        [Field("结束时间")]
        [DbType(DbType.DateTime)]
        public string PROC_JSSJ
        {
            get; set;
        }

        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
    }
}
