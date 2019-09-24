/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:18
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("TicketInfo", "交易小票")]
    public partial class TicketInfoEntity : TableEntityBase
    {
        public TicketInfoEntity()
        {
        }

        public TicketInfoEntity(string branchid)
        {
            BRANCHID = branchid;
        }

        /// <summary>
        /// 门店编号
        /// <summary>
        [PrimaryKey]
        [Field("门店编号")]
        public string BRANCHID
        {
            get; set;
        }

 
        /// <summary>
        /// 票头文字
        /// <summary>
        [Field("票头文字")]
        public string HEAD
        {
            get; set;
        }
        /// <summary>
        /// 管理部门
        /// <summary>
        [Field("票尾文字")]
        public string TAIL
        {
            get; set;
        }
        /// <summary>
        /// 打印次数
        /// </summary>
        [Field("打印次数")]
        public string PRINTCOUNT
        {
            get; set;
        }
        /// <summary>
        /// 二维码广告
        /// <summary>
        [Field("二维码广告")]
        public string ADQRCODE
        {
            get; set;
        }
        /// <summary>
        /// 文字广告位
        /// <summary>
        [Field("文字广告位")]
        public string ADCONTENT
        {
            get; set;
        }
      
    }
}
