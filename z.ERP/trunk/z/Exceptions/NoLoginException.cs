using System;

namespace z.Exceptions
{
    /// <summary>
    /// 没有登陆
    /// </summary>
    [Serializable]
    public class NoLoginException : LogicException
    {
        public NoLoginException() : base("没有登陆")
        {

        }
        public override int Flag
        {
            get
            {
                return 103;
            }
        }
    }
}
