/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:53
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("WORKITEM", "工作项")]
    public partial class WORKITEMEntity : EntityBase
    {
        public WORKITEMEntity()
        {
        }

        public WORKITEMEntity(string menuid, string billid, string roleid)
        {
            MENUID = menuid;
            BILLID = billid;
            ROLEID = roleid;
        }

        /// <summary>
        /// 菜单编号
        /// <summary>
        [PrimaryKey]
        [Field("菜单编号")]
        public string MENUID
        {
            get; set;
        }
        /// <summary>
        /// 单据编号
        /// <summary>
        [PrimaryKey]
        [Field("单据编号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 卖场ID
        /// <summary>
        [Field("卖场ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 角色编号
        /// <summary>
        [PrimaryKey]
        [Field("角色编号")]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 处理状态(未处理和一段时间内已处理)
        /// <summary>
        [Field("处理状态(未处理和一段时间内已处理)")]
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 发生时间
        /// <summary>
        [Field("发生时间")]
        [DbType(DbType.DateTime)]
        public string CREATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 已处理时间
        /// <summary>
        [Field("已处理时间")]
        [DbType(DbType.DateTime)]
        public string PROC_TIME
        {
            get; set;
        }
    }
}
