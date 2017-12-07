using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    /// <summary>
    /// Used to reference information about the board the user is connected to.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Gets or sets the board identifier.
        /// </summary>
        /// <value>
        /// The board identifier.
        /// </value>
        public Guid BoardID { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Board"/> is owned by the user connected to it.
        /// </summary>
        /// <value>
        ///   <c>true</c> if owner; otherwise, <c>false</c>.
        /// </value>
        public bool Owner { set; get; }

        /// <summary>
        /// Gets or sets the connected users.
        /// </summary>
        /// <value>
        /// The connected users.
        /// </value>
        public List<ConnectedUser> ConnectedUsers { set; get; } = new List<ConnectedUser>();
    }
}
