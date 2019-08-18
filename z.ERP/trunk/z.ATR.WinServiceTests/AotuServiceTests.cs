using Microsoft.VisualStudio.TestTools.UnitTesting;
using z.ATR.WinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ATR._96262API;
using System.Data;

namespace z.ATR.WinService.Tests
{
    [TestClass()]
    public class AotuServiceTests
    {
        [TestMethod()]
        public void a()
        {
            AotuService a = new AotuService();
            a.Do();
        }

        [TestMethod()]
        public void bbbbbbbbbbbb()
        {
            try
            {
                ReportForm t = new ReportForm();
                List<DataTable> dts = t.Get("010200000000129", "20171201");
            }
            catch (Exception ex)
            {

            }
        }
    }
}