using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    public class User
    {
        public string UserName { set; get; }
        public Guid Token { set; get; }
        public Board Board { set; get; }
    }
}
