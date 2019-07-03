using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Model.Tree
{
    /// <summary>
    /// 树结构数据
    /// </summary>
    public class TreeEntity
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string code { get; set; }        
        /// <summary>
        /// 节点提示
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 节点数值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 是否显示勾选框
        /// </summary>
        public bool @checked { get; set; }
        
        /// <summary>
        /// 是否有子节点
        /// </summary>
        public bool hasChildren { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool expand { get; set; }
        /// <summary>
        /// 子节点列表数据
        /// </summary>
        public List<TreeEntity> children { get; set; }      
        /// <summary>
        /// 父级节点ID
        /// </summary>
        public string parentId { get; set; }
    }
}
