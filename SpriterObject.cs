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
        /// <summary>
        /// Helper Struct used for calculating positional translation
        /// </summary>
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

        /// <summary>
        /// Flags whether the animation is finished
        /// </summary>
        public bool IsFinished
        {
            get
            {
                if (_currentAnimation != null && 
                    !_currentAnimation.IsLoop && _elapsedTime == _currentAnimation.Length)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// The currently selected animation
        /// </summary>
        public string CurrentAnimation
        {
            get
            {
                if (_currentAnimation != null)
                    return _currentAnimation.Name;

                return null;
            }
        }
        private SpriterAnimation _currentAnimation;

        /// <summary>
        ///  The drawing offset
        /// </summary>
        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        private Vector2 _offset;

        /// <summary>
        /// The base layer depth for this spriter entity
        /// </summary>
        public float DepthBase
        {
            get { return _depthBase; }
            set { _depthBase = value; }
        }
        private float _depthBase;
 
        private Dictionary<int, Dictionary<int, Texture2D>> _sprites;
        private SpriterEntity _entity;
        private double _elapsedTime;
        private int _frame;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a SpriterObject
        /// </summary>
        /// <param name="name">The name of the object</param>
        /// <param name="fps">The frames per second the animations run at</param>
        /// <param name="entity">The entity used by this object</param>
        /// <param name="sprites">The Texture2D sprites used by the object</param>
        /// <param name="depthBase">The base depth for this object</param>
        internal SpriterObject(string name, int fps, SpriterEntity entity, Dictionary<int, Dictionary<int, Texture2D>> sprites, float depthBase)
        {
            _name = name;
            _fps = fps;
            _entity = entity;
            _sprites = sprites;
            _depthBase = depthBase;

            if (entity.Animations.Count > 0)
                _currentAnimation = entity.Animations[0];

            _isPlaying = false;
            _offset = Vector2.Zero;
        }
        #endregion


        #region Animation Methods
        /// <summary>
        /// Play the animation
        /// </summary>
        public void Play()
        {
            _isPlaying = true;
        }

        /// <summary>
        /// Pause the animation
        /// </summary>
        public void Pause()
        {
            _isPlaying = false;
        }

        /// <summary>
        /// Stop the animation
        /// </summary>
        public void Stop()
        {
            _isPlaying = false;
            Reset();
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
        /// Sets the animation to a specific frame
        /// </summary>
        /// <param name="frame">The frame to set the animation to</param>
        public void SetFrame(int frame)
        {
            _frame = (int)MathHelper.Clamp(frame, 0, _currentAnimation.Frames.Count - 1);
        }


        /// <summary>
        /// Transitions to an animation without resetting the position.
        /// Note that the animation being transitioned to needs to be 
        /// of the same length, or longer, as the one being swapped out.
        /// </summary>
        /// <param name="id">The id of the animation</param>
        public void TransitionAnimation(int id)
        {
            if (_entity.Animations.ContainsKey(id))
            {
                _currentAnimation = _entity.Animations[id];
                
                if (_entity.Animations[id].Frames.Count >= _frame)
                    Reset();
            }
        }


        /// <summary>
        /// Transitions to an animation without resetting the position.
        /// Note that the animation being transitioned to needs to be 
        /// of the same length, or longer, as the one being swapped out.
        /// </summary>
        /// <param name="name">The name of the animation</param>
        public void TransitionAnimation(string name)
        {
            int animId = _entity.GetAnimationId(name);
            if (animId != -1)
                TransitionAnimation(animId);
        }


        /// <summary>
        /// Sets a specific animation and resets the current frame position
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
        /// Sets a specific animation and resets the current frame position
        /// </summary>
        /// <param name="name">The name of the animation</param>
        public void SetAnimation(string name)
        {
            int animId = _entity.GetAnimationId(name);
            if (animId != -1)
                SetAnimation(animId);
        }

        /// <summary>
        /// Checks if an entity has a specific animation listed
        /// </summary>
        /// <param name="name">The name of the animation</param>
        /// <returns>True if animation was found</returns>
        public bool HasAnimation(string name)
        {
            if (_entity.GetAnimationId(name) != -1)
                return true;

            return false;
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


        #region Update & Draw
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
                    (int)Math.Ceiling((_elapsedTime * 0.001) * _fps),
                    0,
                    _currentAnimation.Frames.Count - 1
                );
            }
        }


        /// <summary>
        /// Calculates the rendered position of the sprite
        /// </summary>
        /// <param name="image">The Spriter Frame image</param>
        /// <returns>A new rendered position object</returns>
        private RenderedPosition GetRenderedPosition(SpriterFrameImage image)
        {
            // Apply transforms
            SpriterFrameTransform transform = image.Transform;
            RenderedPosition result = new RenderedPosition();

            result.Alpha = transform.Alpha;

            result.Position.Y = Position.Y + (transform.Position.Y * (DoFlipY ? -1 : 1));
            result.Position.X = Position.X + (transform.Position.X * (DoFlipX ? -1 : 1));
            result.Position += _offset;

            bool flipX = DoFlipX;
            bool flipY = DoFlipY;

            result.Angle = transform.Angle;
            if (flipX != flipY)
            {
                result.Angle *= -1;
            }

            result.Pivot = image.Pivot;
            if (flipX)
            {
                result.Pivot.X = _sprites[image.Folder][image.File].Width - result.Pivot.X;
            }

            if (flipY)
            {
                result.Pivot.Y = _sprites[image.Folder][image.File].Height - result.Pivot.Y;
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
                    _depthBase - ((float)frameImage.ZIndex * 0.0000001f) // TODO: using float.Epsilon is fucking up the layer situation so this is a temporary shitfix
                );
            }
        }


        /// <summary>
        /// Debug function
        /// </summary>
        /// <param name="graphicsDevice">The graphic device that holds the created texture (NEED TO CACHE THIS TEXTURE CREATION)</param>
        /// <param name="spriteBatch">The spritebatch</param>
        public void DrawPoints(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            // TESTING
            // TODO: Cache this and generate on creation
            Texture2D rect = new Texture2D(graphicsDevice, 4, 4);
            Color[] data = new Color[4*4];
            for(int i=0; i < data.Length; ++i) 
                data[i] = Color.Red;
            rect.SetData(data);

            SpriterFrame frame = _currentAnimation.Frames[_frame];
            foreach (SpriterFramePoint framePoint in frame.Points)
            {
                RenderedPosition pos = GetRenderedPosition(framePoint);
                spriteBatch.Draw(
                    rect,
                    pos.Position - new Vector2(2, 2),
                    null,
                    Color.White * pos.Alpha,
                    pos.Angle,
                    pos.Pivot,
                    pos.Scale,
                    pos.Effects,
                    _depthBase - ((float)framePoint.ZIndex * 0.0000001f) // TODO: using float.Epsilon is fucking up the layer situation so this is a temporary shitfix
                );
            }
        }


        /// <summary>
        /// Calculates the rendered position of the point
        /// </summary>
        /// <param name="point">The Spriter Frame point</param>
        /// <returns>A new rendered position object</returns>
        private RenderedPosition GetRenderedPosition(SpriterFramePoint point)
        {
            // Apply transforms
            SpriterFrameTransform transform = point.Transform;
            RenderedPosition result = new RenderedPosition();

            result.Alpha = transform.Alpha;

            result.Position.Y = Position.Y + (transform.Position.Y * (DoFlipY ? -1 : 1));
            result.Position.X = Position.X + (transform.Position.X * (DoFlipX ? -1 : 1));
            result.Position += _offset;

            bool flipX = DoFlipX;
            bool flipY = DoFlipY;

            result.Angle = transform.Angle;
            if (flipX != flipY)
            {
                result.Angle *= -1;
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

        #endregion

    }
}
