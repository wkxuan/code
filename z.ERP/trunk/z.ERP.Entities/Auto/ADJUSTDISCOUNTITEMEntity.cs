using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("ADJUSTDISCOUNTITEM", "折扣调整子表")]
    public partial class ADJUSTDISCOUNTITEMEntity:TableEntityBase
    {
        public ADJUSTDISCOUNTITEMEntity() {
        }
        public ADJUSTDISCOUNTITEMEntity(string adid,string phase,string goodsid) {
            ADID = adid;
            PHASE = phase;
            GOODSID = goodsid;
        }
        /// <summary>
        /// 主键id
        /// </summary>
        [PrimaryKey]
        [Field("ADID")]
        public string ADID
        {
            get; set;
        }
        /// <summary>
        /// 分单号预留
        /// </summary>
        [Field("分单号（预留）")]
        public string PHASE
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
        /// 折扣下限
        /// </summary>
        [Field("折扣下限")]
        public string DISCOUNT_LOWER
        {
            get; set;
        }
        /// <summary>
        /// 折扣上限
        /// </summary>
        [Field("折扣上限")]
        public string DISCOUNT_CEILING
        {
            get; set;
        }
        /// <summary>
        /// 折扣降点
        /// </summary>
        [Field("折扣降点")]
        public string DISCOUNT_DROP_POINTS
        {
            get; set;
        }
        /// <summary>
        /// 旧折扣率
        /// </summary>
        [Field("旧折扣率")]
        public string OLD_DISCOUNT
        {
            get; set;
        }
        /// <summary>
        /// 新折扣率
        /// </summary>
        [Field("新折扣率")]
        public string NEW_DISCOUNT
        {
            get; set;
        }
        /// <summary>
        /// 折扣升点
        /// </summary>
        [Field("折扣升点")]
        public string DISCOUNT_RISE_POINTS
        {
            get; set;
        }
    }
}
