using System;
using System.Collections.Generic;
using System.Linq;
using z.ERP.API.PosServiceAPI;
using System.Text;

namespace z.ERP.Entities.Service.Pos
{
    public class TableInfo
    {
        string tblName;

        public string TblName
        {
            get { return tblName; }
            set { tblName = value; }
        }
        int recordCount;

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
    }

    public class ReportYYYDept
    {
        string bmdm;

        public string Bmdm
        {
            get { return bmdm; }
            set { bmdm = value; }
        }
        string dept_name;

        public string Dept_name
        {
            get { return dept_name; }
            set { dept_name = value; }
        }
        int xsje;

        public int Xsje
        {
            get { return xsje; }
            set { xsje = value; }
        }
    }
    #region  Goods //商品
    public class Goods
    {


        int iBJ_DBJF;//积分标记

        public int IBJ_DBJF
        {
            get { return iBJ_DBJF; }
            set { iBJ_DBJF = value; }
        }
        private int iGetSOSteG = -1;//vip折上折的标记

        public int IGetSOSteG
        {
            get { return iGetSOSteG; }
            set { iGetSOSteG = value; }
        }
        private double fzk_Rate;

        public double Fzk_Rate
        {
            get { return fzk_Rate; }
            set { fzk_Rate = value; }
        }

        private double limitCount;

        public double LimitCount
        {
            get { return limitCount; }
            set { limitCount = value; }
        }

        private int vipDisRule;//vip折扣规则（0 : 仅后台折扣　1：最大折　2：折上折）
        /// <summary>
        /// vip折扣规则（0 : 仅后台折扣　1：最大折　2：折上折）
        /// </summary>
        public int VipDisRule
        {
            get { return vipDisRule; }
            set { vipDisRule = value; }
        }
        private string ghdw;

        public string Ghdw
        {
            get { return ghdw; }
            set { ghdw = value; }
        }

        private string sbName;//

        public string SbName
        {
            get { return sbName; }
            set { sbName = value; }
        }


        private string personDeptidCode;

        public string PersonDeptidCode
        {
            get { return personDeptidCode; }
            set { personDeptidCode = value; }
        }

        private int personDeptid;

        public int PersonDeptid
        {
            get { return personDeptid; }
            set { personDeptid = value; }
        }
        private int zkjd;

        public int Zkjd
        {
            get { return zkjd; }
            set { zkjd = value; }
        }

        private int personId;

        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        private string personCode;

        public string PersonCode
        {
            get { return personCode; }
            set { personCode = value; }
        }

        private int discountType;

        public int DiscountType
        {
            get { return discountType; }
            set { discountType = value; }
        }

        private double vipDiscRate;

        public double VipDiscRate
        {
            get { return vipDiscRate; }
            set { vipDiscRate = value; }
        }

        private int vipDiscBillId;

        public int VipDiscBillId
        {
            get { return vipDiscBillId; }
            set { vipDiscBillId = value; }
        }
        int id;


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string code;


        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        string barCode;


        public string BarCode
        {
            get { return barCode; }
            set { barCode = value; }
        }

        string name;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        string unit;


        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        string classType;//商品分类


        public string ClassType
        {
            get { return classType; }
            set { classType = value; }
        }

        int logo;  //商品商标


        public int Logo
        {
            get { return logo; }
            set { logo = value; }
        }

        int deptId;


        public int DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }

