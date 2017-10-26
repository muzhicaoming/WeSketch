using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    public class BoardPoint
    {
        public string User { set; get; }
        public float PressureFactor { set; get; }
        public double X { set; get; }
        public double Y { set; get; }
    }
}
