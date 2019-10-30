using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities
{
    public class CONTRACTOUTPUTEntity
    {
        ///合同号
        public string CONTRACTID { set; get; }
        //门店名称
        public string BRANCHNAME { set; get; }
        //门店名称1
        public string BRANCHNAME1 { set; get; }
        //商户名称
        public string MERCHANTNAME { set; get; }
        //门店地址
        public string BRANCHADDRESS { set; get; }
        //租赁商铺号
        public string SHOPCODE { set; get; }
        //租赁面积
        public string AREAR { set; get; }
        //末级业态
        public string CATEGORYNAME { set; get; }
        //品牌名称
        public string BRANDNAME { set; get; }
        //品牌名称2
        public string BRANDNAME2 { set; get; }
        //合同开始
        public string CONT_START { set; get; }
        //合同结束
        public string CONT_END { set; get; }
        //免租天数
        public string FREEDAYS { set; get; }
        //免租开始日期
        public string FREE_BEGIN { set; get; }
        //免租结束日期
        public string FREE_END { set; get; }
        //合同开始日期1
        public string CONT_START1 { set; get; }
        //合同结束日期1
        public string CONT_END1 { set; get; }
        //日租金 单价
        public string RZJ_PRICE { set; get; }
        //物业费 单价
        public string WYF_PRICE { set; get; }
        //租赁天数
        public string CONT_DAYS { set; get; }
        //租金合计
        public string SUMRENTS { set; get; }
        //物业费合计
        public string SUMWYF { set; get; }
        //物业费，租金合计
        public string SUMWYF_RENTS { set; get; }
        //支付规则
        public string FEERULE_RENT { set; get; }
        //保证金
        public string BZJAMOUNT { set; get; }
        ///保证金大写
        public string BZJAMOUNT_DX { set; get; }
    }
}
