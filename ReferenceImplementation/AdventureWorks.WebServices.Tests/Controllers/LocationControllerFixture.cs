

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.WebServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureWorks.WebServices.Tests.Controllers
{
    [TestClass]
    public class LocationControllerFixture
    {
        [TestMethod]
        public void Get_All_States()
        {
            var controller = new LocationController();
            var result = controller.GetStates();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
