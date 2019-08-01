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
        Feesubject = 5,
        /// <summary>
        /// 区域
        /// </summary>
        Region = 6,
        /// <summary>
        /// 业态
        /// </summary>
        Category = 7,
        /// <summary>
        /// 预警项目
        /// </summary>
        Alert = 8,

        /// <summary>
        /// 部门(包含上级部门)
        /// </summary>
        FullOrg = 9


    }
}
