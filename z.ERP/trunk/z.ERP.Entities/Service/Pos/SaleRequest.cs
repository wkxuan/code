using System;
using System.Collections.Generic;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleRequest
    {
        public string posno
        {
            get; set;
        }

        public int dealid
        {
            get; set;
        }
        public DateTime sale_time
        {
            get; set;
        }
        public DateTime account_date
        {
            get; set;
        }
        public int cashierid
        {
            get; set;
        }
        public decimal sale_amount    
        {
            get; set;
        }
        public decimal change_amount
        {
            get; set;
        }
        public string member_cardid
        {
            get; set;
        }
        public int crm_recordid
        {
            get; set;
        }
        public string posno_old
        {
            get; set;
        }
        public int? dealid_old
        {
            get; set;
        }
        public List<GoodsResult> goodslist
        {
            get; set;
        }
        public List<PayResult> paylist
        {
            get; set;
        }
        public List<ClerkResult> clerklist
        {
            get; set;
        }
        public List<PayRecord> payRecord
        {
            get; set;
        }


    }
}
