using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.POS80.Entities.Pos
{
    public class DiscountRecord
    {
        public int sheetid
        {
            get; set;
        }
        public int inx
        {
            get; set;
        }

        public int type   //折扣类型
        {
            get;
            set;
        }

        public double amount  //折扣金额
        {
            get;
            set;
        }

        public double rate  //折扣率
        {
            get;
            set;
        }

        public int refno  //折扣单号
        {
            get;
            set;
        }
    }
}
