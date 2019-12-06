using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poise;
using Pose;
using System.Collections.Generic;
using System.Diagnostics;
using Testing.TestClasses;

namespace PoiseTests
{
    [TestClass]
    public class ShimmyTests
    {
        [TestMethod]
        public void BasicTest()
        {
            var test = string.Empty;

            var testShimmy = Shimmy.CreateShimmy<ShimMe>(typeof(ShimMe));

            var realShim = testShimmy.GetShim(nameof(ShimMe.PublicInstanceParameters), false, typeof(string), typeof(string));
            realShim.With((ShimMe @this, string s) => "Wow, getting Shims worked right out of the box!");

            Poise.Poise.Run(() => {
                test = DontShimMe.TestMeThough();
            }, new List<Shimmy>() { testShimmy });

            Debug.WriteLine(test);
        }
    }
}
