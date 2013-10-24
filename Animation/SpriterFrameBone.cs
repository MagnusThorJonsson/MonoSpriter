using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Frame Bone.
    /// Precalculated bones used by the frame animation.
    /// </summary>
    internal sealed class SpriterFrameBone
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
        /// Bone content folder id
        /// </summary>
        public int Folder 
        { 
            get { return _spriterFolder; }
            internal set { _spriterFolder = value; }
        }
        private int _spriterFolder;

        /// <summary>
        /// Bone content file id
        /// </summary>
        public int File 
        { 
            get { return _spriterFile; }
            internal set { _spriterFile = value; } 
        }
        private int _spriterFile;

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
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a Frame Bone
        /// </summary>
        public SpriterFrameBone()
        {
            _parent = -1;
            _spriterFolder = -1;
            _spriterFile = -1;
            _position = Vector2.Zero;
            _angle = 0f;
            _scale = Vector2.One;
            _alpha = 1f;
        }

        /// <summary>
        /// Creates a Frame Bone
        /// </summary>
        /// <param name="folder">Spriter content folder id</param>
        /// <param name="file">Spriter content file id</param>
        /// <param name="position">Position</param>
        /// <param name="angle">Angle</param>
        /// <param name="scale">Scale</param>
        /// <param name="alpha">Alpha</param>
        public SpriterFrameBone(int folder, int file, Vector2 position, float angle, Vector2 scale, float alpha)
        {
            _parent = -1;
            _spriterFolder = folder;
            _spriterFile = file;
            _position = position;
            _angle = angle;
            _scale = scale;
            _alpha = alpha;
        }


        /// <summary>
        /// Creates a Frame Bone
        /// </summary>
        /// <param name="parent">The frame bone parent id</param>
        /// <param name="folder">Spriter content folder id</param>
        /// <param name="file">Spriter content file id</param>
        /// <param name="position">Position</param>
        /// <param name="angle">Angle</param>
        /// <param name="scale">Scale</param>
        /// <param name="alpha">Alpha</param>
        public SpriterFrameBone(int parent, int folder, int file, Vector2 position, float angle, Vector2 scale, float alpha)
        {
            _parent = parent;
            _spriterFolder = folder;
            _spriterFile = file;
            _position = position;
            _angle = angle;
            _scale = scale;
            _alpha = alpha;
        }
        #endregion

    }
}
