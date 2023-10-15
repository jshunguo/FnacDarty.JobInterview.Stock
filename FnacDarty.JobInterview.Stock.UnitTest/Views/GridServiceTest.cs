using FnacDarty.JobInterview.Stock.Views;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.UnitTest.Views
{
    [TestFixture]
    internal class GridServiceTest
    {
        private Mock<IGridView> mockGridView;
        private Mock<IGridViewFactory> mockFactory;
        private GridService service;
        private object data;
        private List<GridColumn> columns;

        [SetUp]
        public void SetUp()
        {
            mockGridView = new Mock<IGridView>();
            mockFactory = new Mock<IGridViewFactory>(MockBehavior.Strict);

            mockFactory.Setup(f => f.CreateNew(It.IsAny<IReadOnlyList<GridColumn>>()))
                       .Returns(mockGridView.Object);

            service = new GridService(mockFactory.Object);

            data = new { Name = "Test" };
            columns = new List<GridColumn> { new GridColumn("Name") };
        }

        [Test]
        public void DisplayDataInGrid_WithValidSource_BindsDataToGridView()
        {
            service.DisplayDataInGrid(data, columns);
            mockGridView.Verify(v => v.Bind(data), Times.Once);
        }

        [Test]
        public void DisplayDataInGrid_WithValidSource_ReturnsCorrectGridView()
        {
            var result = service.DisplayDataInGrid(data, columns);
            Assert.AreEqual(mockGridView.Object, result);
        }
    }
}
