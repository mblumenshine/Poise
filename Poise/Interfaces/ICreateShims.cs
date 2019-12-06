using System;
using System.Collections.Generic;
using System.Reflection;
using Pose;

namespace Poise.Interfaces
{
    public interface ICreateShims
    {
        Shim CreatePublicInstanceShim<T>(MethodInfo methodInfo, Type type);
        Shim CreatePublicStaticShim(MethodInfo methodInfo);
        Shim CreateConstructorShim<T>(ConstructorInfo constructorInfo, Type type);
        Shim CreatePropertyGetShim<T>(MethodInfo methodInfo, Type type);
        Shim CreatePropertySetShim<T>(MethodInfo methodInfo, Type type);
    }
}
