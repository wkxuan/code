using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.MVC5.Models
{
    public class ErrorModel
    {
        public ErrorModel()
        {

        }

        /// <summary>
        /// 位置
        /// </summary>
        public string Site
        {
            get;
            set;
        }

        public Exception Ex
        {
            get;
            set;
        }
    }
}
