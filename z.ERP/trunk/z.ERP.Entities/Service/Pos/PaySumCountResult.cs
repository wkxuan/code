namespace z.ERP.Entities.Service.Pos
{
    public class PaySumCountResult
    {
        public int payid
        {
            get;
            set;
        }

        public string payname
        {
            get;
            set;
        }

        public decimal amountreturn
        {
            get;
            set;
        }

        public decimal amountsum
        {
            get;
            set;
        }

        public int countsum
        {
            get;
            set;
        }

        public int returncount
        {
            get;
            set;
        }

    }
}
