/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:22
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
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

        /// <summary>
        /// pos终端号
        /// <summary>
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
