using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RecipeManagerWebApi.Tests.Utilities
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
            IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            //Orders the test cases based on their priority attribute values then returns the sorted list.

            string assemblyName = typeof(TestPriorityAttribute).AssemblyQualifiedName!;

            List<TTestCase> orderedTestCases = testCases.ToList(); 

            orderedTestCases.Sort((prev, curr) =>
            {
                int prevPriority = prev.TestMethod.Method.GetCustomAttributes(assemblyName).FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                int currPriority = curr.TestMethod.Method.GetCustomAttributes(assemblyName).FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                return prevPriority.CompareTo(currPriority); //Comparison function determines where an element goes before, after or is same in priority as another
            });

            return orderedTestCases;
        }
    }
}