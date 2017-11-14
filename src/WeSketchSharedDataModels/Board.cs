﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    public class Board
    {
        public Guid BoardID { set; get; }
        public bool Owner { set; get; }
        public List<ConnectedUser> ConnectedUsers { set; get; } = new List<ConnectedUser>();
    }
}
