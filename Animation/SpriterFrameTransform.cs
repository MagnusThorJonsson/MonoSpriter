using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Frame Transform.
    /// Precalculated frame transformation used by the Spriter Frame animation object.
    /// </summary>
    public struct SpriterFrameTransform
    {
        #region Variables & Properties
        /// <summary>
        /// The frame position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The frame angle
        /// </summary>
        public float Angle;

        /// <summary>
        /// The frame scale
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The frame alpha
        /// </summary>
        public float Alpha;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a SpriterFrameTransform
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="angle">The angle in radians</param>
        /// <param name="scale">The scale</param>
        /// <param name="alpha">The image alpha</param>
        public SpriterFrameTransform(Vector2 position, float angle, Vector2 scale, float alpha)
        {
            Angle = angle;
            Position = position;
            Scale = scale;
            Alpha = alpha;
        }
        #endregion
    }
}
