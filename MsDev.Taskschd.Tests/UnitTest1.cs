using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSDev.Taskschd.Helpers;
using System.Linq;

namespace MSDev.Taskschd.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = RssCrawler.GetDevBlogs().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            result.ToList().ForEach(x => Console.WriteLine(x.Title));

            result = RssCrawler.GetCloudNews().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            result.ToList().ForEach(x => Console.WriteLine(x.Title));
        }
    }
}