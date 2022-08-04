using System;

namespace RecipeManagerWebApi.Tests.Utilities
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        //Custom attribute to give priority to unit tests.
        //Very simple, just stores an integer value which essentially gives the corresponding method a priority level.

        public int Priority { get; private set; }

        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
}
