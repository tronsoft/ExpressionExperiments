using System;

namespace ExpressionExperiments
{
    public class ServiceFactory : IServiceFactory
    {
        public object GetInstance(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(typeof(IFoo)))
            {
                return new Foo();
            }
            
            throw new NotSupportedException();
        }
    }
}