/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:04
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("STATION_PAY", "POS终端支付方式")]
    public partial class STATION_PAYEntity : EntityBase
    {
        public STATION_PAYEntity()
        {
        }

        /// <summary>
        /// POS终端编号
        /// <summary>
        [Field("POS终端编号")]
        public string STATIONBH
        {
            get; set;
        }
        /// <summary>
        /// 收款方式
        /// <summary>
        [Field("收款方式")]
        public string PAYID
        {
            get; set;
        }
    }
}
