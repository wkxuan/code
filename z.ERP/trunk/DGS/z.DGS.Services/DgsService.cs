using System;
using System.Collections.Generic;
using z.Extensions;
using System.Data;
using System.Linq;
using z.ServiceHelper;
using System.Diagnostics;
using z.DBHelper.DBDomain;

namespace z.DGS.Services
{


    public class DgsService : ServiceBase
    {

        internal DgsService()
        {

        }

        public string Test()
        {
            return DbHelper.ExecuteTable("select Ip from STATION where TYPE=3  and STATIONBH= {{code}}", new zParameter("code", employee.Id)).Rows[0][0].ToString();
        }
    }
}