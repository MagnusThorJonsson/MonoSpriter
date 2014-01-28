using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoSpriter.Animation;
using MonoSpriter.Data;
using MonoSpriter.Mainline;
using MonoSpriter.Timeline;

namespace MonoSpriter
{
    /// <summary>
    /// Spriter Factory.
    /// 
    /// A Factory class for SpriterObjects and animations
    /// </summary>
    public class SpriterFactory
    { 
        #region Variables & Properties
        private static Dictionary<int, Dictionary<int, Texture2D>> _sprites;
        private static List<SpriterEntity> _entities;
        internal static Dictionary<int, SpriterFolder> _folders;
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes static variables
        /// </summary>
        static SpriterFactory()
        {
            _sprites = new Dictionary<int, Dictionary<int, Texture2D>>();
            _entities = new List<SpriterEntity>();
            _folders = new Dictionary<int, SpriterFolder>();


        }
        #endregion


        #region Factory Methods
        /// <summary>
        /// Creates a new Spriter Object from the passed in data
        /// </summary>
        /// <param name="xmlDoc">The SCML file</param>
        /// <param name="content">The content manager</param>
        /// <param name="path">The path to the entities assets</param>
        /// <param name="name">The name of the entity</param>
        /// <param name="fps">The frames per second the animations run at</param>
        /// <param name="offset">The offset</param>
        /// <param name="baseDepth">The base depth for this entity</param>
        /// <param name="tint">The color overlay for the sprite</param>
        /// <returns>A new spriter object</returns>
        public static SpriterObject Create(XDocument xmlDoc, ContentManager content, string path, string name, int fps, Vector2 offset, float baseDepth, Color tint)
        {
            // Prepare data
            LoadData(path, xmlDoc, content);

            return Create(name, fps, offset, baseDepth, tint);
        }

        /// <summary>
        /// Creates a new Spriter Object from already loaded data
        /// </summary>
        /// <param name="name">The name of the entity</param>
        /// <param name="fps">The frames per second the animations run at</param>
        /// <param name="offset">The offset</param>
        /// <param name="baseDepth">The base depth for this entity</param>
        /// <param name="tint">The color overlay for the sprite</param>
        /// <returns>A new spriter object</returns>
        public static SpriterObject Create(string name, int fps, Vector2 offset, float baseDepth, Color tint)
        {
            // TODO: All of this needs a refactoring, data structure not completely A-OK
            // Get the entity by name
            SpriterEntity entity = _entities.Find(s => string.Equals(s.Name, name));
            if (entity == null)
                throw new ArgumentNullException("name");

            foreach (KeyValuePair<int, SpriterAnimation> animation in entity.Animations)
            {
                // TODO: Change this into a configuration parameter
                int frameStep = 1000 / fps;
                for (int frameTime = 0; frameTime <= animation.Value.Length; frameTime += frameStep)
                {
                    // Prepare tweening process
                    float currentTime = 0;
                    float nextTime = 0;

                    // Fetch and set the frames
                    MainlineKey currentFrame = (from cf in animation.Value.MainlineKeys
                                                where cf.Time <= frameTime
                                                orderby cf.Time descending
                                                select cf).FirstOrDefault();//animation.Value.FindCurrentFrame(frameId);
                    MainlineKey nextFrame = (from cf in animation.Value.MainlineKeys
                                             where cf.Time > frameTime
                                             orderby cf.Time ascending
                                             select cf).FirstOrDefault();//animation.Value.FindNextFrame(frameId);

                    if (nextFrame == null)
                    {
                        nextFrame = animation.Value.IsLoop ? animation.Value.MainlineKeys[0] : currentFrame;
                        nextTime = animation.Value.Length;
                    }
                    else
                        nextTime = nextFrame.Time;

                    currentTime = frameTime - currentFrame.Time;
                    nextTime -= currentFrame.Time;

                    // Tween objects and bones
                    float amount = MathHelper.Clamp(currentTime / nextTime, 0.0f, 1.0f);
                    List<SpriterFrameImage> frameImages = prepareImageFrames(currentFrame, nextFrame, animation, amount);
                    List<SpriterFrameBone> frameBones = prepareBoneFrames(currentFrame, nextFrame, animation, amount);
                    List<SpriterFramePoint> framePoints = preparePointFrames(currentFrame, nextFrame, animation, amount);

                    // Create Frames
                    SpriterFrame frame = new SpriterFrame(
                        frameImages.OrderBy(x => x.ZIndex).ToList(),
                        frameBones,
                        framePoints
                    );

                    foreach (SpriterFrameImage image in frame.Frames)
                        image.Transform = frame.Transform(image, Vector2.One, offset);

                    foreach (SpriterFramePoint point in frame.Points)
                        point.Transform = frame.Transform(point, Vector2.One, offset);

                    animation.Value.Frames.Add(frame);
                }
            }

            return new SpriterObject(name, fps, entity, new Dictionary<int,Dictionary<int,Texture2D>>(_sprites), baseDepth, tint);
        }
        #endregion


