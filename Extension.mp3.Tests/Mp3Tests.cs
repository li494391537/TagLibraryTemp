using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lirui.TagLibray.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lirui.TagLibray.Extension.Tests {
    [TestClass()]
    public class Mp3Tests {
        [TestMethod()]
        public void GetTagsTest() {
            var test = new Mp3(@"K:\music\40㍍P\41m\恋愛裁判.mp3");
            test.GetTags();
            //Assert.Fail();
        }
    }
}