using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateMethodCall();

            var typePageContext = typeof(PageContext);
            var typeActionContext = typeof(ActionContext);
            var constructorInfo = typeof(Page).GetTypeInfo().GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] {typePageContext, typeActionContext}, null);
            var pageContextParam = Expression.Parameter(typePageContext, "pageContext");
            var actionContextParam = Expression.Parameter(typeActionContext, "actionContext");
            var newExpression = Expression.New(constructorInfo, pageContextParam, actionContextParam); // new Page(pageContextParam, actionContextParam)
            var lambda = Expression.Lambda<Func<PageContext, ActionContext, Page>>(newExpression, pageContextParam, actionContextParam); // (pageContent, actionContext) => new Page(pageContent, actionContextParam); 
            var result = lambda.Compile();
            var instance = result(new PageContext(), new ActionContext());

            Console.ReadKey();
        }

        private static void CreateMethodCall() 
        {
            var serviceType = typeof(IFoo);
            var methodInfo = typeof(IServiceFactory).GetTypeInfo().DeclaredMethods.Single(m => m.Name == "GetInstance" && !m.IsGenericMethod && m.GetParameters().Length == 1);
            var parameterExpression = Expression.Parameter(typeof(IServiceFactory), "serviceFactory"); // The instance that Expression.Call is going to work on.
            var methodCallExpression = Expression.Call(parameterExpression, methodInfo, Expression.Constant(serviceType)); // serviceFactory.GetInstance(typeof(IFoo))
            var converted = Expression.Convert(methodCallExpression, serviceType); // (IFoo)serviceFactory.GetInstance(typeof(IFoo))
            var @delegate = Expression.Lambda(converted, parameterExpression).Compile(); // serviceFactory => (IFoo)serviceFactory.GetInstance(typeof(IFoo));
            var foo = (IFoo) @delegate.DynamicInvoke(new ServiceFactory());
            foo.WriteLine("Hello");
        }
    }
}