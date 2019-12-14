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
        private bool _shouldReturnEmptyObjects;
        private bool _shouldReturnEmptyStrings;

        public DefaultValueShimCreator(bool shouldReturnEmptyObjects, bool shouldReturnEmptyStrings)
        {
            _shouldReturnEmptyObjects = shouldReturnEmptyObjects;
            _shouldReturnEmptyStrings = shouldReturnEmptyStrings;
        }

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
            return Shim.ReplaceAutoInstance<T>(methodInfo, type, true).With(delegate (T @this) { }, true);
        }

        public Shim CreatePublicInstanceShim<T>(MethodInfo methodInfo, Type type)
        {
            var returnType = methodInfo.ReturnType;

            if (returnType == typeof(void)) return Shim.ReplaceAuto(methodInfo, type).With(delegate (T @this) { }, true);
            return GetReturnTypeDelegate<T>(Shim.ReplaceAuto(methodInfo, type), returnType);
        }

        private Shim GetReturnTypeDelegate<T>(Shim shim, Type returnType)
        {
            if (returnType.IsValueType)
            {
                if (returnType == typeof(int))
                    return shim.With((T @this) => (int)GetDefault(returnType), true);
                if (returnType == typeof(short))
                    return shim.With((T @this) => (short)GetDefault(returnType), true);
                if (returnType == typeof(sbyte))
                    return shim.With((T @this) => (sbyte)GetDefault(returnType), true);
                if (returnType == typeof(byte))
                    return shim.With((T @this) => (byte)GetDefault(returnType), true);
                if (returnType == typeof(ushort))
                    return shim.With((T @this) => (ushort)GetDefault(returnType), true);
                if (returnType == typeof(uint))
                    return shim.With((T @this) => (uint)GetDefault(returnType), true);
                if (returnType == typeof(long))
                    return shim.With((T @this) => (long)GetDefault(returnType), true);
                if (returnType == typeof(ulong))
                    return shim.With((T @this) => (ulong)GetDefault(returnType), true);
                if (returnType == typeof(char))
                    return shim.With((T @this) => (char)GetDefault(returnType), true);
                if (returnType == typeof(float))
                    return shim.With((T @this) => (float)GetDefault(returnType), true);
                if (returnType == typeof(double))
                    return shim.With((T @this) => (double)GetDefault(returnType), true);
                if (returnType == typeof(decimal))
                    return shim.With((T @this) => (decimal)GetDefault(returnType), true);
                if (returnType == typeof(bool))
                    return shim.With((T @this) => (bool)GetDefault(returnType), true);
                if (returnType == typeof(string))
                    return shim.With((T @this) => (string)GetDefault(returnType), true);
            }

            if (returnType == typeof(string))
                return shim.With((T @this) => (string)GetDefault(returnType), true);
            return shim.With(delegate (T @this) { GetDefault(returnType); }, true);
        }

        //delegate void HelpDamnIt();

        public Shim CreatePublicStaticShim(MethodInfo methodInfo)
        {

            var returnType = methodInfo.ReturnType;
            if (returnType == typeof(void)) return Shim.ReplaceAuto(methodInfo).With(() => { }, true);
            return Shim.ReplaceAuto(methodInfo).With(() => GetDefault(returnType), true);
        }

        private object GetDefault(Type type)
        {
            if (type == typeof(string) && _shouldReturnEmptyStrings) return string.Empty;

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return _shouldReturnEmptyObjects && !type.IsAbstract && !type.IsInterface ? FormatterServices.GetUninitializedObject(type) : null;
        }
    }
}
