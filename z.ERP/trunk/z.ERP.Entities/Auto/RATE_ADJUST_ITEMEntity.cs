using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("RATE_ADJUST_ITEM", "折扣调整明细表")]
    public partial class RATE_ADJUST_ITEMEntity:TableEntityBase
    {
        public RATE_ADJUST_ITEMEntity() {
        }
        public RATE_ADJUST_ITEMEntity(string id,string sheetid,string goodsid) {
            ID = id;
            SHEETID = sheetid;
            GOODSID = goodsid;
        }
        /// <summary>
        /// 主键id
        /// </summary>
        [PrimaryKey]
        [Field("ID")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 分单号预留
        /// </summary>
        [Field("分单号")]
        public string SHEETID
        {
            get; set;
        }
        /// <summary>
        /// 商品id
        /// </summary>
        [Field("商品id")]
        public string GOODSID
        {
            get; set;
        }

        /// <summary>
        /// 折扣降点
        /// </summary>
        [Field("折扣降点")]
        public string DOWN_RATE
        {
            get; set;
        }

        /// <summary>
        /// 折扣升点
        /// </summary>
        [Field("折扣升点")]
        public string UP_RATE
        {
            get; set;
        }

        /// <summary>
        /// 折扣上限
        /// </summary>
        [Field("折扣上限")]
        public string LIMIT_UP
        {
            get; set;
        }

        /// <summary>
        /// 折扣下限
        /// </summary>
        [Field("折扣下限")]
        public string LIMIT_DOWN
        {
            get; set;
        }


        /// <summary>
        /// 旧折扣率
        /// </summary>
        [Field("原扣率")]
        public string RATE_OLD
        {
            get; set;
        }
        /// <summary>
        /// 新折扣率
        /// </summary>
        [Field("新扣率")]
        public string RATE_NEW
        {
            get; set;
        }

    }
}
