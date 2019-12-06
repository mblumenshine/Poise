using Poise.Interfaces;
using Pose;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Poise.ShimCreators
{
    internal class DefaultValueShimCreator : ICreateShims
    {
        public Shim CreateConstructorShim<T>(ConstructorInfo constructorInfo, Type type)
        {
            return Shim.ReplaceAuto(constructorInfo, type).With(() => (T)FormatterServices.GetUninitializedObject(type), true);
            //var defaults = constructorInfo.GetParameters().Select(x => x.DefaultValue);

            ////Shim ctorShim = Shim.Replace(() => constructorInfo.Invoke(defaults.ToArray())).With((defaults.ToArray()) => FormatterServices.GetUninitializedObject(constructorInfo.ReflectedType));

            //return ctorShim;
        }

        public Shim CreatePropertyGetShim<T>(MethodInfo methodInfo, Type type)
        {
            var returnType = methodInfo.ReturnType;

            return Shim.ReplaceAutoInstance<T>(methodInfo, type, false).With((T @this) => GetDefault(returnType), true);
        }

        public Shim CreatePropertySetShim<T>(MethodInfo methodInfo, Type type)
        {
            return Shim.ReplaceAutoInstance<T>(methodInfo, type, true).With(delegate (T @this) {  }, true);
        }

        public Shim CreatePublicInstanceShim<T>(MethodInfo methodInfo, Type type)
        {
            var returnType = methodInfo.ReturnType;

            if (returnType == typeof(void)) return Shim.ReplaceAuto(methodInfo, type).With(delegate (T @this) { }, true);
            return Shim.ReplaceAuto(methodInfo, type).With(delegate (T @this) { GetDefault(returnType); }, true);
        }

        //delegate void HelpDamnIt();

        public Shim CreatePublicStaticShim(MethodInfo methodInfo)
        {
            //var defaults = methodInfo.GetParameters().Select(x => x.DefaultValue);

            //var tArgs = new List<Type>();
            //foreach (var param in methodInfo.GetParameters())
            //    tArgs.Add(param.ParameterType);
            var returnType = methodInfo.ReturnType;
            //tArgs.Add(methodInfo.ReturnType);
            //var delDecltype = Expression.GetDelegateType(tArgs.ToArray());
            //var delegateT = Delegate.CreateDelegate(delDecltype, methodInfo);

            //HelpDamnIt help = delegate () { };

            if (returnType == typeof(void)) return Shim.ReplaceAuto(methodInfo).With(() => { }, true);
            //return //Shim.Replace(() => ShimMe.PublicStaticParameters(Is.A<string>())).With((string test) => (string)GetDefault(returnType));//Shim.ReplaceAutoStatic(methodInfo).With((string test) => (string)GetDefault(returnType));
            return Shim.ReplaceAuto(methodInfo).With(() => GetDefault(returnType), true);
        }

        public IEnumerable<Shim> CreateSetterShims()
        {
            throw new NotImplementedException();
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
