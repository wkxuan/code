using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace z.Context
{
    public class HttpApplicationContext : ApplicationContextBase
    {
        public override T GetData<T>(string name)
        {
            if (_context.Items.Contains(name))
            {
                return _context.Items[name] as T;
            }
            return null;
        }

        public override void SetData<T>(string name, T data)
        {
            _context.Items[name] = data;
        }

        public override void RemoveData(string name)
        {
            _context.Items.Remove(name);
        }

        HttpContext _context
        {
            get
            {
                HttpContext hc = HttpContext.Current;
                if (hc == null)
                {
                    throw new InvalidOperationException("请确保这个是正确的web项目并且在合适的阶段");
                }
                return hc;
            }
        }

        public override IPrincipal principal
        {
            get
            {
                return _context.User;
            }
            set
            {
                _context.User = value;
            }
        }
    }
}
