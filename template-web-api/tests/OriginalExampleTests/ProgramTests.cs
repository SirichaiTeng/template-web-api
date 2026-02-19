using DocumentFormat.OpenXml.Drawing.Charts;
using OriginalExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginalExampleTests
{
    public class ProgramTests
    {
        [Fact]
        public async Task Program_Main()
        {
            // Arrange
            string[] args = { "test" };

            // Act
            var result = Program.Main(args);

            // Assert
        }
    }
}

