/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:04
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLINSTOCKITETM", "物料购进单子表WLINSTOCKITETM")]
    public partial class WLINSTOCKITETMEntity : TableEntityBase
    {
        public WLINSTOCKITETMEntity()
        {
        }

        public WLINSTOCKITETMEntity(string billid, string goodsid)
        {
            BILLID = billid;
            GOODSID = goodsid;
        }
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 收款方式
        /// <summary>
        [PrimaryKey]
        public string GOODSID
        {
            get; set;
        }


        public string QUANTITY
        {
            get; set;
        }
    }
}
