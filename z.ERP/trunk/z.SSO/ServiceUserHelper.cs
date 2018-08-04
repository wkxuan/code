using System;
using System.Security.Principal;
using z.Context;
using z.Encryption;
using z.Exceptions;
using z.Extensions;
using z.ServiceHelper;
using z.SSO.Model;

namespace z.SSO
{
    public class ServiceUserHelper : UserHelper
    {
        public ServiceUserHelper(SSOSettings _settings) : base(_settings)
        {
        }

        public static string GetSrc(ServiceUser user)
        {
            return RSAEncryption.Encrypt(Base64Encryption.Decrypt(PublicKey), user.ToJson());
        }

        static string PublicKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjFnd2ZJbDhNb1pyZ3hrbHFyMVBqRzJUSTFvN0tCOGlOQUxwRUJxUHY5b1lhY2d3c0ZkZjd3b1dFeHpkWi8wdjV4Yi9CbFhJbmxocGFvcHI1bEtLRENQK1FZOFRlUExHOFkvRThOdmxibUJ3ZlZJVnZoRmZ0K1BoVU1OcFllMnduanR1YWlUeW94Zi9MRGUyTUhmMEIxQ21kcTRwOWVZQzEvZldyQWxleFptMD08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+";
        static string PrivateKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjFnd2ZJbDhNb1pyZ3hrbHFyMVBqRzJUSTFvN0tCOGlOQUxwRUJxUHY5b1lhY2d3c0ZkZjd3b1dFeHpkWi8wdjV4Yi9CbFhJbmxocGFvcHI1bEtLRENQK1FZOFRlUExHOFkvRThOdmxibUJ3ZlZJVnZoRmZ0K1BoVU1OcFllMnduanR1YWlUeW94Zi9MRGUyTUhmMEIxQ21kcTRwOWVZQzEvZldyQWxleFptMD08L01vZHVsdXM + PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPi8xaytBZHpJRUpqK3daUmpZNFFDN0xQc3Z0dzBrbk9EcWVvUWJwbVlwbDAzZmxxZ3A5YXlOZUREUklnWTVuQXRDSXNxMnZoSGlPZ2p2M0dLekp0SE93PT08L1A + PFE + MXBmb1FJYzdEL0xPTDVpZWpXZHRQSHBCazlhemVIdUZBd3ljRUozMW5GTmQ1Q3dDa2dabTZJalgwai9TL2VmbW05clRxT3oydFluNk9tVHhTcGsrZHc9PTwvUT48RFA + cVlHamNQY1A5RHlyK1BNNVd3bDZLNGx4SW0zcGxFS01aNUlTb0dqajlhUXh2M1lINmdMU3dJTlkvTGhmMXpFbUkrTEdheCtmMVJsTkNid2t5SmhYbXc9PTwvRFA + PERRPmlOT2xEL2ZwemRwNFkxckJ4ZEdya2dNMTZ3amJ1RGV4OE9iS1g4SUlDQndUNHRlNDc5akxKdnVSK1FvZkF5d3BHemtrK2pIVmdKMHdncWs0UE9PSE13PT08L0RRPjxJbnZlcnNlUT5Xc3pOK3hjN2hXZDUxWXhoWGMyMVRUdzd5YzZPZUw4SGVnbHlZekx1akFydFlzc0FwUmlqOGhIUHkxYmZIYzFkOWtKdE9WSFBLbll3M2hwV2dpYW95UT09PC9JbnZlcnNlUT48RD5yT3ZVQnorRWc5WU1ld1I2eExQZmlKVHVUNi9CZFhKR3YyeTdZaVgweVZOamIvbDNjQm94ME45dStrWTBWYlE2VDlCdmd0MWJMNytRczBiQ0ZKc0pnYkxWYTZyS05WR2ZVSTRDdXhSYkQxc01DSkQ2b01idk9vK1lPRUlxb281YUNJY1JWVU95NmJLV3VHRGExYUhwR1poRjkybVplRjQ3V1dzb01XRk53WDA9PC9EPjwvUlNBS2V5VmFsdWU+";

        public override T GetUser<T>()
        {
            if (ConfigExtension.TestModel)//测试模式
            {
                ServiceUser teste = new ServiceUser()
                {
                    Id = ConfigExtension.TestModel_User,
                    Name = $"测试模式:{ConfigExtension.TestModel_User}",
                    PlatformId = "-1"
                };
                return teste as T;
            }
            string key = ApplicationContextBase.GetContext()?.principal?.Identity.Name;
            string UserJson;
            if (RSAEncryption.TryDecrypt(Base64Encryption.Decrypt(PrivateKey), key, out UserJson))
            {
                ServiceUser user = UserJson.ToObj<ServiceUser>();
                //---------------这里额外判断下ip是否匹配
                return user as T;
            }
            else
            {
                throw new NoLoginException();
            }
        }


        public override void Login(string username, string password)
        {
            if (username.IsEmpty())
                throw new NoLoginException();
            //------------这里接入验证登陆和权限配置
            ApplicationContextBase.GetContext().principal = new GenericPrincipal(new GenericIdentity(username), null);
        }


        public override void LogOut()
        {
            throw new Exception("服务接口不能主动登出");
        }

    }
}



