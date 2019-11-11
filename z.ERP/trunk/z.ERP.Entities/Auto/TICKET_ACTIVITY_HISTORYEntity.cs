using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("TICKET_ACTIVITY_HISTORY", "小票活动历史记录")]
    public partial class TICKET_ACTIVITY_HISTORYEntity : TableEntityBase
    {
        public TICKET_ACTIVITY_HISTORYEntity()
        {
        }

        public TICKET_ACTIVITY_HISTORYEntity(string id,string posno,string dealid)
        {
            PROMOTIONID = id;
            POSNO = posno;
            DEALID = dealid;
        }
        /// <summary>
        /// 活动id
        /// <summary>
        [PrimaryKey]
        [Field("活动id")]
        public string PROMOTIONID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("款台")]
        public string POSNO
        {
            get; set;
        }
        [PrimaryKey]
        [Field("交易号")]
        public string DEALID
        {
            get; set;
        }        
        /// <summary>
        /// 录入人
        /// <summary>
        [Field("录入人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入人名称
        /// <summary>
        [Field("录入人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入人时间
        /// <summary>
        [Field("录入人时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }      
    }
}

