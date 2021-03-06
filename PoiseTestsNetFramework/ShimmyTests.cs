﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poise;
using Pose;
using System.Collections.Generic;
using System.Diagnostics;
using Testing.TestClasses;

namespace PoiseTests
{
    using System;
    using System.Linq;

    [TestClass]
    public class ShimmyTests
    {
        [TestMethod]
        public void BasicTest()
        {
            var test = string.Empty;

            var testShimmy = Shimmy.CreateShimmy<ShimMe>(typeof(ShimMe), new ShimmySettings());
            // var iEnumerableShimmy = Shimmy.CreateShimmy<IEnumerable<string>>(typeof(IEnumerable<string>));

            // iEnumerableShimmy.GetShim(nameof(IEnumerable<string>.GetEnumerator)).With()

            var realShim = testShimmy.GetShim(nameof(ShimMe.PublicInstanceParameters), false, typeof(string), typeof(string));
            realShim.With((ShimMe @this, string s) => "Wow, getting Shims worked right out of the box!");

            var newFrameworkShim =
                Shim.Replace(() => Is.A<IEnumerable<string>>().FirstOrDefault(Is.A<Func<string, bool>>()))
                    .With((IEnumerable<string> @this, Func<string, bool> func) => @this.FirstOrDefault(func));
            //var otherFrameworkShim = Shim.Replace(() => string.Format(Is.A<string>(), Is.A<object[]>()))
            //    .With((string replacement, object[] args) => string.Format(replacement, args));
            //var otherOtherFrameworkShim = Shim.Replace(() => string.Format(Is.A<string>(), Is.A<object>()))
            //    .With((string replacement, object args) => string.Format(replacement, args));
            var otherOtherOtherFrameworkShim = Shim.Replace(() => string.Format(Is.A<string>(), Is.A<object>(), Is.A<object>()), false)
                .With((string replacement, object args, object otherArg) =>
                {
                    return string.Format(replacement, args, otherArg);
                });
            var moreFrameworkShim = Shim.Replace(() => string.Format(Is.A<string>(), Is.A<object>()), false)
                .With((string replacement, object arg) =>
                {
                    return string.Format(replacement, arg);
                });

            // var otherRealShim = testShimmy.GetShim("get_PublicProperty");

            Poise.Poise.Run(() => { test = DontShimMe.TestMeThough(); }, new List<Shimmy> { testShimmy },
               new List<Shim>() { newFrameworkShim, otherOtherOtherFrameworkShim, moreFrameworkShim });//otherOtherOtherFrameworkShim });//, iEnumerableShimmy });

            Debug.WriteLine(test);
        }
    }
}
