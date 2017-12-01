/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:28
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("STATION", "")]
    public partial class STATIONEntity : EntityBase
    {
        public STATIONEntity()
        {
        }

        public STATIONEntity(string stationbh)
        {
            STATIONBH = stationbh;
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
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string IP
        {
            get; set;
        }
    }
}
