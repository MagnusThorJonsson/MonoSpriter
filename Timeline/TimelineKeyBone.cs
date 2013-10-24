using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Timeline
{
    /// <summary>
    /// Spriter TimelineKey Bone data object.
    /// Used with the Timeline animations.
    /// 
    /// DATA:
    ///   <key id="0">
    ///     <bone x="5" y="40" angle="451.59114" scale_x="0.18527"/>
    /// </summary>
    internal class TimelineKeyBone
    {
        #region Variables & Properties
        /// <summary>
        /// Bone position
        /// </summary>
        public Vector2 Position { get { return position; } }
        protected Vector2 position;

        /// <summary>
        /// Bone angle
        /// </summary>
        public float Angle { get { return angle; } }
        protected float angle;

        /// <summary>
        /// Bone scale
        /// </summary>
        public Vector2 Scale { get { return scale; } }
        protected Vector2 scale;

        /// <summary>
        /// Bone alpha
        /// </summary>
        public float Alpha { get { return alpha; } }
        protected float alpha;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter timeline key bone data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public TimelineKeyBone(XElement element)
        {
            position = Vector2.Zero;
            if (element.Attribute("x") != null)
                position.X = float.Parse(element.Attribute("x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("y") != null)
                position.Y = -float.Parse(element.Attribute("y").Value, CultureInfo.InvariantCulture);

            if (element.Attribute("angle") != null)
                angle = -MathHelper.ToRadians(float.Parse(element.Attribute("angle").Value, CultureInfo.InvariantCulture));

            scale = Vector2.One;
            if (element.Attribute("scale_x") != null)
                scale.X = float.Parse(element.Attribute("scale_x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("scale_y") != null)
                scale.Y = float.Parse(element.Attribute("scale_y").Value, CultureInfo.InvariantCulture);

            if (element.Attribute("a") != null)
                alpha = float.Parse(element.Attribute("a").Value, CultureInfo.InvariantCulture);
            else
                alpha = 1f;
        }
        #endregion
    }
}
