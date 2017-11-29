
namespace z.DBHelper.Connection
{
    /// <summary>
    /// 配置文件的基类
    /// </summary>
    public interface IDbConnectionInfo
    {
        /// <summary>
        /// 转为链接字符串
        /// </summary>
        /// <returns></returns>
        string ToConnectionString();
    }
}
