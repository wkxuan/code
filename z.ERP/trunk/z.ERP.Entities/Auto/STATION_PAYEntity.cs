/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:02
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("STATION_PAY", "")]
    public partial class STATION_PAYEntity : EntityBase
    {
        public STATION_PAYEntity()
        {
        }

        public STATION_PAYEntity(string stationbh, string payid)
        {
            STATIONBH = stationbh;
            PAYID = payid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string STATIONBH
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string PAYID
        {
            get; set;
        }
    }
}
