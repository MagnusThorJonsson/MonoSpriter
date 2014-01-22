using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Frame Image.
    /// Precalculated images used by the frame animation.
    /// </summary>
    public sealed class SpriterFrameImage
    {
        #region Variables & Properties
        /// <summary>
        /// Image parent id
        /// </summary>
        public int Parent 
        { 
            get { return _parent; }
            internal set { _parent = value; }
        }
        private int _parent;

        /// <summary>
        /// Image content folder id
        /// </summary>
        public int Folder 
        { 
            get { return _spriterFolder; }
            internal set { _spriterFolder = value; }
        }
        private int _spriterFolder;

        /// <summary>
        /// Image content file id
        /// </summary>
        public int File 
        { 
            get { return _spriterFile; }
            internal set { _spriterFile = value; } 
        }
        private int _spriterFile;

        /// <summary>
        /// Image position
        /// </summary>
        public Vector2 Position 
        { 
            get { return _position; }
            internal set { _position = value; } 
        }
        private Vector2 _position;

        /// <summary>
        /// Image angle
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            internal set { _angle = value; }
        }
        private float _angle;

        /// <summary>
        /// Image pivot
        /// </summary>
        public Vector2 Pivot
        {
            get { return _pivot; }
            internal set { _pivot = value; }
        }
        private Vector2 _pivot;

        /// <summary>
        /// Image scale
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            internal set { _scale = value; }
        }
        private Vector2 _scale;

        /// <summary>
        /// Image alpha
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
        /// Checks whether image is clockwise
        /// </summary>
        public bool IsClockwise
        {
            get { return _isClockwise; }
            internal set { _isClockwise = value; }
        }
        private bool _isClockwise;

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


        #region Constructor
        /// <summary>
        /// Creates a basic SpriterFrameImage
        /// </summary>
        public SpriterFrameImage()
        {
            _parent = -1;
            _spriterFolder = -1;
            _spriterFile = -1;
            _position = Vector2.Zero;
            _angle = 0f;
            _pivot = Vector2.Zero;
            _scale = Vector2.One;
            _alpha = 1f;
            _zIndex = 0;
            _isClockwise = false;
            _transform = new SpriterFrameTransform();
        }

        /// <summary>
        /// Creates a basic SpriterFrameImage
        /// </summary>
        /// <param name="folder">The Spriter content folder</param>
        /// <param name="file">The Spriter content file</param>
        /// <param name="position">The position</param>
        public SpriterFrameImage(int folder, int file, Vector2 position)
        {
            _parent = -1;
            _spriterFolder = folder;
            _spriterFile = file;
            _position = position;
            _angle = 0f;
            _pivot = Vector2.Zero;
            _scale = Vector2.One;
            _alpha = 1f;
            _zIndex = 0;
            _isClockwise = false;
            _transform = new SpriterFrameTransform();
        }

        /// <summary>
        /// Creates a basic SpriterFrameImage
        /// </summary>
        /// <param name="parent">The parent id</param>
        /// <param name="folder">The Spriter content folder</param>
        /// <param name="file">The Spriter content file</param>
        /// <param name="position">The position</param>
        public SpriterFrameImage(int parent, int folder, int file, Vector2 position)
        {
            _parent = parent;
            _spriterFolder = folder;
            _spriterFile = file;
            _position = position;
            _angle = 0f;
            _pivot = Vector2.Zero;
            _scale = Vector2.One;
            _alpha = 1f;
            _zIndex = 0;
            _isClockwise = false;
            _transform = new SpriterFrameTransform();
        }
        #endregion
    }
}
