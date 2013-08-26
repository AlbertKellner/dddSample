using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace DDD.Sample.Foundation
{
    public static class IoC
    {
        private readonly static ConcurrentDictionary<Type, Registration> Table = new ConcurrentDictionary<Type, Registration>();

        public static TAbstract Resolve<TAbstract>()
        {
            return Table[typeof(TAbstract)].Instace<TAbstract>();
        }

        public static void Register<TAbstract, TConcrete>()
            where TConcrete : TAbstract
        {
            var registration = new Registration
            {
                AbstractType = typeof(TAbstract),
                ConcreteType = typeof(TConcrete),
            };
            Table.TryAdd(registration.AbstractType, registration);
        }

        public static void Register<TAbstract, TConcrete>(Func<object> factory = null)
            where TConcrete : TAbstract
        {
            var registration = new Registration
            {
                AbstractType = typeof(TAbstract),
                ConcreteType = typeof(TConcrete),
                Factory = factory
            };
            Table.TryAdd(registration.AbstractType, registration);
        }
    }

    struct Registration
    {
        public Type AbstractType;
        public Type ConcreteType;
        public Func<object> Factory;

        public TAbstract Instace<TAbstract>(params object[] args) 
        {
            if (Factory != null)
                return (TAbstract) Factory();
            
            return (TAbstract)Activator.CreateInstance(ConcreteType, args);
        }
    }

}
