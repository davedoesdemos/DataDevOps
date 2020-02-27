using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testingintro
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(3, 2, 1, "Numbers don't match");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(3, 6, 1, "Numbers don't match");
        }

        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(3, 2, 0, "Numbers don't match");
        }
    }
}
