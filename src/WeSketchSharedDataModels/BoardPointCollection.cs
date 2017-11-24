using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    public class BoardPointCollection
    {
        public Guid ID { set; get; }
        public string User { set; get; }
        public string Color { set; get; }
        public List<BoardPoint> Points { set; get; } = new List<BoardPoint>();
    }
}
