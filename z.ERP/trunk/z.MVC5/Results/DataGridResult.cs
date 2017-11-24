using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace z.MVC5.Results
{
    public class DataGridResult : UIResult
    {
        public DataGridResult(DataTable dt, int allcount)
        {
            Obj = new
            {
                rows = dt,
                total = allcount
            };
        }
    }
}
