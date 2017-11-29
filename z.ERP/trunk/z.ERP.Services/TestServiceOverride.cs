using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Context;

namespace z.ERP.Services
{
    public class TestServiceOverride : TestService
    {
        internal TestServiceOverride()
        {
        }
        public override string a()
        {
            return "TestManagerChildren";
        }
    }
}
