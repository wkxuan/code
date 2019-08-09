using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleListsFilter
    {
        public int saleType  //交易类型 0 全部 1 正交易 2 负交易
        {
            get;
            set;
        }

        public string posNo
        {
            get;
            set;
        }

        public int  payId  //0 全部 非0 收款方式id
        {
            get;
            set;
        }


        public double payAmount
        {
            get;
            set;
        }


        public string saleBeginDate
        {
            get;
            set;
        }
        public string saleEndDate
        {
            get;
            set;
        }

        public PageInfo pageInfo
        {
            get;
            set;
        }

    }
}
