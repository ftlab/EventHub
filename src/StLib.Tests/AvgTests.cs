using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace StLib.Tests
{
    [TestClass]
    public class AvgTests
    {
        [TestMethod]
        public void TestAvg()
        {
            var n = new double[] { 1, 2, 3, 4, 5, 6, 7 };

            var e = n.Average();
            var a = n.Avg();

            Assert.AreEqual(e, a);
        }
    }
}
