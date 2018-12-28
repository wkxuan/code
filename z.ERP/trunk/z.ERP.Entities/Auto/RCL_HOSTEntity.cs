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
    [DbTable("RCL_HOST", "日结表为了互斥")]
    public partial class RCL_HOSTEntity : TableEntityBase
    {
        public RCL_HOSTEntity()
        {
        }

        [PrimaryKey]
        [Field("机器名")]
        public string HOSTNAME
        {
            get; set;
        }
    }
}
