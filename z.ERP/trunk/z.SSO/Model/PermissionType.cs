using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.SSO.Model
{
    public enum PermissionType
    {
        /// <summary>
        /// 菜单
        /// </summary>
        Menu=0,
        /// <summary>
        /// 部门
        /// </summary>
        Org=1,
        /// <summary>
        /// 门店
        /// </summary>
        Branch = 2,
        /// <summary>
        /// 楼层
        /// </summary>
        Floor = 3,
        /// <summary>
        /// 店铺
        /// </summary>
        Shop = 4,
        /// <summary>
        /// 收费项目
        /// </summary>
        Feesubject = 5
    }
}
