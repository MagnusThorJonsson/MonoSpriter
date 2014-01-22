using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Frame.
    /// Precalculated frames used by the frame animation.
    /// </summary>
    internal sealed class SpriterFrame
    {
        #region Variables & Properties
        /// <summary>
        /// The list of frames
        /// </summary>
        public List<SpriterFrameImage> Frames 
        { 
            get { return _frames; }
            internal set { _frames = value; }
        }
        private List<SpriterFrameImage> _frames;

        /// <summary>
        /// The list of bones
        /// </summary>
        public List<SpriterFrameBone> Bones 
        { 
            get { return _bones; }
            internal set { _bones = value; } 
        }
        private List<SpriterFrameBone> _bones;


        /// <summary>
        /// The list of points
        /// </summary>
        public List<SpriterFramePoint> Points
        {
            get { return _points; }
            internal set { _points = value; }
        }
        private List<SpriterFramePoint> _points;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a SpriterFrame
        /// </summary>
        /// <param name="frames">The image frames</param>
        /// <param name="bones">The bones</param>
        public SpriterFrame(List<SpriterFrameImage> frames, List<SpriterFrameBone> bones)
        {
            _frames = frames;
            _bones = bones;
        }

        /// <summary>
        /// Creates a SpriterFrame
        /// </summary>
        /// <param name="frames">The image frames</param>
        /// <param name="bones">The bones</param>
        /// <param name="points">The points</param>
        public SpriterFrame(List<SpriterFrameImage> frames, List<SpriterFrameBone> bones, List<SpriterFramePoint> points)
        {
            _frames = frames;
            _bones = bones;
            _points = points;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Applies transformation to a SpriterFrameTransform object
        /// </summary>
        /// <param name="transform">The transform to manipulate</param>
        /// <param name="position">The position to transform to</param>
        /// <param name="scale">The scale to transform to</param>
        /// <param name="angle">The angle to transform to</param>
        /// <param name="alpha">The alpha to transform to</param>
        /// <returns>A new transform object</returns>
        private SpriterFrameTransform Transform(SpriterFrameTransform baseTransform, Vector2 scale, float angle, Vector2 position, float alpha)
        {
            Matrix transMatrix = Matrix.CreateScale(baseTransform.Scale.X, baseTransform.Scale.Y, 0) *
                        Matrix.CreateRotationZ(baseTransform.Angle) *
                        Matrix.CreateTranslation(baseTransform.Position.X, baseTransform.Position.Y, 0);

            SpriterFrameTransform result = new SpriterFrameTransform();
            result.Position = Vector2.Transform(position, transMatrix);
            result.Angle = baseTransform.Angle + angle;
            result.Scale = baseTransform.Scale * scale;
            result.Alpha = baseTransform.Alpha * alpha;

            return result;
        }

        /// <summary>
        /// Transforms a Spriter Frame Bone
        /// </summary>
        /// <param name="bone">The bone to transform</param>
        /// <param name="scale">The scale</param>
        /// <param name="offset">The position offset</param>
        /// <returns>A new transform object</returns>
        private SpriterFrameTransform Transform(SpriterFrameBone bone, Vector2 scale, Vector2 offset)
        {
            SpriterFrameTransform baseTransform = (bone.Parent != -1)
                                                  ? Transform(Bones[bone.Parent], scale, offset)
                                                  : new SpriterFrameTransform(offset, 0f, scale, 1f);

            return Transform(baseTransform, bone.Scale, bone.Angle, bone.Position, bone.Alpha);
        }


        /// <summary>
        /// Transforms a Spriter Frame Point
        /// </summary>
        /// <param name="bone">The point to transform</param>
        /// <param name="scale">The scale</param>
        /// <param name="offset">The position offset</param>
        /// <returns>A new transform object</returns>
        public SpriterFrameTransform Transform(SpriterFramePoint point, Vector2 scale, Vector2 offset)
        {
            SpriterFrameTransform baseTransform = (point.Parent != -1)
                                                  ? Transform(Bones[point.Parent], scale, offset)
                                                  : new SpriterFrameTransform(offset, 0f, scale, 1f);

            return Transform(baseTransform, point.Scale, point.Angle, point.Position, point.Alpha);
        }

        /// <summary>
        /// Transforms a Spriter Frame Image
        /// </summary>
        /// <param name="image">The image to transform</param>
        /// <param name="scale">The scale</param>
        /// <param name="offset">The position offset</param>
        /// <returns>A new transform object</returns>
        public SpriterFrameTransform Transform(SpriterFrameImage image, Vector2 scale, Vector2 offset)
        {
            // Apply transforms from self and/or parent
            SpriterFrameTransform baseTransform = (image.Parent != -1)
                                                   ? Transform(Bones[image.Parent], scale, offset)
                                                   : new SpriterFrameTransform(offset, 0f, scale, 1f);

            return Transform(baseTransform, image.Scale, image.Angle, image.Position, image.Alpha);
        }
        #endregion
    }
}
