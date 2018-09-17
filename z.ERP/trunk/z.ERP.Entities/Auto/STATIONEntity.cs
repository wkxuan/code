/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:19
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("STATION", "POS终端")]
    public partial class STATIONEntity : TableEntityBase
    {
        public STATIONEntity()
        {
        }

        public STATIONEntity(string stationbh)
        {
            STATIONBH = stationbh;
        }

        /// <summary>
        /// pos终端号
        /// <summary>
        [PrimaryKey]
        [Field("pos终端号")]
        public string STATIONBH
        {
            get; set;
        }
        /// <summary>
        /// 类型
        /// <summary>
        [Field("类型")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// IP
        /// <summary>
        [Field("IP")]
        public string IP
        {
            get; set;
        }

        [Field("店铺")]
        public string SHOPID
        {
            get;set;
        }
    }
}
