using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    /// <summary>
    /// Used to convert a WPF InkCanvas Stroke into a serializable collection of points.
    /// </summary>
    public class BoardPointCollection
    {
        /// <summary>
        /// Gets or sets the identifier for the board point collection.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid ID { set; get; }

        /// <summary>
        /// Gets or sets the user that generated the collection.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string User { set; get; }

        /// <summary>
        /// Gets or sets the color.  The color is used for each point in the collection.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { set; get; }

        /// <summary>
        /// Gets or sets the width of the brush.
        /// </summary>
        /// <value>
        /// The width of the brush.
        /// </value>
        public double BrushWidth { set; get; }

        /// <summary>
        /// Gets or sets the height of the brush.
        /// </summary>
        /// <value>
        /// The height of the brush.
        /// </value>
        public double BrushHeight { set; get; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public List<BoardPoint> Points { set; get; } = new List<BoardPoint>();
    }
}
