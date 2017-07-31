// Authored by Scott B. Norton

namespace DotNetHero.Core.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    ///     Restricts the class and subclasses of <see cref="T" /> to the Singleton Pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonComponent<T>
        where T : SingletonComponent<T>

    {
        static readonly Lazy<T> Lazy;

        static SingletonComponent()
        {
            Lazy = new Lazy<T>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public static T Instance => Lazy.Value;

        static T CreateInstance()
        {
            Type type = typeof(T);
            TypeInfo typeInfo = type.GetTypeInfo();

            // The singleton must have no public constructors and at least one private constructor.
            // The private constructor serves to instance the singleton, but also prevents a 
            // constructor-less class from being instanciated with the default no-argument constructor.
            IEnumerable<ConstructorInfo> ctors = typeInfo.DeclaredConstructors;
            ConstructorInfo singletonCtor = ctors?.FirstOrDefault(ctor => ctor.IsPrivate && ctor.GetParameters().Length == 0);
            if (singletonCtor == null || !ctors.All(ctor => ctor.IsPrivate))
                throw new Exception(
                    $"{typeof(SingletonComponent<T>)} cannot be a singleton because it has a publically accessable constructor or lacks at least one private, parameterless constructor.");

            // A singleton must be sealed to prevent more than one instance of the class.
            // If not, it could be possible to create multiple derivatives through inheritance.
            if (!typeInfo.IsSealed)
                throw new Exception(
                    $"{typeof(SingletonComponent<T>)} cannot be a singleton because it is inheritable.");

            T result = null;
            try
            {
                result = (T)singletonCtor.Invoke(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not instantiate singleton, exception thrown inside constructor.");
                Console.WriteLine(ex.InnerException?.Message);
                Console.WriteLine(ex.InnerException?.StackTrace);
            }

            return result;
        }
    }
}