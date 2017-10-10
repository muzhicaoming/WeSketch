using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    public class Result
    {
        public string ResultJSON { set; get; }
        public bool Error { set; get; }
        public string ErrorMessage { set; get; }
    }
}
