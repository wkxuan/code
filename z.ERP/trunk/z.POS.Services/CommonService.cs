
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using z.DBHelper.DBDomain;
using z.Exceptions;
using z.Extensions;

namespace z.POS.Services
{
    public class CommonService : ServiceBase
    {
        internal CommonService()
        {
        }
    }
}
