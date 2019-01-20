using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.SSO.Model
{
    public class User
    {
        /// <summary>
        /// 操作人id
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
