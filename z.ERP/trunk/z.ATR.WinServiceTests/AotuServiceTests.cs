using Microsoft.VisualStudio.TestTools.UnitTesting;
using z.ATR.WinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ATR.WinService.Tests
{
    [TestClass()]
    public class AotuServiceTests
    {
        [TestMethod()]
        public void testTest()
        {
            AotuService a = new AotuService();
            a.Do();
        }
    }
}