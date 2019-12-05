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
            //var shimMe = new ShimMe();
            //shimMe.PublicInstance();
            //ShimMe.PublicStatic();
            //ShimMe.PublicStaticParameters("hello");
            var what = Shimmy.CreateShimmy<ShimMe>(typeof(ShimMe));
           // Shim shim = Shim.Replace(() => Is.A<ShimMe>().PublicInstanceParameters("hi"));
            var helpABrotherOut = Is.A<ShimMe>();
            DontShimMe.TestMeThough();

            var test = string.Empty;

            Poise.Poise.Run(() => {
                test = DontShimMe.TestMeThough();
                //test = shimMe.PublicInstance();
                //ShimMe.PublicStatic();
                //ShimMe.PublicStaticParameters("hello");
            }, new List<Shimmy>() { what });
            
            Debug.WriteLine(test);
        }
    }
}
