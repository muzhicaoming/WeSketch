using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    public class BoardPoint
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string User { set; get; }
        /// <summary>
        /// Gets or sets the pressure factor.
        /// </summary>
        /// <value>
        /// The pressure factor.
        /// </value>
        public float PressureFactor { set; get; }
        /// <summary>
        /// Gets or sets the x location for the point.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { set; get; }
        /// <summary>
        /// Gets or sets the y location for the point.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { set; get; }
    }
}
