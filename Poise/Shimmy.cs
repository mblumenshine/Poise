using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Poise.Exceptions;
using Poise.Interfaces;
using Poise.ShimCreators;
using Pose;

namespace Poise
{
    public class Shimmy
    {
        private readonly IList<ShimmyInternal> _shimmyInternals;

        private Shimmy(IList<ShimmyInternal> shimmyInternals)
        {
            _shimmyInternals = shimmyInternals ?? throw new ArgumentNullException(nameof(shimmyInternals));
        }

        public static Shimmy CreateShimmy<T>(Type type, ICreateShims shimCreator = null)
        {
            shimCreator = shimCreator ?? new DefaultValueShimCreator();

            // The reflection magic that will build up the shims from binding flags etc
            var staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);
            var instanceMethods = type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            var properties = type.GetProperties();
            var constructors = type.GetConstructors();
            //foreach (var constructor in constructors)
            //{
            //    _shims.Add(_shimCreator.CreateConstructorShim(constructor));
            //}
            var shimmyInternals = new List<ShimmyInternal>();
            foreach (var method in staticMethods)
            {
                shimmyInternals.Add(new ShimmyInternal(method, shimCreator.CreatePublicStaticShim(method)));
            }
            foreach (var method in instanceMethods)
            {
                shimmyInternals.Add(new ShimmyInternal(method, shimCreator.CreatePublicInstanceShim<T>(method, type)));
            }
            foreach (var constructor in constructors)
            {
                shimmyInternals.Add(new ShimmyInternal(constructor, shimCreator.CreateConstructorShim<T>(constructor, type)));
            }
            foreach (var property in properties)
            {
                shimmyInternals.Add(new ShimmyInternal(property, shimCreator.CreatePropertyGetShim<T>(property, type), true));
                shimmyInternals.Add(new ShimmyInternal(property, shimCreator.CreatePropertySetShim<T>(property, type), true));
            }

            return new Shimmy(shimmyInternals);
        }

        public Shim GetShim(string name, bool isSetter = false, params Type[] types)
        {
            var shim = _shimmyInternals.Where(x => x.IsSetter == isSetter).FirstOrDefault(x => IsCorrectShim(x, name, types))?.Shim;

            if (shim == null) throw new ShimNotFoundException();

            return shim;
        }

        private static bool IsCorrectShim(ShimmyInternal shimmyInternal, string name, params Type[] types)
        {
            if (shimmyInternal.Name != name) return false;

            return types.Any(type => !shimmyInternal.Types.Any(x => x != type));
        }

        internal IEnumerable<Shim> GetShims() => _shimmyInternals.Select(x => x.Shim);

        private class ShimmyInternal
        {
            internal readonly string Name;
            internal readonly IList<Type> Types = new List<Type>();
            internal readonly Shim Shim;
            internal readonly bool IsSetter;

            internal ShimmyInternal(MethodInfo methodInfo, Shim shim)
            {
                Name = methodInfo.Name;
                foreach (var param in methodInfo.GetParameters())
                    Types.Add(param.ParameterType);
                Types.Add(methodInfo.ReturnType);
                Shim = shim;
            }

            internal ShimmyInternal(ConstructorInfo constructorInfo, Shim shim)
            {
                Name = constructorInfo.Name;
                foreach (var param in constructorInfo.GetParameters())
                    Types.Add(param.ParameterType);
                Shim = shim;
            }

            internal ShimmyInternal(PropertyInfo propertyInfo, Shim shim, bool isSetter)
            {
                IsSetter = isSetter;
                Name = propertyInfo.Name;
                Shim = shim;
            }
        }
    }
}
