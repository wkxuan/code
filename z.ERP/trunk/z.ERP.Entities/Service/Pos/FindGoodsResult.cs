using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class FindGoodsResult
    {
        public string Goodsid
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 类型：1品类2大类
        /// </summary>
        public string Type
        {
            get; set;
        }
        public string Price
        {
            get; set;
        }
        public string Member_Price
        {
            get; set;
        }

    }
}
