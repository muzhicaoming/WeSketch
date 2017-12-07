using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSketchSharedDataModels
{
    /// <summary>
    /// User information sent to a user after they authenticate.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { set; get; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid UserID { set; get; }

        /// <summary>
        /// Gets or sets the board.
        /// </summary>
        /// <value>
        /// The board.
        /// </value>
        public Board Board { set; get; }
    }
}
