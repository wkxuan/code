using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using z.Exceptions;
using z.Extensions;

namespace z.Verify
{
    public class VerifyBase
    {
        public VerifyBase()
        {
            _errorMsg = new List<string>();
        }

        List<string> _errorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 验证,不通过则抛异常
        /// </summary>
        public virtual void Verify()
        {
            if (_errorMsg.Count > 0)
            {
                throw new LogicException(string.Join("\r\n", _errorMsg));
            }
        }

        /// <summary>
        /// 验证,返回验证结果
        /// </summary>
        /// <param name="separator">连字符</param>
        /// <returns>验证结果,成功则返回null</returns>
        public virtual string VerifyWithoutError(string separator = "\r\n")
        {
            if (_errorMsg.Count > 0)
            {
                return string.Join(separator, _errorMsg);
            }
            else
                return null;
        }

        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="str"></param>
        protected void SetError(string str)
        {
            _errorMsg.Add(str);
        }


    }
}
