using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class ImageDelDTO
    {
        public int id { get; set; }
        public List<string> url { get; set; } = new List<string>();
    }
}
