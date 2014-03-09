using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardsNeverDie.Dialog
{
    [Serializable]
    public class Speaker
    {
        #region Declarations

        public int AvatarIndex;
        public string Message;

        #endregion

        #region Constructor

        /// <summary>
        /// Add a new Speaker to a Conversation
        /// </summary>
        /// <param name="avatar">Avatar Index for Speaker</param>
        /// <param name="msg">Speaker's Message</param>
        public Speaker(int avatar, string msg)
        {
            AvatarIndex = avatar;
            Message = msg;
        }

        /// <summary>
        /// Needed for Serialization. Not intended for use.
        /// </summary>
        public Speaker()
        {
            AvatarIndex = 0;
            Message = "";
        }

        #endregion
    }
}
