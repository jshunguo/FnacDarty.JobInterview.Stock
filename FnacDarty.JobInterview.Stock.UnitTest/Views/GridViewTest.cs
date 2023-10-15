using FnacDarty.JobInterview.Stock.Views;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FnacDarty.JobInterview.Stock.UnitTest.Views
{
    [TestFixture]
    internal class GridViewTest
    {
        [Test]
        public void Bind_WithNullSource_ThrowsArgumentNullException()
        {
            var columns = new List<GridColumn>();
            var gridView = new GridView(columns);

            Assert.Throws<ArgumentNullException>(() => gridView.Bind(null));
        }

        [Test]
        public void Bind_WithSingleObject_DoesNotThrows()
        {
            var columns = new List<GridColumn>();
            var gridView = new GridView(columns);
            var mockObject = new Mock<object>();

            Assert.DoesNotThrow(() => gridView.Bind(mockObject.Object));
        }

        [Test]
        public void Render_ValidDataSource_Success() 
        {
            var gridView = new GridView(new GridColumn[]
            {
                new GridColumn("Name")
            });

            var mockTextWriter = new Mock<TextWriter>(MockBehavior.Strict);
            int numberOfWriteCount = 0;
            int numberOfWriteLineCount = 0;

            mockTextWriter.Setup(tw => tw.Write(It.IsAny<string>())).Callback(() => {
                numberOfWriteCount++;
            });
            mockTextWriter.Setup(tw => tw.WriteLine()).Callback(() => {
                numberOfWriteLineCount++;
            });

            gridView.Bind(2);

            gridView.Render(mockTextWriter.Object);

            mockTextWriter.Verify(tw => tw.Write(It.IsAny<string>()), Times.Exactly(numberOfWriteCount));
            mockTextWriter.Verify(tw => tw.WriteLine(), Times.Exactly(numberOfWriteLineCount));
        }
    }
}
