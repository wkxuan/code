using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class DealResult
    {
        public string sale_time
        {
            get; set;
        }
        public string account_date
        {
            get; set;
        }
        public string cashierid
        {
            get; set;
        }
        public string sale_amount
        {
            get; set;
        }
        public string change_amount
        {
            get; set;
        }
        public string member_type
        {
            get; set;
        }
        public string manage_card
        {
            get; set;
        }
        public string posno_old
        {
            get; set;
        }
        public string dealid_old
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
        public List<ClerkResultt> clerklist
        {
            get; set;
        }

    }
}
