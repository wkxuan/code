namespace z.ERP.WebService.Model
{
    public class LoginResponseDTO
    {
        public bool success
        {
            get;
            set;
        }

        public string errorMsg
        {
            get;
            set;
        }

        public string secretKey
        {
            get;
            set;
        }

        public string userId
        {
            get;
            set;
        }

        public string userName
        {
            get;
            set;
        }

    }
}