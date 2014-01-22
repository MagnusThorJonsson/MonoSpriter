using System;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Timeline
{

    /// <summary>
    /// Spriter TimelineKey Point data object.
    /// Used with the Timeline animations.
    /// 
    /// DATA:
    ///   <key id="0">
    ///     <bone x="5" y="40" angle="451.59114" scale_x="0.18527"/>
    /// </summary>
    internal class TimelineKeyPoint : TimelineKeyBone, ITimelineKeyItem
    {

        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter timeline key bone data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public TimelineKeyPoint(XElement element)
            : base(element)
        {
        }
        #endregion
    }
}
