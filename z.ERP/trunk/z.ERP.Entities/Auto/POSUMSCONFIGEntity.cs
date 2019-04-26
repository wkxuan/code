using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("POSUMSCONFIG", "POS银联支付配置")]
    public partial class POSUMSCONFIGEntity:TableEntityBase
    {
        public POSUMSCONFIGEntity() {

        }
        public POSUMSCONFIGEntity(string posno) {
            POSNO = posno;
        }
        [PrimaryKey]
        [Field("终端号")]
        public string POSNO
        {
            get; set;
        }
        [Field("IP")]
        public string IP
        {
            get; set;
        }
        [Field("备用IP")]
        public string IP_BAK
        {
            get; set;
        }
        [Field("端口")]
        public string PORT
        {
            get; set;
        }
        [Field("银联实体卡商户号")]
        public string CFX_MCHTID
        {
            get; set;
        }
        [Field("银联实体卡终端号")]
        public string CFX_TERMID
        {
            get; set;
        }
        [Field("银行扫码商户名称")]
        public string CFXMPAY_MCHTNAME
        {
            get; set;
        }
        [Field("银行扫码商户号")]
        public string CFXMPAY_MCHTID
        {
            get; set;
        }
        [Field("银行扫码终端号")]
        public string CFXMPAY_TERMID
        {
            get; set;
        }
    }
}
