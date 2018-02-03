using NUnit.Framework;
using System.Collections;
using System.Reflection;

namespace Config4Net.Tests
{
    /// <summary>
    /// https://stackoverflow.com/questions/318210/compare-equality-between-two-objects-in-nunit
    /// </summary>
    public static class AssertEx
    {
        public static void PropertyValuesAreEquals(object expected, object actual)
        {
            var properties = expected.GetType().GetProperties();
            foreach (var property in properties)
            {
                var expectedValue = property.GetValue(expected, null);
                var actualValue = property.GetValue(actual, null);

                if (actualValue is IList list)
                    AssertListsAreEquals(property, list, (IList)expectedValue);
                else if (!Equals(expectedValue, actualValue))
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}", property.DeclaringType?.Name, property.Name, expectedValue, actualValue);
            }
        }

        public static void PropertyValuesAreNotEquals(object expected, object actual)
        {
            try
            {
                PropertyValuesAreEquals(expected, actual);
                Assert.Fail("Expected value {0} are equals to {1}", expected, actual);
            }
            catch (AssertionException) { }
        }

        private static void AssertListsAreEquals(PropertyInfo property, IList actualList, IList expectedList)
        {
            if (actualList.Count != expectedList.Count)
                Assert.Fail("Property {0}.{1} does not match. Expected IList containing {2} elements but was IList containing {3} elements", property.PropertyType.Name, property.Name, expectedList.Count, actualList.Count);

            for (var i = 0; i < actualList.Count; i++)
                if (!Equals(actualList[i], expectedList[i]))
                    Assert.Fail("Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
        }
    }
}