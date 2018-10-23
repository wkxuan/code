using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class VOUCHER_PARAMEntity
    {
        /// <summary>
        /// 模板ID
        /// <summary>
        [Field("模板ID")]
        public Int32 VOUCHERID
        {
            get; set;
        }
        /// <summary>
        /// 分店ID
        /// <summary>
        [Field("分店ID")]
        public Int32 BRANCHID
        {
            get; set;
        }        
        /// <summary>
        /// 年月
        /// <summary>
        [Field("年月")]
        public Int32 CWNY
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("开始日期")]
        public DateTime DATE1
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("结束日期")]
        public DateTime DATE2
        {
            get; set;
        }
        /// <summary>
        /// 凭证日期
        /// <summary>
        [Field("凭证日期")]
        public DateTime PZRQ
        {
            get; set;
        }
    }
}
