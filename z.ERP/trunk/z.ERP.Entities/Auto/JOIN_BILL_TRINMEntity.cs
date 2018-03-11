/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:19:00
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("JOIN_BILL_TRINM", "联营结算单项目")]
    public partial class JOIN_BILL_TRINMEntity : EntityBase
    {
        public JOIN_BILL_TRINMEntity()
        {
        }

        public JOIN_BILL_TRINMEntity(string billid, string trimid, string inx)
        {
            BILLID = billid;
            TRIMID = trimid;
            INX = inx;
        }

        /// <summary>
        /// 单号
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 费用项目
        /// <summary>
        [PrimaryKey]
        [Field("费用项目")]
        public string TRIMID
        {
            get; set;
        }
        /// <summary>
        /// 序号
        /// <summary>
        [PrimaryKey]
        [Field("序号")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 金额
        /// <summary>
        [Field("金额")]
        public string JE
        {
            get; set;
        }
        /// <summary>
        /// 账单编号
        /// <summary>
        [Field("账单编号")]
        public string FINAL_BILLID
        {
            get; set;
        }
    }
}
