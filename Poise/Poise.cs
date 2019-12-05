using Pose;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poise
{
    public static class Poise
    {
        public static void Run(Action act, IEnumerable<Shimmy> shimmies)
        {
            var shims = shimmies.SelectMany(x => x.GetShims()).ToList();
            //shims.Add(Shim.Replace(() => string.Format(Is.A<string>(), Is.A<object[]>())).With((string s, object[] parms) => string.Format(s, parms)));
            PoseContext.Isolate(act, shims.ToArray());
        }
    }
}
