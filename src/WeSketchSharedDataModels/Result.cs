using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    /// <summary>
    /// Used to send messages to clients.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets or sets the result json.  Used to store serialized objects and send them to clients.
        /// </summary>
        /// <value>
        /// The result json.
        /// </value>
        public string ResultJSON { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Result"/> is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if error; otherwise, <c>false</c>.
        /// </value>
        public bool Error { set; get; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { set; get; }
    }
}
