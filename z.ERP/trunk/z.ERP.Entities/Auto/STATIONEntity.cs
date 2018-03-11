/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:19:02
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("STATION", "POS终端")]
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
    }
}
