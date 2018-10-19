using System;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleSummaryFilter
    {

        public string posno
        {
            get;
            set;
        }
        public DateTime? saledate_begin
        {
            get;
            set;
        }
        public DateTime? saledate_end
        {
            get;
            set;
        }
    }
}
