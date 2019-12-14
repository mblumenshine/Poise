using System;
using System.Collections.Generic;
using System.Text;

namespace Testing.TestClasses
{
    using System.Linq;

    public static class DontShimMe
    {
        public static string TestMeThough()
        {
            var test = new ShimMe();
            test.PublicProperty = 5;
            test.PublicProperty2 = "dave";
            test.PublicInstance();
            var hello = test.PublicProperty2;

            IEnumerable<string> dave = new List<string>() {"what", "how"};
            var what = dave.FirstOrDefault(x => x == "what");

            var shimMeDependency = test.PublicInstanceReturnsDependency();
            var dependencyValue = shimMeDependency.MyProperty;

            //return $"This should be {what} empty unless explicitly overriden: {test.PublicInstanceParameters("hello")} :anything before this should be empty unless overriden.";
            //return $"This should not say 5: {test.ToString()}. Also if this works, it means there is a severe issue with that exception handling thing";
            //return $"This should {what} not say 5: {test.PublicProperty}. Also if this works, it means there is a severe issue with that exception handling thing";
            return $"Let's see how this one works, should not be negative: {dependencyValue}";
            //return hello;
            //return "Dave";
            //return $"This should be nothing: {test.PublicProperty3}";
        }
    }
}
