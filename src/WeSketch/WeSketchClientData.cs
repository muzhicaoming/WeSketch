using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSketchSharedDataModels;

namespace WeSketch
{
    public class WeSketchClientData
    {
        public User User { set; get; }
        public string Color { set; get; }
        private static Lazy<WeSketchClientData> _instance = new Lazy<WeSketchClientData>(() => new WeSketchClientData());
        public static WeSketchClientData Instance => _instance.Value;
        private WeSketchClientData()
        {

        }
    }
}
