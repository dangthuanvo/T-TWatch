using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Web.Watch.Controllers;

namespace Web.Test
{
    [TestClass]
    public class TestGetAppsettingString
    {
        [TestMethod]
        public void TestMethod1()
        {
          HomeController controller = new HomeController();
            string tmp = controller.getStringAppSettingVNPayTest();
            Console.WriteLine(tmp);



        }
    }
}
