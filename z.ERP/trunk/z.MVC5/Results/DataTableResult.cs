using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace z.MVC5.Results
{
    public class DataTableResult : UIResult
    {
        public DataTableResult(DataTable dt)
        {
            Obj = dt;
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator DataTableResult(DataTable d)
        {
            return new DataTableResult(d);
        }
    }
}
