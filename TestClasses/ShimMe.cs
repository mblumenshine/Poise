using System;
using System.Diagnostics;

namespace Testing.TestClasses
{
    public class ShimMe
    {
        public ShimMe()
        {
            var text = "Constructor";
            //Debug.WriteLine(text);
        }

        public ShimMe(string parameter)
        {
            var text = $"Constructor: {parameter}";
            //Debug.WriteLine(text);
        }

        public string PublicInstance()
        {
            var text = "PublicInstance";
            //Debug.WriteLine(text);
            PublicStatic();
            var test = PublicStaticParameters("what");
            //Debug.WriteLine($"Let us see: {test}");
            return $"xxx {test} yyy";
        }

        public string PublicInstanceParameters(string parameter)
        {
            var text =  $"PublicInstanceParameters: {parameter}";
            //Debug.WriteLine(text);

            return text;
        }

        public ShimMeDependency PublicInstanceReturnsDependency()
        {
            return new ShimMeDependency();
        }

        public static void PublicStatic()
        {
            var text = "PublicStatic";
            //Debug.WriteLine(text);
        }

        public static string PublicStaticParameters(string parameter)
        {
            var text = $"PublicStaticParameters: {parameter}";
            //Debug.WriteLine(text);

            return text;
        }

        private int _privateField = int.MaxValue;
        public int PublicProperty
        {
            get
            {
                //Debug.WriteLine($"Value from getter: {_privateField}");
                return _privateField;
            }
            set
            {
                //Debug.WriteLine($"Input to setter: {value}");
                _privateField = value;
            }
        }

        private string _privateField2;
        public string PublicProperty2
        {
            get
            {
                //Debug.WriteLine($"Value from getter: {_privateField2}");
                return _privateField2;
            }
            set
            {
                //Debug.WriteLine($"Input to setter: {value}");
                _privateField2 = value;
            }
        }

        public string PublicProperty3 => "let's see here";

        internal string PublicProperty4 => "a test of internals";
    }
}
