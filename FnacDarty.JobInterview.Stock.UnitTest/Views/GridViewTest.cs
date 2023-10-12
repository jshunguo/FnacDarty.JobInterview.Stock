using FnacDarty.JobInterview.Stock.Views;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FnacDarty.JobInterview.Stock.UnitTest.Views
{
    [TestFixture]
    internal class GridViewTest
    {
        private IGridView _gridView;
        private StringWriter _writer;

        [SetUp]
        public void SetUp()
        {
            _gridView = GridView.Definition.AddColumns<MovementView>().CreateView();
            _writer = new StringWriter();
        }

        [TearDown]
        public void TearDown()
        {
            _writer.Dispose();
            _writer = null;
        }

        [Test]
        public void Render_ValidDataSource_Success()
        {
            var movementList = MovementView.Generate();

            _gridView.Bind(movementList);

            _gridView.Render(_writer);

            Assert.AreEqual(Properties.Resources.Expected_Render_Content, _writer.ToString());
        }
    }
}
