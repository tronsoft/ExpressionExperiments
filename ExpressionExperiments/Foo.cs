using System;

namespace ExpressionExperiments
{
    public class Foo : IFoo
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}