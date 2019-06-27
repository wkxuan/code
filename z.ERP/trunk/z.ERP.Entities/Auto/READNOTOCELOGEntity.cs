using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("READNOTOCELOG", "通知已读表")]
    public partial class READNOTOCELOGEntity:TableEntityBase
    {
        public READNOTOCELOGEntity()
        {
        }
        public READNOTOCELOGEntity(string noticeid,string userid)
        {
            NOTICEID = noticeid;
            USERID = userid;
        }
        /// <summary>
        /// 主键id
        /// </summary>
        [PrimaryKey]
        [Field("消息id")]
        public string NOTICEID
        {
            get; set;
        }
        [Field("人员id")]
        public string USERID
        {
            get; set;
        }
    }
}
