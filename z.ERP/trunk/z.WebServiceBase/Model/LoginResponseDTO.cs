namespace z.WebServiceBase.Model
{
    public class LoginResponseDTO
    {
        public bool Success
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 其他配置信息
        /// 每个系统可能不同
        /// </summary>
        public string ConfigInfo
        {
            get;
            set;
        }

    }
}