        #region Private Helper Methods
        /// <summary>
        /// Precalculates tweens for the sprite animations
        /// </summary>
        /// <param name="current">The current frame</param>
        /// <param name="nextFrame">The next frame</param>
        /// <param name="animation">The animation</param>
        /// <param name="amount">The amount of tween</param>
        /// <returns>A list of image frames</returns>
        private static List<SpriterFrameImage> prepareImageFrames(MainlineKey current, MainlineKey nextFrame, KeyValuePair<int, SpriterAnimation> animation, float amount)
        {
            List<SpriterFrameImage> frameImages = new List<SpriterFrameImage>();
            foreach (MainlineKeyObject currentKey in current.ObjectRefs)
            {
                // TODO: IF TIMELINE DOESN'T HAVE A KEY AT THE END THE TWEEN WILL BE CHOPPY ON REPLAY
                // TODO: Change the LINQ so that only the Object timelinekey items are selected
                MainlineKeyObject nextKey = nextFrame.ObjectRefs.Where(x => x.Timeline == currentKey.Timeline).FirstOrDefault();//.FindObject(currentKey.Id);
                SpriterTimeline timeline = (nextKey == null ? animation.Value.Timelines[currentKey.Timeline]
                                                          : animation.Value.Timelines[nextKey.Timeline]);

                // TODO: Too many g'damn checks
                if (timeline.Type != TimelineType.Object)
                    continue;

                TimelineKey timelineKey = (nextKey == null ? timeline.Keys[currentKey.Key] : timeline.Keys[nextKey.Key]);
                if (timelineKey == null)
                    continue;

                TimelineKeyObject next = timelineKey.Item as TimelineKeyObject;
                frameImages.Add(
                    Tween(
                        animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].Item as TimelineKeyObject,
                        next,
                        amount,
                        animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].DoSpin,
                        currentKey.ZIndex,
                        currentKey.Parent
                    )
                );
            }

