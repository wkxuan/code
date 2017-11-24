using System;

namespace z.Exceptions
{
    /// <summary>
    /// 没有权限
    /// </summary>
    [Serializable]
    public class NoPermissionException : LogicException
    {
        public NoPermissionException() : base("没有权限")
        {

        }
        public override int Flag
        {
            get
            {
                return 104;
            }
        }
    }
}
