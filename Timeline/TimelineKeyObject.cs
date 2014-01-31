using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using MonoSpriter.Data;
using MonoSpriter.Animation;

namespace MonoSpriter.Timeline
{
    /// <summary>
    /// Spriter TimelineKey Object data object.
    /// Used with the Timeline animations.
    /// 
    /// DATA:
    ///   < key id="4" time="1668" spin="0">
    ///     < object folder="6" file="1" x="32.480097" y="-1.612087" angle="355.339223" scale_x="5.553945"/>
    /// </summary>
    internal sealed class TimelineKeyObject : TimelineKeyBone, ITimelineKeyItem
    {
        #region Variables & Properties
        /// <summary>
        /// Object pivot
        /// </summary>
        public Vector2 Pivot { get { return _pivot; } }
        private Vector2 _pivot;

        /// <summary>
        /// Object content folder id
        /// </summary>
        public int Folder { get { return _spriterFolder; } }
        private int _spriterFolder;

        /// <summary>
        /// Object content file id
        /// </summary>
        public int File { get { return _spriterFile; } }
        private int _spriterFile;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter timeline key object data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public TimelineKeyObject(XElement element)
            : base(element)
        {
            if (element.Attribute("folder") != null)
                _spriterFolder = int.Parse(element.Attribute("folder").Value);
            else
                _spriterFolder = -1;

            if (element.Attribute("file") != null)
                _spriterFile = int.Parse(element.Attribute("file").Value);
            else
                _spriterFile = -1;

            SpriterFile file = SpriterFactory._currentFolders[Folder].Files[File];
            _pivot = Vector2.Zero;
            if (element.Attribute("pivot_x") != null)
                _pivot.X = float.Parse(element.Attribute("pivot_x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("pivot_y") != null)
                _pivot.Y = float.Parse(element.Attribute("pivot_y").Value, CultureInfo.InvariantCulture); // Flip below

            // Calculate pivot and flip Y
            if (_pivot.X > 0)
                _pivot.X = _pivot.X * file.Width;
            else
                _pivot.X = file.PivotX * file.Width;

            if (_pivot.Y > 0)
                _pivot.Y = (1f - _pivot.Y) * file.Height;
            else
                _pivot.Y = (1f - file.PivotY) * file.Height;
        }
        #endregion
    }
}
