using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, 2, 0, "don't match");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(1, 1, 0, "don't match");
        }
    }
}
