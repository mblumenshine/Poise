using System;
using System.Collections.Generic;
using System.Text;

namespace Poise
{
    public class ShimmySettings
    {
        public bool ShouldMockProperties { get; set; } = true;
        public bool ShouldReturnEmptyObjects { get; set; } = true;
        public bool ShouldReturnEmptyStrings { get; set; } = true;
    }
}
