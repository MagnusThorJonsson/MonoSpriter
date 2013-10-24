using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoSpriter.Animation;

namespace MonoSpriter
{
    /// <summary>
    /// Spriter Object.
    /// The main object, handles animations and texture content
    /// </summary>
    public sealed class SpriterObject
    {
        private struct RenderedPosition
        {
            public Vector2 Position;
            public Vector2 Pivot;
            public float Angle;
            public Vector2 Scale;
            public float Alpha;
            public SpriteEffects Effects;
        }


        #region Variables & Properties
        /// <summary>
        /// The Spriter object name
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// The animation frames per second 
        /// </summary>
        public int FPS { get { return _fps; } }
        private int _fps;

        /// <summary>
        /// Object Position
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _position;

        /// <summary>
        /// Flip the animation on the x axis
        /// </summary>
        public bool DoFlipX
        {
            get { return _doFlipX; }
            set { _doFlipX = value; }
        }
        private bool _doFlipX;

        /// <summary>
        /// Flip the animation on the y axis
        /// </summary>
        public bool DoFlipY
        {
            get { return _doFlipY; }
            set { _doFlipY = value; }
        }
        private bool _doFlipY;

        /// <summary>
        /// Is the animation playing
        /// </summary>
        public bool IsPlaying { get { return _isPlaying; } }
        private bool _isPlaying;

        private SpriterEntity _entity;
        private double _elapsedTime;
        private int _frame;

        private SpriterAnimation _currentAnimation;
        private Dictionary<int, Dictionary<int, Texture2D>> _sprites;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a SpriterObject
        /// </summary>
        /// <param name="name">The name of the object</param>
        /// <param name="fps">The frames per second the animations run at</param>
        /// <param name="entity">The entity used by this object</param>
        /// <param name="sprites">The Texture2D sprites used by the object</param>
        internal SpriterObject(string name, int fps, SpriterEntity entity, Dictionary<int, Dictionary<int, Texture2D>> sprites)
        {
            _name = name;
            _fps = fps;
            _entity = entity;
            _sprites = sprites;

            if (entity.Animations.Count > 0)
                _currentAnimation = entity.Animations[0];

            _isPlaying = true;
        }
        #endregion


        #region Animation Methods
        /// <summary>
        /// Sets the animation to a specific frame
        /// </summary>
        /// <param name="frame">The frame to set the animation to</param>
        public void SetFrame(int frame)
        {
            _frame = (int)MathHelper.Clamp(frame, 0, _currentAnimation.Frames.Count - 1);
        }

        /// <summary>
        /// Resets the current animation
        /// </summary>
        public void Reset()
        {
            _elapsedTime = 0;
            _frame = 0;
        }


        /// <summary>
        /// Sets a specific animation to play
        /// </summary>
        /// <param name="id">The id of the animation</param>
        public void SetAnimation(int id)
        {
            if (_entity.Animations.ContainsKey(id))
            {
                _currentAnimation = _entity.Animations[id];
                Reset();
            }
        }


        /// <summary>
        /// Sets a specific animation to play
        /// </summary>
        /// <param name="name">The name of the animation</param>
        public void SetAnimation(string name)
        {
            int animId = _entity.GetAnimationId(name);
            if (animId != -1)
                SetAnimation(animId);
        }
        #endregion


        #region Skin Methods
        /// <summary>
        /// Applies a Character Map (Skin) to the entity
        /// </summary>
        /// <param name="id">The id of the Character Map</param>
        public void ApplyCharacterMap(int id)
        {
            CharacterMap map = _entity.GetCharacterMap(id);
            if (map != null)
            {
                foreach (MapInstruction item in map.Maps)
                    _sprites[item.Folder][item.File] = _sprites[item.TargetFolder][item.TargetFile];
            }

        }


        /// <summary>
        /// Applies a Character Map (Skin) to the entity
        /// </summary>
        /// <param name="name">The name of the skin</param>
        public void ApplyCharacterMap(string name)
        {
            CharacterMap map = _entity.GetCharacterMap(name);
            if (map != null)
            {
                foreach (MapInstruction item in map.Maps)
                    _sprites[item.Folder][item.File] = _sprites[item.TargetFolder][item.TargetFile];
            }
        }
        #endregion


        #region Main Methods
        /// <summary>
        /// Updates the object
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            if (_isPlaying)
            {
                _elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_elapsedTime > _currentAnimation.Length)
                {
                    if (_currentAnimation.IsLoop)
                        _elapsedTime = _elapsedTime % _currentAnimation.Length;
                    else
                        _elapsedTime = _currentAnimation.Length;
                }

                _frame = (int)MathHelper.Clamp(
                    (int)Math.Ceiling((_elapsedTime / 1000.0) * _fps),
                    0,
                    _currentAnimation.Frames.Count - 1
                );
            }
        }

        private RenderedPosition GetRenderedPosition(SpriterFrameImage fimg)
        {
            // Apply transforms
            SpriterFrameTransform transform = fimg.Transform;
            RenderedPosition result = new RenderedPosition();

            result.Alpha = transform.Alpha;

            result.Position.Y = Position.Y + (transform.Position.Y * (DoFlipY ? -1 : 1));
            result.Position.X = Position.X + (transform.Position.X * (DoFlipX ? -1 : 1));

            bool flipX = DoFlipX;
            bool flipY = DoFlipY;

            result.Angle = transform.Angle;
            if (flipX != flipY)
            {
                result.Angle *= -1;
            }

            result.Pivot = fimg.Pivot;
            if (flipX)
            {
                result.Pivot.X = _sprites[fimg.Folder][fimg.File].Width - result.Pivot.X;
            }

            if (flipY)
            {
                result.Pivot.Y = _sprites[fimg.Folder][fimg.File].Height - result.Pivot.Y;
            }

            result.Scale = transform.Scale;
            if (result.Scale.X < 0)
            {
                result.Scale.X = -result.Scale.X;
                flipX = !flipX;
            }
            if (result.Scale.Y < 0)
            {
                result.Scale.Y = -result.Scale.Y;
                flipY = !flipY;
            }

            result.Effects = SpriteEffects.None;
            if (flipX)
            {
                result.Effects |= SpriteEffects.FlipHorizontally;
            }
            if (flipY)
            {
                result.Effects |= SpriteEffects.FlipVertically;
            }

            return result;
        }

        /// <summary>
        /// Draws the Spriter object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriterFrame frame = _currentAnimation.Frames[_frame];

            float layer = 0;
            foreach (SpriterFrameImage frameImage in frame.Frames)
            {
                RenderedPosition pos = GetRenderedPosition(frameImage);
                spriteBatch.Draw(
                    _sprites[frameImage.Folder][frameImage.File],
                    pos.Position,
                    null,
                    Color.White * pos.Alpha,
                    pos.Angle,
                    pos.Pivot,
                    pos.Scale,
                    pos.Effects,
                    layer
                );

                layer += 0.01f;
            }
        }
        #endregion

    }
}
