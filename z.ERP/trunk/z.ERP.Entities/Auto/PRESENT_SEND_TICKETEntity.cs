using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PRESENT_SEND_TICKET", "赠品发放使用小票")]
    public partial class PRESENT_SEND_TICKETEntity: TableEntityBase
    {
        public PRESENT_SEND_TICKETEntity() { }
        public PRESENT_SEND_TICKETEntity(string id,string posno,string dealid) {
            BILLID = id;
            POSNO = posno;
            DEALID = dealid;
        }
        /// <summary>
        /// id
        /// <summary>
        [PrimaryKey]
        [Field("id")]
        public string BILLID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("小票终端号")]
        public string POSNO
        {
            get; set;
        }
        [PrimaryKey]
        [Field("小票号")]
        public string DEALID
        {
            get; set;
        }
        [Field("小票金额")]
        public string AMOUNT
        {
            get; set;
        }
        [Field("活动id")]
        public string FGID
        {
            get; set;
        }
        [Field("时间")]
        [DbType(DbType.DateTime)]
        public string SALE_TIME
        {
            get; set;
        }
    }
}
