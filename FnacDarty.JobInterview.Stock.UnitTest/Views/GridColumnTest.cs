using FnacDarty.JobInterview.Stock.Views;
using NUnit.Framework;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.UnitTest.Views
{
    [TestFixture]
    internal class GridColumnTest
    {
        private const string Name = "Name";
        private object _testObject;
        private GridColumn _column;

        [SetUp] 
        public void SetUp()
        {
            _testObject = new { Name = "TestName" };
            _column = new GridColumn(Name);
        }

        [Test]
        public void Constructor_SetsNameProperty()
        {
            Assert.AreEqual(Name, _column.Name);
        }

        [Test]
        public void GetValueAsString_FromDefinedProperty_ReturnsPropertyStringValue()
        {
            var result = _column.GetValueAsString(_testObject.GetType(), _testObject);
            Assert.AreEqual("TestName", result);
        }

        [Test]
        public void GetValueAsString_FromEnumerable_ReturnsElement()
        {
            var column = new GridColumn("Test");
            column.Ordinal = 1;
            var testList = new List<string> { "Item0", "Item1", "Item2" };

            var result = column.GetValueAsString(testList.GetType(), testList);

            Assert.AreEqual("Item1", result);
        }

        [Test]
        public void GetValueAsString_FromObject_ReturnsObjectStringValue()
        {
            var column = new GridColumn("DoesNotExist");

            var result = column.GetValueAsString(_testObject.GetType(), _testObject);

            Assert.AreEqual(_testObject.ToString(), result);
        }

        [Test]
        public void IsProperty_ForExistingProperty_ReturnsTrue()
        {
            var result = _column.IsProperty(_testObject.GetType());
            Assert.True(result);
        }

        [Test]
        public void IsProperty_ForNonExistingProperty_ReturnsFalse()
        {
            var column = new GridColumn("DoesNotExist");

            var result = column.IsProperty(_testObject.GetType());

            Assert.False(result);
        }
    }
}
