using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    /// <summary>
    /// Used to inform clients of basic information for a user that is connected to a board that they are connected to.
    /// </summary>
    [Serializable]
    public class ConnectedUser
    {
        /// <summary>
        /// Gets or sets the user name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { set; get; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConnectedUser"/> is the owner of the board.
        /// </summary>
        /// <value>
        ///   <c>true</c> if owner; otherwise, <c>false</c>.
        /// </value>
        public bool Owner { set; get; }
    }
}
