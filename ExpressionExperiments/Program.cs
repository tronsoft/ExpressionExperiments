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
            //CreateMethodCall();

            var pageContextParam = Expression.Parameter(typeof(PageContext), "pageContext");
            var actionContextParam = Expression.Parameter(typeof(ActionContext), "actionContext");
            var newExpression = Expression.New(typeof(Page));
            var lambda = Expression.Lambda<Func<PageContext, ActionContext, object>>(newExpression, pageContextParam, actionContextParam);
            var result = lambda.Compile();
            var instance = result(new PageContext(), new ActionContext());

            Console.ReadKey();
        }

        private static void CreateMethodCall()
        {
            var serviceType = typeof(IFoo);
            var methodInfo = /*typeof(IServiceFactory).GetMethod("GetInstance", new[] { typeof(Type) }); */
                typeof(IServiceFactory).GetTypeInfo().DeclaredMethods
                                       .Single(m => m.Name == "GetInstance" && !m.IsGenericMethod && m.GetParameters().Length == 1);
            var parameterExpression = Expression.Parameter(typeof(IServiceFactory), "serviceFactory");
            var methodCallExpression = Expression.Call(parameterExpression, methodInfo, Expression.Constant(serviceType));
            var converted = Expression.Convert(methodCallExpression, serviceType);
            var @delegate = Expression.Lambda(converted, parameterExpression).Compile();
            var foo = (IFoo) @delegate.DynamicInvoke(new ServiceFactory());
            foo.WriteLine("Hello");
        }
    }

    internal class Page
    {
        public Page()
        {
            
        }
        
        public Page(PageContext pageContext, ActionContext actionContext)
        {
            PageContext = pageContext;
            ActionContext = actionContext;
        }
        
        public PageContext PageContext { get; set; }
        public ActionContext ActionContext { get; set; }
    }

    internal class PageContext
    {
        public PageContext()
        {
            
        }

        public PageContext(ActionContext context)
        {
            
        }
        
        
    }

    internal class ActionContext
    {
        
    }

    public interface IServiceFactory
    {
        object GetInstance(Type serviceType);
    }

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

    public interface IFoo
    {
        void WriteLine(string message);
    }

    public class Foo : IFoo
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}