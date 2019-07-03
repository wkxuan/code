using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Model.Tree
{
    public static class ToTrees
    {
        public static List<TreeEntity> ToTree(this List<TreeEntity> list)
        {
            Dictionary<string, List<TreeEntity>> childrenMap = new Dictionary<string, List<TreeEntity>>();
            Dictionary<string, TreeEntity> parentMap = new Dictionary<string, TreeEntity>();
            List<TreeEntity> res = new List<TreeEntity>();

            //首先按照
            foreach (var node in list)
            {
                node.hasChildren = false;
                // 注册子节点映射表
                if (!childrenMap.ContainsKey(node.parentId))
                {
                    childrenMap.Add(node.parentId, new List<TreeEntity>());
                }
                else if (parentMap.ContainsKey(node.parentId))
                {
                    parentMap[node.parentId].hasChildren = true;
                }
                childrenMap[node.parentId].Add(node);
                // 注册父节点映射节点表
                parentMap.Add(node.code, node);

                // 查找自己的子节点
                if (!childrenMap.ContainsKey(node.code))
                {
                    childrenMap.Add(node.code, new List<TreeEntity>());
                }
                else
                {
                    node.hasChildren = true;
                }
                node.children = childrenMap[node.code];
            }
            // 获取祖先数据列表
            foreach (var item in childrenMap)
            {
                if (!parentMap.ContainsKey(item.Key))
                {
                    res.AddRange(item.Value);
                }
            }
            return res;
        }
    }
}
