Roadmap:
- Release nuget for new settings class and empty objects/strings work
- Update readme to discuss settings class (and to be cleaned up generally)
- Allow registering concrete type to enable returning empty (instead of null) abstract classes/interfaces.
- Improving the GetShim method to remove the need for "get_" and "set_" prefixes when getting properties
- Assembly scanning to grab any concrete type (for sake of unit tests, won't really matter which as long as it is mocked)
- Cover Poise with unit tests
- Clean up code base, as it's a mess
- Update Pose unit tests with fixes made
- Looking into automatic fix for the Pose workarounds that require creating shims for framework methods with lambdas that simply call themselves (like linq and string.format, see below)
- Events?


This is very much in alpha stage. There is a lot of cleanup that must be done and there will certainly be bugs to fix. More details (including a better readme/documentation) will be coming in the next few days (as of 12/12/19).

Please do the right thing and follow proper SOLID principles (ie dependency injection). If you need this library while writing new code, I urge you to look into the aforementioned principles. If you're working with legacy systems, this library may help.

Note, this library is built on top of Pose. Poise classes are essentially wrappers using reflection to simplify the use of Pose, but this library also provides some critical bug fixes for Pose. As wrappers, Poise -> Pose, and Shimmy -> Shim. It may be useful to review the Pose documentation here: https://www.nuget.org/packages/Pose/ It will be essential for changing the behaviors of individual methods/properties for your mocks.

To use this framework, run your unit tests within a call to Poise.Run by providing a lambda, example:

    Poise.Run(() => ClassUnderTest.TestMeThough(), _shimmyCollection);
    
When unit testing, you generally want to execute a single public method on a class and ensure that any other calls or properties to other classes are mocked. ClassUnderTest above is the class we are testing, while TestMeThough is the method we are testing.

The second parameter to Poise.Run -- \_shimmyCollection -- contains a list of Shimmies. Shimmies are mocks; much like Moq, they provide default, empty implementations that return default values. If you've reviewed Pose or skipped ahead, the purpose of this is essentially to give you default behaviors for many methods so you only need to write Shims for what you specifically need for a test. It even accomplishes this for constructors: constructors use FormatterServices.GetUninitializedObject to provide objects with default properties and without the side effects of constructors (it is on the road map to change this and other behaviors per Shimmy you create).

To create Shimmies, use the following:

    _shimmyCollection.Add( Shimmy.CreateShimmy<ShimMe>(typeof(ShimMe)) ); // yes, passing type twice! This will be cleaned in future
    
There are a couple optional parameters for CreateShimmy
1) a boolean for whether or not you'd like to shim properties or let them run as usual (maybe they have side-effect producing code in their getters or setters).
2) ICreateShims interface for different types of shim creators (which handles construction of shims for properties, static methods, instance methods etc... most won't need to worry about this)

Finally, to change the behavior of individual methods, you can get the underlying Shim and use .With by providing a lambda. The lambda must have the same parameters and return type as that which it is mocking. If instance, it also needs to have an initial parameter of the type of the class which contains the method:

__Note: if these are properties, append "get\_" or "set\_" per whatever you are changing the behavior of. A fix for this isn't difficult and on the roadmap__

__Note also that if the method is overloaded, there is an overloaded version of GetShim that accepts all the types, with the return type being last, in order to differentiate the different methods__

Instance:

      // public class ShimMe
      //public string MyInstanceMethod(int arg, string arg2)

      var myShimmy = Shimmy.CreateShimmy<ShimMe>(typeof(ShimMe));
      myShimmy.GetShim(nameof(ShimMe.MyInstanceMethod)).With((ShimMe @this, int arg, string arg2) => "my string");
      
Static:

      // public class ShimMe
      //public static string MyStaticMethod(int arg, string arg2)

      var myShimmy = Shimmy.CreateShimmy<ShimMe>(typeof(ShimMe));
      myShimmy.GetShim(nameof(ShimMe.MyStaticMethod)).With((int arg, string arg2) => "my string");

Note that if you're already using this despite it being in alpha, bear in mind that you will need to "setup" many calls to framework and third-party libraries. An example is with LINQ. IEnumerable.FirstOrDefault doesn't work very well with the Pose library, but you can force it into good behavior with the following:

    _frameworkShims.Add(
        Shim.Replace(() => Is.A<IEnumerable<string>>().FirstOrDefault(Is.A<Func<string, bool>>()))
            .With((IEnumerable<string> @this, Func<string, bool> func) => @this.FirstOrDefault(func))
    );
    
Where IEnumerable<string> above would be whatever your data type is, and whatever it's type argument is would need to match the "Func<string," portion of the other arguments. 
    
Any framework shims you require are then passed as a third argument to Poise.Run, ie:

        Poise.Run(() => ClassUnderTest.TestMeThough(), _shimmyCollection, _frameworkShims);
        
With framework shims, make sure you are using the exact same overload that you need in your code under test! An example would be string.Format and its various versions.


