using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Frame Point.
    /// Precalculated points used by the frame animation.
    /// </summary>
    internal sealed class SpriterFramePoint
    {
        #region Variables & Properties
        /// <summary>
        /// Bone parent id
        /// </summary>
        public int Parent
        {
            get { return _parent; }
            internal set { _parent = value; }
        }
        private int _parent;

        /// <summary>
        /// Bone position
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            internal set { _position = value; }
        }
        private Vector2 _position;

        /// <summary>
        /// Bone angle
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            internal set { _angle = value; }
        }
        private float _angle;

        /// <summary>
        /// Bone scale
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            internal set { _scale = value; }
        }
        private Vector2 _scale;

        /// <summary>
        /// Bone alpha
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
            internal set { _alpha = value; }
        }
        private float _alpha;


        /// <summary>
        /// Image Z index
        /// </summary>
        public int ZIndex
        {
            get { return _zIndex; }
            internal set { _zIndex = value; }
        }
        private int _zIndex;

        /// <summary>
        /// The image transform
        /// </summary>
        public SpriterFrameTransform Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        private SpriterFrameTransform _transform;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a Frame Point
        /// </summary>
        public SpriterFramePoint()
        {
            _parent = -1;
            _position = Vector2.Zero;
            _angle = 0f;
            _scale = Vector2.One;
            _alpha = 1f;
            _zIndex = 0;

            _transform = new SpriterFrameTransform();
        }

        /// <summary>
        /// Creates a Frame Point
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="angle">Angle</param>
        /// <param name="scale">Scale</param>
        /// <param name="alpha">Alpha</param>
        public SpriterFramePoint(Vector2 position, float angle, Vector2 scale, float alpha)
        {
            _parent = -1;
            _position = position;
            _angle = angle;
            _scale = scale;
            _alpha = alpha;
            _zIndex = 0;

            _transform = new SpriterFrameTransform();
        }


        /// <summary>
        /// Creates a Frame Point
        /// </summary>
        /// <param name="parent">The frame point parent id</param>
        /// <param name="position">Position</param>
        /// <param name="angle">Angle</param>
        /// <param name="scale">Scale</param>
        /// <param name="alpha">Alpha</param>
        public SpriterFramePoint(int parent, Vector2 position, float angle, Vector2 scale, float alpha)
        {
            _parent = parent;

            _position = position;
            _angle = angle;
            _scale = scale;
            _alpha = alpha;
            _zIndex = 0;

            _transform = new SpriterFrameTransform();
        }

        /// <summary>
        /// Creates a Frame Point
        /// </summary>
        /// <param name="parent">The frame point parent id</param>
        /// <param name="position">Position</param>
        /// <param name="angle">Angle</param>
        /// <param name="scale">Scale</param>
        /// <param name="alpha">Alpha</param>
        /// <param name="zIndex">The Z index of the point</param>
        public SpriterFramePoint(int parent, Vector2 position, float angle, Vector2 scale, float alpha, int zIndex)
        {
            _parent = parent;

            _position = position;
            _angle = angle;
            _scale = scale;
            _alpha = alpha;
            _zIndex = zIndex;

            _transform = new SpriterFrameTransform();
        }
        #endregion

    }
}
