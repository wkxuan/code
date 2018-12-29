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
    [DbTable("WL_GOODSSTOCK", "物料库存表")]
    public partial class WL_GOODSSTOCKEntity : TableEntityBase
    {
        public WL_GOODSSTOCKEntity()
        {
        }

        public WL_GOODSSTOCKEntity(string goodsid)
        {
            GOODSID = goodsid;
        }
        [PrimaryKey]
        public string GOODSID
        {
            get; set;
        }


        public string QTY
        {
            get; set;
        }
    }
}
