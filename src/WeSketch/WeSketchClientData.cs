using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSketchSharedDataModels;

namespace WeSketch
{
    /// <summary>
    /// Holds user and board identifiers as well the users color.
    /// </summary>
    public class WeSketchClientData
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { set; get; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { set; get; }

        /// <summary>
        /// The instance.
        /// </summary>
        private static Lazy<WeSketchClientData> _instance = new Lazy<WeSketchClientData>(() => new WeSketchClientData());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static WeSketchClientData Instance => _instance.Value;

        /// <summary>
        /// Prevents a default instance of the <see cref="WeSketchClientData"/> class from being created.
        /// </summary>
        private WeSketchClientData()
        {

        }
    }
}
