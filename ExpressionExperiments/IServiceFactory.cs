using System;

namespace ExpressionExperiments
{
    public interface IServiceFactory
    {
        object GetInstance(Type serviceType);
    }
}