            return frameImages;
        }

        /// <summary>
        /// Precalculates tweens for the point positions
        /// </summary>
        /// <param name="current">The current frame</param>
        /// <param name="nextFrame">The next frame</param>
        /// <param name="animation">The animation</param>
        /// <param name="amount">The amount of tween</param>
        /// <returns>A list of point frames</returns>
        private static List<SpriterFramePoint> preparePointFrames(MainlineKey current, MainlineKey nextFrame, KeyValuePair<int, SpriterAnimation> animation, float amount)
        {
            List<SpriterFramePoint> framePoints = new List<SpriterFramePoint>();
            foreach (MainlineKeyObject currentKey in current.ObjectRefs)
            {
                // TODO: IF TIMELINE DOESN'T HAVE A KEY AT THE END THE TWEEN WILL BE CHOPPY ON REPLAY
                // TODO: Change the LINQ so that only the Point timelinekey items are selected
                MainlineKeyObject nextKey = nextFrame.ObjectRefs.Where(x => x.Timeline == currentKey.Timeline).FirstOrDefault();//.FindObject(currentKey.Id);
                SpriterTimeline timeline = (nextKey == null ? animation.Value.Timelines[currentKey.Timeline]
                                                          : animation.Value.Timelines[nextKey.Timeline]);

                // TODO: Too many g'damn checks
                if (timeline.Type != TimelineType.Point)
                    continue;

                TimelineKey timelineKey = (nextKey == null ? timeline.Keys[currentKey.Key] : timeline.Keys[nextKey.Key]);
                if (timelineKey == null)
                    continue;

                TimelineKeyPoint next = timelineKey.Item as TimelineKeyPoint;
                framePoints.Add(
                    Tween(
                        animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].Item as TimelineKeyPoint,
                        next,
                        amount,
                        animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].DoSpin,
                        currentKey.ZIndex,
                        currentKey.Parent
                    )
                );
            }

            return framePoints;
        }


        /// <summary>
        /// Precalculates tweens for the bone animations
        /// </summary>
        /// <param name="current">The current frame</param>
        /// <param name="nextFrame">The next frame</param>
        /// <param name="animation">The animation</param>
        /// <param name="amount">The amount of tween</param>
        /// <returns>A list of bone frames</returns>
        private static List<SpriterFrameBone> prepareBoneFrames(MainlineKey current, MainlineKey nextFrame, KeyValuePair<int, SpriterAnimation> animation, float amount)
        {
            List<SpriterFrameBone> frameBones = new List<SpriterFrameBone>();
            foreach (MainlineKeyBone currentBone in current.BoneRefs)
            {
                MainlineKeyBone nextBone = nextFrame.BoneRefs.Where(x => x.Timeline == currentBone.Timeline).FirstOrDefault();//FindBone(currentBone.Id);
                TimelineKey timelineKey = (nextBone == null ? animation.Value.Timelines[currentBone.Timeline].Keys[currentBone.Key]
                                                         : animation.Value.Timelines[nextBone.Timeline].Keys[nextBone.Key]);

                if (timelineKey == null)
                    continue;

                TimelineKeyBone next = timelineKey.Item as TimelineKeyBone;
                frameBones.Add(
                    Tween(
                        animation.Value.Timelines[currentBone.Timeline].Keys[currentBone.Key].Item as TimelineKeyBone,
                        next,
                        amount,
                        animation.Value.Timelines[currentBone.Timeline].Keys[currentBone.Key].DoSpin,
                        currentBone.Parent
                    )
                );
            }

            return frameBones;
        }


        /// <summary>
        /// Loads all the relevant data from the SCML file
        /// </summary>
        /// <param name="path">The path to the entities assets</param>
        /// <param name="xmlDoc">The SCML data</param>
        /// <param name="content">The content manager to hold the sprites</param>
        private static void LoadData(string path, XDocument xmlDoc, ContentManager content)
        {
            // Reset content
            _sprites.Clear();
            _entities.Clear();
            _folders.Clear();

            // Ready Folders and Textures
            foreach (XElement folderRow in xmlDoc.Descendants("spriter_data").Elements("folder"))
            {
                SpriterFolder folder = new SpriterFolder(folderRow);
                _folders.Add(folder.Id, folder);

                Dictionary<int, Texture2D> spriteList = new Dictionary<int, Texture2D>();
                foreach (SpriterFile file in folder.Files.Values)
                {
                    try
                    {
                        string assetName = file.Name.Substring(0, file.Name.Length - System.IO.Path.GetExtension(file.Name).Length);
                        Texture2D tex = content.Load<Texture2D>(path + assetName.Replace("/", "\\"));
                        spriteList[file.Id] = tex;
                    }
                    catch (ContentLoadException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                _sprites.Add(folder.Id, spriteList);
            }

            // Ready Entities
            foreach (XElement entityRow in xmlDoc.Descendants("spriter_data").Elements("entity"))
                _entities.Add(new SpriterEntity(entityRow));
        }
        #endregion


        #region Tween Methods
        //TODO: Make into a template function

        /// <summary>
        /// Makes a tween of two objects
        /// </summary>
        /// <param name="prev">Previous frame</param>
        /// <param name="next">Next frame</param>
        /// <param name="amount">The amount to tween</param>
        /// <param name="isClockwise">Whether this is a clockwise tween</param>
        /// <param name="zIndex">The zIndex of the frame</param>
        /// <param name="parent">The parent (-1 for root)</param>
        /// <returns>A new Spriter Frame Image</returns>
        private static SpriterFrameImage Tween(TimelineKeyObject prev, TimelineKeyObject next, float amount, bool isClockwise, int zIndex, int parent)
        {
            SpriterFrameImage result = new SpriterFrameImage(parent, prev.Folder, prev.File, Vector2.Lerp(prev.Position, next.Position, amount));
            result.IsClockwise = isClockwise;
            result.ZIndex = zIndex;
            result.Pivot = Vector2.Lerp(prev.Pivot, next.Pivot, amount);
            result.Alpha = MathHelper.Lerp(prev.Alpha, next.Alpha, amount);
            result.Scale = Vector2.Lerp(prev.Scale, next.Scale, amount);

            float angleA = prev.Angle;
            float angleB = next.Angle;

            if (isClockwise)
            {
                if (angleB - angleA < 0.0f)
                {
                    angleB += MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }
            else
            {
                if (angleB - angleA > 0.0f)
                {
                    angleB -= MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }

            result.Angle = MathHelper.Lerp(angleA, angleB, amount);
            return result;
        }


        /// <summary>
        /// Makes a tween of two bones
        /// </summary>
        /// <param name="prev">Previous bone</param>
        /// <param name="next">Next bone</param>
        /// <param name="amount">The amount to tween</param>
        /// <param name="isClockwise">Whether this is a clockwise tween</param>
        /// <param name="parent">The parent (-1 for root)</param>
        /// <returns>A new frame bone</returns>
        private static SpriterFrameBone Tween(TimelineKeyBone prev, TimelineKeyBone next, float amount, bool isClockwise, int parent)
        {
            SpriterFrameBone result = new SpriterFrameBone();
            result.Parent = parent;
            result.Scale = Vector2.Lerp(prev.Scale, next.Scale, amount);
            result.Position = Vector2.Lerp(prev.Position, next.Position, amount);
            result.Alpha = MathHelper.Lerp(prev.Alpha, next.Alpha, amount);

            float angleA = prev.Angle;
            float angleB = next.Angle;
            if (isClockwise)
            {
                if (angleB - angleA < 0)
                {
                    angleB += MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }
            else
            {
                if (angleB - angleA > 0.0f)
                {
                    angleB -= MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }

            result.Angle = MathHelper.Lerp(angleA, angleB, amount);
            return result;
        }

        /// <summary>
        /// Makes a tween of two points
        /// </summary>
        /// <param name="prev">Previous point</param>
        /// <param name="next">Next point</param>
        /// <param name="amount">The amount to tween</param>
        /// <param name="isClockwise">Whether this is a clockwise tween</param>
        /// <param name="zIndex">The zIndex of the frame</param>
        /// <param name="parent">The parent (-1 for root)</param>
        /// <returns>A new frame point</returns>
        private static SpriterFramePoint Tween(TimelineKeyPoint prev, TimelineKeyPoint next, float amount, bool isClockwise, int zIndex, int parent)
        {
            SpriterFramePoint result = new SpriterFramePoint();
            result.Parent = parent;
            result.ZIndex = zIndex;
            result.Scale = Vector2.Lerp(prev.Scale, next.Scale, amount);
            result.Position = Vector2.Lerp(prev.Position, next.Position, amount);
            result.Alpha = MathHelper.Lerp(prev.Alpha, next.Alpha, amount);

            float angleA = prev.Angle;
            float angleB = next.Angle;
            if (isClockwise)
            {
                if (angleB - angleA < 0)
                {
                    angleB += MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }
            else
            {
                if (angleB - angleA > 0.0f)
                {
                    angleB -= MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }

            result.Angle = MathHelper.Lerp(angleA, angleB, amount);
            return result;
        }
        #endregion
    }
}
