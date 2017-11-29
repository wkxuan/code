using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.Info
{
    public class MergeInfo
    {
        private string _tableName;
        private Dictionary<string, MergePramInfo> _pk;
        private Dictionary<string, MergePramInfo> _insertPram;
        private Dictionary<string, MergePramInfo> _updatePram;

        /// <summary>
        /// 更新字段
        /// </summary>
        public Dictionary<string, MergePramInfo> UpdatePram
        {
            get { return _updatePram; }
            set { _updatePram = value; }
        }

        /// <summary>
        /// 插入字段
        /// </summary>
        public Dictionary<string, MergePramInfo> InsertPram
        {
            get { return _insertPram; }
            set { _insertPram = value; }
        }

        /// <summary>
        /// 主键
        /// </summary>
        public Dictionary<string, MergePramInfo> Pk
        {
            get { return _pk; }
            set { _pk = value; }
        }

        /// <summary>
        /// 目标表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

    }

    public class MergePramInfo
    {
        private Type _type;
        private string _value;

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public static implicit operator MergePramInfo(string value)
        {
            return new MergePramInfo()
            {
                Value = value,
                Type = typeof(string)
            };
        }

    }
}
