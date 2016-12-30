// <copyright file="GlobalConfigTest.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016. All rights reserved.
// </copyright>
// <summary>
// </summary>

namespace jbDesign_Common_TestLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using jbDesign.Config;
    using log4net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GlobalConfigTest
    {
        [TestMethod]
        public void GlobalConfigTest_Container()
        {
            Assert.IsNotNull(GlobalConfig.Instance);
            Assert.IsNotNull(GlobalConfig.Instance.Container);
            //            Assert.IsNotNull(GlobalConfig.Instance.ConfigSection);
        }

        [TestMethod]
        public void GlobalConfigTest_LoggerMustBeForSameClass()
        {
            Assert.IsNotNull(GlobalConfig.Instance.Logger);
            Assert.AreEqual(this.GetType().FullName, GlobalConfig.Instance.Logger.Logger.Name);
        }
    }
}