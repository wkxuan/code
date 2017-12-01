/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:23
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTJSKL", "")]
    public partial class CONTJSKLEntity : EntityBase
    {
        public CONTJSKLEntity()
        {
        }

        public CONTJSKLEntity(string contractid, string groupno, string inx, string startdate)
        {
            CONTRACTID = contractid;
            GROUPNO = groupno;
            INX = inx;
            STARTDATE = startdate;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string GROUPNO
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        [DbType(DbType.DateTime)]
        public string STARTDATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string ENDDATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SALES_START
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SALES_END
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JSKL
        {
            get; set;
        }
    }
}
