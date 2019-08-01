using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("INVOICE", "发票管理")]
    public partial class InvoiceEntity:TableEntityBase
    {
        public InvoiceEntity(){
            
        }
        public InvoiceEntity(string InvoiceID) {
            INVOICEID = InvoiceID;
        }
        /// <summary>
        /// 发票id
        /// </summary>
        [PrimaryKey]
        public string INVOICEID {
            set;get;
        }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string INVOICENUMBER {
            set; get;
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string TYPE {
            set; get;
        }
        /// <summary>
        /// 商户id
        /// </summary>
        public string MERCHANTID {
            set;get;
        }
        /// <summary>
        /// 开票日期
        /// </summary>
        [DbType(DbType.DateTime)]
        public string INVOICEDATE {
            set;get;
        }
        /// <summary>
        /// 发票金额
        /// </summary>
        public string INVOICEAMOUNT {
            set; get;
        }
        /// <summary>
        /// 增值税金额
        /// </summary>
        public string VATAMOUNT {
            set; get;
        }
        /// <summary>
        /// 不含税金额
        /// </summary>
        public string NOVATAMOUNT {
            set; get;
        }
        /// <summary>
        /// 录入人id
        /// </summary>
        public string REPORTER {
            set; get;
        }
        /// <summary>
        /// 录入人姓名
        /// </summary>
        public string REPORTER_NAME
        {
            set; get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME { set; get; }
        /// <summary>
        /// 作废人id
        /// </summary>
        public string DISCARD
        {
            set; get;
        }
        /// <summary>
        /// 作废人姓名
        /// </summary>
        public string DISCARD_NAME
        {
            set; get;
        }
        /// <summary>
        /// 作废时间
        /// </summary>
        [DbType(DbType.DateTime)]
        public string DISCARD_TIME { set; get; }
        /// <summary>
        /// 门店
        /// </summary>
        public string BRANCHID
        {
            set; get;
        }
        /// <summary>
        ///状态
        /// </summary>
        public string STATUS
        {
            set; get;
        }
    }
}
