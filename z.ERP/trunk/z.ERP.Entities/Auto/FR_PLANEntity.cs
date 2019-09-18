using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("FR_PLAN", "满减方案")]
    public partial class FR_PLANEntity: TableEntityBase
    {
        public FR_PLANEntity() { }
        public FR_PLANEntity(string id) {
            ID = id;
        }
        /// <summary>
        /// 规则id
        /// <summary>
        [PrimaryKey]
        [Field("规则id")]
        public string ID
        {
            get; set;
        }
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
        [Field("限额")]
        public string LIMIT
        {
            get; set;
        }
        [Field("满减方式 1阶梯满减 2循环满减")]
        public string FRTYPE
        {
            get; set;
        }
        [Field("状态 1 未使用 2 已使用")]
        public string STATUS
        {
            get; set;
        }
    }
}
