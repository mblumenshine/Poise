using Pose;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poise
{
    public static class Poise
    {
        public static void Run(Action act, IEnumerable<Shimmy> shimmies, IEnumerable<Shim> frameworkShims = null)
        {
            var shims = shimmies.SelectMany(x => x.GetShims()).ToList();
            //shims.Add(Shim.Replace(() => Is.A<IEnumerable<string>>().FirstOrDefault(Is.A<Func<string, bool>>())).With((IEnumerable<string> @this, Func<string, bool> func) => "MGLTL"));
            //shims.Add(Shim.Replace(() => IEnumerable<string>.));
            //shims.Add(Shim.Replace(() => string.Format(Is.A<string>(), Is.A<object[]>())).With((string s, object[] parms) => string.Format(s, parms)));
            if (frameworkShims != null) shims.AddRange(frameworkShims);
            PoseContext.Isolate(act, shims.ToArray());
        }
    }
}