        string deptCode;


        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }

        double price;


        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        int goodsType; //商品类型


        public int GoodsType
        {
            get { return goodsType; }
            set { goodsType = value; }
        }

        double minPrice;


        public double MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }

        double vipPrice;


        public double VipPrice
        {
            get { return vipPrice; }
            set { vipPrice = value; }
        }

        int status;


        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        int ctrlNegSell;


        public int CtrlNegSell
        {
            get { return ctrlNegSell; }
            set { ctrlNegSell = value; }
        }

        double negSell;


        public double NegSell
        {
            get { return negSell; }
            set { negSell = value; }
        }

        bool canDecimal;


        public bool CanDecimal
        {
            get { return canDecimal; }
            set { canDecimal = value; }
        }

        int contractId;


        public int ContractId
        {
            get { return contractId; }
            set { contractId = value; }
        }

        bool packaged;


        public bool Packaged
        {
            get { return packaged; }
            set { packaged = value; }
        }

        double saleCount;


        public double SaleCount
        {
            get { return saleCount; }
            set { saleCount = value; }
        }

        double saleMoney;


        public double SaleMoney
        {
            get { return saleMoney; }
            set { saleMoney = value; }
        }
        //assistantId

        int assistantId;
        public int AssistantId
        {
            get { return assistantId; }
            set { assistantId = value; }
        }


        double discount;
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        double preferentialMoney;//优惠金额


        public double PreferentialMoney
        {
            get { return preferentialMoney; }
            set { preferentialMoney = value; }
        }

        double frontDiscount;


        public double FrontDiscount
        {
            get { return frontDiscount; }
            set { frontDiscount = value; }
        }

        double backDiscount;


        public double BackDiscount
        {
            get { return backDiscount; }
            set { backDiscount = value; }
        }

        decimal backRate;
        public decimal BackRate
        {
            get { return backRate; }
            set { backRate = value; }
        }

        double memberDiscount;
        public double MemberDiscount
        {
            get { return memberDiscount; }
            set { memberDiscount = value; }
        }


        double decreaseDiscount;//满百减折

        public double DecreaseDiscount
        {
            get { return decreaseDiscount; }
            set { decreaseDiscount = value; }
        }

        int bjsMbje; //满减折扣2
        public int BjsMbje
        {
            get { return bjsMbje; }
            set { bjsMbje = value; }
        }

        double changeDiscount; //找零折扣


        public double ChangeDiscount
        {
            get { return changeDiscount; }
            set { changeDiscount = value; }
        }

        double discoaddDiscount; //折上折扣


        public double DiscoaddDiscount
        {
            get { return discoaddDiscount; }
            set { discoaddDiscount = value; }
        }


        int gonghsDiscount; //供货商折扣


        public int GonghsDiscount
        {
            get { return gonghsDiscount; }
            set { gonghsDiscount = value; }
        }

        int jiaslbDiscount; //加水量变折扣


        public int JiaslbDiscount
        {
            get { return jiaslbDiscount; }
            set { jiaslbDiscount = value; }
        }



        int decreasePreferential;//满百优惠


        public int DecreasePreferential
        {
            get { return decreasePreferential; }
            set { decreasePreferential = value; }
        }



        int shopId;


        public int ShopId
        {
            get { return shopId; }
            set { shopId = value; }
        }

        int discountBillId;


        public int DiscountBillId
        {
            get { return discountBillId; }
            set { discountBillId = value; }
        }

        int discountBillInx;


        public int DiscountBillInx
        {
            get { return discountBillInx; }
            set { discountBillInx = value; }
        }


        double discountCount;


        public double DiscountCount
        {
            get { return discountCount; }
            set { discountCount = value; }
        }


        //string MemberOffRate

        double memberOffRate;
        public double MemberOffRate
        {
            get { return memberOffRate; }
            set { memberOffRate = value; }
        }

        int iHsfs;


        public int IHsfs
        {
            get { return iHsfs; }
            set { iHsfs = value; }
        }


        bool bLimitSell;


        public bool BLimitSell
        {
            get { return bLimitSell; }
            set { bLimitSell = value; }
        }


        int iRefNo_ZK;


        public int IRefNo_ZK
        {
            get { return iRefNo_ZK; }
            set { iRefNo_ZK = value; }
        }

        int iRefNo_MJ;


        public int IRefNo_MJ
        {
            get { return iRefNo_MJ; }
            set { iRefNo_MJ = value; }
        }


        int iRefNo_HY;


        public int IRefNo_HY
        {
            get { return iRefNo_HY; }
            set { iRefNo_HY = value; }
        }

        int iRefNo_JS;


        public int IRefNo_JS
        {
            get { return iRefNo_JS; }
            set { iRefNo_JS = value; }
        }


        int crmInx;


        public int CrmInx
        {
            get { return crmInx; }
            set { crmInx = value; }
        }


        int mobileBillId;



        public int MobileBillId
        {
            get { return mobileBillId; }
            set { mobileBillId = value; }
        }


        int mobileGoodsInx;



        public int MobileGoodsInx
        {
            get { return mobileGoodsInx; }
            set { mobileGoodsInx = value; }
        }

        int subTicktInx;


        public int SubTicktInx
        {
            get { return subTicktInx; }
            set { subTicktInx = value; }
        }


        int subGoodsInx;


        public int SubGoodsInx
        {
            get { return subGoodsInx; }
            set { subGoodsInx = value; }
        }



        int subTicktInx_old;


        public int SubTicktInx_old
        {
            get { return subTicktInx_old; }
            set { subTicktInx_old = value; }
        }


        int subGoodsInx_old;


        public int SubGoodsInx_old
        {
            get { return subGoodsInx_old; }
            set { subGoodsInx_old = value; }
        }


        string oTherStr1;


        public string OTherStr1
        {
            get { return oTherStr1; }
            set { oTherStr1 = value; }
        }

        double oTherInt1;

        public double OTherInt1
        {
            get { return oTherInt1; }
            set { oTherInt1 = value; }
        }

        //2018.04.23
        int fkcxsbj;

        public int Fkcxsbj
        {
            get { return fkcxsbj; }
            set { fkcxsbj = value; }
        }

        //2018.04.23
        int vipzkgz;

        public int Vipzkgz
        {
            get { return vipzkgz; }
            set { vipzkgz = value; }
        }

        //单据ID
        int decreaseBillId;
        public int DecreaseBillId
        {
            get { return decreaseBillId; }
            set { decreaseBillId = value; }
        }
        //规则ID  
        int decreaseRuleId;
        public int DecreaseRuleId
        {
            get { return decreaseRuleId; }
            set { decreaseRuleId = value; }
        }

        List<CXJSLBFA_Item> cXJSLBFA = new List<CXJSLBFA_Item>();

        public List<CXJSLBFA_Item> CXJSLBFA
        {
            get { return cXJSLBFA; }
            set { cXJSLBFA = value; }
        }

        BackDiscountBill backDiscountBill = new BackDiscountBill();

        public BackDiscountBill BackDiscountBill
        {
            get { return backDiscountBill; }
            set { backDiscountBill = value; }
        }





    }
    #endregion

    public class SubTicket//子小票
    {
        int subTicktInx;


        public int SubTicktInx
        {
            get { return subTicktInx; }
            set { subTicktInx = value; }
        }

        int personId;


        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        string personCode;

        public string PersonCode
        {
            get { return personCode; }
            set { personCode = value; }
        }
        int personDeptid;


        public int PersonDeptid
        {
            get { return personDeptid; }
            set { personDeptid = value; }
        }
        string deptCode;

        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }

        int mobileBillId;

        public int MobileBillId
        {
            get { return mobileBillId; }
            set { mobileBillId = value; }
        }

        List<Goods> goods;

        public List<Goods> Goods
        {
            get { return goods; }
            set { goods = value; }
        }

    }



    public class ConfigInfo
    {
        string describe;

        public string Describe
        {
            get { return describe; }
            set { describe = value; }
        }

        string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }//需要
     //beforeSaveData
    public class CrmBeforeSaveData
    {

        private int serverBillId;

        public int ServerBillId
        {
            get { return serverBillId; }
            set { serverBillId = value; }
        }
        private List<RSaleBillPayment> paymentList;

        public List<RSaleBillPayment> PaymentList
        {
            get { return paymentList; }
            set { paymentList = value; }
        }
        private int payBackCouponVipId;

        public int PayBackCouponVipId
        {
            get { return payBackCouponVipId; }
            set { payBackCouponVipId = value; }
        }
        private bool couponPaid;

        public bool CouponPaid
        {
            get { return couponPaid; }
            set { couponPaid = value; }
        }


    }
    //上传商品 andbeforesave and savetodatabase 返回的参数（）


    public class PosInfo
    {
        string id;


        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        int discountControlType;


        public int DiscountControlType
        {
            get { return discountControlType; }
            set { discountControlType = value; }
        }

        int backGoodsControlType;


        public int BackGoodsControlType
        {
            get { return backGoodsControlType; }
            set { backGoodsControlType = value; }
        }

        bool reportControlType;


        public bool ReportControlType
        {
            get { return reportControlType; }
            set { reportControlType = value; }
        }

        bool dayReportControlType;


        public bool DayReportControlType
        {
            get { return dayReportControlType; }
            set { dayReportControlType = value; }
        }

        string supermarketCode;


        public string SupermarketCode
        {
            get { return supermarketCode; }
            set { supermarketCode = value; }
        }

        int supermarketId;


        public int SupermarketId
        {
            get { return supermarketId; }
            set { supermarketId = value; }
        }

        bool useMarketPrice;


        public bool UseMarketPrice
        {
            get { return useMarketPrice; }
            set { useMarketPrice = value; }
        }

        int posserverId;


        public int PosserverId
        {
            get { return posserverId; }
            set { posserverId = value; }
        }



        int defaultPaymoneyId;

        public int DefaultPaymoneyId
        {
            get { return defaultPaymoneyId; }
            set { defaultPaymoneyId = value; }
        }

        string oTherStr1;


        public string OTherStr1
        {
            get { return oTherStr1; }
            set { oTherStr1 = value; }
        }

        string oTherStr2;


        public string OTherStr2
        {
            get { return oTherStr2; }
            set { oTherStr2 = value; }
        }


        string oTherStr3;


        public string OTherStr3
        {
            get { return oTherStr3; }
            set { oTherStr3 = value; }
        }


        string oTherStr4;


        public string OTherStr4
        {
            get { return oTherStr4; }
            set { oTherStr4 = value; }
        }

        string oTherStr5;


        public string OTherStr5
        {
            get { return oTherStr5; }
            set { oTherStr5 = value; }
        }


        int oTherInt1;


        public int OTherInt1
        {
            get { return oTherInt1; }
            set { oTherInt1 = value; }
        }

        int oTherInt2;


        public int OTherInt2
        {
            get { return oTherInt2; }
            set { oTherInt2 = value; }
        }


        int oTherInt3;


        public int OTherInt3
        {
            get { return oTherInt3; }
            set { oTherInt3 = value; }
        }

        int oTherInt4;


        public int OTherInt4
        {
            get { return oTherInt4; }
            set { oTherInt4 = value; }
        }

        int oTherInt5;


        public int OTherInt5
        {
            get { return oTherInt5; }
            set { oTherInt5 = value; }
        }


    }//需要


    public class CrmConfig
    {
        string machine;


        public string Machine
        {
            get { return machine; }
            set { machine = value; }
        }

        string store;


        public string Store
        {
            get { return store; }
            set { store = value; }
        }

        string serverIp;


        public string ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }

        int serverPort;


        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }

        string userName;


        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        string password;


        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        string sURL;


        public string SURL
        {
            get { return sURL; }
            set { sURL = value; }
        }

        string sCZKURL;


        public string SCZKURL
        {
            get { return sCZKURL; }
            set { sCZKURL = value; }
        }
    }//需要

    /// <summary>
    /// 员工
    /// </summary>
    public class Staff
    {
        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int deptId;
        public int DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }

        string deptCode;
        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }
        string deptName;
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }

    }//需要


    public class Payment//需要
    {
        int id;//code


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string name;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        int paymentType;


        public int PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }

        int couponId;//


        public int CouponId//
        {
            get { return couponId; }
            set { couponId = value; }
        }

        int changeType;//找零方式

        public int ChangeType
        {
            get { return changeType; }
            set { changeType = value; }
        }

        double payedMoney;//


        public double PayedMoney
        {
            get { return payedMoney; }
            set { payedMoney = value; }
        }
        int yhjid;

        public int Yhjid//优惠券id
        {
            get { return yhjid; }
            set { yhjid = value; }
        }
        int zlclfs;//找零处理方式

        public int Zlclfs
        {
            get { return zlclfs; }
            set { zlclfs = value; }
        }
        int xssx;//显示顺序

        public int Xssx
        {
            get { return xssx; }
            set { xssx = value; }
        }
        int bj_jf;//积分标记

        public int Bj_jf
        {
            get { return bj_jf; }
            set { bj_jf = value; }
        }

        double morePayedMoney;//

        public double MorePayedMoney
        {
            get { return morePayedMoney; }
            set { morePayedMoney = value; }
        }



        double realUsedMoney;

        public double RealUsedMoney
        {
            get { return realUsedMoney; }
            set { realUsedMoney = value; }
        }


        bool directInput;  //是否允许在收款界面，直接输入


        public bool DirectInput
        {
            get { return directInput; }
            set { directInput = value; }
        }

        private bool point;  //是否积分


        public bool IsPoint
        {
            get { return point; }
            set { point = value; }
        }

        private bool offerCoupon;  //是否返券


        public bool IsOfferCoupon
        {
            get { return offerCoupon; }
            set { offerCoupon = value; }
        }

        private bool decreaseDiscount;   //满百减折


        public bool IsDecreaseDiscount
        {
            get { return decreaseDiscount; }
            set { decreaseDiscount = value; }
        }
        private int iMainID;

        public int IMainID
        {
            get { return iMainID; }
            set { iMainID = value; }
        }
        private int iMainBJ;

        public int IMainBJ
        {
            get { return iMainBJ; }
            set { iMainBJ = value; }
        }
        private String sMainMC;

        public String SMainMC
        {
            get { return sMainMC; }
            set { sMainMC = value; }
        }

        private String typeCode;
        public String TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; }
        }

        private String typeName;
        public String TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

    }
    public class Main_Skfs
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int iMainID;

        public int IMainID
        {
            get { return iMainID; }
            set { iMainID = value; }
        }
        private int iMainBJ;

        public int IMainBJ
        {
            get { return iMainBJ; }
            set { iMainBJ = value; }
        }
        private String sMainMC;

        public String SMainMC
        {
            get { return sMainMC; }
            set { sMainMC = value; }
        }
    }
    public class LoadConfig
    {
        private int fdbh;

        public int Fdbh
        {
            get { return fdbh; }
            set { fdbh = value; }
        }
        private string bmdm;

        public string Bmdm
        {
            get { return bmdm; }
            set { bmdm = value; }
        }
        List<Payment> skfs = new List<Payment>();
        List<ConfigInfo> configs = new List<ConfigInfo>();
        List<Main_Skfs> main_skfs = new List<Main_Skfs>();

        public List<Main_Skfs> Main_skfs
        {
            get { return main_skfs; }
            set { main_skfs = value; }
        }
        string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        string storeCode;

        public string StoreCode
        {
            get { return storeCode; }
            set { storeCode = value; }
        }
        string company;

        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        int storeId;

        public int StoreId
        {
            get { return storeId; }
            set { storeId = value; }
        }


        public List<ConfigInfo> Configs
        {
            get { return configs; }
            set { configs = value; }
        }
        public List<Payment> Skfs
        {
            get { return skfs; }
            set { skfs = value; }
        }


        int person_id;

        public int Person_id
        {
            get { return person_id; }
            set { person_id = value; }
        }
        string person_rydm;

        public string Person_rydm
        {
            get { return person_rydm; }
            set { person_rydm = value; }
        }
        string person_name;

        public string Person_name
        {
            get { return person_name; }
            set { person_name = value; }
        }
        int dept_id;

        public int Dept_id
        {
            get { return dept_id; }
            set { dept_id = value; }
        }
        string dept_name;

        public string Dept_name
        {
            get { return dept_name; }
            set { dept_name = value; }
        }
        string dept_bmdm;

        public string Dept_bmdm
        {
            get { return dept_bmdm; }
            set { dept_bmdm = value; }
        }

        int skt_can_gz;

        public int Skt_can_gz
        {
            get { return skt_can_gz; }
            set { skt_can_gz = value; }
        }
        int skt_can_bar;

        public int Skt_can_bar
        {
            get { return skt_can_bar; }
            set { skt_can_bar = value; }
        }
        int skt_can_zk;

        public int Skt_can_zk
        {
            get { return skt_can_zk; }
            set { skt_can_zk = value; }
        }
        int skt_can_th;

        public int Skt_can_th
        {
            get { return skt_can_th; }
            set { skt_can_th = value; }
        }
        int skt_can_bb;

        public int Skt_can_bb
        {
            get { return skt_can_bb; }
            set { skt_can_bb = value; }
        }
        int skt_qsfs;

        public int Skt_qsfs
        {
            get { return skt_qsfs; }
            set { skt_qsfs = value; }
        }
        int skt_csmcdm;

        public int Skt_csmcdm
        {
            get { return skt_csmcdm; }
            set { skt_csmcdm = value; }
        }

        int jlbh;

        public int Jlbh
        {
            get { return jlbh; }
            set { jlbh = value; }
        }
        string sktno;

        public string Sktno
        {
            get { return sktno; }
            set { sktno = value; }
        }

    }

    public class Pad
    {

        List<string> configs = new List<string>();

        public List<string> Configs
        {
            get { return configs; }
            set { configs = value; }
        }
        string padnum;

        public string Padnum
        {
            get { return padnum; }
            set { padnum = value; }

        }
        string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        string mac;

        public string Mac
        {
            get { return mac; }
            set { mac = value; }
        }
        string rydm;

        public string Rydm
        {
            get { return rydm; }
            set { rydm = value; }
        }
        string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        //string address;
        //string port;
    }


    public class BackGoods
    {
        int id;


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        int price;


        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        int deptId;


        public int DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }

        double saleCount;


        public double SaleCount
        {
            get { return saleCount; }
            set { saleCount = value; }
        }

        int saleMoney;


        public int SaleMoney
        {
            get { return saleMoney; }
            set { saleMoney = value; }
        }

        int discount;


        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        int preferentialMoney;//优惠金额


        public int PreferentialMoney
        {
            get { return preferentialMoney; }
            set { preferentialMoney = value; }
        }

        int frontDiscount;


        public int FrontDiscount
        {
            get { return frontDiscount; }
            set { frontDiscount = value; }
        }

        int backDiscount;


        public int BackDiscount
        {
            get { return backDiscount; }
            set { backDiscount = value; }
        }

        int memberDiscount;


        public int MemberDiscount
        {
            get { return memberDiscount; }
            set { memberDiscount = value; }
        }

        int decreaseDiscount;//满百减


        public int DecreaseDiscount
        {
            get { return decreaseDiscount; }
            set { decreaseDiscount = value; }
        }

        int changeDiscount; //找零折扣


        public int ChangeDiscount
        {
            get { return changeDiscount; }
            set { changeDiscount = value; }
        }

        int discoaddDiscount; //折上折扣


        public int DiscoaddDiscount
        {
            get { return discoaddDiscount; }
            set { discoaddDiscount = value; }
        }


        int gonghsDiscount; //供货商折扣


        public int GonghsDiscount
        {
            get { return gonghsDiscount; }
            set { gonghsDiscount = value; }
        }

        int jiaslbDiscount; //加水量变折扣


        public int JiaslbDiscount
        {
            get { return jiaslbDiscount; }
            set { jiaslbDiscount = value; }
        }


        int personId;


        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        double backCount;


        public double BackCount
        {
            get { return backCount; }
            set { backCount = value; }
        }

        string barCode;


        public string BarCode
        {
            get { return barCode; }
            set { barCode = value; }
        }


        int decreasePreferential;//满百优惠


        public int DecreasePreferential
        {
            get { return decreasePreferential; }
            set { decreasePreferential = value; }
        }


        int iZKDBH;


        public int IZKDBH
        {
            get { return iZKDBH; }
            set { iZKDBH = value; }
        }


        int iZKDINX;


        public int IZKDINX
        {
            get { return iZKDINX; }
            set { iZKDINX = value; }
        }


        int iRefNo_ZK;


        public int IRefNo_ZK
        {
            get { return iRefNo_ZK; }
            set { iRefNo_ZK = value; }
        }

        int iRefNo_MJ;


        public int IRefNo_MJ
        {
            get { return iRefNo_MJ; }
            set { iRefNo_MJ = value; }
        }


        int iRefNo_HY;


        public int IRefNo_HY
        {
            get { return iRefNo_HY; }
            set { iRefNo_HY = value; }
        }

        int iRefNo_JS;


        public int IRefNo_JS
        {
            get { return iRefNo_JS; }
            set { iRefNo_JS = value; }
        }


        int crmInx;


        public int CrmInx
        {
            get { return crmInx; }
            set { crmInx = value; }
        }


        int subTicktInx_old;


        public int SubTicktInx_old
        {
            get { return subTicktInx_old; }
            set { subTicktInx_old = value; }
        }


        int subGoodsInx_old;


        public int SubGoodsInx_old
        {
            get { return subGoodsInx_old; }
            set { subGoodsInx_old = value; }
        }


        string oTherStr1;


        public string OTherStr1
        {
            get { return oTherStr1; }
            set { oTherStr1 = value; }
        }

        string oTherStr2;


        public string OTherStr2
        {
            get { return oTherStr2; }
            set { oTherStr2 = value; }
        }

        string oTherStr3;


        public string OTherStr3
        {
            get { return oTherStr3; }
            set { oTherStr3 = value; }
        }

        int oTherInt1;


        public int OTherInt1
        {
            get { return oTherInt1; }
            set { oTherInt1 = value; }
        }

        int oTherInt2;


        public int OTherInt2
        {
            get { return oTherInt2; }
            set { oTherInt2 = value; }
        }

        int oTherInt3;


        public int OTherInt3
        {
            get { return oTherInt3; }
            set { oTherInt3 = value; }
        }


    }//需要

    public class BackDiscountBill
    {

        int billId;


        public int BillId
        {
            get { return billId; }
            set { billId = value; }
        }

        int inx;


        public int Inx
        {
            get { return inx; }
            set { inx = value; }
        }

        int iREFNO;


        public int IREFNO
        {
            get { return iREFNO; }
            set { iREFNO = value; }
        }

        double limitCount;


        public double LimitCount
        {
            get { return limitCount; }
            set { limitCount = value; }
        }

        double discountedCount;


        public double DiscountedCount
        {
            get { return discountedCount; }
            set { discountedCount = value; }
        }

        double discountRate;


        public double DiscountRate
        {
            get { return discountRate; }
            set { discountRate = value; }
        }

        int discountMoney;


        public int DiscountMoney
        {
            get { return discountMoney; }
            set { discountMoney = value; }
        }

        int vipDiscountType;


        public int VipDiscountType
        {
            get { return vipDiscountType; }
            set { vipDiscountType = value; }
        }

        int dicountPrecision;


        public int DicountPrecision
        {
            get { return dicountPrecision; }
            set { dicountPrecision = value; }
        }


    }//需要
    public class Member
    {
        private float stageCent;

        public float StageCent
        {
            get { return stageCent; }
            set { stageCent = value; }
        }

        private float validCent;

        public float ValidCent
        {
            get { return validCent; }
            set { validCent = value; }
        }
        private string memberName;

        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }



        private string cardTypeName;

        public string CardTypeName
        {
            get { return cardTypeName; }
            set { cardTypeName = value; }
        }

        int memberId;


        public int MemberId
        {
            get { return memberId; }
            set { memberId = value; }
        }

        int memberType;


        public int MemberType
        {
            get { return memberType; }
            set { memberType = value; }
        }

        string memberNo;


        public string MemberNo
        {
            get { return memberNo; }
            set { memberNo = value; }
        }

        int typeLevel;
        public int TypeLevel
        {
            get { return typeLevel; }
            set { typeLevel = value; }
        }

        string wxOpenId;
        public string WxOpenId
        {
            get { return wxOpenId; }
            set { wxOpenId = value; }
        }


        //typeLevel; 
    }//会员
    public class CashCardItem
    {
        private int sUser;

        public int SUser
        {
            get { return sUser; }
            set { sUser = value; }
        }
        private String sCardNo;

        public String SCardNo
        {
            get { return sCardNo; }
            set { sCardNo = value; }
        }
        private int iCardID;

        public int ICardID
        {
            get { return iCardID; }
            set { iCardID = value; }
        }
        private int mMoney; //消费金额

        public int MMoney
        {
            get { return mMoney; }
            set { mMoney = value; }
        }
        private int mLeft; //储值卡余额

        public int MLeft
        {
            get { return mLeft; }
            set { mLeft = value; }
        }
        private int mYXTZ; //有效透支金额

        public int MYXTZ
        {
            get { return mYXTZ; }
            set { mYXTZ = value; }
        }
        private int mPD; //铺底金额

        public int MPD
        {
            get { return mPD; }
            set { mPD = value; }
        }
        private String sdateYXQ; //有效期

        public String SdateYXQ
        {
            get { return sdateYXQ; }
            set { sdateYXQ = value; }
        }
        private int iJYH;

        public int IJYH
        {
            get { return iJYH; }
            set { iJYH = value; }
        }
    }

    /*

    public class TicketInfo
    {
        private List<CashCardItem> cashCardList;

        public List<CashCardItem> CashCardList
        {
            get { return cashCardList; }
            set { cashCardList = value; }
        }

        string oldSkyName;

        public string OldSkyName
        {
            get { return oldSkyName; }
            set { oldSkyName = value; }
        }
        string oldJysj;

        public string OldJysj
        {
            get { return oldJysj; }
            set { oldJysj = value; }
        }

        List<Payment> payMents;

        public List<Payment> PayMents
        {
            get { return payMents; }
            set { payMents = value; }
        }
        List<SubTicket> subTickets;
        public List<SubTicket> SubTickets
        {
            get { return subTickets; }
            set { subTickets = value; }
        }
        Member member;

        public Member Member
        {
            get { return member; }
            set { member = value; }
        }
        string posId;
        public string PosId
        {
            get { return posId; }
            set { posId = value; }
        }

        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        DateTime checkOutDate;
        public DateTime CheckOutDate
        {
            get { return checkOutDate; }
            set { checkOutDate = value; }
        }

        int personId;


        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        string personCode;


        public string PersonCode
        {
            get { return personCode; }
            set { personCode = value; }
        }

        string mangerCardNo;


        public string MangerCardNo
        {
            get { return mangerCardNo; }
            set { mangerCardNo = value; }
        }

        double totalMoney;
        public double TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; }
        }
        public double getTotalMoney()
        {
            double totalM = 0;

            for (int i = 0; i < this.SubTickets.Count; i++)
            {
                for (int j = 0; j < this.subTickets[i].Goods.Count; j++)
                {
                    totalM += (this.subTickets[i].Goods[j].SaleMoney - this.subTickets[i].Goods[j].Discount);
                }
            }
            return totalM;
        }
        public double GetGoodsCount()
        {
            double count = 0;
            foreach (SubTicket subTic in subTickets)
            {
                foreach (Goods g in subTic.Goods) count += g.SaleCount;
            }
            return count;
        }
        int change;
        public int Change
        {
            get { return change; }
            set { change = value; }
        }

        int crmBillId;
        public int CrmBillId
        {
            get { return crmBillId; }
            set { crmBillId = value; }
        }
        int iTHFHR;
        public int ITHFHR
        {
            get { return iTHFHR; }
            set { iTHFHR = value; }
        }
        string oldPosId;
        public string OldPosId
        {
            get { return oldPosId; }
            set { oldPosId = value; }
        }
        int oldId;
        public int OldId
        {
            get { return oldId; }
            set { oldId = value; }
        }

    }//小票    


    */


    public class CrmGetDMZFListPacketed
    {
        //    List<CrmCodedCouponPayment> paymentList = new List<CrmCodedCouponPayment>();

        //    public List<CrmCodedCouponPayment> PaymentList
        //    {
        //        get { return paymentList; }
        //        set { paymentList = value; }
        //    }
        //    CrmStoreInfo stireInfo;

        //    public CrmStoreInfo StireInfo
        //    {
        //        get { return stireInfo; }
        //        set { stireInfo = value; }
        //    }
        //    string cashier;

        //    public string Cashier
        //    {
        //        get { return cashier; }
        //        set { cashier = value; }
        //    }
        //    string dateTime;

        //    public string DateTime 
        //    {
        //        get { return dateTime; }
        //        set { dateTime = value; }
        //    }

        //}
        //public class CrmGetMemberGrantVoucherpacketed
        //{
        //    int serverBillId;

        //    public int ServerBillId
        //    {
        //        get { return serverBillId; }
        //        set { serverBillId = value; }
        //    }


        //    int condType;

        //    public int CondType
        //    {
        //        get { return condType; }
        //        set { condType = value; }
        //    }
        //    string condValue;

        //    public string CondValue
        //    {
        //        get { return condValue; }
        //        set { condValue = value; }
        //    }
        //    string cardCodeToCheck;

        //    public string CardCodeToCheck
        //    {
        //        get { return cardCodeToCheck; }
        //        set { cardCodeToCheck = value; }
        //    }
        //    string verifyCode;

        //    public string VerifyCode
        //    {
        //        get { return verifyCode; }
        //        set { verifyCode = value; }
        //    }

    }
    public class CrmReturnParamFromCheckOut
    {
        private double billCentcheckout;

        public double BillCentcheckout
        {
            get { return billCentcheckout; }
            set { billCentcheckout = value; }
        }
        private double vipCent;

        public double VipCent
        {
            get { return vipCent; }
            set { vipCent = value; }
        }

        private List<SaleMoneyLeftWhenPromCalc> crmSaleMoneyLeftWhenPromCalc;

        public List<SaleMoneyLeftWhenPromCalc> CrmSaleMoneyLeftWhenPromCalc
        {
            get { return crmSaleMoneyLeftWhenPromCalc; }
            set { crmSaleMoneyLeftWhenPromCalc = value; }
        }

        private List<OfferCoupon> offerCouponListCheckout;

        public List<OfferCoupon> OfferCouponListCheckout
        {
            get { return offerCouponListCheckout; }
            set { offerCouponListCheckout = value; }
        }
        String offerCouponVipCode;

        public String OfferCouponVipCode
        {
            get { return offerCouponVipCode; }
            set { offerCouponVipCode = value; }
        }

    }

    public class ZFBResponse
    {
        string out_trade_no; //外部订单号

        public string Out_trade_no
        {
            get { return out_trade_no; }
            set { out_trade_no = value; }
        }

        string trade_no; //支付宝流水号

        public string Trade_no
        {
            get { return trade_no; }
            set { trade_no = value; }
        }

        string result; //请求是否成功

        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        string buyer_logon_id; //买家注册账号ID

        public string Buyer_logon_id
        {
            get { return buyer_logon_id; }
            set { buyer_logon_id = value; }
        }

        string buyer_user_id; //买家用户ID

        public string Buyer_user_id
        {
            get { return buyer_user_id; }
            set { buyer_user_id = value; }
        }

        string result_code; //交易状态码

        public string Result_code
        {
            get { return result_code; }
            set { result_code = value; }
        }

        string return_sign; //返回签名

        public string Return_sign
        {
            get { return return_sign; }
            set { return_sign = value; }
        }

        string error; //错误信息

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        string detail_error_des; //错误信息

        public string Detail_error_des
        {
            get { return detail_error_des; }
            set { detail_error_des = value; }
        }

        string trade_status; //交易状态

        public string Trade_status
        {
            get { return trade_status; }
            set { trade_status = value; }
        }

        string partner; //返回合作伙伴

        public string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        string retry_flag; //撤销是否可重复标记

        public string Retry_flag
        {
            get { return retry_flag; }
            set { retry_flag = value; }
        }

        string display_message; //退货显示信息

        public string Display_message
        {
            get { return display_message; }
            set { display_message = value; }
        }

        string fund_change; //退货是否有资金变动

        public string Fund_change
        {
            get { return fund_change; }
            set { fund_change = value; }
        }

        string qrstring; //二维码码串

        public string Qrstring
        {
            get { return qrstring; }
            set { qrstring = value; }
        }

        string bigurl; //二维码big

        public string Bigurl
        {
            get { return bigurl; }
            set { bigurl = value; }
        }

        string smallurl; //二维码small

        public string Smallurl
        {
            get { return smallurl; }
            set { smallurl = value; }
        }

        string url; //二维码url

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        string voucher_type; //优惠券

        public string Voucher_type
        {
            get { return voucher_type; }
            set { voucher_type = value; }
        }
    }

    public class ZFBRequest
    {
        string out_trade_no; //外部订单号

        public string Out_trade_no
        {
            get { return out_trade_no; }
            set { out_trade_no = value; }
        }

        string trade_no; //支付宝订单号

        public string Trade_no
        {
            get { return trade_no; }
            set { trade_no = value; }
        }

        string out_request_no; //退货子流水号

        public string Out_request_no
        {
            get { return out_request_no; }
            set { out_request_no = value; }
        }

        string refund_amount; //退款金额

        public string Refund_amount
        {
            get { return refund_amount; }
            set { refund_amount = value; }
        }

        string refund_reason; //退款原因

        public string Refund_reason
        {
            get { return refund_reason; }
            set { refund_reason = value; }
        }

        string total_fee; //金额

        public string Total_fee
        {
            get { return total_fee; }
            set { total_fee = value; }
        }

        string body; //订单描述

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        string operator_id; //操作员ID

        public string Operator_id
        {
            get { return operator_id; }
            set { operator_id = value; }
        }

        int dynamic_id_type; //动态ID类型 1：条码 2：二维码 3：声波

        public int Dynamic_id_type
        {
            get { return dynamic_id_type; }
            set { dynamic_id_type = value; }
        }

        string dynamic_id;

        public string Dynamic_id
        {
            get { return dynamic_id; }
            set { dynamic_id = value; }
        }

        string terminal_id; //posId

        public string Terminal_id
        {
            get { return terminal_id; }
            set { terminal_id = value; }
        }

        string store_id; //门店ID

        public string Store_id
        {
            get { return store_id; }
            set { store_id = value; }
        }

        public ZFBRequest(ZFBRequest zfbRequest_tmp)
        {
            if (zfbRequest_tmp.Out_trade_no != null)
                Out_trade_no = zfbRequest_tmp.Out_trade_no;
            if (zfbRequest_tmp.Trade_no != null)
                Trade_no = zfbRequest_tmp.Trade_no;
            if (zfbRequest_tmp.Out_request_no != null)
                Out_request_no = zfbRequest_tmp.Out_request_no;
            if (zfbRequest_tmp.Refund_amount != null)
                Refund_amount = zfbRequest_tmp.Refund_amount;
            if (zfbRequest_tmp.Refund_reason != null)
                Refund_reason = zfbRequest_tmp.Refund_reason;
            if (zfbRequest_tmp.Total_fee != null)
                Total_fee = zfbRequest_tmp.Total_fee;
            if (zfbRequest_tmp.Body != null)
                Body = zfbRequest_tmp.Body;
            if (zfbRequest_tmp.Operator_id != null)
                Operator_id = zfbRequest_tmp.Operator_id;
            Dynamic_id_type = zfbRequest_tmp.Dynamic_id_type;
            if (zfbRequest_tmp.Dynamic_id != null)
                Dynamic_id = zfbRequest_tmp.Dynamic_id;
            if (zfbRequest_tmp.Terminal_id != null)
                Terminal_id = zfbRequest_tmp.Terminal_id;
            if (zfbRequest_tmp.Store_id != null)
                Store_id = zfbRequest_tmp.Store_id;
        }
    }
    public class JlbhCashPayed
    {
        int iJlbh;


        public int IJlbh
        {
            get { return iJlbh; }
            set { iJlbh = value; }
        }

        int iSkfsid;


        public int ISkfsid
        {
            get { return iSkfsid; }
            set { iSkfsid = value; }
        }

        int mSkfsje;


        public int MSkfsje
        {
            get { return mSkfsje; }
            set { mSkfsje = value; }
        }


        int mSkfsyy;


        public int MSkfsyy
        {
            get { return mSkfsyy; }
            set { mSkfsyy = value; }
        }

        int iSkfslx;


        public int ISkfslx
        {
            get { return iSkfslx; }
            set { iSkfslx = value; }
        }


        string sSkfsmc;


        public string SSkfsmc
        {
            get { return sSkfsmc; }
            set { sSkfsmc = value; }
        }

    }
    public class CyDataTimeInfo
    {

        DateTime checkOutDate;


        public DateTime CheckOutDate
        {
            get { return checkOutDate; }
            set { checkOutDate = value; }
        }

        int cyId;


        public int CyId
        {
            get { return cyId; }
            set { cyId = value; }
        }


    }
    public class CFTDetail
    {
        private String sktno;

        public String Sktno
        {
            get { return sktno; }
            set { sktno = value; }
        }
        private string managerCardNo;

        public string ManagerCardNo
        {
            get { return managerCardNo; }
            set { managerCardNo = value; }
        }
        private String tradeNo;

        public String TradeNo
        {
            get { return tradeNo; }
            set { tradeNo = value; }
        }
        private String outTradeNo;

        public String OutTradeNo
        {
            get { return outTradeNo; }
            set { outTradeNo = value; }
        }
        private DateTime jYSJ;

        public DateTime JYSJ
        {
            get { return jYSJ; }
            set { jYSJ = value; }
        }
        private DateTime jYRQ;

        public DateTime JYRQ
        {
            get { return jYRQ; }
            set { jYRQ = value; }
        }
        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private int jE;

        public int JE
        {
            get { return jE; }
            set { jE = value; }
        }
        private int sJJE;

        public int SJJE
        {
            get { return sJJE; }
            set { sJJE = value; }
        }
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        private string gN;

        public string GN
        {
            get { return gN; }
            set { gN = value; }
        }
    }


    public class GYDetail
    {
        private String sktno;

        public String Sktno
        {
            get { return sktno; }
            set { sktno = value; }
        }
        private int tradeNo;

        public int TradeNo
        {
            get { return tradeNo; }
            set { tradeNo = value; }
        }
        private String outTradeNo;

        public String OutTradeNo
        {
            get { return outTradeNo; }
            set { outTradeNo = value; }
        }
        private DateTime jYSJ;

        public DateTime JYSJ
        {
            get { return jYSJ; }
            set { jYSJ = value; }
        }
        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        private List<Payment> payments;


        public List<Payment> Payments
        {
            get { return payments; }
            set { payments = value; }
        }
    }

    public class ZFBDetail
    {
        private String sktno;

        public String Sktno
        {
            get { return sktno; }
            set { sktno = value; }
        }
        private string managerCardNo;

        public string ManagerCardNo
        {
            get { return managerCardNo; }
            set { managerCardNo = value; }
        }
        private String tradeNo;

        public String TradeNo
        {
            get { return tradeNo; }
            set { tradeNo = value; }
        }
        private String outTradeNo;

        public String OutTradeNo
        {
            get { return outTradeNo; }
            set { outTradeNo = value; }
        }
        private DateTime jYSJ;

        public DateTime JYSJ
        {
            get { return jYSJ; }
            set { jYSJ = value; }
        }
        private DateTime jYRQ;

        public DateTime JYRQ
        {
            get { return jYRQ; }
            set { jYRQ = value; }
        }
        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private int jE;

        public int JE
        {
            get { return jE; }
            set { jE = value; }
        }
        private int sJJE;

        public int SJJE
        {
            get { return sJJE; }
            set { sJJE = value; }
        }
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        private int gN;

        public int GN
        {
            get { return gN; }
            set { gN = value; }
        }
    }

    public class XYKItem
    {

        private int YHID; // 银行代码

        public int iYHID
        {
            get { return YHID; }
            set { YHID = value; }
        }
        private string YHName; // 银行名称

        public string sYHName
        {
            get { return YHName; }
            set { YHName = value; }
        }
        private string CardNo; // 信用卡号

        public string sCardNo
        {
            get { return CardNo; }
            set { CardNo = value; }
        }
        private int Money; // 消费金额

        public int mMoney
        {
            get { return Money; }
            set { Money = value; }
        }
        private string LSH; // 交易流水号

        public string sLSH
        {
            get { return LSH; }
            set { LSH = value; }
        }
        private int type;//类型

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
    public class JKD_SKFS_LIST
    {

        int iSkfs_ID;

        public int ISkfs_ID
        {
            get { return iSkfs_ID; }
            set { iSkfs_ID = value; }
        }
        int mSUMJE;

        public int MSUMJE
        {
            get { return mSUMJE; }
            set { mSUMJE = value; }
        }


    }
    public class PayOther
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private int paymentType;

        public int PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
        private int payedCount;

        public int PayedCount
        {
            get { return payedCount; }
            set { payedCount = value; }
        }
        private int payedMoney;

        public int PayedMoney
        {
            get { return payedMoney; }
            set { payedMoney = value; }
        }
    }
    public class CXJSLBFA_Item
    {
        private int iJSLBID;

        public int IJSLBID
        {
            get { return iJSLBID; }
            set { iJSLBID = value; }
        }
        private int iSL;

        public int ISL
        {
            get { return iSL; }
            set { iSL = value; }
        }
        private double fZKL;

        public double FZKL
        {
            get { return fZKL; }
            set { fZKL = value; }
        }

    }
    public class VipzszGzsd
    {

        int iJLBH;


        public int IJLBH
        {
            get { return iJLBH; }
            set { iJLBH = value; }
        }

        int iGZBH;


        public int IGZBH
        {
            get { return iGZBH; }
            set { iGZBH = value; }
        }

        double fZKQD;


        public double FZKQD
        {
            get { return fZKQD; }
            set { fZKQD = value; }
        }


        int iCLFS_SPBM;


        public int ICLFS_SPBM
        {
            get { return iCLFS_SPBM; }
            set { iCLFS_SPBM = value; }
        }

        int iCLFS_SPFL;


        public int ICLFS_SPFL
        {
            get { return iCLFS_SPFL; }
            set { iCLFS_SPFL = value; }
        }

        int iCLFS_HT;


        public int ICLFS_HT
        {
            get { return iCLFS_HT; }
            set { iCLFS_HT = value; }
        }

        int iCLFS_SB;


        public int ICLFS_SB
        {
            get { return iCLFS_SB; }
            set { iCLFS_SB = value; }
        }

        int iCLFS_SP;


        public int ICLFS_SP
        {
            get { return iCLFS_SP; }
            set { iCLFS_SP = value; }
        }

        int iBJ_CJ;


        public int IBJ_CJ
        {
            get { return iBJ_CJ; }
            set { iBJ_CJ = value; }
        }

        public class BackDiscountBill
        {
            int billId;

            public int BillId
            {
                get { return billId; }
                set { billId = value; }
            }
            int inx;

            public int Inx
            {
                get { return inx; }
                set { inx = value; }
            }
            int iREFNO;

            public int IREFNO
            {
                get { return iREFNO; }
                set { iREFNO = value; }
            }
            double limitCount;

            public double LimitCount
            {
                get { return limitCount; }
                set { limitCount = value; }
            }
            double discountedCount;

            public double DiscountedCount
            {
                get { return discountedCount; }
                set { discountedCount = value; }
            }
            double discountRate;

            public double DiscountRate
            {
                get { return discountRate; }
                set { discountRate = value; }
            }
            int discountMoney;

            public int DiscountMoney
            {
                get { return discountMoney; }
                set { discountMoney = value; }
            }
            int vipDiscountType;

            public int VipDiscountType
            {
                get { return vipDiscountType; }
                set { vipDiscountType = value; }
            }
            int dicountPrecision;

            public int DicountPrecision
            {
                get { return dicountPrecision; }
                set { dicountPrecision = value; }
            }
        }
    }

    //2015.10.07:新加类型
    public class MoneyData
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        int inx;

        public int Inx
        {
            get { return inx; }
            set { inx = value; }
        }
        int tckInx;

        public int TckInx
        {
            get { return tckInx; }
            set { tckInx = value; }
        }
        int skje;

        public int Skje
        {
            get { return skje; }
            set { skje = value; }
        }
        int yyje;

        public int Yyje
        {
            get { return yyje; }
            set { yyje = value; }
        }
        int kfqje;

        public int Kfqje
        {
            get { return kfqje; }
            set { kfqje = value; }
        }
        int djje;

        public int Djje
        {
            get { return djje; }
            set { djje = value; }
        }
        int goodsId;

        public int GoodsId
        {
            get { return goodsId; }
            set { goodsId = value; }
        }
        string deptId;

        public string DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }
    }

    public class XSJLYHQItem
    {
        //JLBH,SKTNO,SP_ID,SKFS,DEPTID,YHJE,YQBL
        private string sktno;
        public string Sktno
        {
            get { return sktno; }
            set { sktno = value; }
        }
        private int jlbh;
        public int Jlbh
        {
            get { return jlbh; }
            set { jlbh = value; }
        }
        private int spId;
        public int SpId
        {
            get { return spId; }
            set { spId = value; }
        }
        private int skfs;
        public int Skfs
        {
            get { return skfs; }
            set { skfs = value; }
        }
        private string deptid;
        public string Deptid
        {
            get { return deptid; }
            set { deptid = value; }
        }
        private int yhje;
        public int Yhje
        {
            get { return yhje; }
            set { yhje = value; }
        }
        private double yqbl;
        public double Yqbl
        {
            get { return yqbl; }
            set { yqbl = value; }
        }
        private int inx;
        public int Inx
        {
            get { return inx; }
            set { inx = value; }
        }
    }

    //2015.03.22:新加类型_基础类型
    //可以再精减

    public class Person
    {
        int personId;


        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }


        string personCode;


        public string PersonCode
        {
            get { return personCode; }
            set { personCode = value; }
        }


        string personName;


        public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }


        int personDeptId;


        public int PersonDeptId
        {
            get { return personDeptId; }
            set { personDeptId = value; }
        }



        string personDeptCode;


        public string PersonDeptCode
        {
            get { return personDeptCode; }
            set { personDeptCode = value; }
        }

        string personDeptName;


        public string PersonDeptName
        {
            get { return personDeptName; }
            set { personDeptName = value; }
        }

        string password;


        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        string oTherStr1;


        public string OTherStr1
        {
            get { return oTherStr1; }
            set { oTherStr1 = value; }
        }

        string oTherStr2;


        public string OTherStr2
        {
            get { return oTherStr2; }
            set { oTherStr2 = value; }
        }


        string oTherStr3;

        public string OTherStr3
        {
            get { return oTherStr3; }
            set { oTherStr3 = value; }
        }



        int oTherInt1;


        public int OTherInt1
        {
            get { return oTherInt1; }
            set { oTherInt1 = value; }
        }

        int oTherInt2;


        public int OTherInt2
        {
            get { return oTherInt2; }
            set { oTherInt2 = value; }
        }

        int oTherInt3;


        public int OTherInt3
        {
            get { return oTherInt3; }
            set { oTherInt3 = value; }
        }
    }
    public struct TTranPayments
    {
        public int Id;  //1现金 2储值卡 3银行卡       
        public double PayMoney;
    }

    public struct TTradePayments
    {
        public int id;  //1现金 2储值卡 3银行卡       
        public double payMoney;
        public string name;
        public int type;
        public int yhqid;

    }

    //设备的收款方式
    public struct TDevicePayments
    {
        //1现金:CashMoney 2储值卡:CashCard 3银行卡:Bank
        public int payId;     //付款方式ID
        public string payName;  //付款方式名字 
        public int payTypeId;     //付款方式类型ID 
        public string payTypeName;  //付款方式类型名称 
        public string payTypeCode;  //付款方式类型代码
        public int couponId; //券ID
        public string couponName;//券名称 
    }
    //设备的配置
    public struct TDeviceConfig
    {
        public int configId;     //配置ID
        public string configName;  //配置名字 
        public string configCode;  //配置代码
        public string curValue;     //配置当前数据        
    }

    public struct TTranCoupon
    {
        public int CardId;
        public int CouponId;
        public int CouponType;
        public string CouponName;
        public double Balance;
        public double AccountsPayable;
        public double OutOfPocketAmount;
        public string PayID;
    }

    //public struct TTranGoods
    public class TTranGoods
    {
        //商品ID
        public int tickInx;
        public int assistantId; //营业员ID
        public int inx;
        public int id;
        public string code;
        public string name;
        public double price;
        public double count;
        public double totalOffAmount;
        public double accountsPayable;
        public int deptID;
        public string deptCode;
        public double frontendOffAmount;
        public double backendOffAmount;
        public double changeDiscount; //零钱折扣
        public int backendOffID; //后台折扣单据ID
        public double memberOff; //会员折扣金额
        public int memberOffID;//会员折扣单据ID
        public double fullCutOffAmount;//满减折扣金额        
        public int fullCutOffID; //满减折扣单据ID       
        public double roundOff; //四舍五入折扣 
    }
    public class JsonReqTranGoods
    {
        public int ContractID;
        public List<TTranGoods> GoodsList;
    }

    public class JsonReqTranPays
    {
        public List<TTranPayments> Pays;
    }

    public class JsonReqTranCoupons
    {
        public List<TTranCoupon> CouponList;
    }
    public class UniGoods
    {
        int Id;
        public int id
        {
            get { return Id; }
            set { Id = value; }
        }


        string Code;
        public string code
        {
            get { return Code; }
            set { Code = value; }
        }

        string BarCode;
        public string barCode
        {
            get { return BarCode; }
            set { BarCode = value; }
        }

        string Name;
        public string name
        {
            get { return Name; }
            set { Name = value; }
        }


        string Unit;
        public string unit
        {
            get { return Unit; }
            set { Unit = value; }
        }

        string ClassCode;//商品分类
        public string classCode
        {
            get { return ClassCode; }
            set { ClassCode = value; }
        }

        int Logo;  //商品商标
        public int logo
        {
            get { return Logo; }
            set { Logo = value; }
        }

        int DeptId;
        public int deptId
        {
            get { return DeptId; }
            set { DeptId = value; }
        }

        string DeptCode;
        public string deptCode
        {
            get { return DeptCode; }
            set { DeptCode = value; }
        }

        double Price;
        public double price
        {
            get { return Price; }
            set { Price = value; }
        }


        double MinPrice;
        public double minPrice
        {
            get { return MinPrice; }
            set { MinPrice = value; }
        }

        double VipPrice;
        public double vipPrice
        {
            get { return VipPrice; }
            set { VipPrice = value; }
        }


        int GoodsType; //商品类型
        public int goodsType
        {
            get { return GoodsType; }
            set { GoodsType = value; }
        }

        int Status;
        public int status
        {
            get { return Status; }
            set { Status = value; }
        }



        int ContractId;
        public int contractId
        {
            get { return ContractId; }
            set { ContractId = value; }
        }

        bool Packaged;
        public bool packaged
        {
            get { return Packaged; }
            set { Packaged = value; }
        }

        double SaleCount;
        public double saleCount
        {
            get { return SaleCount; }
            set { SaleCount = value; }
        }

        double SaleMoney;
        public double saleMoney
        {
            get { return SaleMoney; }
            set { SaleMoney = value; }
        }

        double Discount;
        public double discount
        {
            get { return Discount; }
            set { Discount = value; }
        }

        double PreferentialMoney;//优惠金额
        public double preferentialMoney
        {
            get { return PreferentialMoney; }
            set { PreferentialMoney = value; }
        }

        double FrontDiscount;
        public double frontDiscount
        {
            get { return FrontDiscount; }
            set { FrontDiscount = value; }
        }

        double BackDiscount;
        public double backDiscount
        {
            get { return BackDiscount; }
            set { BackDiscount = value; }
        }

        double MemberDiscount;
        public double memberDiscount
        {
            get { return MemberDiscount; }
            set { MemberDiscount = value; }
        }

        double DecreaseDiscount;//满百减折
        public double decreaseDiscount
        {
            get { return DecreaseDiscount; }
            set { DecreaseDiscount = value; }
        }

        double ChangeDiscount; //找零折扣
        public double changeDiscount
        {
            get { return ChangeDiscount; }
            set { ChangeDiscount = value; }
        }

        double DiscoaddDiscount; //折上折扣
        public double discoaddDiscount
        {
            get { return DiscoaddDiscount; }
            set { DiscoaddDiscount = value; }
        }






        int DecreasePreferential;//满百优惠
        public int decreasePreferential
        {
            get { return DecreasePreferential; }
            set { DecreasePreferential = value; }
        }



        int ShopId;
        public int shopId
        {
            get { return ShopId; }
            set { ShopId = value; }
        }

        int DiscountBillId;
        public int discountBillId
        {
            get { return DiscountBillId; }
            set { DiscountBillId = value; }
        }

        int DiscountBillInx;
        public int discountBillInx
        {
            get { return DiscountBillInx; }
            set { DiscountBillInx = value; }
        }


        double DiscountCount;
        public double discountCount
        {
            get { return DiscountCount; }
            set { DiscountCount = value; }
        }


        int Hsfs;
        public int hsfs
        {
            get { return Hsfs; }
            set { Hsfs = value; }
        }


        bool LimitSell;
        public bool limitSell
        {
            get { return LimitSell; }
            set { LimitSell = value; }
        }


        int RefNo_ZK;
        public int refNo_ZK
        {
            get { return RefNo_ZK; }
            set { RefNo_ZK = value; }
        }

        int RefNo_MJ;
        public int refNo_MJ
        {
            get { return RefNo_MJ; }
            set { RefNo_MJ = value; }
        }


        int RefNo_HY;
        public int refNo_HY
        {
            get { return RefNo_HY; }
            set { RefNo_HY = value; }
        }

        int RefNo_JS;
        public int refNo_JS
        {
            get { return RefNo_JS; }
            set { RefNo_JS = value; }
        }


        int CrmInx;
        public int crmInx
        {
            get { return CrmInx; }
            set { CrmInx = value; }
        }


        int MobileBillId;
        public int mobileBillId
        {
            get { return MobileBillId; }
            set { MobileBillId = value; }
        }


        int MobileGoodsInx;
        public int mobileGoodsInx
        {
            get { return MobileGoodsInx; }
            set { MobileGoodsInx = value; }
        }

        int SubTicktInx;
        public int subTicktInx
        {
            get { return SubTicktInx; }
            set { SubTicktInx = value; }
        }


        int SubGoodsInx;
        public int subGoodsInx
        {
            get { return SubGoodsInx; }
            set { SubGoodsInx = value; }
        }



        int SubTicktInx_old;
        public int subTicktInx_old
        {
            get { return SubTicktInx_old; }
            set { SubTicktInx_old = value; }
        }


        int SubGoodsInx_old;
        public int subGoodsInx_old
        {
            get { return SubGoodsInx_old; }
            set { SubGoodsInx_old = value; }
        }


        string Remarks;
        public string remarks
        {
            get { return Remarks; }
            set { Remarks = value; }
        }
    }

    public struct TPayableCoupon
    {
        public int cardId;
        public int couponId;
        public int couponType;
        public string couponName;
        public double balance;
        public double accountsPayable;
        public string payID;
        public string payName;
        public string validity;
    }

    public class DecDiscRule
    {
        double saleMoney;

        public double SaleMoney
        {
            get { return saleMoney; }
            set { saleMoney = value; }
        }

        double discMoney;

        public double DiscMoney
        {
            get { return discMoney; }
            set { discMoney = value; }
        }
    }

    public class DecDiscBill
    {
        int iQZFS;

        public int IQZFS
        {
            get { return iQZFS; }
            set { iQZFS = value; }
        }
        int mQDJE;

        public int MQDJE
        {
            get { return mQDJE; }
            set { mQDJE = value; }
        }

        int mDZJE;
        public int MDZJE
        {
            get { return mDZJE; }
            set { mDZJE = value; }
        }
        int mDZZK;

        public int MDZZK
        {
            get { return mDZZK; }
            set { mDZZK = value; }
        }

        int billId;

        public int BillId
        {
            get { return billId; }
            set { billId = value; }
        }

        int maxDecDisc;

        public int MaxDecDisc
        {
            get { return maxDecDisc; }
            set { maxDecDisc = value; }
        }

        bool onlyOne;

        public bool OnlyOne
        {
            get { return onlyOne; }
            set { onlyOne = value; }
        }

        List<DecDiscRule> rules;

        public List<DecDiscRule> Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        int totalBase;

        public int TotalBase
        {
            get { return totalBase; }
            set { totalBase = value; }
        }

        int totalDecDisc;

        public int TotalDecDisc
        {
            get { return totalDecDisc; }
            set { totalDecDisc = value; }
        }

        int[] goodsInx;

        public int[] GoodsInx
        {
            get { return goodsInx; }
            set { goodsInx = value; }
        }

        int goodsCount;

        public int GoodsCount
        {
            get { return goodsCount; }
            set { goodsCount = value; }
        }

        string decDiscName;

        public string DecDiscName
        {
            get { return decDiscName; }
            set { decDiscName = value; }
        }

        public DecDiscBill()
        {
            rules = new List<DecDiscRule>();
            goodsInx = new int[999];
            onlyOne = false;
            goodsCount = 0;
        }
    }

    public struct TGoods
    {
        public int id;
        public string code;
        public double price;  //int
        public double frontendOffAmount;
        public double count;
        //2017.07.11:新加,主要用于计算商品的售价格
        public int deptID;
        public string deptCode;
    }

    public class GoodsDetails
    {
        int Id;
        public int id
        {
            get { return Id; }
            set { Id = value; }
        }

        string Name;
        public string name
        {
            get { return Name; }
            set { Name = value; }
        }


        string Specification;
        public string specification
        {
            get { return Specification; }
            set { Specification = value; }
        }

        string Code;
        public string code
        {
            get { return Code; }
            set { Code = value; }
        }

        string Unit;
        public string unit
        {
            get { return Unit; }
            set { Unit = value; }
        }

        int LimitedPrice;
        public int limitedPrice
        {
            get { return LimitedPrice; }
            set { LimitedPrice = value; }
        }


        double Price;
        public double price
        {
            get { return Price; }
            set { Price = value; }
        }

        int DepartmentID;
        public int departmentID
        {
            get { return DepartmentID; }
            set { DepartmentID = value; }
        }


        string DepartmentCode;
        public string departmentCode
        {
            get { return DepartmentCode; }
            set { DepartmentCode = value; }
        }


        string ClassCode;
        public string classCode
        {
            get { return ClassCode; }
            set { ClassCode = value; }
        }


        int ContractID;
        public int contractID
        {
            get { return ContractID; }
            set { ContractID = value; }
        }


        string Remarks;
        public string remarks
        {
            get { return Remarks; }
            set { Remarks = value; }
        }


        decimal BackendOffRate;
        public decimal backendOffRate
        {
            get { return BackendOffRate; }
            set { BackendOffRate = value; }
        }

        int BackendOffAmount;
        public int backendOffAmount
        {
            get { return BackendOffAmount; }
            set { BackendOffAmount = value; }
        }

        int BackendOffID;
        public int backendOffID
        {
            get { return BackendOffID; }
            set { BackendOffID = value; }
        }

        int MemberOff;
        public int memberOff
        {
            get { return MemberOff; }
            set { MemberOff = value; }
        }

        int MemberOffID;
        public int memberOffID
        {
            get { return MemberOffID; }
            set { MemberOffID = value; }
        }

        string MemberOffRate;
        public string memberOffRate
        {
            get { return MemberOffRate; }
            set { MemberOffRate = value; }
        }
    }

    //会员相关
    public struct ReqMemberCard
    {
     //   public int id;
        public string storeCode;
     //   public string mobilePhone;
        public string validType;
        public string validID;
     //   public string memberNo;
     //   public string couponPassword;
    }
    //会员
    public struct MemberCard
    {
        public int id;
        public string name;
        public string mobilePhone;
        public string sex;
        public string validType;
        public string validID;
        public int memberType;
        public string memberTypeName;
        public string memberNo;
        public string totalCent;
        public string ticketCent;
        public string validity;
        public int typeLevel;
        public string openId; //会员ID
    }

    //储值卡
    public struct CashCardDetails
    {
        public int cardId;
        public string cardNo;
        public double useMoney;
        public double amount;
        public int payID;
        public int cardTypeId;
    }
    //优惠券
    public class CouponDetails
    {
        int CardId;
        public int cardId
        {
            get { return CardId; }
            set { CardId = value; }
        }

        int CouponId;
        public int couponId
        {
            get { return CouponId; }
            set { CouponId = value; }
        }

        int CouponType;
        public int couponType
        {
            get { return CouponType; }
            set { CouponType = value; }
        }

        string CouponName;
        public string couponName
        {
            get { return CouponName; }
            set { CouponName = value; }
        }

        double Amount;
        public double amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        double AmountCanUse;
        public double amountCanUse
        {
            get { return AmountCanUse; }
            set { AmountCanUse = value; }
        }


        double UseMoney;
        public double useMoney
        {
            get { return UseMoney; }
            set { UseMoney = value; }
        }

        string CardNo;
        public string cardNo
        {
            get { return CardNo; }
            set { CardNo = value; }
        }

        string Valid_date;
        public string valid_date
        {
            get { return Valid_date; }
            set { Valid_date = value; }
        }

        //收款方式 
        string PayID;
        public string payID
        {
            get { return PayID; }
            set { PayID = value; }
        }

        string PayName;
        public string payName
        {
            get { return PayName; }
            set { PayName = value; }
        }

        //返券金额 
        double ReturnMoney;
        public double returnMoney
        {
            get { return ReturnMoney; }
            set { ReturnMoney = value; }
        }
    }
    //返券明细
    public struct TDealReturnCoupon
    {
        public int CardId;
        public int CouponId;
        public int CouponType;
        public string CouponName;
        public double Balance;
        public double ReturnMoney;
        public string ValidDate;
    }

    //待返券明细
    public struct TDealSaleMoneyLeft
    {
        public double SaleMoney;
        public string RuleName;
        public string AddupTypeDesc;
        public string PromotionName;
        public string CouponTypeName;
    }

    //saleMoneyLeft
    public struct TDealMemberCard
    {
        public int id;
        public string name;
        public string mobilePhone;
        public string sex;
        public string validType;
        public string validID;
        public string memberType;
        public string memberNo;
        public string ticketCent;
        public string totalCent;
    }

    public struct TDealPerson
    {
        public int personId;
        public string personName;
        public string personCode;
        public int deptId;
        public string deptCode;
        public string deptName;
        public string password;
        public int workType;
    }

    public struct TDealDevice
    {
        public int deviceId;      //设备ID
        public string deviceName;//设备名
        public string deviceCode;//设备代码
        public string deviceType;//设备类型:包括移动端设备，PC设备
        public int shopId;       //设备所属店的店ID 
        public string shopCode;  //设备所属店的店代码
        public string shopName;  //设备所属店的店名称 
        public string lastLogoutTime; //设备最后登出时间
        public string lastLogoutPerson;//设备最后登出人
    }

    //报表:操作人员信息
    public struct TReportOperInfo
    {
        public int operId;       //操作人Id
        public string operCode;   //操作人Code
        public string operName;  //操作人Name
        public int saleNum;       //销售次数
        public int returnNum;     //退款次数
        public int saleAmount;    //销售金额
        public int returnAmount;  //退款金额
    }
    //报表:收款方式信息
    public struct TReportPaymentItem
    {
        public int payId;        //付款方式ID
        public string payName;   //付款方式
        public int payAmount;    //付款金额
        public int excessAmount; //溢余金额
    }

    public struct TReportDeptItem
    {
        public int deptId;        //部门ID
        public string deptCode;   //部门代码
        public string deptName;   //部门名
        public int saleAmount;    //部门金额
    }

    public class GetReportDetailsResult
    {
        public int code;
        public string text;
        public TReportOperInfo operInfo;
        public List<TReportPaymentItem> paymentInfo;
        public List<TReportDeptItem> deptInfo;
    }

    public class CookingStyleResult
    {
        public int code;
        public string text;
        public List<TCookingStyleItem> styleList;
    }

    public class ReqCookingStyle
    {
        public int styleId;
        public string styleCode;
        public string styleName;
    }


    public class TCookingStyleItem
    {
        public int styleId;
        public string styleCode;
        public string styleName;
    }

    public class GoodListResult
    {
        public int code;
        public string text;
        public List<TFoodItem> foodList;
    }

    public class ReqGoodList
    {
        public int styleId;
    }


    public class TFoodItem
    {
        public int foodId;  //菜品编号(主键)
        public int styleId; //菜系编号 
        public string unit; //单位
        public string foodName; //菜的名字
        public string foodNameSpell; //菜的名字:拼音缩写
        public int price;// 菜品单价
        public string saleNum; //商品数量
        public int cost;  /// 商品成本
        public int saleMoney; //商品总额
        public int depotTypeId; ///// 仓库类型编号
        public int isSaleOut; /// 是否沽清 销售完了

        public int priceVip; //贵宾价
        public int disc;     //折扣
        public string taste; //口味
    }



    public class ReqGetReport
    {
        public int operId;
        public string operCode;
        public string reportDate;
    }

    public class ReqChangePwd
    {
        public int operId;
        public string oldPwd;
        public string operCode;
        public string newPwd;
    }

    public class ChangePwdResult
    {
        public int code;
        public string text;
        public string newPwd;
    }






    //2015.03.22:新加类型_输入输出类型
    public class JsonReqGoods
    {
        public int ContractID;
        public string validType;
        public string validID;
        public int deptId;
        public string deptCode;
        public int assistantId; //营业员ID
        public List<string> GoodsList;
    }

    public class GetGoodsDetailsResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }

        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        public List<GoodsDetails> GoodsList;      //优惠券List
    }


    public class GetMemberCardDetailsResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }

        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        public MemberCard MemberInfo;                 //会员
        public CashCardDetails CashCard;             //CZK信息
        public List<CouponDetails> CouponList;      //优惠券List
    }


    //计算商品的售价
    public class ReqGetGoods
    {
        public string storeCode;  //门店id
        public int contractID;
        public int vipIsDiscount; //0:作VIP折 1:不作VIP折
        public string validType;
        public string ValidID;
        public int deptID;
        public string deptCode;
        public List<TGoods> goodsList;
    }

    public class ReqGetCardPayable
    {
        public string storeCode; //门店id
        public int crmTranID;
        public string validType;
        public string validID;
        public string password;
        public string couponPassword;
        public string verifyCode;
        public string cardCodeToCheck;
    }

    public class CalcAccountsPayableResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }

        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }


        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        //crmBillId
        int CrmTranID;
        public int crmTranID
        {
            get { return CrmTranID; }
            set { CrmTranID = value; }
        }

        public MemberCard MemberInfo;        //会员
        public List<UniGoods> GoodsList;      //商品List
        public CashCardDetails CashCard;         //CZK信息
        public List<TPayableCoupon> CouponList;      //优惠券List
    }


    public class GetCardPayableResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }

        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }


        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        //crmBillId
        int CrmBillId;
        public int crmBillId
        {
            get { return CrmBillId; }
            set { CrmBillId = value; }
        }

        public MemberCard MemberInfo;        //会员
        public CashCardDetails CashCard;         //CZK信息
        public List<TPayableCoupon> CouponList;      //优惠券List
    }

    public class ReqConfirmDeal
    {
        public string storeCode;
        public int contractID;
        public string validType;
        public string validID;

        public int deptID;
        public string deptCode;
        public string outOrder;
        public string erpTranID;
        public string crmTranID;
        public string DDJLBH;
        public List<TTranGoods> goodsList;
        public List<TTranPayments> paysList;
        public List<TTranCoupon> couponsList;
        public List<CashCardDetails> cashCashList;
        public List<CreditDetail> creditDetailList; //银行付款明细
    }

    public class TTicketBaseInfo
    {
        public string erpTranID;
        public string crmTranID;
        public int backPerson; //退款人:如果没有退过款，则为-1
        public string tradeTime; //交易时间 
    }
    //保存
    public class ConfirmDealResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        int CrmTranID;
        public int crmTranID
        {
            get { return CrmTranID; }
            set { CrmTranID = value; }
        }

        int ErpPSN;
        public int ERPPSN
        {
            get { return ErpPSN; }
            set { ErpPSN = value; }
        }

        string QrUrl;
        public string qrUrl
        {
            get { return QrUrl; }
            set { QrUrl = value; }
        }

        public TDealMemberCard MemberInfo;             //会员
        public List<TTranGoods> GoodsList;           //商品List        
        public List<TDealReturnCoupon> ReturnCouponList;  //所送优惠券List
        public List<TDealSaleMoneyLeft> ReturningCouponList;  //所送优惠券List
    }


    //退款申请
    public class ReqConfirmBackDeal
    {
        public string storeCode;  //crm门店代码
        public int contractID;
        public string validType;
        public string validID;
        public int deptID;
        public string deptCode;
        public string outOrder;
        public string erpTranID;
        public string crmTranID;
        public string oldDeviceNo;
        public string oldErpTranID;
        public List<TTranGoods> goodsList;
        public List<TTranPayments> paysList;
        public List<TTranCoupon> couponsList;
        public List<CashCardDetails> cashCashList;
        public List<CreditDetail> creditDetailList;   //20190424增加
    }

    public class ConfirmBackDealResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        int CrmTranID;
        public int crmTranID
        {
            get { return CrmTranID; }
            set { CrmTranID = value; }
        }

        int ErpPSN;
        public int ERPPSN
        {
            get { return ErpPSN; }
            set { ErpPSN = value; }
        }

        public TDealMemberCard MemberInfo;             //会员
        public List<TTranGoods> GoodsList;           //商品List        
        public List<TDealReturnCoupon> ReturnCouponList;  //所送优惠券List
        public List<TDealSaleMoneyLeft> CanReturnCouponList;  //所送优惠券List
    }


    public class ReqManageCard
    {
        public string cardCode;
        public string pwd;
        public string deptCode;
    }

    public class RespManageCard
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }


        public string cardCode;
        public string name;
        public string discountRate;
        public string maxDiscount;
        public string maxBack;
        public string startTime;
        public string endTime;
        public string totalDiscountLimit;
        public string totalBackLimit;
        public string totalDiscount;
        public string totalBack;

        public string curTicketDiscountLimit;
        public string curTicketBackLimit;
    }

    public class ReqLogin
    {
        public string loginPwd;
    }

    public class ReqAssistant
    {
        public string loginPwd;
        public string assistantCode;
    }

    public class LoginResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        public TDealPerson personInfo;
        public TDealDevice deviceInfo;
        public List<TDevicePayments> paysList;    // 收款方式List        
        public List<TDeviceConfig> configList;    //配置List
    }

    public class GetTranIDResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        public TDealDevice deviceInfo;
    }


    public class ShopAssistantResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        public TDealPerson personInfo;
    }

    public struct TConditionData
    {
        public int conditionId;
        public string conditionName;
        public int conditionInx;
        public string tableName;
        public string fieldName;
        public string fieldValue;
        public int operType;
        public string prompt;
    }


    public class TReqTicketInfo
    {
        public int ticketId;
        public string deviceCode;
    }

    public class TRespTicketInfo
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        public TTicketBaseInfo ticketInfo;
        public TDealMemberCard memberInfo;
        public List<TTranGoods> goodsList;
        public List<TTranPayments> paysList;
    }//小票



    public class ReqBackAble
    {
        public string storeCode; //crm门店代码
        public int contractID;
        public int vipIsDiscount; //0:作VIP折 1:不作VIP折
        public string validType;
        public string ValidID;
        public int deptID;
        public string deptCode;
        public string oldErpTranID; //原交易号
        public string oldDeviceNo;  //原设备号

        public List<TTranGoods> goodsList;
    }

    public class RespBackable
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }

        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }


        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        //crmBillId
        int CrmTranID;
        public int crmTranID
        {
            get { return CrmTranID; }
            set { CrmTranID = value; }
        }

        public MemberCard MemberInfo;        //会员
        public List<UniGoods> GoodsList;      //商品List
        public CashCardDetails CashCard;         //CZK信息
        public List<TPayableCoupon> CouponList;      //优惠券List
        public List<TTranPayments> paysList;
    }


    //订金操作
    public class ReqConfirmDepositTran
    {
        public int contractID;
        public string validType;
        public string validID;

        //public int deptID;
        //public string deptCode;
        public string erpTranID;
        public string telPhone;
        public int totalMoney;
        public int depositMoney;
        public List<TTranGoods> goodsList;
        public List<TTranPayments> paysList;
        public List<CashCardDetails> cashCashList;
    }


    public class ConfirmDepositTranResult
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranID;
        public int erpTranID
        {
            get { return ErpTranID; }
            set { ErpTranID = value; }
        }

        int DepositTranID;
        public int depositTranID
        {
            get { return DepositTranID; }
            set { DepositTranID = value; }
        }
    }


    //配置:支付的数据库配置
    public class ReqDepositDetail
    {
        public int DepositTranID;//1)支付和微信的服务地址        
    }

    public class DepositDetailResult
    {
        public int DepositTranID;//1)支付和微信的服务地址      

        public string erpTranID;
        public string telPhone;
        public int totalMoney;
        public int depositMoney;
        public List<TTranGoods> goodsList;
        public List<TTranPayments> paysList;
        //public List<CashCardDetails> cashCashList;
    }


    //配置:支付的数据库配置
    public class TO2OCFGData
    {
        public string sUrlAddress;//1)支付和微信的服务地址
        public string sMchId; //2)支付宝的PID 微信的商户号
        public string sKey;   //3)支付宝1.0: 使用的密钥
        public string sSeller; //4)支付宝1.0：卖家帐户
        public string sLogPath; //5)日志：目录
    }

    public class PosO2ORequest_Base
    {
        public string Mch_Id;//商户号 [支付宝对应的是PID-合作伙伴ID]
        public string Key_o2o; //秘钥
        public string Seller; //卖家标示 [可能是商户号也可能是email]
        public string LogPath; //日志路径 [为空则默认使用web.config里面的logPath]        
    }

    public class PosO2ORequest_Work
    {
        public string AppCode; //[ZFB]支付宝，[WX]微信，[HDLP]邯郸礼品 ...
        public string AppType; //[sale]消费，[query]查询，[cancel]撤销，[refund]退款
        public string PosId; //收款台号[SKTNO]
        public string ReceiptNo; //交易小票号  [JLBH]
        public string OutTradeNo; //外部订单号_唯一[6POS+8JLBH+9hhmmsszzz]  退货时为退款单号
        public string Desc;  //订单商品描述
        public string PersonId; //收款员号 [Person_code]
        public string ShopId; //门店号 [FDBH]
        public string TotalFee; //交易金额  退货时为退款金额
        public string ClientIp;  //收款机IP
        public string BuyerCode; //买家用户号  [被扫为买家手机端条码或者二维码的码串]
        public string OutTradeNo_Old;  //原交易外补订单号  [退款时使用]
        public string OldPosId;  //原交易款台号  [退款时使用]
        public string TotalFee_Old; //原交易金额
        public string Attach;  //附加信息
        public string TransactionId;  //第三方交易号
        public string TransactionId_Old;  //原第三方交易号  [退款时使用]
    }



    public class PosO2ORequest
    {
        public PosO2ORequest_Base Req_Base; //系统参数
        public PosO2ORequest_Work Req_Work; //业务参数       
    }

    public class PosO2OResponse
    {
        public string ReturnMsg; //返回信息  [成功返回交易成功，失败返回错误信息]
        public string UserId; //用户标示
        public int ActTotalFee; //实付金额
        public int CouponFee_Shop; //优惠金额  [商户]
        public int CouponFee; //优惠金额  [第三方]
        public string TransactionId; //交易流水号  [第三方]
        public string OutTradeNo; //外部订单号
        public string TradeState; //交易状态
        public string OutRefundNo; //商户退款单号
        public string RefundId; //退款流水号  [第三方]
        public byte[] RBytes;  //二维码码串  [主扫使用]
        public string QrString; //码串内容        
    }

    public class PosO2OResponse2
    {
        public int ReturnCode; //返回Id:0:成功 -1:失败 1:正在处理中
        public string ReturnMsg; //返回信息  [成功返回交易成功，失败返回错误信息]
        public string UserId; //用户标示
        public int ActTotalFee; //实付金额
        public int CouponFee_Shop; //优惠金额  [商户]
        public int CouponFee; //优惠金额  [第三方]
        public string TransactionId; //交易流水号  [第三方]
        public string OutTradeNo; //外部订单号
        public string TradeState; //交易状态
        public string OutRefundNo; //商户退款单号
        public string RefundId; //退款流水号  [第三方]
        public byte[] RBytes;  //二维码码串  [主扫使用]
        public string QrString; //码串内容        
    }

    public class DecMoneyItem
    {
        private int articleId;
        public int ArticleId
        {
            get { return articleId; }
            set { articleId = value; }
        }
        private string deptID;
        public string DeptID
        {
            get { return deptID; }
            set { deptID = value; }
        }

        private int discRuleNo;
        public int DiscRuleNo
        {
            get { return discRuleNo; }
            set { discRuleNo = value; }
        }
        private double discRate;
        public double DiscRate
        {
            get { return discRate; }
            set { discRate = value; }
        }

        private int decMoney;
        public int DecMoney
        {
            get { return decMoney; }
            set { decMoney = value; }
        }

        private int discType;
        public int DiscType
        {
            get { return discType; }
            set { discType = value; }
        }
        //商品的序号，方便查找.
        private int inx;
        public int Inx
        {
            get { return inx; }
            set { inx = value; }
        }
    }
    public class MBJZ_Info
    {
        public MBJZ_Info()
        {
            iGZ_ZK_Array = new List<float>();
            iGZ_XS_Array = new List<float>();
            goodsInxArray = new List<int>();
        }

        int ZKDBH;

        public int iZKDBH
        {
            get { return ZKDBH; }
            set { ZKDBH = value; }
        }

        int GZCount;

        public int iGZCount
        {
            get { return GZCount; }
            set { GZCount = value; }
        }

        float MBJZXE;

        public float iMBJZXE
        {
            get { return MBJZXE; }
            set { MBJZXE = value; }
        }

        List<float> GZ_ZK_Array;

        public List<float> iGZ_ZK_Array
        {
            get { return GZ_ZK_Array; }
            set { GZ_ZK_Array = value; }
        }

        List<float> GZ_XS_Array;

        public List<float> iGZ_XS_Array
        {
            get { return GZ_XS_Array; }
            set { GZ_XS_Array = value; }
        }

        List<int> goodsInxArray;// 有满减的商品在商品中的inx值

        public List<int> GoodsInxArray
        {
            get { return goodsInxArray; }
            set { goodsInxArray = value; }
        }

    }

    //2017.11.23 如下是银行的付款明细,一般采用异步保存,当时付款了。马上保存 最好不要和交易一起保存
    /*  public struct CreditDetail
      {
          public string deviceCode;       //设备号
          public int ticketId;            //票据号
          public int personId;            //人员ID 
          public string personCode;       //人员代码
          public string operTime;         //发生的时间
          public string accoutTime;       //记帐日期,和销售记录中的记帐日期相同,如果为空,记为当前日期 
          public string typeCode;         //付款的收款类型代码:
          public int skfsId;              //收款方式ID
          public string skfsCode;         //收款方式代码
          public int inx;             //第几条数据
          public string operType;        //银行的操作类型[可以是银行的操作代码]:如:01:执行消费 02:撤消  03:退款
          public string bankCode;         //银行代码
          public int bankId;              //银行ID 
          public string onLineReferenceNumber;  //线上交易参考号
          public string onLineSerialNumber;     //线上交易流水号 
          public string offLineOrderNumber;     //线下交易号
          public int orderMoney;            //订单金额[包括:付款金额+折扣金额] 例如:订单100,实付:90,厂家出:10 我们记:按100
          public int payMoney;            //付款金额
          public int discMoney;           //折扣金额
          public int banlanceMoney;       //帐户余额
          public string other;            //其它项目.此处以备后用。可以用json,也可以用竖线相隔
      } */

    public class CreditDetail
    {
        public int inx
        {
            get;
            set;
        }
        public int payid
        {
            get;
            set;
        }

        public string cardno
        {
            get;
            set;
        }

        public string bank
        {
            get;
            set;
        }

        public int bankid
        {
            get;
            set;
        }

        public double amount
        {
            get;
            set;
        }

        public string serialno
        {
            get;
            set;
        }

        public string refno
        {
            get;
            set;
        }

        public string opertime  //yyyy-mm-dd HH24:MI:SS
        {
            get;
            set;
        }


    }

    public class ReqSaveCreditDetail
    {
        public List<CreditDetail> dataList;
    }

    public class ResultSaveCreditDetail
    {
        public int code;
        public string text;
    }

    //如下是电子订单的结构:2018.04.29

    public class StoreDetail
    {
        public int storeId;//店的Id
        public string storeName; //店的名字
        public string storeCode; //店的代码


        public int subStoreId; //分店Id
        public string subStoreName; //店的名字
        public string subStoreCode; //店的代码
    }


    public class EOrderDetail
    {
        public string storeCode; //店的代码
        public int id;           //订单明细的Id
        public string code;      //订单明细的代码
        public string name;      //订单明细的名称 
        public string value;     //订单明细的值
    }

    public class EOrderItem
    {
        public int id;           //订单明细的Id
        public string code;      //订单明细的代码
        public string value;     //订单明细的值
    }

    //获取店的定义_

    public class ReqEOrderDef
    {
        public string storeCode; //店的代码
    }
    public class RespEOrderDef
    {
        public int code;
        public string text;
        public List<EOrderDetail> data;
    }

    public class ReqEOrderTran
    {
        public int contractID;
        public string validType;
        public string validID;

        public string device;
        public string erpTranId;
        public int totalMoney;
        public int discMoney;
        public List<TTranGoods> goodsList;
        public List<EOrderItem> detailsList;
    }

    public class RespEOrderTran
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        int ErpTranId;
        public int erpTranId
        {
            get { return ErpTranId; }
            set { ErpTranId = value; }
        }

        int EOrderId;
        public int eOrderId
        {
            get { return EOrderId; }
            set { EOrderId = value; }
        }
    }


    public class RespErrData
    {
        public int code;
        public string text;
    }

    //2018.08.27:
    public class OrderInfo
    {
        public string orderNo; //  ORDERNO>21300005</ORDERNO>
        public string phone; // <PHONENO></PHONENO>
        public string mobil; // <MOBILE>123</MOBILE>
        public int priceType; // <PRICETYPE>1</PRICETYPE>
        public string contactMan; // <CONTACTMAN>耿然</CONTACTMAN>
        public string sendAddr; // <SENDADDR>吴</SENDADDR>
        public string sendDate; //  <SENDDATE>2018-8-3 0:00:00</SENDDATE>
        public string sendMethod; // <SENDMETHOD>6</SENDMETHOD>
        public string saleMan; // <SALEMAN>1228</SALEMAN>
        public string endDate;  //ENDDATE
        public int status; // <STATUS>3</STATUS>
        public int djje; //  <DJJE>0</DJJE>
        public int sendMoney; //  <SENDMONEY>0</SENDMONEY>
        public int invoiceMan; // <INVOICEMAN>2597</INVOICEMAN>
        public Person assistant;

        public List<UniGoods> items;
    }

    public class RespJDOrderList
    {
        int Code;
        public int code
        {
            get { return Code; }
            set { Code = value; }
        }


        string Text;
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        public List<OrderInfo> data;
    }

    public class ErrorMessage
    {
        public ErrorMessage()
        {
            errorType = 2;
            message = "";
        }

        int errorType; //1 需要show的错误 2 不需要show的错误 3 offline

        public int ErrorType
        {
            get { return errorType; }
            set { errorType = value; }
        }

        string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

}